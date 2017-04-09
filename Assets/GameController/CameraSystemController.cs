using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystemController : MonoBehaviour
{
    public string skipButton;
    public GameObject stageIntroUI;
    public Camera sceneCamera;
    public Transform[] cameraPositions;

    public float speed = 1.0f;
    public float startDelay = 0.5f;
    public float transitionDelay = 0.8f;
    public bool cycling = false;
    public bool lerp = true;

    private int mCurrentPoint = 0;
    private int mNextPoint = 0;

    private float mStartTime;
    private float mJourneyLength;
    private bool mTransitionPending = false;
    private bool mSystemActive = false;

    public bool CameraSystemIsActive
    {
        get { return mSystemActive; }
        private set { mSystemActive = value; }
    }

    private void SetCameraTransrofm(int i)
    {
        sceneCamera.transform.position = cameraPositions[i].position;
        sceneCamera.transform.rotation = cameraPositions[i].rotation;
    }

    private IEnumerator NextPoint(float time)
    {
        yield return new WaitForSeconds(time);
        mTransitionPending = true;
        SetTimeAndDistance();
    }

    private void SetTimeAndDistance()
    {
        if (mCurrentPoint == cameraPositions.Length)
        {
            mCurrentPoint = 0;
            mNextPoint = mCurrentPoint + 1;
        }
        else if (!cycling && mCurrentPoint == cameraPositions.Length - 1)
        {
            StopCamera();
        }
        mStartTime = Time.time;
        mJourneyLength = Vector3.Distance(cameraPositions[mCurrentPoint].position, cameraPositions[mNextPoint].position);
    }

    private float LerpCamera(int fromPoint, int toPoint)
    {
        float distCovered = 0.0f;
        float fracJourney = 0.0f;
        if (lerp)
        {
            distCovered = (Time.time - mStartTime) * speed;
            fracJourney = distCovered / mJourneyLength;

            sceneCamera.transform.position = Vector3.Lerp(cameraPositions[fromPoint].position, cameraPositions[toPoint].position, fracJourney);
            sceneCamera.transform.rotation = Quaternion.Lerp(cameraPositions[fromPoint].rotation, cameraPositions[toPoint].rotation, fracJourney);
        }
        else
        {
            SetCameraTransrofm(toPoint);
            fracJourney = 1.1f;
        }
        return fracJourney;
    }

    void Start()
    {
        ResetCameras();
    }

    public void ResetCameras()
    {
        mCurrentPoint = 0;
        mNextPoint = mCurrentPoint + 1;

        SetCameraTransrofm(mCurrentPoint);
        StartCoroutine(NextPoint(startDelay));

        sceneCamera.gameObject.SetActive(true);
        stageIntroUI.SetActive(true);

        CameraSystemIsActive = true;
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown(skipButton))
        {
            StopCamera();
        }
        if (mTransitionPending)
        {

            if (LerpCamera(mCurrentPoint, mNextPoint) > 1.0f)
            {
                if (mCurrentPoint == cameraPositions.Length - 2)
                {
                    mCurrentPoint = cameraPositions.Length - 1;
                    mNextPoint = 0;
                }
                else
                {
                    ++mCurrentPoint;
                    ++mNextPoint;
                }
                mTransitionPending = false;
                StartCoroutine(NextPoint(transitionDelay));
            }
        }

    }

    public void StopCamera()
    {
        mTransitionPending = false;
        sceneCamera.gameObject.SetActive(false);
        stageIntroUI.SetActive(false);
        CameraSystemIsActive = false;
    }
}
