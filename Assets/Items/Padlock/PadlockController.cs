using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadlockController : MonoBehaviour
{
    public string playerTag;
    public string itemTagName;
    public string keyItemName;
    public Transform keyTarget;
    public Animator mLockedAnimator;

    private Rigidbody mRigidBody;
    private bool mIsLocked = true;

    // Use this for initialization
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        mLockedAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == playerTag && mIsLocked)
        {
            ItemController playerItemController = other.gameObject.GetComponent<ItemController>();
            GameObject itemHoldedByPlayer = playerItemController.HoldedItem;

            if (itemHoldedByPlayer != null)
            {
                if (itemHoldedByPlayer.tag == itemTagName && itemHoldedByPlayer.name.Contains(keyItemName))
                {
                    SetKeyPosition(itemHoldedByPlayer);

                    playerItemController.HoldedItem = null;

                    gameObject.tag = itemTagName;
                    mRigidBody.isKinematic = false;
                    mIsLocked = false;
                    this.enabled = false;

                    //Enable locked animator
                    mLockedAnimator.enabled = true;
                }
            }
        }

    }

    private void SetKeyPosition(GameObject key)
    {
        key.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        key.gameObject.GetComponent<Collider>().enabled = false;
        key.gameObject.transform.SetParent(keyTarget);
        key.gameObject.transform.localPosition = new Vector3();
        key.gameObject.transform.rotation = keyTarget.rotation;
        key.gameObject.tag = "UsedItem";
    }
}
