using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraSystemController : MonoBehaviour
{
    //public string skipButton;
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

    private GameController mGameController;

    private Text mStageNameText;
    private Text mBestScoresText;

    void Awake()
    {
        mGameController = GetComponent<GameController>();
        Text[] pauseScreenElements = stageIntroUI.GetComponentsInChildren<Text>();

        mStageNameText = pauseScreenElements[0];
        mBestScoresText = pauseScreenElements[2];

        mStageNameText.text = mGameController.stageName;
    }

    void Update()
    {
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
        mBestScoresText.text = GameData.Instance.GetData.GetLevelSave(mGameController.stageName).GetBestScoresString(10);

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
