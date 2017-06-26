using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour
{
    //Buttons
    public string pauseButtonName;
    public string skipButtonName;
    public string retryButtonName;

    //Player related 
    public PlayerController playerController;
    public Camera playerCamera;

    //Script references
    private CameraSystemController cameraSystem;
    private KeyItemSpawner keyItemSpawner;
    private ScoreCounter scoreCounter;

    //GameObjectives
    public string stageName;
    public float targetTime;
    public float countdownTime;
    public GameObject arrowPointer;
    public Transform startPosition;
    public Transform endPosition;

    public delegate void PickupHandler(PickUp p);
    public PickupHandler pickupHandler;

    //Game pause
    private static bool mGamePaused = false;

    private IEnumerator mGamePauseCoroutine;

    public static bool GamePaused
    {
        get { return mGamePaused; }
    }

    //Starting the game
    private bool mGameStart = false;
    private bool mExitToMenu = false;
    private bool mRestart = false;

    public void StartGame()
    {
        mGameStart = true;
    }
    public void ExitToMenu()
    {
        mExitToMenu = true;
    }
    public void RestartStage()
    {
        mRestart = true;
    }


    //Gameplay 

    public bool CheckpointReached
    {
        set { mCheckpointReached = mGotoExit ? value : false; }
        get { return mCheckpointReached; }
    }

    private bool mCheckpointReached = false;
    private bool mGotoExit = false;
    private bool mStageWon = false;

    private bool mPlayerLost = false;
    private string mStageLostReason = "";

    void Start()
    {
        //Load game saves
        GameData.Instance.Load();

        //Setup scripts references
        cameraSystem = GetComponent<CameraSystemController>();
        keyItemSpawner = GetComponent<KeyItemSpawner>();
        scoreCounter = GetComponent<ScoreCounter>();

        pickupHandler = scoreCounter.HandlePickUp;

        RestartGame();
    }

    void Update()
    {

    }

    public void RestartGame()
    {
        //TODO: Reseting scene state, without reloading it / async level loading

        //Stop all previous coroutines
        StopAllCoroutines();

        //Unpause game
        mGamePaused = false;
        mPlayerLost = false;
        mGameStart = false;
        mExitToMenu = false;
        mRestart = false;

        //Spawn key items
        keyItemSpawner.Reset();
        keyItemSpawner.SpawnItems();

        //Set player position and rotation deactivate player controller
        playerController.SetPlayerPosition(startPosition);
        playerController.enabled = false;
        playerCamera.gameObject.SetActive(false);


        //Reset intro camera, enable intro ui
        InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.INTRO, true);
        cameraSystem.Reset();

        ///PauseMenu.Instance.EnableButton(PauseMenu.PauseMenuButtons.RETRY, false);

        //Start intro
        StartCoroutine(WaitForStart());

        //Start pause corutine
        mGamePauseCoroutine = WaitForPause();

        StartCoroutine(mGamePauseCoroutine);
    }

    private IEnumerator WaitForStart()
    {
        IntroUI.Instance.SetText(IntroUI.TextElements.STAGE_NAME, stageName);

        string topScores = GameData.Instance.GetData.GetLevelSave(stageName).GetBestScoresString(20);
        IntroUI.Instance.SetText(IntroUI.TextElements.TOP_SCORE_LIST, topScores);

        while (cameraSystem.SystemIsActive)
        {
            if (mGameStart && !mGamePaused)
            {
                cameraSystem.Skip();
                break;
            }
            yield return null;
        }
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.INTRO, false);
        InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.GAME, true);
        scoreCounter.ResetCounter(keyItemSpawner.KeyItemsCount, keyItemSpawner.KeyItemName, targetTime);

        playerCamera.gameObject.SetActive(true);

        //TODO: Repair UI

        //PauseMenu.Instance.EnableButton(PauseMenu.PauseMenuButtons.RETRY, true);

        float timeLeft = countdownTime;

        while (timeLeft > 0.1f)
        {
            timeLeft -= Time.deltaTime;

            GameUI.Instance.SetText(GameUI.TextElements.INFO, "Get ready : " + (int)(timeLeft+1));
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(StartStage());

        GameUI.Instance.SetText(GameUI.TextElements.INFO, "Foxels!");
        GameUI.Instance.RotateUiText(GameUI.TextElements.INFO);

        yield return new WaitForSeconds(2);

        GameUI.Instance.EnableText(GameUI.TextElements.INFO, false);
    }

    //Gameplay

    public void EndGame(string message)
    {
        if (!mPlayerLost && playerController.enabled)
        {
            mPlayerLost = true;
            mStageLostReason = message;
        }
    }


    private IEnumerator StartStage()
    {

        //Enable player movement and reset counter
        playerController.enabled = true;
        playerController.SetPlayerPosition(startPosition);

        //Score counting loop

        //Default: end of time
        mStageLostReason = "End of time...";

        while (scoreCounter.PlayerHasTime)
        {
            //Refresh player timer
            scoreCounter.SubtractTime = Time.deltaTime;

            //If minimal score has been reached, activate the arrow pointing to exit
            if (scoreCounter.ItemsCollected)
            {
                //Set stage exit pointer active
                if (!arrowPointer.gameObject.activeSelf)
                {
                    //GameUI.Instance.SetText(GameUI.TextElements.OBJECTIVE, "Retreat to exit");
                    arrowPointer.gameObject.SetActive(true);
                    mGotoExit = true;
                }

                //Check if player retreated from stage
                if (CheckpointReached)
                {
                    mStageWon = true;
                    break;
                }            
            }

            //Check if player lost for some reason
            if (mPlayerLost)
            {
                mStageWon = false;
                break;
            }

            yield return null;
        }

        if (mStageWon)
        {
            GameUI.Instance.SetText(GameUI.TextElements.INFO, "You won!");
        }
        else
        {
            GameUI.Instance.SetText(GameUI.TextElements.INFO, mStageLostReason);
        }

        GameUI.Instance.EnableText(GameUI.TextElements.INFO, true);
        arrowPointer.gameObject.SetActive(false);

        playerController.enabled = false;
        playerController.SetAnimatorState = 0.0f;

        //Wait for 2 seconds and start score screen
        yield return new WaitForSeconds(2);

        GameUI.Instance.EnableText(GameUI.TextElements.INFO, false);

        InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.GAME, false);
        InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.SCORE, true);

        if (mStageWon)
        {

            //TODO : Score saving etc
            ScoreUI.Instance.EnableBestScoreInput(true);
            scoreCounter.SetScoreText();

            //fox goes away
            //a* to the point
        }
        else
        {
            arrowPointer.gameObject.SetActive(false);
            playerController.enabled = false;

            ScoreUI.Instance.scoreText.text = "Stage failed: " + mStageLostReason;
            ScoreUI.Instance.EnableBestScoreInput(false);
        }

        StartCoroutine(WaitForExit());
    }

    private IEnumerator WaitForExit()
    {

        //Wait 2 sec before enablling exit to menu
        yield return new WaitForSeconds(2);

        bool exitStage = false;

        while (true)
        {
            if (mRestart && !mGamePaused)
            {
                break;
            }
            else if (mExitToMenu && !mGamePaused)
            {
                exitStage = true;
                break;
            }
            yield return null;
        }

        //Save score
        scoreCounter.SaveScore(stageName, mStageWon);

        StopCoroutine(mGamePauseCoroutine);

        if (exitStage)
        {
            GameData.Instance.Save();
            SceneManager.LoadScene(0);
        }
        else
        {
            //TODO: reload scene async
            GameData.Instance.Save();
            Debug.Log("Load scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //Pause Game
    private IEnumerator WaitForPause()
    {
        while (true)
        {
            if (Input.GetButtonDown(pauseButtonName) && !mGamePaused)
            {
                InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.PAUSE, true);
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
        InGameUI.Instance.SetInterfaceGroup(InGameUI.InterfaceGroups.PAUSE, false);
    }

}
