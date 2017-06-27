using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private int mPlayerScore;
    private float mPlayerTime;

    private string mKeyItemName;
    private int mItemsToCollect;
    private float mTargetTime;
    private int mPlayerItemsCollected;

    private GameSoundController mSoundController;

    public float PlayerTime
    {
        get { return mPlayerTime; }
    }

    public float SubtractTime
    {
        set
        {
            mPlayerTime -= value;
            //Set timer ui

            //Rotate timer text each 10 sec TODO : parametrize
            if (((int)mPlayerTime % 10) == 0)
            {
                GameUI.Instance.RotateUiText(GameUI.TextElements.TIMER_VALUE);
            }

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //Update timer
        ProgressBar timeBar = GameUI.Instance.GetProgressBar(GameUI.ProgressBars.TIMER);
        timeBar.ValueText = (int)mPlayerTime + "";
        timeBar.ProgressBarPct = mPlayerTime / mTargetTime;

        //GameUI.Instance.SetText(GameUI.TextElements.SCORE, "Score: " + mPlayerScore);
        //GameUI.Instance.SetText(GameUI.TextElements.KEY_ITEMS, "Key items: " + mPlayerItemsCollected + " / " + mItemsToCollect);


        //GameUI.Instance.SetText(GameUI.TextElements.KEY_ITEMS, mPlayerItemsCollected + " / " + mItemsToCollect);
    }

    public bool PlayerHasTime
    {
        get { return mPlayerTime > 0.1f; }
    }

    public bool ItemsCollected
    {
        get { return (mPlayerItemsCollected >= mItemsToCollect); }
    }

    // Use this for initialization
    void Start()
    {
        mSoundController = GetComponent<GameSoundController>();
        Debug.Assert(mSoundController != null);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePickUp(PickUp pickup)
    {
        //Play sound
        mSoundController.PlayGameSound(GameSoundController.GameSounds.PICKUP_COLLECTED);

        bool isKeyItem = pickup.gameObject.name.Contains(mKeyItemName);
        if (isKeyItem)
        {
            mPlayerItemsCollected +=1;
            GameUI.Instance.Objectives.IncrementObjetiveCounter();
        }
      

        mPlayerTime += pickup.timeValue;
        mPlayerScore += pickup.scoreValue;

        //TODO : Adding score, score multiplier etc

        UpdateUI();
    }

    public void ResetCounter(int itemsToCollect, string keyItemName, float targetTime)
    {
        mItemsToCollect = itemsToCollect;
        mKeyItemName = keyItemName;
        mTargetTime = targetTime;

        mPlayerTime = mTargetTime;
        mPlayerScore = 0;
        mPlayerItemsCollected = 0;

        GameUI.Instance.Objectives.CreateObjectives(itemsToCollect);

        //GameUI.Instance.SetText(GameUI.TextElements.OBJECTIVE, "Collect "+ keyItemName+"s");
        //ProgressBar progressBar = GameUI.Instance.GetProgressBar(GameUI.ProgressBars.FOXPOWER);
        //progressBar.ValueText = "100%";

        UpdateUI();
    }

    public void SetScoreText()
    {
        Debug.Log(ScoreUI.Instance);
        ScoreUI.Instance.scoreText.text =
                "Items collected: " + mPlayerItemsCollected + "/" + mItemsToCollect +
                "\nScore: " + mPlayerScore +
                "\nTime left: " + mPlayerTime +
                "\nSummary: " + mPlayerScore * (int)mPlayerTime;
    }

    public void SaveScore(string stageName, bool stageWon)
    {
        if (stageWon)
        {
            string name = ScoreUI.Instance.inputField.text.Length > 0 ? ScoreUI.Instance.inputField.text : "Player";
            GameData.Instance.GetData.GetLevelSave(stageName).AddScore(name, mPlayerScore * (int)mPlayerTime);
        }
    }
}
