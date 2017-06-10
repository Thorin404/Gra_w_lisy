using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public bool displayPathGizmos;
    public Transform[] path;
    public float speed = 2;
	public float chasingSpeed = 3;
    public float turnSpeed = 3;
	public float chasingTurn = 4;
    public float stoppingDst = 2;
	public float reachDst = 1;
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

			if (dist <= stoppingDst) {
				speedPercent = 0.5f;
			}
			if (dist <= reachDst) {
				speedPercent = 0;
			}

            Quaternion targetRotation = Quaternion.LookRotation(path[currentPoint].position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);

            if (dist <= reachDst)
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
                    Gizmos.DrawSphere(path[i].position, reachDst);
                }
            }
        }
    }
}
