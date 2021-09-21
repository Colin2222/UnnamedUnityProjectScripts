using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGuardMover : MonoBehaviour
{
    public CharacterScript characterScript;

    [System.NonSerialized]
    public bool isHit = false;
    [System.NonSerialized]
    public bool isLunging = false;
    [System.NonSerialized]
    public bool isWinding = false;
    [System.NonSerialized]
    public bool isWaiting = true;
    [System.NonSerialized]
    public bool isCooling = false;
    [System.NonSerialized]
    public bool isAware = false;

    public float lungeSpeed;
    public float lungeTime;
    public float windupTime;
    public float cooldownTime;

    private float lungeTimer;
    private float windupTimer;
    private float cooldownTimer;


    [System.NonSerialized]
    public GameObject target;
    private float targetDistance = 0.0f;

    [System.NonSerialized]
    public int direction = -1;

    Rigidbody2D rigidbody2d;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = characterScript.gameObject.GetComponent<Rigidbody2D>();
        animator = characterScript.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // update target if it has changed (detected from the player detector)
        if(characterScript.targetUpdated){
            characterScript.targetUpdated = false;
            target = characterScript.target;
        }

        // update distance between this and target
        if(target != null){
            targetDistance = Vector3.Distance(transform.root.position, target.transform.root.position);
        }

        // update to face the target if not currently hit, lunging, or winding up for lunge
        if(!isHit && !isLunging && !isWinding && target != null){
            if((direction == 1 && transform.root.position.x > target.transform.root.position.x) || (direction == -1 && transform.root.position.x < target.transform.root.position.x)){
                changeDirection();
            }
        }

        // handle cooldown
        if(isCooling){
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0){
                isCooling = false;
                isWaiting = true;
            }
        }

        // initiate winding up for lunge if waiting and target is present
        if(isWaiting && target != null){
            isWaiting = false;
            isWinding = true;
            windupTimer = windupTime;
            animator.SetBool("IsWinding", true);
            animator.SetBool("IsStanding", false);
        }

        // handle winding up
        if(isWinding){
            windupTimer -= Time.deltaTime;
            if(windupTimer <= 0){
                isWinding = false;
                isLunging = true;
                lungeTimer = lungeTime;
                animator.SetBool("IsLunging", true);
                animator.SetBool("IsWinding", false);

                // rotate character for the lunge
                characterScript.characterHitbox.transform.rotation = Quaternion.Euler(0,0,90);
                characterScript.characterHurtbox.transform.rotation = Quaternion.Euler(0,0,90);
            }
        }

        if(isLunging){
            lungeTimer -= Time.deltaTime;
            if(lungeTimer <= 0){
                isLunging = false;
                isCooling = true;
                cooldownTimer = cooldownTime;
                animator.SetBool("IsLunging", false);
                animator.SetBool("IsStanding", true);

                // unrotate character once the lunge is completed
                characterScript.characterHitbox.transform.rotation = Quaternion.Euler(0,0,0);
                characterScript.characterHurtbox.transform.rotation = Quaternion.Euler(0,0,0);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isLunging){
            rigidbody2d.velocity = new Vector2(direction * lungeSpeed, rigidbody2d.velocity.y);
        }
    }

    public void changeDirection()
    {
        direction = direction * -1;
        Vector3 newScale = characterScript.transform.localScale;
        newScale.x *= -1;
        characterScript.transform.localScale = newScale;
    }
}
