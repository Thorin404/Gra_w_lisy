using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    public static IntroUI Instance;

    public enum TextElements { STAGE_NAME, TOP_SCORE_LABEL, TOP_SCORE_LIST, SKIP_TEXT };
    private Text[] mUiTextElements;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        mUiTextElements = GetComponentsInChildren<Text>();

        //foreach (Text t in mUiTextElements)
        //{
        //    Debug.Log(t.gameObject.name);
        //}
    }

    public void SetText(TextElements element, string text)
    {
        mUiTextElements[(int)element].text = text;
    }

}
