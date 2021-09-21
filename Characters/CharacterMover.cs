using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public CharacterScript characterScript;

    Animator animator;

    // behavior vars
    public float speed;
    [System.NonSerialized]
    public int direction = 1;

    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = characterScript.gameObject.GetComponent<Rigidbody2D>();
        animator = characterScript.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // change direction if player is alive and running into wall
        if(characterScript.physicsChecker.isWalled && !(characterScript.characterHealth.isDying))
        {
            if(direction == characterScript.physicsChecker.wallSide)
            {
                changeDirection();
            }
            characterScript.physicsChecker.isWalled = false;
        }

        // chnage direction if player is alive and detects danger/hazard
        if(characterScript.dangerChecker.dangerDetected && direction == characterScript.dangerChecker.direction && !(characterScript.characterHealth.isDying))
        {
            changeDirection();
        }

        // move the player forward if alive and touching ground
        if(characterScript.physicsChecker.isGrounded)
        {
            if(!(characterScript.characterHealth.isDying))
            {
                Move();
            }
            animator.SetBool("IsGrounded",true);
        }
        else
        {
            animator.SetBool("IsGrounded",false);
        }

        // change the animator if the player is dying
        if(characterScript.characterHealth.isDying)
        {
            animator.SetBool("IsDying",true);
        }


    }

    void Move()
    {
        rigidbody2d.velocity = new Vector2(direction * speed, rigidbody2d.velocity.y);
    }

    public void changeDirection()
    {
        direction = direction * -1;
        Vector3 newScale = characterScript.transform.localScale;
        newScale.x *= -1;
        characterScript.transform.localScale = newScale;
    }
}
