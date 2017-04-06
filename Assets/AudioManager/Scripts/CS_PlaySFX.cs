using UnityEngine;
using System.Collections;

public class CS_PlaySFX : MonoBehaviour {
	public AudioClip[] mySFX;
	public bool playOnStart;
	// Use this for initialization
	void Start () {
		if (playOnStart)
			PlaySFX (0);
	}

	public void PlaySFX (int t_number) {
		CS_AudioManager.Instance.PlaySFX (mySFX [t_number]);
	}
}
