using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using UnityStandardAssets.Characters.FirstPerson;

public class CS_Space : MonoBehaviour {

	[SerializeField] AudioReverbZone myReverbZone;
//	[SerializeField] AudioReverbZone myReverbZone_Exit;


//	[SerializeField] AudioSource myDoorAudioSource;

//	[SerializeField] AudioClip[] myFootsteps;
//	[SerializeField] AudioClip myJumpSound;
//	[SerializeField] AudioClip myLandSound;
//
//	private AudioClip[] myDefaultFootsteps;
//	private AudioClip myDefaultJumpSound;
//	private AudioClip myDefaultLandSound;



	// Use this for initialization
//	void Start () {
//		SetActiveReverbZone (myReverbZone_Enter, false);
//	}
//	//	
//	//	// Update is called once per frame
//	//	void Update () {
//	//		
//	//	}
//
//	private void SetActiveReverbZone (AudioReverbZone g_ReverbZone, bool g_isActive) {
//		if (g_ReverbZone != null)
//		if (g_ReverbZone.gameObject.activeSelf == !g_isActive)
//			g_ReverbZone.gameObject.SetActive (g_isActive);
//	}

	void OnTriggerEnter (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			CS_SpaceManager.Instance.SetReverbZone (myReverbZone);
		}
	}

//	void OnTriggerExit (Collider g_Collider) {
//		if (g_Collider.tag == "Player") {
//
//			mySnapShot_Exit.TransitionTo (1f);
//
//			SetActiveReverbZone (myReverbZone_Enter, false);
//			SetActiveReverbZone (myReverbZone_Exit, true);
//
//			if (myDoorAudioSource != null)
//				myDoorAudioSource.Play ();
//		}
//	}
}
