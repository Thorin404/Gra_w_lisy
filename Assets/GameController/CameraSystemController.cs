using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystemController : MonoBehaviour
{
    public string skipButton;
    public GameObject stageIntroUI;
    public Camera sceneCamera;
    public Transform[] cameraPositions;

    public float cameraSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float transitionDelay = 0.8f;

    public bool cycling = true;

    private IEnumerator mCurrentCoroutine;
    private bool mSystemActive = false;

    public bool SystemIsActive
    {
        get { return mSystemActive; }
        private set { mSystemActive = value; }
    }

    void Start()
    {
        //Reset();
    }

    void Update()
    {
        if (Input.GetButtonDown(skipButton))
        {
            if (mCurrentCoroutine != null)
            {
                Skip();
            }
        }
    }

    void OnDrawGizmos()
    {
        foreach (Transform t in cameraPositions)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(t.position, new Vector3(1, 1, 1));
        }
    }

    private void SetCameraTransrofm(int i)
    {
        sceneCamera.transform.position = cameraPositions[i].position;
        sceneCamera.transform.rotation = cameraPositions[i].rotation;
    }

    public void Reset()
    {
        SystemIsActive = true;
        SetCameraTransrofm(0);
        sceneCamera.gameObject.SetActive(true);
        stageIntroUI.gameObject.SetActive(true);

        mCurrentCoroutine = NextPoint();
        StartCoroutine(mCurrentCoroutine);
    }

    public void Skip()
    {
        StopCoroutine(mCurrentCoroutine);
        sceneCamera.gameObject.SetActive(false);
        stageIntroUI.gameObject.SetActive(false);
        SystemIsActive = false;
    }

    IEnumerator NextPoint()
    {
        do
        {
            foreach (Transform t in cameraPositions)
            {
                yield return StartCoroutine(MoveCamera(t.position, t.rotation, cameraSpeed));
            }
        }
        while (cycling);
        Skip();
    }

    IEnumerator MoveCamera(Vector3 point, Quaternion rotation, float speed)
    {
        while (sceneCamera.transform.position != point)
        {
            sceneCamera.transform.position = Vector3.MoveTowards(sceneCamera.transform.position, point, speed * Time.deltaTime);
            StartCoroutine(RotateCamera(rotation, rotationSpeed));
            yield return null;
        }
        yield return new WaitForSeconds(transitionDelay);
    }

    IEnumerator RotateCamera(Quaternion rotation, float speed)
    {
        while (sceneCamera.transform.rotation != rotation)
        {
            sceneCamera.transform.rotation = Quaternion.RotateTowards(sceneCamera.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
