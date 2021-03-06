﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyItemSpawner : MonoBehaviour
{
    public GameObject[] keyItemPrefabs;
    public Transform keyItemPositionsHolder;
    public float keyItemsToSpawnPct;

    public GameObject[] bonusItemsPrefabs;
    public Transform bonusItemPositionsHolder;
    public float bonusItemsToSpawnPct;

    //Tools
    public GameObject[] toolItems;

    //Use item hints
    public GameObject[] toolUsageItems;

    private Transform[] mKeyItemsPositions;
    private Transform[] mBonusItemsPositions;

    private static GameObject[] mKeyItems;
    private static GameObject[] mBonusItems;
    private static GameObject[] mToolItems;
    private static GameObject[] mToolUsageItems;

    private int mKeyItemsCount = -1;
    private int mBonusItemsCount = -1;

    public static GameObject[] ToolItems
    {
        get
        {
            return mToolItems;
        }
    }

    public static GameObject[] ToolUsageItems
    {
        get
        {
            return mToolUsageItems;
        }
    }

    public static GameObject[] KeyItems
    {
        get
        {
            mKeyItems = RemoveEmptyElements(mKeyItems);
            return mKeyItems;
        }
    }
    public static GameObject[] BonusItems
    {
        get
        {
            mBonusItems = RemoveEmptyElements(mBonusItems);
            return mBonusItems;
        }
    }

    public int KeyItemsCount
    {
        get { return mKeyItemsCount; }
    }
    public int BonusItemsCount
    {
        get { return mBonusItemsCount; }
    }

    public string KeyItemName
    {
        get
        {
            if (keyItemPrefabs != null)
            {
                return keyItemPrefabs[0].name;
            }
            return null;
        }
    }

    public void Start()
    {
        Debug.Assert(toolItems != null && toolUsageItems != null);

        mToolItems = new GameObject[toolItems.Length];
        toolItems.CopyTo(mToolItems, 0);

        mToolUsageItems = new GameObject[toolUsageItems.Length];
        toolUsageItems.CopyTo(mToolUsageItems, 0);
    }

    private static GameObject[] RemoveEmptyElements(GameObject[] array)
    {
        //Filter empty array elements 
        if (array != null)
        {
            return array.Where(x => x != null).ToArray();
        }
        return null;
    }

    public void Reset()
    {
        mKeyItemsPositions = CreatePositionsArray(keyItemPositionsHolder);
        mKeyItemsCount = (int)(mKeyItemsPositions.Length * keyItemsToSpawnPct);

        mBonusItemsPositions = CreatePositionsArray(bonusItemPositionsHolder);
        mBonusItemsCount = (int)(mBonusItemsPositions.Length * bonusItemsToSpawnPct);

        mKeyItems = null;
        mBonusItems = null;
    }

    private Transform[] CreatePositionsArray(Transform holder)
    {
        Transform[] positions = holder.GetComponentsInChildren<Transform>();
        positions = positions.Skip(1).ToArray();
        return positions;
    }

    public void SpawnItems()
    {
        if (mKeyItems == null)
        {
            mKeyItems = SpawnPrefabsRandomly(mKeyItemsPositions, keyItemPrefabs, mKeyItemsCount);
        }
        if (mBonusItems == null)
        {
            mBonusItems = SpawnPrefabsRandomly(mBonusItemsPositions, bonusItemsPrefabs, mBonusItemsCount);
        }
    }

    private GameObject[] SpawnPrefabsRandomly(Transform[] positions, GameObject[] prefab, int spawnCount)
    {
        GameObject[] spawnedObjects = new GameObject[spawnCount];

        System.Random random = new System.Random();
        Transform[] randomArray = positions.OrderBy(x => random.Next()).ToArray();

        for (int i = 0; i < spawnCount; i++)
        {
            spawnedObjects[i] = Instantiate(prefab[random.Next(0, prefab.Length)], randomArray[i].position, randomArray[i].rotation);
        }
        return spawnedObjects;
    }

    void OnDrawGizmos()
    {
        if (keyItemPositionsHolder != null)
        {
            foreach (Transform t in keyItemPositionsHolder.GetComponentsInChildren<Transform>())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(t.position, new Vector3(1, 1, 1));
            }
            foreach (Transform t in bonusItemPositionsHolder.GetComponentsInChildren<Transform>())
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(t.position, new Vector3(1, 1, 1));
            }

        }
    }
}
