using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsChecker : MonoBehaviour
{
    public PlayerScript playerScript;

    [System.NonSerialized]
    public bool isGrounded = false;
    [System.NonSerialized]
    public bool topTouch = false;
    [System.NonSerialized]
    public bool isWalled = false;
    [System.NonSerialized]
    public bool backTouch = false;

    void OnCollisionEnter2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;
    }

    void OnCollisionStay2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;
    }

    void OnCollisionExit2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;
    }
}
