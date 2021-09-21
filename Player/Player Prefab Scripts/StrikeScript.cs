using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeScript : MonoBehaviour
{
    public int damage;
    public float knockback;
    public float vertKnockback;
    public string damageType;
    public string damageSource;

    [System.NonSerialized]
    public int direction = -1;

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterHitbox hitbox = other.GetComponent<CharacterHitbox>();
        if(hitbox != null)
        {
            hitbox.Damage(damage,direction,knockback,vertKnockback,damageType,damageSource);
        }
    }

    // functions used by player controller to determine which way the sword is being swung (for
    // knockback direction etc for the hit character
    public void SetDirection(int side)
    {
        direction = side;
    }

}
