using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IItem {

    public string itemName;
    public string itemHint;
    public Sprite itemSprite;

    // Use this for initialization
    void Start () {
		
	}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public bool ItemAction(ItemController c)
    {
        return false;
    }

    public Sprite GetSprite()
    {
        return itemSprite;
    }

    public string GetName()
    {
        return itemName;
    }

    public string GetHint()
    {
        return itemHint;
    }
}
