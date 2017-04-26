using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupText : MonoBehaviour
{
    public float floatTime;
    public float stayTime;
    public float floatSpeed;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FloatText());
    }

    IEnumerator FloatText()
    {
        float currentTime = floatTime;

        while(floatTime > 0)
        {
            floatTime -= Time.deltaTime;
            transform.position += new Vector3(0.0f, floatSpeed * Time.deltaTime, 0.0f);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(stayTime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
