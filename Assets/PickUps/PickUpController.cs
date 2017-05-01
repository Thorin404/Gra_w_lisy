using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {

    public string pickupTag;
    public GameObject pickupText;

    private GameController mGameController;

    void Start()
    {
        mGameController = FindObjectOfType<GameController>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == pickupTag)
        {
            Debug.Log(other.gameObject.name);
            PickUp pickup = other.gameObject.GetComponent<PickUp>();
            mGameController.HandlePickUp(pickup);

            //Create new PickupText
            GameObject gameObject = Instantiate(pickupText, other.gameObject.transform.position, Quaternion.identity) as GameObject;
            gameObject.GetComponent<TextMesh>().text = pickup.nameToDisplay;

            //Destroy pickup instance
            Destroy(other.gameObject);
        }

    }
}
