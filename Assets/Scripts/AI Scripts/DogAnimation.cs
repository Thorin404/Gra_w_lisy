using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : MonoBehaviour {

	bool isEating = false;

	private Unit unitScript;
	private FieldOfView fieldScript;

	Animator anim;

	void Start() {
		anim = GetComponent<Animator> ();
		unitScript = GetComponent<Unit> ();
		fieldScript = GetComponent<FieldOfView> ();
	}
	
	// Update is called once per frame
	void Update () {
		isEating = unitScript.isWating;
		if (isEating) {
			anim.SetBool ("isEating", true);
			fieldScript.enabled = false;
		} else {
			anim.SetBool ("isEating", false);
			fieldScript.enabled = true;
		}
	}
}
