using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStart : MonoBehaviour, ITutorialQuest {

    public Image imageOverlay;
    public float fadeSpeedPct;

    public string title;
    public string[] description;

    private bool mCompleted;
    private bool mInitCompleted;
    private int mCurrentDescriptionText;

    public PlayerController playerController;

    void Start()
    {
        Debug.Assert(playerController != null);
        mCompleted = false;
        mInitCompleted = false;
    }

    public bool Completed()
    {
        return mCompleted;
    }

    public void EndQuest()
    {
        
    }

    public string[] GetDescription()
    {
        return description;
    }

    public string GetTitle()
    {
        return title;
    }

    public bool InitComplete()
    {
        return mInitCompleted;
    }

    public void InitQuest()
    {
        //Disable player controller
        playerController.enabled = false;

        //Start fadein
        StartCoroutine(ImageFadeIn());
    }

    private IEnumerator ImageFadeIn()
    {
        Color newColor = Color.white;
        newColor.a = 1.0f;

        while(newColor.a > 0.0f)
        {
            newColor.a -= Time.deltaTime * fadeSpeedPct;
            imageOverlay.color = newColor;
            yield return null;
        }

        mInitCompleted = true;
        yield return null;
    }

    string ITutorialQuest.GetDescription()
    {
        if(mCurrentDescriptionText >= description.Length)
        {
            mCompleted = true;
            return "";
        }
        return description[mCurrentDescriptionText++];
    }
}
