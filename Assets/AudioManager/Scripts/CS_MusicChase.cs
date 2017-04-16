using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_MusicChase : MonoBehaviour {

	GameObject player, ghost;

	float distance;

	AudioSource thisSource;

	// Use this for initialization
	void Start () {

		thisSource = GetComponent<AudioSource> ();

		player = FindObjectOfType<CS_Player> ().gameObject;
		ghost = FindObjectOfType<CS_Ghost> ().gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {

		distance = (player.transform.position - ghost.transform.position).magnitude; 

		thisSource.volume = CS_AudioManager.Instance.RemapRange (distance, 0f, 50f, 1.0f, 0f);
		
	}
}
