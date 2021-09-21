using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicsChecker : MonoBehaviour
{
    [System.NonSerialized]
    public bool isGrounded;
    [System.NonSerialized]
    public bool isWalled;
    [System.NonSerialized]
    public int wallSide;

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

        if(layer.value == LayerMask.NameToLayer("Ground") && transform.position.y > otherPos.y)
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
