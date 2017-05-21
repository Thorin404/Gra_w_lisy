using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour, IItem {

    public GameObject seedPrefab;
    public int spawnCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DropSeeds()
    {
        Debug.Log("Drop seeds");
        Instantiate(seedPrefab, transform.position + transform.forward*0.5f,transform.rotation);
    }

    public bool ItemAction(ItemController controller)
    {
        --spawnCount;
        if (spawnCount >= 0)
        {
            DropSeeds();
            if((spawnCount) == 0)
            {
                controller.HoldedItem = null;
                Destroy(transform.gameObject);
            }
        }

        return true;
    }
}
