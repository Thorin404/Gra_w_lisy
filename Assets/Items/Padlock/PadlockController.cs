using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadlockController : MonoBehaviour
{
    public string playerName;
    public string triggerName;
    public float actionTime;
    public GameObject keyItem;
    public Transform keyHolder;

    private Animator mAnimator;
    private Rigidbody mRigidbody;

    private bool mActivated;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();

        Debug.Assert(mAnimator != null);
        Debug.Assert(keyItem != null);
        Debug.Assert(keyHolder != null);
        Debug.Assert(mRigidbody != null);

        mActivated = false;
    }

    private void SetKeyPosition()
    {
        keyItem.transform.SetParent(keyHolder);
        keyItem.transform.localPosition = new Vector3();
        keyItem.transform.localRotation = Quaternion.identity;
        keyItem.GetComponent<Rigidbody>().isKinematic = true;
        keyItem.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator PadlockAction()
    {
        //Set key parent
        SetKeyPosition();

        //Start animation
        mAnimator.SetTrigger(triggerName);

        //Wait for some time
        yield return new WaitForSeconds(actionTime);

        //Enable physics
        mRigidbody.isKinematic = false;

        //Deactivate padlock action
        mActivated = true;

        yield return null;
    }

    public void Unlock()
    {
        if (!mActivated)
        {
            StartCoroutine(PadlockAction());
            mActivated = true;
        }
    }


}
