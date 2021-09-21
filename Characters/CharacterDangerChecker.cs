using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDangerChecker : MonoBehaviour
{
    [System.NonSerialized]
    public bool dangerDetected;
    [System.NonSerialized]
    public int direction = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask layer = other.gameObject.layer;
        Vector3 otherPos = other.transform.position;
        if(layer.value == LayerMask.NameToLayer("Danger"))
        {
            dangerDetected = true;
            if(transform.position.x < otherPos.x)
            {
                direction = 1;
            }
            else if(transform.position.x > otherPos.x)
            {
                direction = -1;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        LayerMask layer = other.gameObject.layer;
        if(layer.value == LayerMask.NameToLayer("Danger"))
        {
            dangerDetected = false;
        }
    }
}
