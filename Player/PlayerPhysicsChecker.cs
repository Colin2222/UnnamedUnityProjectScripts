using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsChecker : MonoBehaviour
{
    [System.NonSerialized]
    public bool isGrounded = false;
    [System.NonSerialized]
    public bool topTouch = false;
    [System.NonSerialized]
    public bool isWalled = false;
    [System.NonSerialized]
    public bool backTouch = false;

    [System.NonSerialized]
    public bool touching;

    void OnCollisionEnter2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;

        if(layer.value == LayerMask.NameToLayer("Physical"))
        {
            touching = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;

        if(layer.value == LayerMask.NameToLayer("Ground"))
        {
            touching = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;

        if(layer.value == LayerMask.NameToLayer("Ground"))
        {
            touching = false;
        }
    }
}
