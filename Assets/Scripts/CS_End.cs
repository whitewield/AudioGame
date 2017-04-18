using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_End : MonoBehaviour {
	[SerializeField] AudioClip mySFX;
	[SerializeField] GameObject myBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			Debug.Log ("WIN!");
			CS_AudioManager.Instance.PlaySFX (mySFX, this.transform.position);
			CS_Ghost.Instance.End ();

			g_Collider.transform.position = myBox.transform.position + Vector3.up;
		}
	}
}
