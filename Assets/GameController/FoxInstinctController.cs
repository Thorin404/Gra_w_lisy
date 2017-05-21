using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoxInstinctController : MonoBehaviour
{

    public string actionButton;
    public string objectsOfInterestTag;
    public float pointerDistance;

    public GameObject bonusItempointerPrefab;
    public GameObject keyItempointerPrefab;
    //public float slowmoTimeScale;

    private GameObject[] mKeyItems;
    private GameObject[] mBonusItems;
    private GameObject[] mAllItems;

    private GameObject[] lastGeneratedPointers;

    void Start()
    {

    }

    //TODO : time constrain, slowmo, bonus pickup pointer

    void Update()
    {
        if (Input.GetButton(actionButton) && !GameController.GamePaused)
        {
            GeneratePointers();
        }
        else
        {
            DestroyPointers();
        }
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

    private void GeneratePointers()
    {
        //if(mAllItems == null)
        //{
        //    mKeyItems = KeyItemSpawner.KeyItems;
        //    mBonusItems = KeyItemSpawner.BonusItems;
        //    mAllItems = mKeyItems.Concat(mBonusItems).ToArray();
        //}

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


        //if (mAllItems != null && lastGeneratedPointers == null)
        //{
        //    lastGeneratedPointers = new GameObject[mAllItems.Length];

        //    for (int i = 0; i < mAllItems.Length; i++)
        //    {
        //        if (mAllItems[i] != null)
        //        {
        //            GameObject gameObject = Instantiate(keyItempointerPrefab, transform.position, Quaternion.identity) as GameObject;
        //            gameObject.GetComponent<ItemPointer>().Target = mAllItems[i].transform;
        //            gameObject.transform.SetParent(this.transform);
        //            lastGeneratedPointers[i] = gameObject;
        //        }
        //        else
        //        {
        //            continue;
        //        }
        //    }
        //}
    }
}
