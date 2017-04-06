using System.Collections;
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
	[SerializeField] float mySoundSpeed = 340;

	[SerializeField] AudioSource myAudioSource;
	[SerializeField] AudioClip myVoice_Ghost;
	private bool isVoiceGhostPlayed = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Send")) {

			Debug.Log ("send");
			CS_AudioManager.Instance.PlaySFX (mySFX_Send, this.transform.position);

			float myVisionDistance = 50;
			Ray ray = new Ray (this.transform.position, this.transform.forward);
			RaycastHit hit;
			int t_layerMask = (int) Mathf.Pow (2, 8); //for the layer you want to do the raycast
			if (Physics.Raycast (ray, out hit, myVisionDistance, t_layerMask))
			if (hit.collider.tag == "Wall") {
				Debug.Log ("wall");
				Invoke ("PlaySoundWall", hit.distance / mySoundSpeed); 
			}
		}
	}

	private void PlaySoundWall() {  
		Debug.Log ("receive");
		CS_AudioManager.Instance.PlaySFX (mySFX_Wall, this.transform.position);
	}

	public void PlayVoice_Ghost (){
		if (isVoiceGhostPlayed == false) {
			isVoiceGhostPlayed = true;
			PlayVoice (myVoice_Ghost);
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
