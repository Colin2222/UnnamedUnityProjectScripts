using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobScript : MonoBehaviour
{
    // NOT FUNCTIONAL ANYMORE, CHANGE TO BE A MODULAR MOVER

    // behavior vars
    public float speed;
    private int direction = 1;
    private bool isGrounded;
    private bool isWalled;
    private int wallSide = 0;

    // health
    public int maxHealth = 5;
    public float timeInvincible = 0.8f;
    public int health { get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    public float damagedJumpForce;
    public GameObject drop;

    // rigidbody
    Rigidbody2D rigidbody2d;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }


    void FixedUpdate()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if(isWalled)
        {
            if(direction == wallSide)
            {
                changeDirection();
            }
            isWalled = false;
        }
        if(isGrounded)
        {
            Move();
        }
    }
    /*
    void checkIfGrounded()
    {
        Vector2 groundedPositionA = new Vector2(feetPos.position.x - (feetPos.localScale.x / 2.0f), feetPos.position.y - (feetPos.localScale.y / 2.0f));
        Vector2 groundedPositionB = new Vector2(feetPos.position.x + (feetPos.localScale.x / 2.0f), feetPos.position.y - (feetPos.localScale.y / 2.0f) - checkRadius);
        Collider2D collider = Physics2D.OverlapArea(groundedPositionA, groundedPositionB, groundLayer);

        if(collider != null)
        {
            isGrounded = true;
        }
        else
        {
           isGrounded = false;
        }
    }

    void checkIfWalled()
    {
        Vector2 groundedPositionA = new Vector2(sidePos.position.x - (sidePos.localScale.x / 2.0f), sidePos.position.y - (sidePos.localScale.y / 2.0f));
        Vector2 groundedPositionB = new Vector2(sidePos.position.x + (sidePos.localScale.x / 2.0f), sidePos.position.y - (sidePos.localScale.y / 2.0f));
        Collider2D collider = Physics2D.OverlapArea(groundedPositionA, groundedPositionB, wallLayer);

        if(collider != null)
        {
            changeDirection();
        }
    }
    */


    void Move()
    {
        rigidbody2d.velocity = new Vector2(direction * speed, rigidbody2d.velocity.y);
    }

    public void changeDirection()
    {
        direction = direction * -1;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    public void ChangeHealth(int amount, int side)
    {
        if (amount < 0)
        {

            if (isInvincible)
            {
                return;
            }
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            rigidbody2d.AddForce(new Vector2(0,damagedJumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            isInvincible = true;
            invincibleTimer = timeInvincible;

            if(direction != side)
            {
                changeDirection();
            }
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);

        if(currentHealth == 0)
        {
            Vector3 dropPosition = transform.TransformPoint(Vector3.up * 0);
            Instantiate(drop,dropPosition,Quaternion.identity);

            Destroy(gameObject);
        }
        //Debug.Log("BLOB DAMAGED");
    }

    // can be improved so blobs change direction when hitting each other/ dont collide at all (not colliding would require rework of physics)
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        PlayerController controller = other.GetComponent<PlayerController>();

        if(controller != null)
        {
            controller.ChangeHealth(-1);
        }
        */

    }
    //
    //
    //
    //
    // physics handlings to determine the state of the blob (grounded, walled)
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
