using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public ItemManager itemManager;
    public CharacterManager characterManager;
    public DialogueManager dialogueManager;
    public InventoryManager inventoryManager;

    public float transitionTime;

    public string sceneName;

    public GameObject playerPrefab;

    [System.NonSerialized]
    public PlayerState playerState;
    public GameObject playerStateObject;
    GameObject playerStateObjectTest;
    GameObject playerObjectTest;
    [System.NonSerialized]
    public PlayerScript player;

    public GameObject dataManagerPrefab;
    [System.NonSerialized]
    public DataManager dataManager;
    GameObject dataManagerTest;

    [System.NonSerialized]
    public Vector3 spawnPosRight;
    [System.NonSerialized]
    public Vector3 spawnPosLeft;


    void Awake()
    {
        dataManagerTest = GameObject.FindWithTag("DataManager");
        if(dataManagerTest == null){
            dataManager = Instantiate(dataManagerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<DataManager>();
        }
        else{
            dataManager = dataManagerTest.GetComponent<DataManager>();
        }

        playerStateObjectTest = GameObject.FindWithTag("PlayerState");
        if(playerStateObjectTest == null)
        {
            playerState = Instantiate(playerStateObject,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerState>();
        }
        else
        {
            playerState = playerStateObjectTest.GetComponent<PlayerState>();
        }

        playerObjectTest = GameObject.FindWithTag("PlayerTag");
        if(playerObjectTest == null){
            //player = Instantiate(playerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerScript>();
        }
        else{
            //player = playerObjectTest.GetComponent<PlayerScript>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SCENE STARTING");
        player.transform.position = spawnPosRight;
        player.inventory.findGemText();
        player.inventory.changeGems(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SwitchScenes(int buildIndex, int entranceNumber, int directionNumber)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(buildIndex);

    }
}
