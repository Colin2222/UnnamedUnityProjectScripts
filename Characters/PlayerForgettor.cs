using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForgettor : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if(controller != null)
        {
            //Debug.Log("Player forgotten");
        }
    }
}
