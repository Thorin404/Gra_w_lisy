using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string actionButton;
    public string itemTagName;
    public Transform holdingTarget;

    private bool mHoldingItem = false;
    private GameObject mHoldedItem;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(actionButton) && mHoldingItem)
        {
            mHoldedItem.transform.SetParent(null);
            mHoldedItem.GetComponent<Rigidbody>().isKinematic = false;
            mHoldingItem = false;
            mHoldedItem = null;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == itemTagName)
        {
            if(Input.GetButtonDown(actionButton) && !mHoldedItem)
            {
                Debug.Log("Item controller, hold item [" + other.name + "]");

                mHoldedItem = other.gameObject;
                mHoldedItem.gameObject.transform.SetParent(holdingTarget);
                mHoldedItem.gameObject.transform.localPosition = new Vector3();
                mHoldedItem.gameObject.transform.rotation = transform.rotation;
                mHoldedItem.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                StartCoroutine(WaitForAction());
            }
        }
    }

    private IEnumerator WaitForAction()
    {
        yield return new WaitForSeconds(0.5f);
        mHoldingItem = true;
    }

}
