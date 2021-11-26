using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private float horizontal = 0;
    private float vertical = 0;
    private float lastHorizontal = 0;
    private float lastVertical = 0;
    public int direction = 1;

    private float conversationHorizontal;
    private float conversationVertical;

    // aiming
    public float aimHorizontal = 0;
    public float aimVertical = 0;
    public float lastAimHorizontal = 0;
    public float lastAimVertical = 0;



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
    public float jumpForgivenessTime;
    [System.NonSerialized]
    public float timeSincePressed;
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

    // teleball throwing
    private bool isTeleballThrowing = false;
    public float teleballThrowCooldown;
    private float teleballThrowCooldownTimer;

    // teleball teleporting
    private bool isTeleporting = false;


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

    // button pressing
    private bool jumpPressed = false;
    private bool jumpJustPressed = false;
    private bool dashPressed = false;
    private bool dashJustPressed = false;
    private bool interactPressed = false;
    private bool interactJustPressed = false;
    private bool strikePressed = false;
    private bool strikeJustPressed = false;
    private bool throwPressed = false;
    private bool throwJustPressed = false;
    private bool teleportPressed = false;
    private bool teleportJustPressed = false;



    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isSpawning = true;
        spawnTimeCounter = spawnTime;
        rigidbody2d = gameObject.transform.parent.GetComponent<Rigidbody2D>();
        strikeChecker = strikeZone.GetComponent<StrikeScript>();

        Debug.Log(string.Join("\n", Gamepad.all));
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
            // handle button pressing

            //horizontal = gamepad.leftStick.x.ReadValue();
            //vertical = gamepad.leftStick.y.ReadValue();

            /*
            if(!isWallJumping && !isDashing){
                horizontal = Input.GetAxisRaw("Horizontal");
            }
            vertical = Input.GetAxis("Vertical");
            */

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

            // teleball throwing
            HandleTeleballThrowing();

            // teleporting to teleball
            HandleTeleporting();

        }
        else
        {
            //horizontal = 0;
            //vertical = 0;
        }



        // interacting
        if(interactPressed && !isInteracting && !isSliding)
        {
            isInteracting = true;
        }
        if(!interactPressed)
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


        // update coyote time and jump forgiveness time
        if(playerScript.physicsChecker.isGrounded){
            timeSinceGrounded = 0.0f;
        } else{
            timeSinceGrounded += Time.fixedDeltaTime;
        }
        if(timeSincePressed < jumpForgivenessTime){
            timeSincePressed += Time.fixedDeltaTime;
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
        // basic jump off ground (including coyote time)
        if((jumpJustPressed || timeSincePressed < jumpForgivenessTime) &&
        (playerScript.physicsChecker.isGrounded || (timeSinceGrounded < coyoteTime && !playerScript.physicsChecker.isWalled) && !isJumping)){
            jumped = true;
            isJumping = true;
            jumpTimeCounter = jumpTime;
            jumpJustPressed = false;
            /*
            Debug.Log("GROUND JUMP");
            Debug.Log("jump just pressed: " + jumpJustPressed);
            Debug.Log("is grounded: " + playerScript.physicsChecker.isGrounded);
            Debug.Log("is walled: " + playerScript.physicsChecker.isWalled);
            */

        }
        // frame perfect forgiveness jump
        else if(jumpJustPressed && !playerScript.physicsChecker.isGrounded && !playerScript.physicsChecker.isWalled){
            timeSincePressed = 0;
            jumpJustPressed = false;
        }
        else if((jumpJustPressed || timeSincePressed < jumpForgivenessTime) && playerScript.physicsChecker.isWalled && !playerScript.physicsChecker.isGrounded)
        {
            jumped = true;
            isJumping = true;
            isWallJumping = true;
            timeSinceGrounded = coyoteTime;
            wallSide = direction * -1;
            wallJumpTimeCounter = wallJumpTime;
            jumpTimeCounter = jumpTime;
            jumpJustPressed = false;
        }
        if(jumpPressed && isJumping)
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
        else if(!jumpPressed)
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
                if(wallJumpTimeCounter < 0){
                    isWallJumping = false;
                    horizontal = lastHorizontal;
                    vertical = lastVertical; 
                }
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
            if(dashJustPressed && !isDashing && dashReloadCounter < 0)
            {
                isDashing = true;
                isJumping = false;
                isWallJumping = false;
                dashTimeCounter = dashTime;
                dashJustPressed = false;
            }
            if(dashTimeCounter > 0)
            {
                dashTimeCounter -= Time.deltaTime;
                horizontal = dashSide;

                if(dashTimeCounter < 0)
                {
                    dashReloadCounter = dashReload;
                    horizontal = lastHorizontal;
                    vertical = lastVertical;
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
        if(strikeJustPressed && !isStriking && strikeCooldownTimer <= 0)
            {
                Debug.Log("STRIKING");
                isStriking = true;
                strikeTimer = strikeTime;
                strikeJustPressed = false;
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

    // TELEBALL THROWING METHODS
    void HandleTeleballThrowing(){
        if(throwJustPressed && !isTeleballThrowing && teleballThrowCooldownTimer <= 0){
            isTeleballThrowing = true;
            teleballThrowCooldownTimer = teleballThrowCooldown;
            throwJustPressed = false;
            playerScript.launcher.launchTeleball(aimHorizontal, aimVertical);

        }
        else if(teleballThrowCooldownTimer > 0){
            teleballThrowCooldownTimer -= Time.deltaTime;
            if(teleballThrowCooldownTimer <= 0){
                    isTeleballThrowing = false;
            }
        }
    }

    void HandleTeleporting(){
        if(teleportJustPressed){
            playerScript.launcher.teleportToBall();
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

    // controller button methods
    private void OnMove(InputValue value){
        Vector2 vector = value.Get<Vector2>();

        horizontal = vector.x;
        vertical = vector.y;
        YNHorizontal = vector.x;
        YNVertical = vector.y;

        lastHorizontal = horizontal;
        lastVertical = vertical;
    }

    private void OnAim(InputValue value){
        Vector2 vector = value.Get<Vector2>();

        aimHorizontal = vector.x;
        aimVertical = vector.y;

        lastAimHorizontal = aimHorizontal;
        lastAimVertical = aimVertical;
    }

    private void OnJump(){
        jumpPressed = !jumpPressed;
        jumpJustPressed = jumpPressed;
    }

    private void OnDash(){
        dashPressed = !dashPressed;
        dashJustPressed = dashPressed;
    }

    private void OnInteract(){
        interactPressed = !interactPressed;
        interactJustPressed = interactPressed;
    }

    private void OnStrike(){
        strikePressed = !strikePressed;
        strikeJustPressed = strikePressed;
    }

    private void OnThrow(){
        throwPressed = !throwPressed;
        throwJustPressed = throwPressed;
    }

    private void OnTeleport(){
        teleportPressed = !teleportPressed;
        teleportJustPressed = teleportPressed; 
    }
}
