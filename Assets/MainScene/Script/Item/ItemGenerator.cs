using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject Item;
    public void Generate()
    {
        if(Item) Item.SetActive(true);
    }

    public void DestroyI()
    {
        if (Item) GameObject.Destroy(Item);
    }

    public bool check()
    {
        if (Item) return Item.activeSelf;
        else return false;
    }
}
