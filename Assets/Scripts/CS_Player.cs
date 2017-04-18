﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Player : MonoBehaviour {

	private static CS_Player instance = null;

	//========================================================================
	public static CS_Player Instance {
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

	[SerializeField] AudioClip mySFX_Send;
	[SerializeField] AudioClip mySFX_Wall;
	[SerializeField] AudioClip mySFX_Door;
	[SerializeField] AudioClip mySFX_Exit;
	[SerializeField] float mySoundSpeed = 340;
	[SerializeField] Vector2 mySoundRange = new Vector2 (0, 100);
	[SerializeField] Vector2 mySFXPitchRange = new Vector2 (1, 1.2f);
//	private float mySFXPitch = 1;

	[SerializeField] AudioSource myVoiceAudioSource;
	[SerializeField] AudioClip myVoice_Ghost;
	private bool isVoiceGhostPlayed = false;
	[SerializeField] AudioClip myVoice_Door;
	private bool isVoiceDoorPlayed = false;
	[SerializeField] AudioClip myVoice_Exit;
	private bool isVoiceExitPlayed = false;

	[SerializeField] float mySpeed = 3;
	[SerializeField] Transform myCameraTransform;
	[SerializeField] Rigidbody myRigidbody;

	[SerializeField] AudioSource mySFXAudioSource;
	[SerializeField] float myFoodstepRatio = 0.5f;
	private float myFoodstepDistance;
	private float  myFoodstepCycle;
	private Vector3 myLastPosition;
	[SerializeField] AudioClip[] mySFX_Footsteps;
	private List<int> mySFX_Footsteps_List = new List<int> ();
	private int mySFX_Footsteps_Last = 1;

	[SerializeField] AudioClip mySFX_HitWall;

	// Use this for initialization
	void Start () {
		myLastPosition = this.transform.position;
		myFoodstepDistance = mySpeed * myFoodstepRatio;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateSend ();
		UpdateMove ();
	}

	void LateUpdate () {
		UpdateStep ();
	}

	private void UpdateStep () {
		if (mySFX_Footsteps_List.Count == 0) {
			List<int> t_list = new List<int> ();
			for (int i = 0; i < mySFX_Footsteps.Length; i++) {
				t_list.Add (i);
			}

			while (t_list.Count > 0) {
				int t_num = Random.Range (0, t_list.Count);
				mySFX_Footsteps_List.Add (t_list [t_num]);
				t_list.RemoveAt (t_num);
			}

			if (mySFX_Footsteps_List [0] == mySFX_Footsteps_Last) {
				mySFX_Footsteps_List [0] = mySFX_Footsteps_List [1];
				mySFX_Footsteps_List [1] = mySFX_Footsteps_Last;
			}

			mySFX_Footsteps_Last = mySFX_Footsteps_List [mySFX_Footsteps_List.Count - 1];
		}

		float t_distance = Vector3.Distance (this.transform.position, myLastPosition);
		myFoodstepCycle += t_distance;
//		Debug.Log (myFoodstepCycle);
		if (myFoodstepCycle > myFoodstepDistance) {
			myFoodstepCycle -= myFoodstepDistance;

			//Play the sound

			Debug.Log (mySFX_Footsteps_List [0]);
			mySFXAudioSource.Stop ();
			mySFXAudioSource.clip = mySFX_Footsteps [mySFX_Footsteps_List [0]];
			mySFX_Footsteps_List.RemoveAt (0);
			mySFXAudioSource.Play ();

		}

		myLastPosition = this.transform.position;
	}

	private void UpdateMove () {
		if (myCameraTransform == null) {
			return;
		}
//		float t_vertical = Input.GetAxis ("Vertical");
//		float t_horizontal = Input.GetAxis ("Horizontal");
//		Vector3 t_direction = (t_vertical * myCameraTransform.forward + t_horizontal * myCameraTransform.right);
//		t_direction = new Vector3 (t_direction.x, 0, t_direction.z).normalized;
//
//		myRigidbody.velocity = new Vector3 (t_direction.x, myRigidbody.velocity.y, t_direction.z) * mySpeed * Time.deltaTime;




//		myRigidbody.velocity = new Vector3 (t_direction.x, myRigidbody.velocity.y, t_direction.y) * mySpeed * Time.deltaTime;

//		this.transform.position += (t_vertical * myCameraTransform.forward + t_horizontal * myCameraTransform.right) * mySpeed * Time.deltaTime;
	
		if (Input.GetButton ("Move")) {
			Debug.Log ("Move");
			Vector3 t_direction = myCameraTransform.forward;
			t_direction = new Vector3 (t_direction.x, 0, t_direction.z).normalized;
//			myRigidbody.velocity = new Vector3 (t_direction.x, myRigidbody.velocity.y, t_direction.z) * mySpeed * Time.deltaTime;
			this.transform.position += t_direction * mySpeed * Time.deltaTime;

		}

//		myRigidbody.velocity = Vector3.zero;
	}

	private void UpdateSend () {
//		if (Input.GetTouch(14).phase == TouchPhase.Began || Input.GetTouch(15).phase == TouchPhase.Began) {
		if (Input.GetButtonDown ("Send")) {

			Debug.Log ("send");
			CS_AudioManager.Instance.PlaySFX (mySFX_Send, this.transform.position);

			float myVisionDistance = mySoundRange.y;
			Ray ray = new Ray (this.transform.position, Vector3.ProjectOnPlane (myCameraTransform.forward, Vector3.up).normalized);
			RaycastHit hit;
			int t_layerMask = (int) Mathf.Pow (2, 8); //for the layer you want to do the raycast
			if (Physics.Raycast (ray, out hit, myVisionDistance, t_layerMask))
			if (hit.collider.tag == "Wall") {
				Debug.Log ("wall");
				StartCoroutine (PlaySound (mySFX_Wall, CS_AudioManager.Instance.RemapRange (hit.distance, mySoundRange.y, mySoundRange.x, mySFXPitchRange.x, mySFXPitchRange.y), 
					hit.distance / mySoundSpeed)
				);
			} else if (hit.collider.tag == "Door") {
				Debug.Log ("Door");
				PlayVoiceOnce (myVoice_Door, ref isVoiceDoorPlayed);
				StartCoroutine (PlaySound (mySFX_Door, CS_AudioManager.Instance.RemapRange (hit.distance, mySoundRange.y, mySoundRange.x, mySFXPitchRange.x, mySFXPitchRange.y), 
					hit.distance / mySoundSpeed)
				);
			} else if (hit.collider.tag == "Exit") {
				Debug.Log ("Exit");
				PlayVoiceOnce (myVoice_Exit, ref isVoiceExitPlayed);
				StartCoroutine (PlaySound (mySFX_Exit, CS_AudioManager.Instance.RemapRange (hit.distance, mySoundRange.y, mySoundRange.x, mySFXPitchRange.x, mySFXPitchRange.y), 
					hit.distance / mySoundSpeed)
				);
			}
		}
	}

	IEnumerator PlaySound(AudioClip g_AudioClip, float g_pitch, float g_time) {  
		yield return new WaitForSeconds (g_time);
		Debug.Log ("receive");
		CS_AudioManager.Instance.PlaySFX (g_AudioClip, this.transform.position, null, 1, g_pitch);
	}

	public void PlayVoice_Ghost () {
		PlayVoiceOnce (myVoice_Ghost, ref isVoiceGhostPlayed);
	}

	public void PlayVoiceOnce (AudioClip g_voice, ref bool g_isPlayed) {
		if (g_isPlayed == false) {
			Debug.Log ("PlayVoice_Ghost");
			g_isPlayed = true;
			PlayVoice (g_voice);
		}
	}

	public void PlayVoice (AudioClip g_SFX) {
		if (g_SFX == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}

		myVoiceAudioSource.clip = g_SFX;
		myVoiceAudioSource.Play ();
	} 

	public void HitWall () {
		mySFXAudioSource.Stop ();
		mySFXAudioSource.clip = mySFX_HitWall;
		mySFXAudioSource.Play ();
	}

	void OnCollisionEnter (Collision collision) {
		Debug.Log ("Hit");
		if (collision.gameObject.tag == "Wall") {
			HitWall ();
		}
	}
}
