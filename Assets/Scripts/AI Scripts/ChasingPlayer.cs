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
			float dist = Vector3.Distance(target.position, transform.position);
			float speedPercent = 1;

			if (dist <= stoppingDst) {
				speedPercent = dist / stoppingDst;
			}
			if (dist <= reachDst) {
				speedPercent = 0;
				tsd.EndGame("You got caught");

			}
			Quaternion targetRotation = Quaternion.LookRotation (target.position - transform.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
			transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
		}
	}
}
