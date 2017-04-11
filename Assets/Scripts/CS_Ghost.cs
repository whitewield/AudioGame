using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CS_Ghost : MonoBehaviour {

	private static CS_Ghost instance = null;

	//========================================================================
	public static CS_Ghost Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		//		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	enum Status {
		Idle,
		Find
	}

	[SerializeField] float mySnapshotTransitionTime = 1.0f;
	[SerializeField] AudioMixerSnapshot mySnapshotIdle;
	[SerializeField] AudioMixerSnapshot mySnapshotFind;
	[SerializeField] AudioMixerSnapshot mySnapshotGone;

	[SerializeField] float myVelocity = 3;
	private Status myStatus = Status.Idle;
	private Vector3 myDirection;

	[SerializeField] Transform myWayPointsParent;
	private List<Transform> myWayPoints = new List<Transform> ();
	[SerializeField] float myArriveDistance = 0.1f;
	private int myNextWayPoint = 0;

//	[SerializeField] AudioClip mySFX;


	// Use this for initialization
	void Start () {
//		CS_AudioManager.Instance.PlaySFX (mySFXTest, Vector3.one);

		for (int i = 0; i < myWayPointsParent.childCount; i++) {
			myWayPoints.Add (myWayPointsParent.GetChild (i));
		}
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
				myNextWayPoint = (myNextWayPoint + 1) % myWayPoints.Count;
			}
		}

		transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
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
			mySnapshotIdle.TransitionTo (mySnapshotTransitionTime);
		} else {
			CS_Player.Instance.PlayVoice_Ghost ();
			myStatus = Status.Find;
			mySnapshotFind.TransitionTo (mySnapshotTransitionTime);
		}
	}

	void OnTriggerEnter (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			Debug.Log ("LOSE!");
		}
	}

	public void End () {
		mySnapshotGone.TransitionTo (mySnapshotTransitionTime);
	}
}
