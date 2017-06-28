﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimation : MonoBehaviour {
	
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
			anim.SetBool ("isWaiting", true);
			fieldScript.viewRadius = 20;
		} else {
			anim.SetBool ("isWaiting", false);
			fieldScript.viewRadius = 10;
		}
		if (chaseScript.isActiveAndEnabled) {
			anim.SetBool ("isChasing", true);
			fieldScript.viewRadius = 20;
		} else {
			anim.SetBool ("isChasing", false);
			fieldScript.viewRadius = 10;
		}
	}
}