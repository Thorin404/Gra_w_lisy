using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePhysics : MonoBehaviour, IEvent, IObjectHint {

    private bool mActive;
    private Rigidbody[] mRigidbodies;
    public string hintName;

    private GameObject mHintObject;

    void Start () {
        mActive = true;
        mRigidbodies = GetComponentsInChildren<Rigidbody>();
        Debug.Assert(mRigidbodies != null);
    }

    public void Activate()
    {
        if (mActive)
        {
            foreach(Rigidbody rb in mRigidbodies)
            {
                rb.isKinematic = false;
            }
            mActive = false;
        }
    }

    public bool IsActive()
    {
        return mActive;
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
        return mActive;
    }

    public HintType GetHintType()
    {
        return HintType.TOOL_USAGE;
    }
}
