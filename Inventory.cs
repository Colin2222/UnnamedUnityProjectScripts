using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.NonSerialized]
    public List<InventoryItem> items;
    [System.NonSerialized]
    public bool isFull;
    private int numItems = 0;

    public int size;

    public bool canHoldGems;
    public bool isPlayerInventory = false;
    public int numGems;

    private Text gemText;

    void Start()
    {
        items = new List<InventoryItem>();
        if(isPlayerInventory){
            gemText = GameObject.FindWithTag("UIGemBag").GetComponent<UIGemBag>().gemNumber;
        }
        // CHANGE THIS TO SYNC WITH PLAYERSTATE TO KEEP GEMS BETWEEN LEVELS
        changeGems(0);

        if(numItems == size){
            isFull = true;
        }
        else{
            isFull = false;
        }
    }

    public void changeGems(int num){
        numGems += num;
        gemText.text = numGems.ToString();
    }

    public void addItem(InventoryItem item){
        if(numItems == size){

        }
        else{
            numItems++;
            items.Add(item);
        }

        if(numItems == size){
            isFull = true;
        }
        else{
            isFull = false;
        }
    }

    public void findGemText(){
        gemText = GameObject.FindWithTag("UIGemBag").GetComponent<UIGemBag>().gemNumber;
    }
}
