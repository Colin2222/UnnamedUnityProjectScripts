using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScript : MonoBehaviour
{
    public int damage;
    public int direction;
    public float knockback;
    public float vertKnockback;
    private string damageType = "Hazard";
    public string damageSource;

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterHitbox hitbox = other.GetComponent<CharacterHitbox>();
        if(hitbox != null)
        {
            hitbox.Damage(damage,direction,knockback,vertKnockback,damageType,damageSource);
        }
    }
}
