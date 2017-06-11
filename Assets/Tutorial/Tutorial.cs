using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public int initialFontSize;
    public int fontAnimationSize;
    public float animationSpeed;

    public Text textToDisplay;
    public string[] mMessages;
    public float[] mMessageTime;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(mMessages != null && mMessageTime != null && textToDisplay != null);
    }

    // Update is called once per frame
    void Update()
    {
        textToDisplay.fontSize = (int)Mathf.PingPong(Time.time * animationSpeed, fontAnimationSize) + initialFontSize;

    }




}
