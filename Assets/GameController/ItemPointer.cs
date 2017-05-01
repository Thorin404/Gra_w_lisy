using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPointer : MonoBehaviour
{
    public Transform pointerObject;

    public float snapDistanceSq;
    public float snapSpeed;
    public Vector3 snapPosition;

    public Transform arrowTarget;
    public Transform Target
    {
        set { arrowTarget = value; }
    }

    private bool mTargetReached = false;

    void Start()
    {
        pointerObject.localPosition = new Vector3(0.0f, 0.0f, 2.0f);
    }

    IEnumerator MoveToTarget(Vector3 point, float speed)
    {
        while (transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    void LateUpdate()
    {
        //TODO: pointer animations

        if (arrowTarget == null)
        {
            Destroy(this.gameObject);
        }

        if (!mTargetReached)
        {
            Vector3 distanceToTarget = (transform.position - arrowTarget.position);
            if (distanceToTarget.sqrMagnitude < snapDistanceSq)
            {
                transform.SetParent(arrowTarget);
                mTargetReached = true;

                pointerObject.localPosition = new Vector3();
                StartCoroutine(MoveToTarget(arrowTarget.transform.position + snapPosition, snapSpeed));
            }
        }
        transform.LookAt(arrowTarget, Vector3.up);
    }
}
