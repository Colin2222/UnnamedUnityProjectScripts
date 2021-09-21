using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSaved : MonoBehaviour
{
    [System.NonSerialized]
    public Dictionary<string,bool> gameData = new Dictionary<string,bool>();
}
