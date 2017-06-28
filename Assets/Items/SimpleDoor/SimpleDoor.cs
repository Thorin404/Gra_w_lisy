using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDoor : MonoBehaviour, ILockable {

    private Animator mAnimator;
    private AudioSource mAudioSource;

    public string openAnimationTrigger;
    public string openDoorAnimationTrigger;

    public void SetOpen()
    {
        mIsLocked = false;
        mAnimator.SetTrigger(openAnimationTrigger);
    }

    private bool mIsLocked = true;

    void Awake () {
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();

        Debug.Assert(mAnimator != null);
        Debug.Assert(mAudioSource != null);

        //mAnimator.SetTrigger(openAnimationTrigger);
        Debug.Log("Simple door start");
    }
	

    public bool IsLocked()
    {
        return mIsLocked;
    }

    public void Lock()
    {
        mIsLocked = true;
        //Default animation: closed door
    }

    public void Unlock()
    {
        if (mIsLocked)
        {
            //Play sound and stuff
            mAudioSource.Play();
            mAnimator.SetTrigger(openDoorAnimationTrigger);
            mIsLocked = false;
        }
    }
}
