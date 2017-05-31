using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public bool displayPathGizmos;
    public Transform[] path;
    public float speed = 2;
    public float turnSpeed = 3.0f;
    public float stoppingDist = 3;
    public float reachDist = 2;
    public int currentPoint = 0;

    void Start()
    {

    }

    void Update()
    {
        if (path[currentPoint] != null)
        {
            float dist = Vector3.Distance(path[currentPoint].position, transform.position);
            float speedPercent = 1;

            if (dist <= stoppingDist)
            {
                speedPercent = Mathf.Clamp01(dist / stoppingDist);
            }

            Quaternion targetRotation = Quaternion.LookRotation(path[currentPoint].position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);

            if (dist <= reachDist)
            {
                currentPoint++;
            }
            if (currentPoint >= path.Length)
            {
                currentPoint = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (path.Length > 0 && displayPathGizmos)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] != null)
                {
                    Gizmos.DrawSphere(path[i].position, reachDist);
                }
            }
        }
    }
}
