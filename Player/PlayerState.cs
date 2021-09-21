using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int health;
    public int entranceNumber;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
