using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .5f;

	public bool displayUnitGizmos;
	public Transform[] target;
	public float speed = 2;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 1.0f;
	public float reachDist = 1.0f;

	public int currentTarget = 0;

	Path path;

	void Start() {
		StartCoroutine (UpdatePath ());
	}

	void Update() {
		bool running = Input.GetKey(KeyCode.Q);
		float dist = Vector3.Distance (target [currentTarget].position, transform.position);

		if (running) {
			currentTarget++;
		}
		if (dist <= reachDist) {
			currentTarget++;
		}
		if (currentTarget >= target.Length - 1) {
			currentTarget = 0;
		}
	}

	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			path = new Path(waypoints, transform.position, turnDst, stoppingDst);
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
		PathRequestManager.RequestPath (transform.position, target[currentTarget].position, OnPathFound);

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target[currentTarget].position;

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			if ((target[currentTarget].position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath (transform.position, target[currentTarget].position, OnPathFound);
				targetPosOld = target[currentTarget].position;
			}
		}
	}

	IEnumerator FollowPath() {

		bool followingPath = true;
		int pathIndex = 0;

		while (followingPath) {
			Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);
			while (path.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {
				Quaternion targetRotation = Quaternion.LookRotation (path.lookPoints [pathIndex] - transform.position);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				transform.Translate (Vector3.forward * Time.deltaTime * speed, Space.Self);
			}

			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null && displayUnitGizmos) {
			path.DrawWithGizmos ();
		}
		if (target.Length > 0 && displayUnitGizmos) {
			for (int i = 0; i < target.Length; i++) {
				if (target [i] != null) {
					Gizmos.DrawSphere (target [i].position, reachDist);
				}
			}
		}
	}
}
