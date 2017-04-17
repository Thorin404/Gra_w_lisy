using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //Misc
    public string pauseButtonName;
    public string introSkipButton;

    //Player related 

    public PlayerController playerController;
    public Camera primaryCamera;
    public Camera secondaryCamera;

    //Ui groups 
    public GameObject pauseUI;
    public GameObject gameUI;
    public GameObject miscUI;

    //Script references

    private CameraSystemController cameraSystem;
    private PauseMenu pauseMenu;

    //Private members
    private bool mStageStarted = false;
    private int mScore = 0;
    private bool mGamePaused = false;

    //UI references 

    private Text timerText;

    void Start()
    {
        cameraSystem = GetComponent<CameraSystemController>();
        pauseMenu = pauseUI.GetComponentInChildren<PauseMenu>();

        timerText = gameUI.GetComponentInChildren<Text>();

        RestartGame();


        pauseMenu.EnableRetryButton(false);
        playerController.gameObject.SetActive(false);
        primaryCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(false);

        //Start intro camera
        cameraSystem.Reset();
        StartCoroutine(WaitForStart());

        //Start pause corutine
        StartCoroutine(WaitForPause());
    }

    void Update()
    {

    }

    private IEnumerator WaitForStart()
    {
        while (cameraSystem.SystemIsActive)
        {
            if (Input.GetButtonDown(introSkipButton) && !mGamePaused)
            {
                cameraSystem.Skip();
            }
            yield return null;
            //Debug.Log("Waiting for startGame");
        }
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        gameUI.gameObject.SetActive(true);
        primaryCamera.gameObject.SetActive(true);
        pauseMenu.EnableRetryButton(true);

        float timeLeft = 3.0f;

        while (timeLeft > 0.1f)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Get ready : " + timeLeft;
            yield return new WaitForEndOfFrame();
        }
        timerText.text = "Time: ";

        StartCoroutine(StartStage());
    }

    private IEnumerator StartStage()
    {
        playerController.gameObject.SetActive(true);

        yield return null;
    }

    private IEnumerator WaitForPause()
    {
        while (true)
        {
            if (Input.GetButtonDown(pauseButtonName) && !mGamePaused)
            {
                pauseUI.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
                mGamePaused = true;
            }
            yield return null;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        mGamePaused = false;
        pauseUI.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        //TODO: Reseting scene state, without reloading it
       
    }


}
