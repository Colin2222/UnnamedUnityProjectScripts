using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterInteractable controller = other.GetComponent<CharacterInteractable>();

        if(controller != null)
        {
            controller.Interact();
        }
    }
}
