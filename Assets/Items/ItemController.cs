using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem{

    /// <summary>
    /// Default item action
    /// </summary>
    /// <returns>False if the item has no special action</returns>
    bool ItemAction(ItemController c);

}

public class ItemController : MonoBehaviour
{
    public string actionButton;
    public string itemTagName;
    public Transform holdingTarget;
    public float itemUseTime;

    private bool mHoldingItem = false;
    private GameObject mHoldedItem;
    private bool mUseItem;
    float mTimeAccumulator;

    public GameObject HoldedItem
    {
        get { return mHoldedItem; }
        set
        {
            if(value == null)
            {
                mHoldedItem = value;
                mHoldingItem = false;
            }
            else
            {
                mHoldedItem = value;
                mHoldingItem = true;
            }
        }
    }

    public bool UseItem
    {
        get { return mUseItem; }
        set { mUseItem = value; }
    }

    // Use this for initialization
    void Start()
    {
        mTimeAccumulator = 0.0f;
    }

    void Update()
    {
        if (Input.GetButton(actionButton))
        {
            if (mHoldingItem)
            {
                if (mTimeAccumulator < itemUseTime)
                {
                    mTimeAccumulator += Time.deltaTime;
                }
                else
                {
                    //item action
                    mTimeAccumulator = 0.0f;
                    StartItemAction();
                }
            }
        }
        else
        {
            mTimeAccumulator = 0.0f;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == itemTagName)
        {
            if (Input.GetButtonDown(actionButton) && !mHoldedItem)
            {
                Debug.Log("Item controller, hold item [" + other.name + "]");
                mHoldedItem = other.gameObject;
                HoldItem();
            }
        }
    }

    private void StartItemAction()
    {
        IItem item = mHoldedItem.GetComponentInChildren<IItem>();

        if (item != null)
        {
            if (!item.ItemAction(this))
            {
                DropItem();
            }
        }
        else
        {
            DropItem();
        }
    }

    public void DropItem()
    {
        if (mHoldedItem != null)
        {
            mHoldedItem.transform.SetParent(null);
            mHoldedItem.GetComponent<Rigidbody>().isKinematic = false;
            mHoldingItem = false;
            mHoldedItem = null;
        }
    }

    private void HoldItem()
    {
        mHoldedItem.gameObject.transform.SetParent(holdingTarget);
        mHoldedItem.gameObject.transform.localPosition = new Vector3();
        mHoldedItem.gameObject.transform.rotation = transform.rotation;
        mHoldedItem.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(WaitForAction());
    }

    private IEnumerator WaitForAction()
    {
        yield return new WaitForSeconds(0.5f);
        mHoldingItem = true;
    }

}
