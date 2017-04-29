using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyItemSpawner : MonoBehaviour
{

    public GameObject keyItemPrefab;
    public Transform keyItemPositionsHolder;
    public float pctToSpawn;

    private Transform[] mKeyItemPositions;

    public int KeyItemsCount
    {
        get
        {
            if (mKeyItemPositions != null)
            {
                return (int)(mKeyItemPositions.Length * pctToSpawn);
            }
            return 0;
        }
    }
    public string KeyItemName
    {
        get
        {
            if (keyItemPrefab != null)
            {
                return keyItemPrefab.name;
            }
            return null;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SpawnItems()
    {
        if (mKeyItemPositions == null)
        {
            mKeyItemPositions = keyItemPositionsHolder.GetComponentsInChildren<Transform>();
            mKeyItemPositions = mKeyItemPositions.Skip(1).ToArray();
        }

        int itemsToSpawn = KeyItemsCount;
        System.Random random = new System.Random();
        Transform[] randomArray = mKeyItemPositions.OrderBy(x => random.Next()).ToArray();

        while (itemsToSpawn > 0)
        {
            Instantiate(keyItemPrefab, randomArray[itemsToSpawn].position, randomArray[itemsToSpawn].rotation);
            itemsToSpawn--;
        }

        Debug.Log("Key Item: ["+KeyItemName+"]x[" + KeyItemsCount+"]");
    }

    void OnDrawGizmos()
    {
        if (keyItemPositionsHolder != null)
        {
            foreach (Transform t in keyItemPositionsHolder.GetComponentsInChildren<Transform>())
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(t.position, new Vector3(1, 1, 1));
            }
        }
    }
}
