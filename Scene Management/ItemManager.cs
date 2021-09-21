using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    int numItems;
    // Start is called before the first frame update
    void Start()
    {
        numItems = 0;
    }

    public int AddItem()
    {
        numItems += 1;
        return numItems;
    }

    public void RemoveItem()
    {
        numItems -= 1;
    }
}
