using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public float animMinScale;
    public float animMaxScale;
    public float animSpeed;

    public Transform arrowTarget;

    public Transform Target
    {
        set { arrowTarget = value; }
    }

    void Start()
    {

    }

    void LateUpdate()
    {
        //TODO : fancy arrow animation

        float arrowScale = Mathf.PingPong(Time.time * animSpeed, animMaxScale);
        transform.localScale = new Vector3(arrowScale + animMinScale, arrowScale + animMinScale, arrowScale + animMinScale);

        transform.LookAt(arrowTarget);
    }
}
