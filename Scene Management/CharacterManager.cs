using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private int numCharacter;

    // Start is called before the first frame update
    void Awake()
    {
        numCharacter = 0;
    }

    public int AddCharacter()
    {
        numCharacter += 1;
        return numCharacter;
    }

    public void RemoveCharacter()
    {
        numCharacter -= 1;
    }
}
