using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    //private Image mBgImage;
    public Image mProgressBarImage;
    public Text mBarName;
    public Text mBarValue;

    public float ProgressBarPct
    {
        set
        {
            if (mProgressBarImage != null)
            {
                mProgressBarImage.fillAmount = value;
            }
        }
    }

    public string NameText
    {
        set
        {
            if (mBarName != null)
            {
                mBarName.text = value;
            }
        }
    }

    public string ValueText
    {
        set
        {
            if (mBarValue != null)
            {
                mBarValue.text = value;
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        //Image[] images = GetComponentsInChildren<Image>();
        ////mBgImage = images[0];
        //mProgressBarImage = images[0];

        //Debug.Assert(mProgressBarImage != null);

        //Text[] textObjects = GetComponentsInChildren<Text>();

        //Debug.Assert(textObjects != null);

        //mBarName = textObjects[0];
        //mBarValue = textObjects[1];

        //Debug.Log(mBarValue.ToString() + mBarName.ToString() + "lol" + mProgressBarImage.ToString());
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
