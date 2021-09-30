using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public CharacterScript characterScript;

    private SceneController sceneManager;

    Rigidbody2D rigidbody2d;

    public int maxHealth = 6;
    public float timeInvincible = 0.8f;
    public int health { get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    public bool damageable;
    [System.NonSerialized]
    public bool isDying = false;
    public float deathTime;
    private float deathTimeCounter;
    private string deathDamageType;
    public GameObject drop;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = characterScript.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        if(deathTimeCounter > 0)
        {
            deathTimeCounter -= Time.deltaTime;
            if(deathTimeCounter <= 0)
            {
                Die(deathDamageType);
            }
        }
    }

    public void Damage(int damage,int side,float knockback,float vertKnockback,string damageType, string damageSource)
    {

        if (isInvincible || !damageable || damageSource.Equals("creature"))
        {
            if(damageType != "Hazard")
            {
                return;
            }
        }
        isInvincible = true;
        invincibleTimer = timeInvincible;

        // immediate death physics reaction (TEMPLATE; CAN BE ALTERED)
        rigidbody2d.velocity = new Vector2(0, 0);
        rigidbody2d.AddForce(new Vector2(knockback * side,vertKnockback), ForceMode2D.Impulse);
        /*
        if(characterScript.characterMover.direction != side)
        {
            characterScript.characterMover.changeDirection();
        }
        */


        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth);

        if(currentHealth == 0 && !isDying)
        {
            /*
            if(drop != null)
            {
                sceneManager.itemManager.Spawn();
                //Instantiate(drop,dropPosition,Quaternion.identity);
            }
            */
            deathDamageType = damageType;
            isDying = true;
            deathTimeCounter = deathTime;
        }
    }

    public void Die(string damageType)
    {
        characterScript.sceneManager.characterManager.RemoveCharacter();
        if(drop != null)
        {
            Instantiate(drop,transform.position,Quaternion.identity);
        }
        Destroy(characterScript.gameObject);
    }
}
