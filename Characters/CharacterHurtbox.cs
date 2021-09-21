using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour
{
    public int damage;
    public int direction;
    public float knockback;
    public float vertKnockback;
    public string damageType;
    public string damageSource;
    public CharacterHitbox self;

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterHitbox hitbox = other.GetComponent<CharacterHitbox>();
        if(hitbox != null && hitbox != self)
        {
            hitbox.Damage(damage,direction,knockback,vertKnockback,damageType,damageSource);
        }
    }
}
