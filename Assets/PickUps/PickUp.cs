using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IObjectHint {
    
    //pickup value
    public int scoreValue;
    //time bonus float [s] 
    public float timeValue;
    //Name to display
    public string nameToDisplay;
    //Check if item is key
    public bool pickupIsKey;

    //Hint
    private GameObject mHintPointer;

    public string GetHintName()
    {
        return nameToDisplay;
    }

    public GameObject ItemObject()
    {
        return gameObject;
    }

    public bool HasActiveHintObject()
    {
        return mHintPointer != null;
    }

    public void SetHintObject(GameObject hintObject)
    {
        mHintPointer = hintObject;
    }

    public bool DisplayHint()
    {
        return true;
    }

    public HintType GetHintType()
    {
        return pickupIsKey ? HintType.OBJECTIVE : HintType.FOOD; 
    }
}
