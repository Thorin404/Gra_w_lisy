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

    private bool mSpawned = false;

    void OnTriggerEnter(Collider other)
    {
        if (!mSpawned)
        {
            mSpawned = true;
            StartCoroutine(SpawnObjects(objectsNumber));
        }
    }

    IEnumerator SpawnObjects(int count)
    {
        int currentItem = count;

        while (currentItem > 0)
        {
            SpawnPrefab();
            currentItem--;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnPrefab()
    {
        Debug.Log("Spawn prefab");
        Instantiate(prefab, spawnSpotTransform.position, randomRotation ? Random.rotation : Quaternion.identity );
    }
}
