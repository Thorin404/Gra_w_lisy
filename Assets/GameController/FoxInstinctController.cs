using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoxInstinctController : MonoBehaviour
{

    public string actionButton;
    public string objectsOfInterestTag;
    public float pointerDistance;
    public GameObject pointerPrefab;
    //public float slowmoTimeScale;

    private GameObject[] mKeyItems;
    private GameObject[] mBonusItems;
    private GameObject[] mAllItems;

    private GameObject[] lastGeneratedPointers;

    void Start()
    {
        mKeyItems = KeyItemSpawner.KeyItems;
        mBonusItems = KeyItemSpawner.BonusItems;
        mAllItems = mKeyItems.Concat(mBonusItems).ToArray();
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

    private void GeneratePointers()
    {
        if (mAllItems != null && lastGeneratedPointers == null)
        {
            lastGeneratedPointers = new GameObject[mAllItems.Length];

            for (int i = 0; i < mAllItems.Length; i++)
            {
                if (mAllItems[i] != null)
                {
                    GameObject gameObject = Instantiate(pointerPrefab, transform.position, Quaternion.identity) as GameObject;
                    gameObject.GetComponent<ItemPointer>().Target = mAllItems[i].transform;
                    gameObject.transform.SetParent(this.transform);
                    lastGeneratedPointers[i] = gameObject;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
