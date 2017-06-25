using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IItem, IObjectHint {

    public string itemName;
    public string itemHint;
    public Sprite itemSprite;

    private GameObject mHintObject;

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

    public string GetHintName()
    {
        return itemName;
    }

    public GameObject ItemObject()
    {
        return mHintObject;
    }

    public void SetHintObject(GameObject hintObject)
    {
        mHintObject = hintObject;
    }

    public bool HasActiveHintObject()
    {
        return mHintObject != null;
    }

    public bool DisplayHint()
    {
        return true;
    }

    public HintType GetHintType()
    {
        return HintType.TOOL;
    }
}
