using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    // NOT FUNCTIONAL ANYMORE
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript controller = other.GetComponent<PlayerScript>();
        BlobScript blobController = other.GetComponent<BlobScript>();

        if(controller != null)
        {
            //controller.ChangeHealth(-1);
        }
        if(blobController != null)
        {
            blobController.changeDirection();
        }
    }
}
