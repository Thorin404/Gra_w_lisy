using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public string horizontalAxis;
    public string verticalAxis;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float yawStartOffset = 180.0f;
    public float pitchStartOffset = 20.0f;

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    void Start()
    {
        yaw = yawStartOffset;
        pitch = pitchStartOffset;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - (transform.forward * dstFromTarget);

        yaw += Input.GetAxis(horizontalAxis) * mouseSensitivity;
        pitch -= Input.GetAxis(verticalAxis) * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
    }

}