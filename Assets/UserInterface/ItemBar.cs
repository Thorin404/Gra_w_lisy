using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBar : MonoBehaviour {

    public Sprite defaultSprite;
    public string defaultName;
    public string defaultHint;

    private Image mItemImage;
    private Text mItemName;
    private Text mItemHint;

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
        mItemImage.sprite = defaultSprite;
        mItemName.text = defaultName;
        mItemHint.text = defaultHint;
    }

	// Use this for initialization
	void Start () {
        mItemImage = GetComponentInChildren<Image>();
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
