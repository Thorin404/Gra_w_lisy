using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{

    public Transform arrowTarget;

    public Transform SetTarget
    {
        set { arrowTarget = value; }
    }

    void Start()
    {

    }

    void LateUpdate()
    {
        //TODO : fancy arrow animation

        float arrowScale = Mathf.PingPong(Time.time / 10.0f, 0.1f);
        transform.localScale = new Vector3(arrowScale + 0.5f, arrowScale + 0.5f, arrowScale + 0.5f);

        transform.LookAt(arrowTarget);
    }
}
