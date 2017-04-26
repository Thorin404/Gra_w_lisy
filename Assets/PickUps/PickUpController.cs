using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {

    public GameController gameController;
    public string pickupTag;
    public GameObject pickupText;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == pickupTag)
        {
            Debug.Log(other.gameObject.name);
            PickUp pickup = other.gameObject.GetComponent<PickUp>();
            gameController.HandlePickUp(pickup);

            //Create new PickupText
            GameObject gameObject = Instantiate(pickupText, other.gameObject.transform.position, Quaternion.identity) as GameObject;
            gameObject.GetComponent<TextMesh>().text = pickup.nameToDisplay;

            //Destroy pickup instance
            Destroy(other.gameObject);
        }

    }
}
