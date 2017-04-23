using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public GameController gameController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnCollisionEnter "+ gameObject.name);
        if (other.gameObject.name == "Player")
        {
            Debug.Log("XDD");
            gameController.CheckpointReached = true;
        }

    }
}
