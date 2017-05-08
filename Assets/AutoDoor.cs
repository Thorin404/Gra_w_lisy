using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour {

    public string playerTag;
    public string openTriggerName;
    public string closeTriggerName;

    private Animator mAnimator;
    private bool mDoorOpen = false;


	// Use this for initialization
	void Start () {
        mAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == playerTag && mAnimator.enabled)
        {
            mDoorOpen = true;
            mAnimator.SetTrigger(openTriggerName);
            mAnimator.ResetTrigger(closeTriggerName);
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (mDoorOpen && col.gameObject.tag == playerTag && mAnimator.enabled)
        {
            mDoorOpen = false;
            mAnimator.SetTrigger(closeTriggerName);
            mAnimator.ResetTrigger(openTriggerName);
        }
    }
}
