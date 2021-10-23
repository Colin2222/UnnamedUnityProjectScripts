using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public CharacterHitbox characterHitbox;
    public CharacterHurtbox characterHurtbox;
    public CharacterMover characterMover;
    public CharacterPhysicsChecker physicsChecker;
    public CharacterHealth characterHealth;
    public CharacterDangerChecker dangerChecker;
    public CharacterInteractions interactions;
    public Inventory inventory;
    public float hurtTime;
    [System.NonSerialized]
    public float hurtTimer = 0.0f;
    [System.NonSerialized]
    public bool isHurt = false;

    [System.NonSerialized]
    public SceneController sceneManager;
    [System.NonSerialized]
    public GameObject target;
    [System.NonSerialized]
    public bool targetUpdated = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();
        GetComponent<SpriteRenderer>().sortingOrder = sceneManager.characterManager.AddCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        if(hurtTimer > 0.0){
            hurtTimer -= Time.deltaTime;
            if(hurtTimer <= 0){
                isHurt = false;
            }
        }
    }

    void FixedUpdate()
    {

    }

    public void UpdateTarget(GameObject newTarget)
    {
        target = newTarget;
        targetUpdated = true;
    }

    public void setHurt()
    {
        isHurt = true;
        hurtTimer = hurtTime;
    }

}
