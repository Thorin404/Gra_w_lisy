using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{

    public string collidingObject = "Player";
    public Transform lineStart;

    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, lineStart.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == collidingObject)
        {
            lineRenderer.SetPosition(1, other.transform.position);
            lineRenderer.enabled = true;

        }
    }
    void OnTriggerExit(Collider other)
    {
        lineRenderer.enabled = false;
    }

}
