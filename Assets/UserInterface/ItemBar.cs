using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBar : MonoBehaviour
{

    public Sprite defaultSprite;
    public string defaultName;
    public string defaultHint;

    private Image mUseItemSign;
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

    public bool EnableUseSign
    {
        set
        {
            mUseItemSign.enabled = value;
        }
        get
        {
            return mUseItemSign.enabled;
        }
    }

    public void SetDefault()
    {
        mProgressBarImage.fillAmount = 0.0f;
        mItemImage.sprite = defaultSprite;
        mItemName.text = defaultName;
        mItemHint.text = defaultHint;
        mUseItemSign.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        Image[] imageElements = GetComponentsInChildren<Image>();
        Debug.Assert(imageElements != null);

        mUseItemSign = imageElements[0];
        mProgressBarImage = imageElements[1];
        mItemImage = imageElements[2];

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
