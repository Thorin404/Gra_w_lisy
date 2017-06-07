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

    void Start()
    {
        mActive = false;
        mUsed = false;
        Debug.Assert(padlock != null);
    }

    public bool ItemAction(ItemController c)
    {
        ///throw new NotImplementedException();
        if (mActive && !mUsed)
        {
            c.HoldedItem = null;
            PadlockController padCtrl = padlock.GetComponent<PadlockController>();
            Debug.Assert(padCtrl != null);
            padCtrl.Unlock(this.gameObject);
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
        if (other.gameObject == padlock && !mUsed)
        {
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
}