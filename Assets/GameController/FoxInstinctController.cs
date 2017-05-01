using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxInstinctController : MonoBehaviour
{

    public string actionButton;
    public string objectsOfInterestTag;
    public float pointerDistance;
    public GameObject pointerPrefab;
    //public float slowmoTimeScale;

    private GameObject[] pickupsFound;
    private GameObject[] lastGeneratedPointers;

    private GameController mGameController;

    void Start()
    {
        mGameController = FindObjectOfType<GameController>();
    }

    //TODO : time constrain, slowmo etc

    void Update()
    {
        if (Input.GetButton(actionButton) && !mGameController.GamePaused)
        {
            if (lastGeneratedPointers == null)
            {
                pickupsFound = GameObject.FindGameObjectsWithTag(objectsOfInterestTag);
                lastGeneratedPointers = new GameObject[pickupsFound.Length];
                for (int i = 0; i < pickupsFound.Length; i++)
                {
                    GameObject gameObject = Instantiate(pointerPrefab, transform.position, Quaternion.identity) as GameObject;
                    gameObject.GetComponent<ItemPointer>().Target = pickupsFound[i].transform;
                    gameObject.transform.SetParent(this.transform);
                    lastGeneratedPointers[i] = gameObject;
                }
            }
        }
        else
        {
            if (lastGeneratedPointers != null)
            {
                for (int i = 0; i < pickupsFound.Length; i++)
                {
                    Destroy(lastGeneratedPointers[i]);
                }
                lastGeneratedPointers = null;
            }
        }
    }
}
