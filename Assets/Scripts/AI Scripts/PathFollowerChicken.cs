using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowerChicken : MonoBehaviour {

	public float speed = 2;
	public float chasingSpeed = 3;
	public float turnSpeed = 3;
	public float chasingTurn = 4;
	public float wanderXMin;
	public float wanderXMax;
	public float wanderZMin;
	public float wanderZMax;
	public float eatingTime;

	void Start() {

	}

	void Update() {
		Quaternion targetRotation;
		if (GameObject.FindGameObjectsWithTag ("Seeds").Length >= 1) {
			GoingToSeeds ();
		} else {
			Vector3 chickenWandering = new Vector3 (Random.Range (wanderXMin, wanderXMax), 0, Random.Range (wanderZMin, wanderZMax));
			targetRotation = Quaternion.LookRotation (chickenWandering - transform.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
		}
		transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
	}

	void GoingToSeeds(){
		Quaternion targetRotation;
		Vector3 seeds = GameObject.FindWithTag ("Seeds").transform.position;
		targetRotation = Quaternion.LookRotation (seeds - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
		if ((Vector3.Distance (seeds, transform.position)) <= 1) {
			StartCoroutine (EatingSeeds ());
		}
	}

	IEnumerator EatingSeeds(){
		speed = 0;
		turnSpeed = 0;
		yield return new WaitForSeconds (eatingTime);
		GameObject tsd = GameObject.FindGameObjectWithTag ("Seeds");
		Destroy (tsd);
		speed = chasingSpeed;
		turnSpeed = chasingTurn;
	}
}
