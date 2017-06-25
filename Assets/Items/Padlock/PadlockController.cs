using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILockable
{
    void Lock();
    void Unlock();
    bool IsLocked();
}

public class PadlockController : MonoBehaviour, IObjectHint
{
    public string animationTriggerName;
    public float actionTime;
    public Transform keyHolder;
    public GameObject lockedObject;

    public string hintName;

    private GameObject mKeyItem;
    private Animator mAnimator;
    private Rigidbody mRigidbody;

    private bool mActivated;

    private GameObject mHintObject;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();

        Debug.Assert(mAnimator != null);
        Debug.Assert(keyHolder != null);
        Debug.Assert(mRigidbody != null);

        mActivated = false;
    }

    private void SetKeyPosition()
    {
        mKeyItem.transform.SetParent(keyHolder);
        mKeyItem.transform.localPosition = new Vector3();
        mKeyItem.transform.localRotation = Quaternion.identity;
        mKeyItem.GetComponent<Rigidbody>().isKinematic = true;
        mKeyItem.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator PadlockAction()
    {
        //Set key parent
        SetKeyPosition();

        //Start animation
        mAnimator.SetTrigger(animationTriggerName);

        //Wait for some time
        yield return new WaitForSeconds(actionTime);

        //Enable physics
        mRigidbody.isKinematic = false;

        //Unlock locked object
        if(lockedObject != null)
        {
            ILockable objectToUnlock = lockedObject.GetComponent<ILockable>();
            if(objectToUnlock != null)
            {
                objectToUnlock.Unlock();
            }
        }

        //Deactivate padlock action
        mActivated = true;

        yield return null;
    }

    public void Unlock(GameObject keyObject)
    {
        mKeyItem = keyObject;
        if (!mActivated && mKeyItem != null)
        {
            StartCoroutine(PadlockAction());
            mActivated = true;
        }
    }

    public string GetHintName()
    {
        return hintName;
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
        return !mActivated;
    }

    public HintType GetHintType()
    {
        return HintType.TOOL_USAGE;
    }
}
