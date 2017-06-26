using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour, ILockable
{
    public BoxCollider triggerCollider;

    public string playerTag;
    public string openTriggerName;
    public string closeTriggerName;
    public float closeTime;

    private Animator mAnimator;
    private bool mDoorOpen = false;
    private bool mActive;

    void Start()
    {
        mAnimator = GetComponentInChildren<Animator>();
        Debug.Assert(mAnimator != null);
        mActive = false;
        triggerCollider.enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == playerTag && mActive && !mDoorOpen)
        {
            mDoorOpen = true;
            mAnimator.SetTrigger(openTriggerName);
            mAnimator.ResetTrigger(closeTriggerName);
        }

    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(closeTime);

        mDoorOpen = false;
        mAnimator.SetTrigger(closeTriggerName);
        mAnimator.ResetTrigger(openTriggerName);
    }

    void OnTriggerExit(Collider col)
    {
        if (mDoorOpen && col.gameObject.tag == playerTag && mActive)
        {
            Debug.Log("Exit trigger");

            StartCoroutine(CloseDoor());
        }
    }

    public void Lock()
    {
        mActive = false;
    }

    public void Unlock()
    {
        mActive = true;
        triggerCollider.enabled = true;
    }

    public bool IsLocked()
    {
        return mActive;
    }
}
