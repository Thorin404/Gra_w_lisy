using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public interface ITutorialAction
{
    bool GetState();
    void TriggerAction();
}


public class Tutorial : MonoBehaviour
{
    public int initialFontSize;
    public int fontAnimationSize;
    public float animationSpeed;
    public string messageSkipButton;

    public float characterTime;

    public Text messageTitle;
    public Text messageDescription;

    public string[] mMessages;
    public string[] mMessagesDescriptions;
    public float[] mMessageTime;

    private string mCurrentMessageString;
    private bool mPrintingMessage;
    private bool mMessageEnd;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(mMessages != null && mMessageTime != null && messageTitle != null);
        Debug.Assert(mMessages.Length == mMessageTime.Length);

        mPrintingMessage = false;
        mMessageEnd = false;
        StartCoroutine(PrintMessages());
    }

    private IEnumerator PrintMessages()
    {
        for (int i = 0; i < mMessages.Length; ++i)
        {
            mMessageEnd = false;
            messageTitle.text = mMessages[i];

            StartCoroutine(PrintMessageDescription(i));

            while (mPrintingMessage)
            {
                yield return null;
            }

            yield return new WaitForSeconds(mMessageTime[i]);

            //Wait until message is skipped
            while (!mMessageEnd)
            {
                yield return null;
            }
        }

        messageTitle.text = "";
        messageDescription.text = "";

        yield return null;
    }

    private IEnumerator PrintMessageDescription(int messageIndex)
    {
        mPrintingMessage = true;
        messageDescription.fontStyle = FontStyle.Normal;
        mCurrentMessageString = "";

        for (int i=0; i< mMessagesDescriptions[messageIndex].Length; ++i)
        {
            mCurrentMessageString += mMessagesDescriptions[messageIndex][i];
            messageDescription.text = mCurrentMessageString;
            yield return new WaitForSeconds(characterTime);
        }

        messageDescription.fontStyle = FontStyle.Italic;
        mPrintingMessage = false;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        messageTitle.fontSize = (int)Mathf.PingPong(Time.time * animationSpeed, fontAnimationSize) + initialFontSize;

        if (Input.GetButtonDown(messageSkipButton))
        {
            mMessageEnd = true;
        }

    }




}
