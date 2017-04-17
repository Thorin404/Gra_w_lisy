using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Class is responsible for operations in pause menu which need ui confirmation + hiding restart btn
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public int mainMenuSceneIndex;

    private Button[] menuButtons;
    private Button retryButton;
    public GameController gameController;

    private int mSelectedOperation = 0;
    public int Operation
    {
        set
        {
            mSelectedOperation = value;
        }
        private get
        {
            return mSelectedOperation;
        }
    }

    //TODO: Joypad menu input

    void Start()
    {

    }

    void Update()
    {

    }

    public void ConfirmOperation()
    {
        //Debug.Log("Confirm operation " + mSelectedOperation);
        switch (mSelectedOperation)
        {
            case 0:
                RestatGame();
                break;
            case 1:
                ExitToMenu();
                break;

            default:
                break;
        }
    }

    //TODO : Async menu loading ?

    private void RestatGame()
    {
        Debug.Log("Restart");
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        gameController.RestartGame();

        //Temp : reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitToMenu()
    {
        Debug.Log("Exit");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void EnableRetryButton(bool active)
    {
        if (menuButtons == null)
        {
            menuButtons = GetComponentsInChildren<Button>();
            foreach (Button b in menuButtons)
            {
                //Debug.Log(b.gameObject.name);
                if (b.gameObject.name == "RetryButton")
                {
                    retryButton = b;
                    break;
                }
            }
        }
        retryButton.gameObject.SetActive(active);
    }

}
