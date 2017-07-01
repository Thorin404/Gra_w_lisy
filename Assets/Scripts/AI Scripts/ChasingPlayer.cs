using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingPlayer : MonoBehaviour {

	public string tag = "Player";

	public float speed = 2;
	public float turnSpeed = 3;

	public bool caught = false;

	public GameController gameControl;

    private GameObject mTarget = null;
    private AudioSource mAudioSource;

	void Start() {
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAudioSource != null);
		mTarget = GameObject.FindWithTag(tag);
    }

	void Update() {
//        if (!caught)
//        {
//            if(mTarget == null)
//            {
//                mTarget = GameObject.FindWithTag(tag);
//            }
//            else
//            {
                Quaternion targetRotation = Quaternion.LookRotation(mTarget.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
//            }
//
//        }

		
	}

//	void OnTriggerEnter(Collider other) {
//		if (other.gameObject.CompareTag ("Player") && !caught){
//			caught = true;
//            mAudioSource.Play();
//            gameControl.EndGame("You got caught");
//			this.enabled = false;
//		}
//	}
}
