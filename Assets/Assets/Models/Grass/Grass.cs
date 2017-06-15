using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {

    public string triggerName;
    private Animator mAnimator;

	void Start () {
        mAnimator = GetComponent<Animator>();
        Debug.Assert(mAnimator != null);
    }
	
    void OnTriggerEnter(Collider other)
    {
        mAnimator.SetTrigger(triggerName);
    }

}
