using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public string pickupTag;
    public GameObject pickupText;

    private GameController mGameController;

    void Start()
    {
        mGameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == pickupTag)
        {
            CollectPickup(other);
        }
    }

    private void CollectPickup(Collider other)
    {
        Debug.Log(other.gameObject.name);
        PickUp pickup = other.gameObject.GetComponent<PickUp>();
        mGameController.pickupHandler(pickup);

        //Create new PickupText
        GameObject gameObject = Instantiate(pickupText, other.gameObject.transform.position, Quaternion.identity) as GameObject;
        gameObject.GetComponent<TextMesh>().text = pickup.nameToDisplay;

        //Destroy pickup instance
        Destroy(other.gameObject);
    }
}
