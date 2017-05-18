using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class CameraSystemController : MonoBehaviour
{
    //public string skipButton;
    public Camera sceneCamera;
    public Transform cameraPositionsHolder;

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

    private Transform[] mCameraPositions;

    void Start()
    {
        mCameraPositions = CreatePositionsArray(cameraPositionsHolder);
    }

    private Transform[] CreatePositionsArray(Transform holder)
    {
        Transform[] positions = holder.GetComponentsInChildren<Transform>();
        positions = positions.Skip(1).ToArray();
        return positions;
    }

    void OnDrawGizmos()
    {
        Transform[] positions = cameraPositionsHolder.GetComponentsInChildren<Transform>();
        foreach (Transform t in positions)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(t.position, new Vector3(1, 1, 1));
        }
    }

    private void SetCameraTransrofm(int i)
    {
        sceneCamera.transform.position = mCameraPositions[i].position;
        sceneCamera.transform.rotation = mCameraPositions[i].rotation;
    }

    public void Reset()
    {
        SystemIsActive = true;
        SetCameraTransrofm(0);
        sceneCamera.gameObject.SetActive(true);

        mCurrentCoroutine = NextPoint();
        StartCoroutine(mCurrentCoroutine);
    }

    public void Skip()
    {
        StopCoroutine(mCurrentCoroutine);
        sceneCamera.gameObject.SetActive(false);
        SystemIsActive = false;
    }

    IEnumerator NextPoint()
    {
        do
        {
            foreach (Transform t in mCameraPositions)
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
            if (Time.timeScale > 0.1f)
            {
                sceneCamera.transform.position = Vector3.MoveTowards(sceneCamera.transform.position, point, speed * Time.deltaTime);
                StartCoroutine(RotateCamera(rotation, rotationSpeed));
            }
            yield return null;
        }
        yield return new WaitForSeconds(transitionDelay);
    }

    IEnumerator RotateCamera(Quaternion rotation, float speed)
    {
        while (sceneCamera.transform.rotation != rotation)
        {
            if (Time.timeScale > 0.1f)
            {
                sceneCamera.transform.rotation = Quaternion.RotateTowards(sceneCamera.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }
}
