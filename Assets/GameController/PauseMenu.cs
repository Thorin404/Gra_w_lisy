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
    public enum PauseMenuButtons { RESUME, RETRY, EXIT };
    public enum ConfirmationDialogButtons { YES, NO };

    public int mainMenuSceneIndex;
    public GameObject[] menuLayers;

    private Button[] mMenuButtons;
    //private Button[] mPanelButtons;

    public static PauseMenu Instance;

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

    void Awake()
    {
		Instance = this;
        mMenuButtons = menuLayers[0].GetComponentsInChildren<Button>();
        //mPanelButtons = menuLayers[1].GetComponentsInChildren<Button>();

        menuLayers[1].SetActive(false);
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

        //Temp : reload entire scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitToMenu()
    {
        Debug.Log("Exit");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void EnableButton(PauseMenuButtons button, bool active)
    {
        mMenuButtons[(int)button].gameObject.SetActive(active);
    }

}
