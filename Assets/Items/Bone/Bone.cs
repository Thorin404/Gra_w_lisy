﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour, IItem {

    public float throwForce;
    public ForceMode forceMode;

    public bool ItemAction(ItemController c)
    {
        Debug.Log("Throw bone");
        c.DropItem();
        GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, forceMode);
        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}