using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject menuCanvas;

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetButtonDown("Menu")){
            if(gamePaused){
                Resume();
            } else{
                Pause();
            }
        }
        */
    }

    private void Resume(){
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    private void Pause(){
        menuCanvas.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }
}
