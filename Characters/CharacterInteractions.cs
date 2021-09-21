using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractions : MonoBehaviour
{
    [System.NonSerialized]
    public bool talking = false;
    private bool silent = false;

    public Dialogue dialogue;
    Dialogue currentDialogue;

    public CharacterScript characterScript;
    [System.NonSerialized]
    public DataManager gameData;
    [System.NonSerialized]
    public DialogueManager manager;

    void Start()
    {
        gameData = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        manager = FindObjectOfType<DialogueManager>();
    }

    void Update(){
        if(!(manager.isSilence) && silent){
            Interact();
        }

        if(manager.isSilence){
            silent = true;
        }
        else{
            silent = false;
        }
    }

    public void Interact()
    {
        if(!manager.isTalking)
        {
            if(dialogue != null){
                talking = manager.StartDialogue(dialogue);
            }
        }
        else
        {

            if(!manager.isSilence){
                talking = manager.HandleNextElement();
            }

        }
    }
}
