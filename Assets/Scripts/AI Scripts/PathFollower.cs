using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public bool displayPathGizmos;
	public bool isChicken;

    public Transform[] path;
    public float speed = 2;
	public float chasingSpeed = 3;
    public float turnSpeed = 3;
	public float chasingTurn = 4;
    public float stoppingDst = 2;
	public float reachDst = 1;
	public float wanderXMin;
	public float wanderXMax;
	public float wanderZMin;
	public float wanderZMax;

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
				speedPercent = dist / stoppingDst;
			}
			if (dist <= reachDst) {
				speedPercent = 0;
			}

			if (isChicken) {
				Vector3 chickenWandering = new Vector3 (Random.Range (wanderXMin, wanderXMax), 0, Random.Range (wanderZMin, wanderZMax));
				Quaternion targetRotation = Quaternion.LookRotation (chickenWandering - transform.position);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
			} else {
				Quaternion targetRotation = Quaternion.LookRotation (path [currentPoint].position - transform.position);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
			}
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
