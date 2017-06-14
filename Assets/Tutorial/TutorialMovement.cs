using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovement : MonoBehaviour, ITutorialQuest
{

    public string title;
    public string[] description;

    public GameObject[] objectsToSpawn;

    private enum MovementTutorialState { Intro, GoToPoint, End };
    private MovementTutorialState mCurrentState;

    private bool mCompleted;
    private bool mInitCompleted;
    private int mCurrentDescription;
    private bool mTaskCompleted;

    public PlayerController playerController;

    private bool mPointReached;

    public void Start()
    {
        Debug.Assert(playerController != null && objectsToSpawn != null);

        mCompleted = false;
        mInitCompleted = false;
        mTaskCompleted = false;
        mCurrentState = MovementTutorialState.Intro;
    }

    private void EnableQuestObjects(bool active)
    {
        foreach (GameObject obj in objectsToSpawn)
        {           
            obj.SetActive(active);
        }
    }

    public void InitQuest()
    {
        mCurrentState = MovementTutorialState.Intro;
        playerController.enabled = true;
        mCurrentDescription = 0;
        EnableQuestObjects(true);
    }

    public bool InitComplete()
    {
        
        mInitCompleted = true;

        return mInitCompleted;
    }

    public void EndQuest()
    {
        //EnableQuestObjects(false);
    }

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description[mCurrentDescription];
    }

    public bool Completed()
    {
        return mCompleted;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains(playerController.gameObject.name) && mCurrentDescription == 2)
        {
            mPointReached = true;
        }
    }

    private void ChangeState(MovementTutorialState state, int descriptionIndex)
    {
        mCurrentDescription = descriptionIndex;
        mCurrentState = state;
        mTaskCompleted = true;
    }

    private void NextMessage()
    {
        mCurrentDescription++;
        mTaskCompleted = true;
    }

    public bool TaskCompleted()
    {
        mTaskCompleted = false;

        switch (mCurrentState)
        {
            case MovementTutorialState.Intro:
                if (mCurrentDescription == 1)
                {
                    ChangeState(MovementTutorialState.GoToPoint, 2);
                }
                else
                {
                    NextMessage();
                    mPointReached = false;
                }
                break;
            case MovementTutorialState.GoToPoint:
                if (mPointReached)
                {
                    mPointReached = false;
                    NextMessage();
                }
                else if(mCurrentDescription == 3)
                {

                    ChangeState(MovementTutorialState.End, 4);
                }
       
                break;
            case MovementTutorialState.End:
                
                mCompleted = true;

                break;
        }

        return mTaskCompleted;
    }
}