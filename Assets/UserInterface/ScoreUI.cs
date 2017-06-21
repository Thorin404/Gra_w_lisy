using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;

    public GameObject mPlayerNameInputBox;
    public Text mScoreDetailsText;
    public InputField mInputField;

    public InputField inputField
    {
        get
        {
            if (mInputField != null)
            {
                return mInputField;
            }
            return null;
        }
    }

    public void EnableBestScoreInput(bool enable)
    {
        mPlayerNameInputBox.SetActive(enable);
    }

    public Text scoreText
    {
        get
        {
            if (mScoreDetailsText != null)
            {
                return mScoreDetailsText;
            }
            return null;
        }
    }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        //mScoreDetailsText = GetComponentInChildren<Text>();
        //mInputField = GetComponentInChildren<InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
