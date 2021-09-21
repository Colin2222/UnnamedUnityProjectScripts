using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        BlobScript blobController = other.GetComponent<BlobScript>();

        if(controller != null)
        {
            controller.ChangeHealth(-1);
        }
        if(blobController != null)
        {
            blobController.changeDirection();
        }
    }
}
