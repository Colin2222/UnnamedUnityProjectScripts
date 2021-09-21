using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionScript : MonoBehaviour
{
    public int buildIndex = 0;

    public int entranceNumber = 0;

    public int entranceDirection = 0;

    public Transform spawnPosRight;
    public Transform spawnPosLeft;

    public SceneController sceneManager;

    void Awake()
    {
        if(sceneManager.playerState.entranceNumber == entranceNumber)
        {
            sceneManager.spawnPosRight = spawnPosRight.position;
            sceneManager.spawnPosLeft = spawnPosLeft.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript player = other.transform.parent.GetComponent<PlayerScript>();
        sceneManager.playerState.entranceNumber = entranceNumber;
        if(player != null && !(player.isSpawning))
        {

            StartCoroutine(sceneManager.SwitchScenes(buildIndex,entranceNumber,entranceDirection));

        }
    }
}
