using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

    public GameObject itemHolder;
    private bool mActive;

	// Use this for initialization
	void Start () {
        mActive = true;
	}

    void OnTriggerEnter(Collider other)
    {
        if (mActive)
        {
            Action();
            Debug.Log(other.gameObject.name);
            mActive = false;
        }    
    }

    private void Action()
    {
        Rigidbody[] objects = itemHolder.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rigbdy in objects)
        {
            rigbdy.isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
