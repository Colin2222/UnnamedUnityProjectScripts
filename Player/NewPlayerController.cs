using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerController : MonoBehaviour
{
    // jump and movement handling
    public float speed = 3.0f;
    public float jumpForce = 10f;
    bool jumped = false;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask groundLayer;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public float coyoteTime;
    // wall jumping
    public Transform sidePos;
    private bool isWalled;
    public LayerMask wallLayer;
    public float slideSpeed = 1.0f;
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


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        cam = GetComponent<Camera>();
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
            animator.SetFloat("Speed", Mathf.Abs(horizontal));
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
            if(Input.GetButtonDown("Jump") && isGrounded)
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
                    rigidbody2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Force);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            if(Input.GetButtonUp("Jump"))
            {
                isJumping = false;
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
                animator.SetBool("IsDashing",true);
                if(dashTimeCounter < 0)
                {
                    dashReloadCounter = dashReload;
                }
            }
            else
            {
                isDashing = false;
                animator.SetBool("IsDashing",false);
            }
            if(dashReloadCounter > 0)
            {
                dashReloadCounter -= Time.deltaTime;
            }

            // walljumping
            if(wallJumpTimeCounter > 0)
            {
                horizontal = 1.0f * wallSide;
                //Debug.Log(wallSide);
                wallJumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isWallJumping = false;
            }



            // striking
            if(Input.GetButtonDown("Strike") && !isStriking)
            {
                isStriking = true;
            }
            if(Input.GetButtonUp("Strike"))
            {
                isStriking = false;
            }
        }
        else
        {
            horizontal = 0;
            vertical = 0;
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
    }

    void FixedUpdate()
    {
        // changes direction of player
        if(horizontal > 0)
        {
             transform.eulerAngles = new Vector2(0,180);
        }
        if(horizontal < 0)
        {
            transform.eulerAngles = new Vector2(0,0);
        }
        if(!isDashing)
        {
            Move();
        }
        checkIfGrounded();
        checkIfWalled();
        if(isWalled && !isWallJumping && !isGrounded && !isDashing)
        {
            Slide();
            animator.SetBool("IsSliding",true);
            isSliding = true;
        }
        else
        {
            animator.SetBool("IsSliding",false);
            isSliding = false;
        }
        if(jumped && isGrounded)
        {
            Jump();
        }
        if(jumped && isWallJumping)
        {
            Jump();
        }
        if(isDashing)
        {
            Dash();
        }

        if(rigidbody2d.velocity.y > 2.0f)
        {
            rigidbody2d.AddForce(new Vector2(0,-4.0f));
        }

        // striking
        if(isStriking)
        {
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
        animator.SetFloat("VertSpeed",rigidbody2d.velocity.y);
        if(isGrounded)
        {
            animator.SetBool("IsGrounded",true);
        }
        else
        {
            animator.SetBool("IsGrounded",false);
        }
    }

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

    public void EnterConversation()
    {
        //Debug.Log("Conversation started");

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

    public void ChangeGems(int value)
    {
        gems += value;
        gemText.text = gems.ToString();
    }

    void Move()
    {
        rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);
    }

    void checkIfGrounded()
        {
            Vector2 groundedPositionA = new Vector2(feetPos.position.x - (feetPos.localScale.x / 2.0f), feetPos.position.y - (feetPos.localScale.y / 2.0f));
            Vector2 groundedPositionB = new Vector2(feetPos.position.x + (feetPos.localScale.x / 2.0f), feetPos.position.y - (feetPos.localScale.y / 2.0f) - checkRadius);
            Collider2D collider = Physics2D.OverlapArea(groundedPositionA, groundedPositionB, groundLayer);

            if(collider != null)
            {
                isGrounded = true;
                //lastTimeGrounded = Time.time;
            }
            else
            {
                isGrounded = false;
            }
        }

    void Jump()
    {
       rigidbody2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
       jumped = false;
    }


    void checkIfWalled()
    {
        Vector2 groundedPositionA = new Vector2(sidePos.position.x - (sidePos.localScale.x / 2.0f), sidePos.position.y - (sidePos.localScale.y / 2.0f));
        Vector2 groundedPositionB = new Vector2(sidePos.position.x + (sidePos.localScale.x / 2.0f), sidePos.position.y - (sidePos.localScale.y / 2.0f));
        Collider2D collider = Physics2D.OverlapArea(groundedPositionA, groundedPositionB, wallLayer);

        if(collider != null && horizontal != 0)
        {
            isWalled = true;
            if(horizontal > 0 && !isWallJumping)
            {
                // -1 is right, 1 is left
                wallSide = -1;
            }
            if(horizontal < 0 && !isWallJumping)
            {
                wallSide = 1;
            }
        }
        else
        {
            isWalled = false;
        }
    }

    void Slide()
    {
        // apply downward friction if player is moving upwards while sliding
        if(rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.AddForce(new Vector2(0,-slideForce * 0.1f));
        }
        // apply upward friction to slow the player while sliding downwards
        else
        {
            if(rigidbody2d.velocity.y < -2.0f)
            {
                rigidbody2d.AddForce(new Vector2(0,slideForce * 2.0f));
            }
            else if(rigidbody2d.velocity.y > 0.0f)
            {
                rigidbody2d.AddForce(new Vector2(0,slideForce * -2.0f));
            }
        }
    }

    void Dash()
    {
        rigidbody2d.velocity = new Vector2(dashSpeed * dashSide, 0);
    }
}