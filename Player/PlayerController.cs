using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // MODULAR START
    public Inventory inventory;



    // MODULAR END



    // jump and movement handling
    public float speed = 3.0f;
    public float jumpForce = 10f;
    bool jumped = false;
    private bool isGrounded;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool isExtraJumping;
    public float coyoteTime;
    private int direction = -1;
    // wall jumping
    private bool isWalled;
    public LayerMask wallLayer;
    public float slideForce = 1.0f;
    private bool isSliding;
    private int wallSide;
    public float wallJumpTime = 0.5f;
    private float wallJumpTimeCounter;
    private bool isWallJumping;
    // dashing
    public float dashTime = 1.0f;
    private float dashTimeCounter;
    private bool isDashing = false;
    public float dashSpeed = 5.0f;
    private int dashSide;
    public float dashReload = 0.6f;
    private float dashReloadCounter = -1.0f;
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
    public GameObject interactZone;
    private bool conversationJustEnded = false;


    // animator handlings
    public Animator animator;

    // health handling
    public int maxHealth = 5;
    public float timeInvincible = 0.8f;
    public int health { get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;

    // gems handling
    public int gems = 0;
    public Text gemText;

    // camera handling
    Camera cam;

    // rigidbody handling
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    float talkingHorizontal;
    float talkingVertical;
    private Vector3 newPos;

    // spawning
    public float spawnTime;
    private float spawnTimeCounter;
    [System.NonSerialized]
    public bool isSpawning;
    [System.NonSerialized]
    public bool isSpawnCooling;
    private float spawnCoolCounter;
    [System.NonSerialized]
    public int spawnDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        strikeChecker = strikeZone.GetComponent<StrikeScript>();
        isSpawning = true;
        spawnTimeCounter = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTalking)
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
            //animator.SetFloat("VertSpeed", rigidbody2d.velocity.y);

            // jumping
            if(Input.GetButtonDown("Jump") && isGrounded && !isJumping)
            {
                jumped = true;
                isJumping = true;
                jumpTimeCounter = jumpTime;
            }
            if(Input.GetButtonDown("Jump") && isWalled && !isGrounded)
            {
                jumped = true;
                isJumping = true;
                isWallJumping = true;
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

            // dashing
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


            // walljumping
            if(wallJumpTimeCounter > 0)
            {
                horizontal = -1.0f * wallSide;
                //Debug.Log(wallSide);
                wallJumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isWallJumping = false;
            }



            // striking
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
        else
        {
            horizontal = 0;
            vertical = 0;
            talkingHorizontal = Input.GetAxisRaw("Horizontal");
            talkingVertical = Input.GetAxisRaw("Vertical");
        }


        // interacting
        if(Input.GetButtonDown("Interact") && !isInteracting)
        {
            isInteracting = true;
        }
        if(Input.GetButtonUp("Interact"))
        {
            isInteracting = false;
        }



        // health invincibility period after getting hit
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
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

        if(isSliding)
        {
            animator.SetBool("IsSliding",true);
        }
        else
        {
            animator.SetBool("IsSliding",false);
        }

        if(isGrounded)
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

    void FixedUpdate()
    {
        // changes direction of player
        if(horizontal > 0)
        {
             transform.eulerAngles = new Vector2(0,180);
             direction = 1;
        }
        if(horizontal < 0)
        {
            transform.eulerAngles = new Vector2(0,0);
            direction = -1;
        }

        if(!isDashing)
        {
            Move();
        }

        // determine if the player is sliding
        if(isWalled && !isWallJumping && !isGrounded && !isDashing)
        {
            Slide();
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }


        // jump if the player pressed jump and they can
        if(jumped && isGrounded)
        {
            Jump();
        }
        if(jumped && isWallJumping)
        {
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



        /*
        // terminal velocity handling
        if(rigidbody2d.velocity.y > 2.0f)
        {
            rigidbody2d.AddForce(new Vector2(0,-4.0f));
        }
        */


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


        // update animation if in air


    }




    //
    //
    //
    //
    // health functions
    public void ChangeHealth(int amount)
    {

        if (amount < 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            rigidbody2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;

        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void Damage(int damage,int side,float knockback,float vertKnockback)
    {

        if (isInvincible)
        {
            return;
        }
        isInvincible = true;
        invincibleTimer = timeInvincible;

        // immediate death physics reaction (TEMPLATE; CAN BE ALTERED)
        rigidbody2d.velocity = new Vector2(0, 0);
        rigidbody2d.AddForce(new Vector2(knockback * side,vertKnockback), ForceMode2D.Impulse);


        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);

        /*
        if(currentHealth == 0)
        {
            Vector3 dropPosition = transform.TransformPoint(Vector3.up * 0);
            if(drop != null)
            {
                Instantiate(drop,dropPosition,Quaternion.identity);
            }
            Destroy(gameObject);
        }
        */
    }




    //
    //
    //
    //
    // Conversation functions
    public void EnterConversation()
    {
        Debug.Log("Conversation started");

        if(!conversationJustEnded)
        {
            isTalking = true;
        }
        else
        {
            conversationJustEnded = false;
        }

    }

    public void ExitConversation()
    {
        Debug.Log("Conversation over");
        isTalking = false;
        conversationJustEnded = true;
    }




    //
    //
    //
    //
    // movement functions
    void Move()
    {
        rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);
    }

    void Jump()
    {
       rigidbody2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
       jumped = false;
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
            rigidbody2d.AddForce(new Vector2(0,-slideForce * .2f));
        }
    }

    void Dash()
    {
        rigidbody2d.velocity = new Vector2(dashSpeed * dashSide, 0);
    }




    //
    //
    //
    //
    // physics handlings to determine the state of the player (grounded, walled)
    void OnCollisionEnter2D(Collision2D collision)
    {
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;

        if(layer.value == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
        if(layer.value == LayerMask.NameToLayer("Wall"))
        {
            isWalled = true;
            if(transform.position.x > otherPos.x)
            {
                wallSide = 1;
            }
            if(transform.position.x < otherPos.x)
            {
                wallSide = -1;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;

        if(layer.value == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
        else if(layer.value == LayerMask.NameToLayer("Wall"))
        {
            isWalled = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        LayerMask layer = collision.gameObject.layer;
        //Vector3 otherPos = collision.gameObject.transform.position;

        if(layer.value == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
        else if(layer.value == LayerMask.NameToLayer("Wall"))
        {
            isWalled = false;
        }
    }
}
