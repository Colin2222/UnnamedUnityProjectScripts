using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public PlayerPhysicsChecker parentPhysics;

    void OnCollisionEnter2D(Collision2D collision){
        parentPhysics.isWalled = true;
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isWalled = true;
    }

    void OnCollisionExit2D(Collision2D collision){
        parentPhysics.isWalled = false;
    }
}
