using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraTarget;
    public float distanceFromTarget;
    public float rotationAngle;

    void Start () {
        transform.rotation = Quaternion.Euler(rotationAngle, rotationAngle, 0.0f);
    }

	void LateUpdate () {
        transform.position = cameraTarget.position - transform.forward * distanceFromTarget;
    }
}
