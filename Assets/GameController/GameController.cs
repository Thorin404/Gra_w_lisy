using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    //Misc
    public string sceneName;
    public float gameOverTime;

    //Player related 

    public PlayerController playerController;
    public Camera primaryCamera;
    public Camera secondaryCamera;

    //Ui groups 

    public GameObject gameUI;
    public GameObject miscUI;
    public GameObject introUI;

    //Ui elements

    public Text gameStatus;
    public Text playerStatus;

    //Script references

    private CameraSystemController cameraSystem;

    //Private members
    private bool mStageStarted = false;
    private int mScore = 0;


    void Start () {
        gameStatus.text = "Collected eggs:" + mScore;

        cameraSystem = GetComponent<CameraSystemController>();

        playerController.gameObject.SetActive(false);
        primaryCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(false);
    }
	
	void Update () {
        if (mStageStarted)
        {
            StageLogic();
        }
        else if(!cameraSystem.CameraSystemIsActive)
        {
            StartStage();
        }
    }

    private void StartStage()
    {
        mStageStarted = true;
        playerController.gameObject.SetActive(true);
        gameUI.SetActive(true);
        primaryCamera.gameObject.SetActive(true);
        //miscUI.SetActive(true);
    }

    private void StageEnd()
    {
        gameStatus.text = "You won +" + mScore;
        StartCoroutine(ReloadScene());
    }

    private void StageLogic()
    {
        if (mScore > 2)
        {
            StageEnd();
        }

        //Debug.Log("Camera system is active" + cameraSystem.CameraSystemIsActive);
    }

    private IEnumerator ReloadScene()
    {
        //Wait and reload scene
        yield return new WaitForSeconds(gameOverTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void AddScore()
    {
        Debug.Log("AddScore");
        mScore += 1;
        gameStatus.text = "Collected eggs:" + mScore;
    }

    public void SetPlayerStatus(string s)
    {
        playerStatus.text = "Player: " +s;
    }

}
