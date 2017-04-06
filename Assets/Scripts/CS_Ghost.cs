using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Ghost : MonoBehaviour {
	enum Status {
		Idle,
		Find
	}

	[SerializeField] float myVelocity = 3;
	private Status myStatus = Status.Idle;
	private Vector3 myDirection;

	[SerializeField] Transform[] myWayPoints;
	[SerializeField] float myArriveDistance = 0.1f;
	private int myNextWayPoint = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateFind ();

		UpdateMove ();
	}

	private void UpdateMove () {
		if (myStatus == Status.Find) {
			myDirection = (CS_GhostTarget.Instance.transform.position - this.transform.position).normalized;
			this.GetComponent<Rigidbody> ().velocity = myDirection * myVelocity;
		}

		if (myStatus == Status.Idle) {
			myDirection = (myWayPoints [myNextWayPoint].position - this.transform.position).normalized;
			this.GetComponent<Rigidbody> ().velocity = myDirection * myVelocity;
			if ((myWayPoints [myNextWayPoint].position - this.transform.position).sqrMagnitude < myArriveDistance) {
				myNextWayPoint = (myNextWayPoint + 1) % myWayPoints.Length;
			}
		}
	}

	private void UpdateFind () {
		float myVisionDistance = 50;
		Ray ray = new Ray (this.transform.position, CS_GhostTarget.Instance.transform.position - this.transform.position);
		RaycastHit hit;
		//		int t_layerMask = (int) Mathf.Pow (2, 8); //for the layer you want to do the raycast
		//		if (Physics.Raycast (ray, out hit, myVisionDistance, t_layerMask))
		if (Physics.Raycast (ray, out hit, myVisionDistance))
		if (hit.collider.tag == "Wall") {
			myStatus = Status.Idle;
		} else {
			myStatus = Status.Find;
		}
	}
}
