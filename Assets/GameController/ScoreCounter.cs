using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private int mPlayerScore;
    private float mPlayerTime;

    private string mKeyItemName;
    private int mItemsToCollect;
    private int mPlayerItemsCollected;

    public float PlayerTime
    {
        get { return mPlayerTime; }
    }

    public float SubtractTime
    {
        set
        {
            mPlayerTime -= value;
            GameUI.Instance.SetText(GameUI.TextElements.TIMER, "TimeLeft: " + (int)mPlayerTime);
        }
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePickUp(PickUp pickup)
    {
        mPlayerItemsCollected += pickup.gameObject.name.Contains(mKeyItemName) ? 1 : 0;

        mPlayerTime += pickup.timeValue;
        mPlayerScore += pickup.scoreValue;

        //TODO : Adding score, score multiplier etc

        //Refresh ui text
        GameUI.Instance.SetText(GameUI.TextElements.SCORE, "Score: " + mPlayerScore);
        GameUI.Instance.SetText(GameUI.TextElements.KEY_ITEMS, "Key items: " + mPlayerItemsCollected + " / " + mItemsToCollect);
    }

    public void ResetCounter(int itemsToCollect, string keyItemName, float playerTime)
    {
        mItemsToCollect = itemsToCollect;
        mKeyItemName = keyItemName;
        mPlayerTime = playerTime;

        mPlayerScore = 0;
        mPlayerItemsCollected = 0;

        GameUI.Instance.SetText(GameUI.TextElements.KEY_ITEMS, mPlayerItemsCollected + " / " + mItemsToCollect);
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

    public void SaveScore(string stageName)
    {
        if (ScoreUI.Instance.inputField.gameObject.gameObject.activeSelf)
        {
            string name = ScoreUI.Instance.inputField.text.Length > 0 ? ScoreUI.Instance.inputField.text : "Player";
            GameData.Instance.GetData.GetLevelSave(stageName).AddScore(name, mPlayerScore * (int)mPlayerTime);
        }
    }
}
