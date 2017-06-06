using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock_key : MonoBehaviour, IItem
{
    public string itemName;
    public string itemHint;
    public Sprite itemSprite;

    public GameObject padlock;

    private bool mUsed;
    private bool mActive;
    public bool Active
    {
        private set
        {
            mActive = value;
        }
        get
        {
            return mActive;
        }
    }


    void Start()
    {
        mActive = false;
        mUsed = false;
        Debug.Assert(padlock != null);
    }

    public bool ItemAction(ItemController c)
    {
        ///throw new NotImplementedException();
        if (Active && !mUsed)
        {
            c.HoldedItem = null;
            PadlockController padCtrl = padlock.GetComponent<PadlockController>();
            Debug.Assert(padCtrl != null);
            padCtrl.Unlock();
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == padlock && !mUsed)
        {
            Active = true;
            //Debug.Log("KeyActive " + Active);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == padlock && !mUsed)
        {
            Active = false;
            //Debug.Log("KeyActive " + Active);
        }
    }
}