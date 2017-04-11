using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Door : MonoBehaviour {
	[SerializeField] AudioClip mySFX_Open;
	[SerializeField] AudioClip mySFX_Close;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			this.gameObject.layer = 9;
			CS_AudioManager.Instance.PlaySFX (mySFX_Open, this.transform.position);
		}
	}

	void OnTriggerExit (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			this.gameObject.layer = 8;
			CS_AudioManager.Instance.PlaySFX (mySFX_Close, this.transform.position);
		}
	}
}
