using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoxInstinctController : MonoBehaviour
{

    public string actionButton;
    public string objectsOfInterestTag;
    public float pointerDistance;

    public float maxActionTime;

    public GameObject bonusItempointerPrefab;
    public GameObject keyItempointerPrefab;
    //public float slowmoTimeScale;

    private GameObject[] mKeyItems;
    private GameObject[] mBonusItems;
    private GameObject[] mAllItems;

    private GameObject[] lastGeneratedPointers;

    private float mCurrentTime;

    void Start()
    {
        mCurrentTime = maxActionTime;
        RefreshUi();
    }

    //TODO : time constrain, slowmo, bonus pickup pointer

    void Update()
    {
        if (Input.GetButton(actionButton) && !GameController.GamePaused)
        {
            StartAction();
        }
        else
        {
            StopAction();
        }
    }

    private void StartAction()
    {
        RefreshUi();
        if (mCurrentTime > 0.0f)
        {
            mCurrentTime -= Time.deltaTime;
            GeneratePointers();
        }
        else
        {
            mCurrentTime = 0.0f;
            DestroyPointers();
        }
    }

    private void StopAction()
    {
        RefreshUi();
        if(mCurrentTime < maxActionTime)
        {
            mCurrentTime += Time.deltaTime;
        }
        else
        {
            mCurrentTime = maxActionTime;
        }
        DestroyPointers();
    }


    private void DestroyPointers()
    {
        if (lastGeneratedPointers != null)
        {
            for (int i = 0; i < mAllItems.Length; i++)
            {
                Destroy(lastGeneratedPointers[i]);
            }
            lastGeneratedPointers = null;
        }
    }

    private void CreatePrefabs(GameObject prefab,GameObject[] positions, int indexStart)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i] != null)
            {
                GameObject gameObject = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
                gameObject.GetComponent<ItemPointer>().Target = positions[i].transform;
                gameObject.transform.SetParent(this.transform);
                lastGeneratedPointers[i+indexStart] = gameObject;
            }
            else
            {
                continue;
            }
        }
    }

    private void RefreshUi()
    {
        ProgressBar progressBar = GameUI.Instance.GetProgressBar(GameUI.ProgressBars.FOXPOWER);
        float pct = mCurrentTime / maxActionTime;
        progressBar.ProgressBarPct = pct;
        progressBar.ValueText = ((int)(pct * 100.0f))+"%";
    }

    private void GeneratePointers()
    {
        if (mKeyItems == null && mBonusItems == null)
        {
            mKeyItems = KeyItemSpawner.KeyItems;
            mBonusItems = KeyItemSpawner.BonusItems;
            mAllItems = mKeyItems.Concat(mBonusItems).ToArray();
        }
        else if (lastGeneratedPointers == null && mAllItems != null)
        {
            lastGeneratedPointers = new GameObject[mAllItems.Length];
            CreatePrefabs(bonusItempointerPrefab, mBonusItems, 0);
            CreatePrefabs(keyItempointerPrefab, mKeyItems, mBonusItems.Length);
        }
    }
}
