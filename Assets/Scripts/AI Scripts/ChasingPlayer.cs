using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingPlayer : MonoBehaviour {

	public string tag = "Player";

	public float speed = 2;
	public float turnSpeed = 3;

	public bool caught = false;

	public GameController gameControl;

	void Start() {
	}

	void Update() {
		Vector3 target = GameObject.FindWithTag (tag).transform.position;
		Quaternion targetRotation = Quaternion.LookRotation (target - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
		transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")){
			caught = true;
			gameControl.EndGame("You got caught");
			this.enabled = false;
		}
	}
}
