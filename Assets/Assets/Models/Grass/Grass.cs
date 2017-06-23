using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {

    public string[] actionTriggers;
    public float triggerReleaseTime;

    public float soundPitchMin;
    public float soundPitchMax;

    private Animator mAnimator;
    private IEnumerator mAnimationStarted;
    private AudioSource mAudioSource;

    

	void Start () {
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAnimator != null && actionTriggers != null && actionTriggers.Length != 0 && mAudioSource != null);
        mAnimationStarted = null;
    }
	
    void OnTriggerEnter(Collider other)
    {
        if(mAnimationStarted == null)
        {
            mAnimationStarted = StartAnimation();
            StartCoroutine(mAnimationStarted);
        } 
    }
    private IEnumerator StartAnimation()
    {
        //Trigger the animation
        string trigger = actionTriggers[Random.Range(0, actionTriggers.Length)];
        Debug.Log("Grass animation" + trigger);
        mAnimator.SetTrigger(trigger);

        //Play sound
        mAudioSource.pitch = Random.Range(soundPitchMin, soundPitchMax);
        mAudioSource.Play();

        //Wait for specified time
        yield return new WaitForSeconds(triggerReleaseTime);

        mAnimationStarted = null;
        yield return null;
    }

}
