using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public enum TextElements { INFO, OBJECTIVE, KEY_ITEMS, SCORE, FP_NAME, FP_VALUE, IB_NAME, IB_HINT, TIMER_NAME, TIMER_VALUE };
    public enum ProgressBars { FOXPOWER, TIMER }

    private Text[] mUiTextElements;
    private ProgressBar[] mProgressBars;
    private IEnumerator[] mTextRotationCorutines;
    private ItemBar mItemBar;

    public float wiggleSpeed;
    public float wiggleAngle;

    public float rotateSpeed;

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

        mTextRotationCorutines = new IEnumerator[mUiTextElements.Length];

        Instance = this;
    }

    void Update()
    {
        for (int i = 0; i < mUiTextElements.Length; ++i)
        {
            float rotation = Mathf.PingPong(Time.time * wiggleSpeed, 2.0f * wiggleAngle);
            mUiTextElements[i].rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, (wiggleAngle - rotation));
        }
    }

    public void RotateUiText(TextElements element)
    {
        if (mTextRotationCorutines[(int)element] == null)
        {
            mTextRotationCorutines[(int)element] = RotateText(element);
            StartCoroutine(mTextRotationCorutines[(int)element]);
        }
    }

    //Rotate text
    private IEnumerator RotateText(TextElements element)
    {
        Text textToRotate = mUiTextElements[(int)element];

        Debug.Log("Rotate" + element);

        float rotateByAngle = 0.0f;

        while (rotateByAngle < 360.0f)
        {
            textToRotate.rectTransform.Rotate(rotateByAngle, 0.0f, 0.0f);
            rotateByAngle += rotateSpeed * Time.deltaTime;
            yield return null;
        }
        Vector3 currentRotation = textToRotate.rectTransform.rotation.eulerAngles;

        textToRotate.rectTransform.rotation = Quaternion.Euler(0.0f, currentRotation.y, currentRotation.z);

        mTextRotationCorutines[(int)element] = null;

        yield return null;
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
