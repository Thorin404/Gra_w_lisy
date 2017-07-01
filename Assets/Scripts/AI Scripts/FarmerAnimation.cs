using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimation : MonoBehaviour {
	
	private Unit unitScript;
	private ChasingPlayer chaseScript;
	private FieldOfView fieldScript;

	public GameController gameControl;

	Animator anim;

	private AudioSource mAudioSource;

	void Start() {
		anim = GetComponent<Animator> ();
		unitScript = GetComponent<Unit> ();
		chaseScript = GetComponent<ChasingPlayer> ();
		fieldScript = GetComponent<FieldOfView> ();

		mAudioSource = GetComponent<AudioSource>();
		Debug.Assert(mAudioSource != null);
	}

	void Update () {
		if (unitScript.isWating) {
			anim.SetBool ("isWaiting", true);
			fieldScript.viewRadius = 20;
		} else {
			anim.SetBool ("isWaiting", false);
			fieldScript.viewRadius = 10;
		}
		if (chaseScript.isActiveAndEnabled && !chaseScript.caught) {
			anim.SetBool ("isChasing", true);
			fieldScript.viewRadius = 20;
		} else {
			anim.SetBool ("isChasing", false);
			fieldScript.viewRadius = 10;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")){
			mAudioSource.Play();
			gameControl.EndGame("You got caught");
			this.enabled = false;
		}
	}
}
