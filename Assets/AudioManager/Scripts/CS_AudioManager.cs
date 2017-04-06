//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_AudioManager : MonoBehaviour {
	
	private static CS_AudioManager instance = null;

	//========================================================================
	public static CS_AudioManager Instance {
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

		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	[SerializeField] GameObject mySFXPrefab;
	[SerializeField] int mySFXMaxNumber = 50;
	private List<GameObject> mySFXList = new List<GameObject> ();

	[SerializeField] AudioSource myAudioSource;

	void Start () {
		for (int i = 0; i < mySFXMaxNumber; i++) {
			mySFXList.Add (Instantiate (mySFXPrefab, this.transform));
		}
	}

	private AudioSource GetIdleAudioSource () {
		for (int i = 0; i < mySFXList.Count; i++) {
			if (mySFXList [i].GetComponent<AudioSource> ().isPlaying == false)
				return mySFXList [i].GetComponent<AudioSource> ();
		}
		Debug.LogError ("Can not find idle audio source!");
		return null;
	}

	public void PlaySFX (CS_AudioClip g_SFX) {
		AudioClip t_AudioClip = g_SFX.GetMyAudioClip ();

		if (t_AudioClip == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}

		AudioSource t_SFXAudioSource = GetIdleAudioSource ();
		if (t_SFXAudioSource == null) {
			Debug.LogError ("Can not find idle audio source!");
			return;
		}

		t_SFXAudioSource.clip = t_AudioClip;
		t_SFXAudioSource.volume = g_SFX.myVolum;
		t_SFXAudioSource.pitch = g_SFX.GetMyPitch ();
		t_SFXAudioSource.Play ();

	}

	public void PlaySFX (AudioClip g_SFX, Vector3 g_position = default(Vector3), Transform g_parent = null, float g_volum = 1, float g_pitch = 1) {
		if (g_SFX == null) {
			Debug.LogWarning ("Can not find the sound effect!");
			return;
		}

		AudioSource t_SFXAudioSource = GetIdleAudioSource ();
		if (g_parent != null) {
			t_SFXAudioSource.gameObject.transform.SetParent (g_parent);
			t_SFXAudioSource.gameObject.transform.localPosition = g_position;
		} else {
			t_SFXAudioSource.gameObject.transform.position = g_position;
		}

		if (t_SFXAudioSource == null) {
			Debug.LogError ("Can not find idle audio source!");
			return;
		}

		t_SFXAudioSource.clip = g_SFX;
		t_SFXAudioSource.volume = g_volum;
		t_SFXAudioSource.pitch = g_pitch;
		t_SFXAudioSource.Play ();
	} 

//	public void PlaySFX (AudioClip g_SFX, Transform g_parent, Vector3 g_localPosition, float g_volum = 1) {
//		if (g_SFX == null) {
//			Debug.LogWarning ("Can not find the sound effect!");
//			return;
//		}
//
//		AudioSource t_SFXAudioSource = GetIdleAudioSource ();
//		t_SFXAudioSource.gameObject.transform.SetParent (g_parent);
//		t_SFXAudioSource.gameObject.transform.localPosition = g_localPosition;
//
//		if (t_SFXAudioSource == null) {
//			Debug.LogError ("Can not find idle audio source!");
//			return;
//		}
//
//		t_SFXAudioSource.clip = g_SFX;
//		t_SFXAudioSource.volume = g_volum;
//		t_SFXAudioSource.Play ();
//	}

//	public void PlaySFX (AudioClip g_SFX, float g_volum = 1) {
//		if (g_SFX == null) {
//			Debug.LogWarning ("Can not find the sound effect!");
//			return;
//		}
//
//		AudioSource t_SFXAudioSource = GetIdleAudioSource ();
//		if (t_SFXAudioSource == null) {
//			Debug.LogError ("Can not find idle audio source!");
//			return;
//		}
//
//		t_SFXAudioSource.clip = g_SFX;
//		t_SFXAudioSource.volume = g_volum;
//		t_SFXAudioSource.Play ();
//	}

	public void PlayBGM (AudioClip g_BGM, float g_Volume = 1) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.volume = g_Volume;
			myAudioSource.Play ();
			return;
		} else if (g_BGM == myAudioSource.clip) {
			myAudioSource.volume = g_Volume;
			return;
		}

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.volume = g_Volume;
		myAudioSource.Play ();
	}

	public void StopBGM () {
		myAudioSource.Stop ();
	}

	//Edit based on Matt Bock's code
	public float RemapRange (float g_input, float g_inputFrom, float g_inputTo, float g_outputFrom, float g_outputTo) {

		//need to test

		//make sure the value between g_inputFrom and g_inputTo;
		float t_input = g_input;
		if (g_inputFrom < g_inputTo)
			t_input = Mathf.Clamp (g_input, g_inputFrom, g_inputTo);
		else 
			t_input = Mathf.Clamp (g_input, g_inputTo, g_inputFrom);
		
		float t_inputRange = (g_inputTo - g_inputFrom);
		float t_outputRange = (g_outputTo - g_outputFrom);
		return (((t_input - g_inputFrom) * t_outputRange) / t_inputRange) + g_outputFrom;
	}
}
