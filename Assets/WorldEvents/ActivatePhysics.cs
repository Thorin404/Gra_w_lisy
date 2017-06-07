using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePhysics : MonoBehaviour, IEvent {

    private bool mActive;
    private Rigidbody[] mRigidbodies;

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
}
