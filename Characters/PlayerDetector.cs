using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public CharacterScript characterScript;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript controller = other.transform.root.gameObject.GetComponent<PlayerScript>();

        if(controller != null && controller.transform.root.gameObject != characterScript.target)
        {
            characterScript.UpdateTarget(controller.transform.root.gameObject);
        }
    }
}
