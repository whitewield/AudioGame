using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CS_SpaceManager : MonoBehaviour {
	
	private static CS_SpaceManager instance = null;

	//========================================================================
	public static CS_SpaceManager Instance {
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

	[SerializeField] AudioReverbZone[] myReverbZones;
	// Use this for initialization
//	void Start () {
////		SetReverbZone
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void SetReverbZone (AudioReverbZone g_ReverbZone) {
		foreach (AudioReverbZone t_ReverbZone in myReverbZones) {
			if (t_ReverbZone == g_ReverbZone && t_ReverbZone.enabled == false) {
				t_ReverbZone.enabled = true;
			} else if (t_ReverbZone != g_ReverbZone && t_ReverbZone.enabled == true) {
				t_ReverbZone.enabled = false;
			}
		}
	}
}
