using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    //private Image mBgImage;
    private Image mProgressBarImage;
    private Text mBarName;
    private Text mBarValue;

    public float ProgressBarPct
    {
        set
        {
            mProgressBarImage.fillAmount = value;
        }
    }

    public string NameText
    {
        set
        {
            mBarName.text = value;
        }
    }

    public string ValueText
    {
        set
        {
            mBarValue.text = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        //mBgImage = images[0];
        mProgressBarImage = images[1];

        Debug.Assert(mProgressBarImage != null);

        Text[] textObjects = GetComponentsInChildren<Text>();

        Debug.Assert(textObjects != null);

        mBarName = textObjects[0];
        mBarValue = textObjects[1];

        Debug.Log(mBarValue.ToString() + mBarName.ToString() + "lol" + mProgressBarImage.ToString());
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
