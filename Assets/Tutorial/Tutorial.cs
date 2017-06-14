using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public interface ITutorialQuest
{
    void InitQuest();
    bool InitComplete();
    void EndQuest();

    string GetTitle();
    string GetDescription();

    bool Completed();
    bool TaskCompleted();
}

public class Tutorial : MonoBehaviour
{
    public int initialFontSize;
    public int fontAnimationSize;
    public float animationSpeed;
    public string messageSkipButton;
    public float characterTime;

    //Play basic sound after printing specific number of characters, then special sound at the end of a sentence.
    public int characterSound;
    public bool randomizedSounds;
    private int mCharacterSoundOffset;

    public AudioClip character;
    public AudioClip sentenceEnd;
    private AudioSource mAudioSource;

    public Text questTitle;
    public Text questDescription;
    public Text skipText;

    public GameObject[] mQuestToComplete;
    public float questInterval;
    public int mainMenuSceneIndex;
    public float exitTime;

    private bool mAnimateTitle;

    private string mCurrentMessageString;
    private bool mPrintingMessage;
    private bool mMessageEnd;

    // Use this for initialization
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAudioSource != null && character != null && sentenceEnd != null);
        Debug.Assert(mQuestToComplete != null);
        Debug.Assert(skipText != null);

        mAnimateTitle = false;

        skipText.text = "(" + messageSkipButton + ") next.";
        skipText.enabled = false;

        mCharacterSoundOffset = characterSound;

        //mPrintingMessage = false;
        //mMessageEnd = false;
        //StartCoroutine(PrintMessages());
        StartCoroutine(StartQuests());
    }

    private IEnumerator StartQuests()
    {
        foreach (GameObject questObj in mQuestToComplete)
        {
            if (questObj == null)
            {
                Debug.Log("No game object...");
                continue;
            }

            ITutorialQuest quest = questObj.GetComponent<ITutorialQuest>();
            if (quest == null)
            {
                Debug.Log("No quest script in object...");
                continue;
            }
            else
            {
                Debug.Log("Starting quest");

                //Init quest
                quest.InitQuest();

                //Title
                questTitle.enabled = true;
                questTitle.text = quest.GetTitle();
                questDescription.text = "";

                //Wait for quest init to complete
                while (!quest.InitComplete())
                {
                    yield return null;
                }

                //Start title animation
                mAnimateTitle = true;

                //Print description
                StartCoroutine(PrintQuestDescription(quest));

                //Wait for quest to complete
                while (!quest.Completed())
                {
                    yield return null;
                }

                //StopTitleAnimation
                mAnimateTitle = false;
            }
            Debug.Log("Stop quest");

            questTitle.enabled = false;
            skipText.enabled = false;

            quest.EndQuest();

            //Wait some time between quests
            yield return new WaitForSeconds(questInterval);

        }
        //Wait for sime time before loading menu scene
        yield return new WaitForSeconds(exitTime);

        LoadMenuScene();

        yield return null;
    }

    private IEnumerator PrintQuestDescription(ITutorialQuest quest)
    {
        //Loop quest messages
        while (!quest.Completed())
        {
            string getCurrentMessage = quest.GetDescription();

            //Print message
            mMessageEnd = false;
            questDescription.fontStyle = FontStyle.Normal;
            mCurrentMessageString = "";

            mAudioSource.clip = character;

            for (int i = 0; i < getCurrentMessage.Length; i++)
            {
                yield return new WaitForSeconds(characterTime);

                //Append single character
                mCurrentMessageString += getCurrentMessage[i];
                questDescription.text = mCurrentMessageString;

                //Play sound 
                if ((i % mCharacterSoundOffset) == 0)
                {
                    mAudioSource.Play();
                    mCharacterSoundOffset = randomizedSounds ? UnityEngine.Random.Range(2, characterSound) : characterSound;
                }

                //Skipping whole message
                if (mMessageEnd)
                {
                    questDescription.text = getCurrentMessage;
                    mMessageEnd = false;
                    break;
                }
            }

            mAudioSource.clip = sentenceEnd;
            mAudioSource.Play();

            questDescription.fontStyle = FontStyle.Italic;

            //Wait for task to finish
            while (!quest.TaskCompleted())
            {
                yield return new WaitForFixedUpdate();
            }

            skipText.enabled = true;

            //Wait for skip buttons
            while (!mMessageEnd)
            {
                yield return null;
            }

            //Reset description text and hide skip message
            questDescription.text = "";
            skipText.enabled = false;

        }

        yield return null;
    }

    void Update()
    {
        questTitle.fontSize = mAnimateTitle ? (int)Mathf.PingPong(Time.time * animationSpeed, fontAnimationSize) + initialFontSize : initialFontSize;

        //Skip message
        if (Input.GetButtonDown(messageSkipButton))
        {
            mMessageEnd = true;
        }

    }

    private void LoadMenuScene()
    {
        Debug.Log("Load menu scene");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
