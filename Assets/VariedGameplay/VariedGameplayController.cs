using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariedGameplayController : MonoBehaviour {

    [Header("Doors and key")]
    public PadlockDoorController[] doors;
    public GameObject padlockKey;
    public Transform[] keyPositions;

    //[Header("Planks / todo")]

    // Use this for initialization
    void Start()
    {
        Debug.Assert(doors != null && padlockKey != null && keyPositions != null);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RandomizeMap()
    {
        Debug.Log("Randomizing map");
        RandomDoors();
        //RandomPlanks etc
    }

    private void RandomDoors()
    {
        //Random doors
        int lockedDoorIndex = Random.Range(0, doors.Length);
        for(int i=0; i< doors.Length; i++)
        {
            if (i != lockedDoorIndex)
            {
                doors[i].SetDoorOpenOnStart();
            }
        }
        //Random key position
        int keyPositionIndex = Random.Range(0, keyPositions.Length);
        padlockKey.transform.position = keyPositions[keyPositionIndex].position;
    }


}
