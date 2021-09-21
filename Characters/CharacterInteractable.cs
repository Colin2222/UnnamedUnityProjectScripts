using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractable : MonoBehaviour
{
    public CharacterScript characterScript;

    public void Interact()
    {
        characterScript.interactions.Interact();
    }
}
