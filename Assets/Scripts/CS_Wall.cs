using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Wall : MonoBehaviour {

	void OnCollisionEnter (Collision collision) {
		Debug.Log ("Hit");
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<CS_Player> ().HitWall ();
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Hit");
		if (other.tag == "Player") {
			other.gameObject.GetComponent<CS_Player> ().HitWall ();
		}
	}
}
