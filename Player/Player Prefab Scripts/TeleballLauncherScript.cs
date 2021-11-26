using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleballLauncherScript : MonoBehaviour
{
    public GameObject teleball;
    private GameObject current;
    public PlayerScript player;
    public Rigidbody2D playerBody;

    public float xOffset;
    public float yOffset;
    public float defaultXDirection;
    public float defaultYDirection;
    public float launchSpeed;
    public float teleballLifetime;
    private float lifetimeCounter;
    private bool ballExists = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lifetimeCounter > 0){
            lifetimeCounter -= Time.deltaTime;
            if(lifetimeCounter <= 0){
                Destroy(current);
                ballExists = false;
            }
        }
    }

    public void launchTeleball(float horizontal, float vertical)
    {
        if(!ballExists){
            Vector3 pos = player.getPos();
            Vector3 ballPos = new Vector3(pos.x, pos.y + yOffset, 0);

            if(horizontal < 0){
                ballPos = new Vector3(ballPos.x - xOffset, ballPos.y, 0);
            } else if(horizontal > 0){
                ballPos = new Vector3(ballPos.x + xOffset, ballPos.y, 0);
            } else{
                if(player.characterMover.direction < 0){
                    ballPos = new Vector3(ballPos.x - xOffset, ballPos.y, 0);
                } else {
                    ballPos = new Vector3(ballPos.x + xOffset, ballPos.y, 0);
                }
            }

            //Debug.Log(horizontal + ", " + vertical);

            Vector3 dir = new Vector3(horizontal, vertical, 0);
            Vector3 result = dir * launchSpeed;
            result = new Vector3(result.x + playerBody.velocity.x, result.y, 0);


            current = Instantiate(teleball, ballPos, Quaternion.identity);
            current.GetComponent<Rigidbody2D>().velocity = result;

            ballExists = true;
            lifetimeCounter = teleballLifetime;
        }
    }

    public void teleportToBall(){
        if(ballExists){
            player.rigidbody2d.position = current.GetComponent<Rigidbody2D>().position;
            player.rigidbody2d.velocity = new Vector3(player.rigidbody2d.velocity.x, current.GetComponent<Rigidbody2D>().velocity.y, 0);
            Destroy(current);
            ballExists = false;
        }
    }
}
