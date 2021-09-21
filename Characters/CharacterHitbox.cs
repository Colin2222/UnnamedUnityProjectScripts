using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    public CharacterScript characterScript;
    public PlayerScript playerScript;

    public void Damage(int value, int direction, float knockback, float vertKnockback, string damageType, string damageSource)
    {
        if(characterScript != null){
            characterScript.characterHealth.Damage(value,direction,knockback,vertKnockback,damageType, damageSource);
        }
        if(playerScript != null){
            playerScript.playerHealth.Damage(value,direction,knockback,vertKnockback,damageType, damageSource);
        }
    }
}
