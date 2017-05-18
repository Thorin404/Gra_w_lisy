using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public enum TextElements { INFO, OBJECTIVE, KEY_ITEMS, SCORE, TIMER };
    private Text[] mUiTextElements;

    // Use this for initialization
    void Awake()
    {
        mUiTextElements = GetComponentsInChildren<Text>();

        //foreach (Text t in mUiTextElements)
        //{
        //    Debug.Log(t.gameObject.name);
        //}

        Instance = this;
    }

    public void SetText(TextElements element, string text)
    {
        mUiTextElements[(int)element].text = text;
    }

    public void EnableElement(TextElements element, bool active)
    {
        mUiTextElements[(int)element].gameObject.SetActive(active);
    }

}
