using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GhostTarget : MonoBehaviour {

	private static CS_GhostTarget instance = null;

	//========================================================================
	public static CS_GhostTarget Instance {
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
