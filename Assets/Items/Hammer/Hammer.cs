using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, IItem
{

    public string itemName;
    public string itemHint;
    public Sprite itemSprite;

    public GameObject[] objectsToActivate;
    private IEvent mEventToTrigger;

    private bool mActive;

    // Use this for initialization
    void Start()
    {
        mActive = false;
        mEventToTrigger = null;
        Debug.Assert(objectsToActivate != null);
    }

    // Update is called once per frame
    void Update()
    {

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


    public bool ItemAction(ItemController c)
    {
        if (mActive)
        {
            if (mEventToTrigger != null && mEventToTrigger.IsActive())
            {
                mEventToTrigger.Activate();
                GameUI.Instance.ItemBar.EnableUseSign = false;
                mEventToTrigger = null;
            }
        }

        return mActive;
    }

    void ActivateTool(Collider other, bool active)
    {
        foreach (GameObject gameObject in objectsToActivate)
        {
            if (other.gameObject == gameObject)
            {
                mActive = active;

                mEventToTrigger = null;

                mEventToTrigger = other.gameObject.GetComponent<IEvent>();

                if(mEventToTrigger != null)
                {
                    GameUI.Instance.ItemBar.EnableUseSign = mEventToTrigger.IsActive();
                }

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ActivateTool(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        ActivateTool(other, false);
        GameUI.Instance.ItemBar.EnableUseSign = false;
    }

}
