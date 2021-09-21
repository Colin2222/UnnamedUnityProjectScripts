using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameDataSaved saved;
    public GameDataUnsaved current;

    [System.NonSerialized]
    public int numData;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SaveGame()
    {
        foreach(KeyValuePair<string,bool> data in current.gameData)
        {
            saved.gameData[data.Key] = data.Value;
        }
    }

    void LoadGame()
    {
        foreach(KeyValuePair<string,bool> data in saved.gameData)
        {
            current.gameData[data.Key] = data.Value;
        }
    }

    public bool GetData(string key)
    {
        return current.gameData[key];
    }
}
