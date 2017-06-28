using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : MonoBehaviour {
	
	private Unit unitScript;
	private ChasingPlayer chaseScript;
	private FieldOfView fieldScript;

	Animator anim;

	void Start() {
		anim = GetComponent<Animator> ();
		unitScript = GetComponent<Unit> ();
		chaseScript = GetComponent<ChasingPlayer> ();
		fieldScript = GetComponent<FieldOfView> ();
	}

	void Update () {
		if (unitScript.isWating) {
			anim.SetBool ("isEating", true);
			fieldScript.enabled = false;
		} else {
			anim.SetBool ("isEating", false);
			fieldScript.enabled = true;
		}
		if (chaseScript.isActiveAndEnabled && !chaseScript.caught) {
			anim.SetBool ("isChasing", true);
		} else {
			anim.SetBool ("isChasing", false);
		}
		if (chaseScript.caught) {
			anim.SetBool ("caught", true);
		}
	}
}
