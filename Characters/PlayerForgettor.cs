using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForgettor : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerScript controller = other.GetComponent<PlayerScript>();

        if(controller != null)
        {
            //Debug.Log("Player forgotten");
        }
    }
}
