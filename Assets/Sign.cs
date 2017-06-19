using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{

    public MeshRenderer mSignTextRenderer;

    void Start()
    {
        Debug.Assert(mSignTextRenderer != null);

        mSignTextRenderer.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        mSignTextRenderer.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        mSignTextRenderer.enabled = false;
    }
}
