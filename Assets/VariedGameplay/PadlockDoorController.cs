using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadlockDoorController : MonoBehaviour {

    public PadlockController padlock;
    public SimpleDoor doorController;

    public void SetDoorOpenOnStart()
    {
        padlock.Deactivate();
        padlock.gameObject.SetActive(false);
        doorController.SetOpen();
    }

	// Use this for initialization
	void Start () {
        Debug.Assert(padlock != null && doorController != null);
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
