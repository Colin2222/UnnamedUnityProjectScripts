using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public PlayerScript playerScript;

    Rigidbody2D rigidbody2d;

    //spawning
    public float spawnTime;
    private float spawnTimeCounter;
    [System.NonSerialized]
    public bool isSpawning;
    [System.NonSerialized]
    public bool isSpawnCooling;
    private float spawnCoolCounter;
    [System.NonSerialized]
    public int spawnDirection;

    //Moving/running
    public float speed = 3.0f;
    private float horizontal;
    private float vertical;
    private int direction = 1;

    private float conversationHorizontal;
    private float conversationVertical;


    //Jumping
    public float jumpForce = 10f;
    bool jumped = false;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool isExtraJumping;
    public float coyoteTime;
    [System.NonSerialized]
    public float timeSinceGrounded;
    public float maxLandingForce;

    //walljumping/sliding
    public float slideForce = 5.0f;
    private bool isSliding;
    private int wallSide;
    public float wallJumpTime = 0.5f;
    private float wallJumpTimeCounter;
    private bool isWallJumping;

    //Dashing
    public float dashTime = 0.5f;
    private float dashTimeCounter;
    private bool isDashing = false;
    public float dashSpeed = 5.0f;
    private int dashSide;
    public float dashReload = 0.6f;
    private float dashReloadCounter = 1.0f;

    // striking
    private bool isStriking = false;
    public GameObject strikeZone;
    StrikeScript strikeChecker;
    public float strikeTime;
    private float strikeTimer;
    public float strikeCooldown;
    private float strikeCooldownTimer;

    // interacting
    private bool isInteracting = false;
    private bool isTalking = false;
    private bool isFrozen = false;
    public GameObject interactZone;
    [System.NonSerialized]
    public bool canReact = false;
    // YES/NO
    [System.NonSerialized]
    public bool isYN = false;
    [System.NonSerialized]
    public bool YNAnswer = false;
    private float YNHorizontal;
    private float YNVertical;
    private bool YNWasUp;
    private bool YNWasDown;
    private bool YNWasRight;
    private bool YNWasLeft;
    [System.NonSerialized]
    public int[] last3 = {0,0,0};
    private int[] noAnswer1 = {1,2,1};
    private int[] noAnswer2 = {2,1,2};
    private int[] yesAnswer1 = {-1,-2,-1};
    private int[] yesAnswer2 = {-2,-1,-2};
    private float decisionThreshold = 1.0f;
    private float timeSinceLastDecision = 0.0f;

    // inventory
    [System.NonSerialized]
    public bool inventoryOpen = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isSpawning = true;
        spawnTimeCounter = spawnTime;
        rigidbody2d = gameObject.transform.parent.GetComponent<Rigidbody2D>();
        strikeChecker = strikeZone.GetComponent<StrikeScript>();
    }

    void Update(){
        if(isTalking){
            isFrozen = true;
            animator.SetBool("IsTalking",true);
            HandleYN();
        }
        else{
            isFrozen = false;
        }

        if(!isFrozen)
        {
            if(!isWallJumping && !isDashing){
                horizontal = Input.GetAxisRaw("Horizontal");
            }
            vertical = Input.GetAxis("Vertical");

            // set speed for animation handling
            if(Mathf.Abs(horizontal) == 0)
            {
                animator.SetBool("IsMoving",false);
            }
            else
            {
                animator.SetBool("IsMoving",true);
            }

            // jumping
            HandleJumping();

            // dashing
            HandleDashing();


            // walljumping
            HandleWallJumping();

            // striking
            HandleStriking();

        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }



        // interacting
        if(Input.GetButtonDown("Interact") && !isInteracting && !isSliding)
        {
            isInteracting = true;
        }
        if(Input.GetButtonUp("Interact"))
        {
            isInteracting = false;
        }



        // animation updating
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetFloat("VertSpeed",rigidbody2d.velocity.y);


        if(isDashing)
        {
            animator.SetBool("IsDashing",true);
        }
        else
        {
            animator.SetBool("IsDashing",false);
        }

        if(playerScript.physicsChecker.isWalled)
        {
            animator.SetBool("IsSliding",true);
        }
        else
        {
            animator.SetBool("IsSliding",false);
        }

        if(playerScript.physicsChecker.isGrounded)
        {
            animator.SetBool("IsGrounded",true);
        }
        else
        {
            animator.SetBool("IsGrounded",false);
        }

        if(spawnTimeCounter > 0)
        {
            spawnTimeCounter -= Time.deltaTime;
            if(spawnTimeCounter <= 0)
            {
                isSpawning = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // changes direction of player
        if(horizontal > 0)
        {
             gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
             direction = 1;
        }
        if(horizontal < 0)
        {
            gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
            direction = -1;
        }

        if(!isDashing)
        {
            Move();
        }

        // determine if the player is sliding
        if(playerScript.physicsChecker.isWalled && !isWallJumping && !isDashing)
        {
            Slide();
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }


        // update coyote time
        if(playerScript.physicsChecker.isGrounded){
            timeSinceGrounded = 0.0f;
        } else{
            timeSinceGrounded += Time.fixedDeltaTime;
        }

        // jump if the player pressed jump and they can
        if(jumped && !isWallJumping)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            Jump();
        }
        if(jumped && isWallJumping)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            Jump();
        }
        if(isExtraJumping)
        {
            rigidbody2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Force);
        }
        if(isDashing)
        {
            Dash();
        }

        // striking
        if(isStriking)
        {
            strikeChecker.SetDirection(direction);
            strikeZone.SetActive(true);
        }
        else
        {
            strikeZone.SetActive(false);
        }

        // interacting
        if(isInteracting)
        {

            interactZone.SetActive(true);
        }
        else
        {
            interactZone.SetActive(false);
        }
    }


    void Move()
    {
        rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);
    }



    // JUMPING METHODS
    void HandleJumping(){
        if(Input.GetButtonDown("Jump") && (playerScript.physicsChecker.isGrounded || timeSinceGrounded < coyoteTime) && !isJumping){
            jumped = true;
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }
        if(Input.GetButtonDown("Jump") && playerScript.physicsChecker.isWalled && !playerScript.physicsChecker.isGrounded)
        {
            jumped = true;
            isJumping = true;
            isWallJumping = true;
            timeSinceGrounded = coyoteTime; 
            wallSide = direction * -1;
            wallJumpTimeCounter = wallJumpTime;
            jumpTimeCounter = jumpTime;
        }
        if(Input.GetButton("Jump") && isJumping)
        {
            if(jumpTimeCounter > 0)
            {
                isExtraJumping = true;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                isExtraJumping = false;
            }
        }
        if(Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            isExtraJumping = false;
        }
    }

    void Jump()
    {
       rigidbody2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
       jumped = false;
    }

    void Hang(){

    }


    // WALLJUMPING/WALLSLIDING METHODS
    void HandleWallJumping(){
        if(wallJumpTimeCounter > 0)
            {
                horizontal = wallSide;
                wallJumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isWallJumping = false;
            }
    }


    void Slide()
    {
        // apply downward friction if player is moving upwards while sliding
        if(rigidbody2d.velocity.y < -2.0f)
        {
            rigidbody2d.AddForce(new Vector2(0,slideForce));
        }
        else if(rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.AddForce(new Vector2(0,-slideForce * 0.5f));
        }
    }




    // DASHING METHODS
    void HandleDashing(){
        if(horizontal > 0.02 && !isDashing)
            {
                dashSide = 1;
            }
            else if(horizontal < -0.02 && !isDashing)
            {
                dashSide = -1;
            }
            if(isSliding && !isDashing)
            {
                dashSide = dashSide * -1;
            }
            if(Input.GetButtonDown("Dash") && !isDashing && dashReloadCounter < 0)
            {
                isDashing = true;
                isJumping = false;
                isWallJumping = false;
                dashTimeCounter = dashTime;
            }
            if(dashTimeCounter > 0)
            {
                dashTimeCounter -= Time.deltaTime;
                horizontal = dashSide;

                if(dashTimeCounter < 0)
                {
                    dashReloadCounter = dashReload;
                }
            }
            else
            {
                isDashing = false;
            }
            if(dashReloadCounter > 0)
            {
                dashReloadCounter -= Time.deltaTime;
            }
    }

    void Dash()
    {
        rigidbody2d.velocity = new Vector2(dashSpeed * dashSide, 0.477f);

    }

    // STRIKING/ATTACKING METHODS
    void HandleStriking(){
        if(Input.GetButtonDown("Strike") && !isStriking && strikeCooldownTimer <= 0)
            {
                isStriking = true;
                strikeTimer = strikeTime;
            }

            if(strikeTimer > 0)
            {
                strikeTimer -= Time.deltaTime;
                if(strikeTimer < 0)
                {
                    isStriking = false;
                    strikeCooldownTimer = strikeCooldown;
                }
            }
            if(strikeCooldownTimer > 0)
            {
                strikeCooldownTimer -= Time.deltaTime;
            }
    }



    // Conversation functions
    public void EnterConversation()
    {
        Debug.Log("Conversation started");

        isTalking = true;

    }

    public void ExitConversation()
    {
        Debug.Log("Conversation over");
        isTalking = false;
        animator.SetBool("IsTalking",false);
    }

    void HandleYN(){
        if(isYN){
                // LAST3 CODE:
                // 1 = right
                // 2 = left
                // -1 = up
                // -2 = down
                timeSinceLastDecision += Time.deltaTime;
                YNHorizontal = Input.GetAxisRaw("Horizontal");
                YNVertical = Input.GetAxisRaw("Vertical");

                if(YNHorizontal == 1 && !YNWasRight){
                    YNWasRight = true;
                    last3[2] = last3[1];
                    last3[1] = last3[0];
                    last3[0] = 1;
                    timeSinceLastDecision = 0.0f;
                }
                if(YNHorizontal != 1){
                    YNWasRight = false;
                }

                if(YNHorizontal == -1 && !YNWasLeft){
                    YNWasLeft = true;
                    last3[2] = last3[1];
                    last3[1] = last3[0];
                    last3[0] = 2;
                    timeSinceLastDecision = 0.0f;
                }
                if(YNHorizontal != -1){
                    YNWasLeft = false;
                }

                if(YNVertical == 1 && !YNWasUp){
                    YNWasUp = true;
                    last3[2] = last3[1];
                    last3[1] = last3[0];
                    last3[0] = -1;
                    timeSinceLastDecision = 0.0f;
                }
                if(YNVertical != 1){
                    YNWasUp = false;
                }

                if(YNVertical == -1 && !YNWasDown){
                    YNWasDown = true;
                    last3[2] = last3[1];
                    last3[1] = last3[0];
                    last3[0] = -2;
                    timeSinceLastDecision = 0.0f;
                }
                if(YNVertical != -1){
                    YNWasDown = false;
                }

                if(timeSinceLastDecision > decisionThreshold){
                    for(int i = 0; i < 3; i++){
                        last3[i] = 0;
                    }
                }

                if(last3[0] == 1 && last3[1] == 2 && last3[0] == 1){
                    isYN = false;
                    YNAnswer = false;
                }
                else if(last3[0] == 2 && last3[1] == 1 && last3[0] == 2){
                    isYN = false;
                    YNAnswer = false;
                }
                else if(last3[0] == -1 && last3[1] == -2 && last3[0] == -1){
                    isYN = false;
                    YNAnswer = true;
                }
                else if(last3[0] == -2 && last3[1] == -1 && last3[0] == -2){
                    isYN = false;
                    YNAnswer = true;
                }

            }
    }
}
