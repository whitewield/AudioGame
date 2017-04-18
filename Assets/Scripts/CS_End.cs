using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_End : MonoBehaviour {
	[SerializeField] AudioClip mySFX;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			Debug.Log ("WIN!");
			CS_AudioManager.Instance.PlaySFX (mySFX, Vector3.zero, g_Collider.transform);
			CS_Ghost.Instance.End ();

			CS_Player.Instance.Win ();
		}
	}
}
