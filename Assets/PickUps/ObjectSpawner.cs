using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform spawnSpotTransform;
    public GameObject prefab;

    public int objectsNumber;
    public float spawnInterval;
    public bool randomRotation;

    private bool mStartSpawn = false;
    private bool mSpawned = false;

    private float mCurrentTime;
    private int mCurrentObject;

    void Start()
    {
        mCurrentObject = 0;
        mCurrentTime = 0.0f;
    }

    void Update()
    {
        if (mStartSpawn && !mSpawned)
        {
            mCurrentTime += Time.deltaTime;
            if (mCurrentTime > spawnInterval)
            {
                SpawnPrefab();
                mCurrentTime = 0.0f;
                mCurrentObject++;
            }
            if(mCurrentObject > objectsNumber)
            {
                mSpawned = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!mStartSpawn && !mSpawned)
        {
            mStartSpawn = true;
        }
    }

    public void SpawnPrefab()
    {
        Debug.Log("Spawn prefab");
        Instantiate(prefab, spawnSpotTransform.position, randomRotation ? Random.rotation : Quaternion.identity );
    }
}
