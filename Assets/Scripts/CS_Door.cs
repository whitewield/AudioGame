using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			this.gameObject.layer = 9;
		}
	}

	void OnTriggerExit (Collider g_Collider) {
		if (g_Collider.tag == "Player") {
			this.gameObject.layer = 8;
		}
	}
}
