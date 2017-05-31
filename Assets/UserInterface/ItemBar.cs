using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBar : MonoBehaviour
{

    public Sprite defaultSprite;
    public string defaultName;
    public string defaultHint;

    private Image mProgressBarImage;
    private Image mItemImage;
    private Text mItemName;
    private Text mItemHint;

    public float ProgressBarPct
    {
        set
        {
            mProgressBarImage.fillAmount = value;
        }
    }

    public string ItemName
    {
        set
        {
            mItemName.text = (value == null ? "" : value);
        }
    }

    public string ItemHint
    {
        set
        {
            mItemHint.text = (value == null ? "" : value);
        }
    }

    public Sprite ItemSprite
    {
        set
        {
            mItemImage.sprite = (value == null ? defaultSprite : value);
        }
    }

    public void SetDefault()
    {
        mProgressBarImage.fillAmount = 0.0f;
        mItemImage.sprite = defaultSprite;
        mItemName.text = defaultName;
        mItemHint.text = defaultHint;
    }

    // Use this for initialization
    void Start()
    {
        Image[] imageElements = GetComponentsInChildren<Image>();
        Debug.Assert(imageElements != null);

        mProgressBarImage = imageElements[0];
        mItemImage = imageElements[1];

        Text[] textElements = GetComponentsInChildren<Text>();
        Debug.Assert(textElements != null);

        mItemName = textElements[0];
        mItemHint = textElements[1];

        SetDefault();
    }

    // Update is called once per frame
    //void Update () {

    //}
}
