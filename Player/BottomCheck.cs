using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    public PlayerPhysicsChecker parentPhysics;

    void OnCollisionEnter2D(Collision2D collision){
        parentPhysics.isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision){
        parentPhysics.isGrounded = false;
    }
}
