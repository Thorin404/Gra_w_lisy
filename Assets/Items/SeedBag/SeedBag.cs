using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour, IItem, IObjectHint
{

    public GameObject seedPrefab;
    public int spawnCount;

    public string itemName;
    public string itemHint;
    public Sprite itemSprite;

    private int mLeftToSpawn;
    private bool mActive;

    private GameObject mHintObject;

    // Use this for initialization
    void Start()
    {
        mLeftToSpawn = spawnCount;
        mActive = true;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public bool DropSeeds()
    {
        if (mActive)
        {
            --mLeftToSpawn;
            Debug.Log("Drop seeds");
            Instantiate(seedPrefab, transform.position + transform.forward * 0.5f, transform.rotation);

            if (mLeftToSpawn == 0)
            {
                //controller.HoldedItem = null;
                itemName += " (empty) ";
                itemHint = "Hold action to drop...";
                mActive = false;
                return true;
            }
        }

        return mActive;
    }

    public bool ItemAction(ItemController controller)
    {
        return DropSeeds();
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
        return itemHint + (mActive ? ("(" + mLeftToSpawn + "/" + spawnCount + ")") : "");
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
        return mActive;
    }

    public HintType GetHintType()
    {
        return HintType.TOOL;
    }
}
