using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    void Activate();
    bool IsActive();
}

public interface IItem
{

    /// <summary>
    /// Default item action
    /// </summary>
    /// <returns>False if the item has no special action</returns>
    bool ItemAction(ItemController c);

    Sprite GetSprite();
    string GetName();
    string GetHint();
}

public class ItemController : MonoBehaviour
{
    public string actionButton;
    public string dropButton;
    public string itemTagName;
    public Transform holdingTarget;
    public float itemUseTime;

    //New elements
    private GameObject approachedItem = null;

    private static bool mHoldingItem = false;
    private static GameObject mHoldedItem;
    private static IItem mItemInterface;
    private ItemBar mItemBar;
    private bool mUseItem;
    private float mTimeAccumulator;

    private bool mAcionPending;

    private PlayerController mPlayerController;

    //Animator
    private Animator mPlayerAnimator;
    public float pickAnimationTime;
    public float throwAnimationTime;
    public string pickUpTrigger;
    public string throwTrigger;
    public string toolAnimationTrigger;

    public static GameObject HoldedItem
    {
        get { return mHoldedItem; }
        set
        {
            if (value == null)
            {
                mHoldedItem = value;
                mItemInterface = null;
                mHoldingItem = false;
                GameUI.Instance.ItemBar.SetDefault();
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
    void Awake()
    {
        mTimeAccumulator = 0.0f;
        mPlayerAnimator = GetComponentInChildren<Animator>();
        mAcionPending = false;
        mPlayerController = GetComponent<PlayerController>();
        Debug.Assert(mPlayerController != null);
    }

    void Update()
    {
        if (Input.GetButton(actionButton) && !mAcionPending)
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
                    StartCoroutine(StartItemAction());
                }
                if (mItemBar != null)
                {
                    mItemBar.ProgressBarPct = mTimeAccumulator / itemUseTime;
                }
                else
                {
                    mItemBar = GameUI.Instance.ItemBar;
                }
            }
        }
        else
        {
            mTimeAccumulator = 0.0f;
            if (mItemBar != null)
            {
                mItemBar.ProgressBarPct = 0.0f;
            }
        }
        if (Input.GetButtonDown(dropButton) && mHoldingItem)
        {
            DropItem();
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == itemTagName && !mHoldingItem)
        {
            if (approachedItem == null)
            {
                approachedItem = other.gameObject;
                Debug.Log("Approched item [" + other.name + "]");
                SetItemBar(approachedItem.GetComponentInChildren<IItem>());
            }
            else if (Input.GetButtonDown(actionButton) && !mHoldedItem)
            {
                Debug.Log("Item controller, hold item [" + other.name + "]");
                mHoldedItem = other.gameObject;
                approachedItem = null;
                StartCoroutine(HoldItem(pickAnimationTime));
            }
            //Debug.Log("Appro" + approachedItem);
            //if (Input.GetButtonDown(actionButton) && !mHoldedItem)
            //{
            //    Debug.Log("Item controller, hold item [" + other.name + "]");
            //    mHoldedItem = other.gameObject;
            //    StartCoroutine(HoldItem(pickAnimationTime));
            //    return;
            //}

            //if (approachedItem == null && approachedItem != mHoldedItem)
            //{
            //    approachedItem = other.gameObject;
            //    Debug.Log("Approched item [" + other.name + "]");
            //    SetItemBar(approachedItem.GetComponentInChildren<IItem>());
            //}

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (approachedItem != null && other.gameObject.tag == itemTagName && !mHoldingItem)
        {
            SetItemBar(null);
            approachedItem = null;
        }
    }

    private IEnumerator StartItemAction()
    {
        mAcionPending = true;
        mPlayerController.AbleToMove = false;
        mPlayerAnimator.SetTrigger(throwTrigger);

        yield return new WaitForSeconds(throwAnimationTime);

        if (mItemInterface != null)
        {
            if (!mItemInterface.ItemAction(this))
            {
                DropItem();
            }
            else
            {
                SetItemBar(mItemInterface);
            }
        }
        else
        {
            DropItem();
        }
        mAcionPending = false;
        mPlayerController.AbleToMove = true;
    }

    public void DropItem()
    {
        if (mHoldedItem != null)
        {
            mHoldedItem.transform.SetParent(null);
            mHoldedItem.GetComponent<Rigidbody>().isKinematic = false;
            mHoldingItem = false;
            mHoldedItem = null;
            mItemInterface = null;

            approachedItem = null;

            GameUI.Instance.ItemBar.SetDefault();
        }
    }

    private void SetItemBar(IItem itemToShow)
    {
        if (itemToShow != null)
        {
            ItemBar itemBar = GameUI.Instance.ItemBar;

            itemBar.ItemName = itemToShow.GetName();
            itemBar.ItemHint = itemToShow.GetHint();
            itemBar.ItemSprite = itemToShow.GetSprite();
        }
        else
        {
            GameUI.Instance.ItemBar.SetDefault();
        }
        GameUI.Instance.RotateUiText(GameUI.TextElements.IB_NAME);
    }

    private IEnumerator HoldItem(float time)
    {
        mAcionPending = true;
        mPlayerController.AbleToMove = false;

        mPlayerAnimator.SetTrigger(pickUpTrigger);
        //TODO : disable player movements
        yield return new WaitForSeconds(time);

        mHoldedItem.gameObject.transform.SetParent(holdingTarget);
        mHoldedItem.gameObject.transform.localPosition = new Vector3();
        mHoldedItem.gameObject.transform.localRotation = Quaternion.identity;
        mHoldedItem.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        mItemInterface = mHoldedItem.GetComponentInChildren<IItem>();
        SetItemBar(mItemInterface);

        mHoldingItem = true;
        mAcionPending = false;
        mPlayerController.AbleToMove = true;
    }

}
