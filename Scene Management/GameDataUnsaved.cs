using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataUnsaved : MonoBehaviour
{
    public Dictionary<string,bool> gameData = new Dictionary<string,bool>(){
        {"Main_FirstQuest_Started",false},
        {"Main_FirstQuest_Completed",false},
        {"test1",true}
    };
}
