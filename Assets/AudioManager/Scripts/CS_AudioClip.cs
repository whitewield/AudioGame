using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CS_AudioClip {
	public AudioClip[] myAudioClips;
	public float myVolum = 1;
	public float myPitchDifference = 0;

	public AudioClip GetMyAudioClip () {
		if (myAudioClips.Length <= 0)
			return null;
		return myAudioClips [Random.Range (0, myAudioClips.Length)];
	}

	public float GetMyPitch () {
		return (1 + myPitchDifference * Random.Range (0, 1.0f));
	}
}
