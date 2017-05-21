using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public Transform target;

	void Update () {
		//transform.Rotate (new Vector3 (0, 30, 0) * Time.deltaTime);
		transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
		//transform.LookAt (target);
	}
}
