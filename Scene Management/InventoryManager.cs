using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory(){
        animator.SetBool("IsOpen",true);
    }

    public void CloseInventory(){
        animator.SetBool("IsOpen",false);
    }
}
