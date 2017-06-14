﻿using System.Collections;
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
	public float eatingTime;

    public int currentPoint = 0;

	int seedsDone = 0;

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
			Quaternion targetRotation;
			if (isChicken) {
				/*if (GameObject.FindGameObjectsWithTag("Seeds").Length) {
					Vector3 chickenSeeds = GameObject.FindWithTag ("Seeds").transform.position;
					targetRotation = Quaternion.LookRotation (chickenSeeds - transform.position);
					transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
					if ((Vector3.Distance (chickenSeeds, transform.position)) <= 1) {
						StartCoroutine (EatingSeeds ());
					}
				}else {/
					Vector3 chickenWandering = new Vector3 (Random.Range (wanderXMin, wanderXMax), 0, Random.Range (wanderZMin, wanderZMax));
					targetRotation = Quaternion.LookRotation (chickenWandering - transform.position);
					transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				//}*/
				switch (GameObject.FindGameObjectsWithTag("Seeds").Length){
				case 1: 
					GoingToSeeds ();
					break;
				default:
					Vector3 chickenWandering = new Vector3 (Random.Range (wanderXMin, wanderXMax), 0, Random.Range (wanderZMin, wanderZMax));
					targetRotation = Quaternion.LookRotation (chickenWandering - transform.position);
					transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
					break;
				}
			} else {
				targetRotation = Quaternion.LookRotation (path [currentPoint].position - transform.position);
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

	void GoingToSeeds(){
		Quaternion targetRotation;
		Vector3 chickenSeeds = GameObject.FindWithTag ("Seeds").transform.position;
		targetRotation = Quaternion.LookRotation (chickenSeeds - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
		if ((Vector3.Distance (chickenSeeds, transform.position)) <= 1) {
			StartCoroutine (EatingSeeds ());
		}
	}

	IEnumerator EatingSeeds(){
		speed = 0;
		turnSpeed = 0;
		yield return new WaitForSeconds (eatingTime);
		GameObject tsd = GameObject.FindGameObjectWithTag ("Seeds");
		Destroy (tsd);
		seedsDone++;
		speed = chasingSpeed;
		turnSpeed = chasingTurn;
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
