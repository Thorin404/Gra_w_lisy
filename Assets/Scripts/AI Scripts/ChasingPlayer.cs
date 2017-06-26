using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingPlayer : MonoBehaviour {

	public Transform target;

	public float speed = 2;
	public float chasingSpeed = 3;
	public float turnSpeed = 3;
	public float chasingTurn = 4;
	public float stoppingDst = 2;
	public float reachDst = 1;

	public GameController tsd;

	void Start() {

	}

	void Update() {
		if (target != null) {
			Quaternion targetRotation = Quaternion.LookRotation (target.position - transform.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
			transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")){
			tsd.EndGame("You got caught");
		}
	}
}
