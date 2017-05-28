using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public enum TextElements { INFO, OBJECTIVE, KEY_ITEMS, SCORE };
    public enum ProgressBars { FOXPOWER, TIMER }

    private Text[] mUiTextElements;
    private ProgressBar[] mProgressBars;
    private ItemBar mItemBar;

    public ItemBar ItemBar
    {
        get
        {
            return mItemBar;
        }
    }

    // Use this for initialization
    void Awake()
    {
        mUiTextElements = GetComponentsInChildren<Text>();
        mProgressBars = GetComponentsInChildren<ProgressBar>();
        mItemBar = GetComponentInChildren<ItemBar>();

        Debug.Assert(mUiTextElements != null);
        Debug.Assert(mProgressBars != null);
        Debug.Assert(mItemBar != null);

        Instance = this;
    }

    public void SetText(TextElements element, string text)
    {
        mUiTextElements[(int)element].text = text;
    }

    public void EnableText(TextElements element, bool active)
    {
        mUiTextElements[(int)element].gameObject.SetActive(active);
    }

    public ProgressBar GetProgressBar(ProgressBars bar)
    {
        return mProgressBars[(int)bar];
    }

}
