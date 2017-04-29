using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour
{
    //Misc
    public string pauseButtonName;
    public string skipButton;
    public string retryButton;

    //Player related 

    public PlayerController playerController;
    public Camera primaryCamera;
    public Camera secondaryCamera;

    //Ui groups 
    public GameObject pauseUI;
    public GameObject gameUI;
    public GameObject scoreUI;
    public GameObject miscUI;

    //Script references

    private CameraSystemController cameraSystem;
    private PauseMenu pauseMenu;
    private KeyItemSpawner keyItemSpawner;

    //UI references 

    private Text infoText;
    private Text gameObjectiveText;
    private Text keyItemsText;
    private Text scoreText;
    private Text timerText;

    private Text scoreDetailsText;

    //GameObjectives
    public GameObject arrowPointer;

    public Transform startPosition;
    public Transform endPosition;



    public float targetTime;

    public bool CheckpointReached
    {
        set { mCheckpointReached = mGotoExit ? value : false; }
        get { return mCheckpointReached; }
    }

    //Private members
    private int itemsToCollect;
    private string keyItemName;

    private bool mGamePaused = false;
    private bool mCheckpointReached = false;
    private bool mGotoExit = false;
    private bool mStageWon = false;

    private int mScoreMultiplier = 20;
    private float mScoreMultiplierTimer = 0.0f;

    private int mPlayerScore = 0;
    private int mPlayerItemsCollected = 0;
    private float mPlayerTime = 0.0f;

    void Start()
    {
        cameraSystem = GetComponent<CameraSystemController>();
        pauseMenu = pauseUI.GetComponentInChildren<PauseMenu>();
        keyItemSpawner = GetComponent<KeyItemSpawner>();

        //Game ui references
        Text[] textFields = gameUI.GetComponentsInChildren<Text>();
        infoText = textFields[0];
        gameObjectiveText = textFields[1];
        keyItemsText = textFields[2];
        scoreText = textFields[3];
        timerText = textFields[4];

        scoreDetailsText = scoreUI.GetComponentInChildren<Text>();

        RestartGame();
    }

    void Update()
    {

    }

    public void RestartGame()
    {
        //TODO: Reseting scene state, without reloading it

        keyItemSpawner.SpawnItems();
        itemsToCollect = keyItemSpawner.KeyItemsCount;
        keyItemName = keyItemSpawner.KeyItemName;

        mGamePaused = false;

        //Set player position and rotation deactivate player controller
        playerController.gameObject.transform.position = startPosition.position;
        playerController.gameObject.transform.rotation = startPosition.rotation;
        playerController.enabled = false;

        //Disable game ui and retry button
        gameUI.gameObject.SetActive(false);
        pauseMenu.EnableRetryButton(false);

        //Disable cameras
        primaryCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(false);

        StopAllCoroutines();

        //Start intro camera
        cameraSystem.Reset();
        StartCoroutine(WaitForStart());

        //Start pause corutine
        StartCoroutine(WaitForPause());
    }

    private IEnumerator WaitForStart()
    {
        while (cameraSystem.SystemIsActive)
        {
            if (Input.GetButtonDown(skipButton) && !mGamePaused)
            {
                cameraSystem.Skip();
                break;
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
            infoText.text = "Get ready : " + (int)timeLeft;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(StartStage());

        //Wait for 2 sec displaying the text
        infoText.text = "Go!!!";
        yield return new WaitForSeconds(2);
        infoText.enabled = false;
    }

    //Gameplay


    public void HandlePickUp(PickUp pickup)
    {
        mPlayerItemsCollected += pickup.gameObject.name.Contains(keyItemName) ? 1 : 0;

        mPlayerTime += pickup.timeValue;
        mPlayerScore += pickup.scoreValue;

        //TODO : Adding score, score multiplier etc

        //Refresh ui text
        scoreText.text = "Score: " + mPlayerScore;
        keyItemsText.text = "Key items: " + mPlayerItemsCollected + " / " + itemsToCollect;
    }

    private IEnumerator StartStage()
    {
        playerController.enabled = true;
        mPlayerScore = 0;
        mPlayerItemsCollected = 0;
        mPlayerTime = targetTime;

        gameObjectiveText.text = "Collect eggs";
        keyItemsText.text = mPlayerItemsCollected + " / " + itemsToCollect;

        //Score counting loop

        while (mPlayerTime > 0.1f)
        {
            //Refresh player timer
            mPlayerTime -= Time.deltaTime;
            timerText.text = "TimeLeft: " + (int)mPlayerTime;

            if(mPlayerTime < targetTime / 3)
            {
                timerText.color = Color.red;
            }

            //If minimal score has been reached, activate the arrow pointing to exit
            if (mPlayerItemsCollected >= itemsToCollect)
            {
                //Set stage exit pointer active
                if (!arrowPointer.gameObject.activeSelf)
                {
                    gameObjectiveText.text = "Retreat to exit";
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

            yield return null;
        }

        if (mStageWon)
        {
            arrowPointer.gameObject.SetActive(false);
            playerController.enabled = false;

            infoText.text = "You won!";
            infoText.enabled = true;

            //TODO : Score saving etc

            scoreDetailsText.text = 
                "Items collected: "+ mPlayerItemsCollected + "/" + itemsToCollect +
                "\nOverall score: " + mPlayerScore +
                "\nTime left: "+ mPlayerTime;

            //fox goes away
            //a* to the point
        }
        else
        {
            arrowPointer.gameObject.SetActive(false);
            playerController.enabled = false;

            infoText.text = "End of time";
            infoText.enabled = true;

            scoreDetailsText.text = "Stage failed: end of time";
        }

        //Wait for 2 seconds and start score screen
        yield return new WaitForSeconds(2);

        infoText.enabled = false;

        StartCoroutine(WaitForExit());
    }

    private IEnumerator WaitForExit()
    {
        //deactivate game ui
        gameUI.gameObject.SetActive(false);
        //Activate score screen
        scoreUI.SetActive(true);
        //Wait 2 sec before enablling exit to menu
        yield return new WaitForSeconds(2);

        bool exitStage = false;

        while (true)
        {
            if (Input.GetButtonDown(skipButton) && !mGamePaused)
            {
                break;
            }
            else if (Input.GetButtonDown(retryButton) && !mGamePaused)
            {
                exitStage = true;
                break;
            }
            yield return null;
        }
        if (exitStage)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            //TODO: reload scene async
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

}
