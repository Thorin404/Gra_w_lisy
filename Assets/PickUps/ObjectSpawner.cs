using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public Transform spawnLocation;
    public GameObject prefab;
    public int countX;
    public int countY;
    public float offset;

    private bool mSpawned = false;

    // Use this for initialization
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!mSpawned)
        {
            SpawnPrefabs();
            mSpawned = true;
        }
    }

    public void SpawnPrefabs()
    {
        for (int z = -countX; z < countX; z++)
        {
            for (int x = -countY; x < countY; x++)
            {
                Vector3 position = new Vector3(spawnLocation.position.x + (x* offset), spawnLocation.position.y, spawnLocation.position.z + (z * offset));
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }
}
