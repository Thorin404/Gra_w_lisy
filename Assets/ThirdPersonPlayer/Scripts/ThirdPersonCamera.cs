﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float mouseSensitivity = 10;

    public Transform target;
    public float distanceFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(40, 85);

    float yaw;
    float pitch;

    public float rotationSmoothTime = 1.2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public bool lockCursor;


	// Use this for initialization
	void Start () {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;

	}
}
