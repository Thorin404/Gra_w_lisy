using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : MonoBehaviour {

	bool isEating = false;

	private Unit unitScript;

	Animator anim;

	void Start() {
		anim = GetComponent<Animator> ();
		unitScript = GetComponent<Unit> ();
	}
	
	// Update is called once per frame
	void Update () {
		isEating = unitScript.isWating;
		if (isEating) {
			anim.SetBool ("isEating", true);
		} else {
			anim.SetBool ("isEating", false);
		}
	}
}
