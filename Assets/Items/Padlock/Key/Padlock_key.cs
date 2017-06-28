using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock_key : MonoBehaviour, IItem, IObjectHint
{
    public string itemName;
    public string itemHint;
    public Sprite itemSprite;

    //public GameObject padlock;
    private PadlockController padlockController;

    public string padlockTag;

    private GameObject mHintObject;

    private bool mUsed;
    private bool mActive;

    void Start()
    {
        mActive = false;
        mUsed = false;
       // Debug.Assert(padlock != null);
    }

    public bool ItemAction(ItemController c)
    {
        ///throw new NotImplementedException();
        if (mActive && !mUsed && padlockController != null)
        {
            ItemController.HoldedItem = null;
            Debug.Assert(padlockController != null);
            padlockController.Unlock(this.gameObject);
            mUsed = true;
        }
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

    void ActivateKey(Collider other, bool active)
    {
        if (other.gameObject.tag.Contains(padlockTag) && !mUsed)
        {
            padlockController = other.gameObject.GetComponent<PadlockController>();
            mActive = active;
            GameUI.Instance.ItemBar.EnableUseSign = active;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ActivateKey(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        ActivateKey(other, false);
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
        return !mUsed;
    }

    public HintType GetHintType()
    {
        return HintType.TOOL;
    }
}