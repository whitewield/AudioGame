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

	[SerializeField] AudioSource myAudioSource;
	[SerializeField] AudioClip myVoice_Ghost;
	private bool isVoiceGhostPlayed = false;
	[SerializeField] AudioClip myVoice_Door;
	private bool isVoiceDoorPlayed = false;
	[SerializeField] AudioClip myVoice_Exit;
	private bool isVoiceExitPlayed = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Send")) {

			Debug.Log ("send");
			CS_AudioManager.Instance.PlaySFX (mySFX_Send, this.transform.position);

			float myVisionDistance = mySoundRange.y;
			Ray ray = new Ray (this.transform.position, this.transform.forward);
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

		myAudioSource.clip = g_SFX;
		myAudioSource.Play ();
	} 
}
