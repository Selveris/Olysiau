using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutManager : MonoBehaviour {

    private AudioClip[] voices;

	// Use this for initialization
	void Start () {
        voices = Resources.LoadAll<AudioClip>("Sounds/FXs/Dance/");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayVoices()
	{
		AudioClip voice = voices[Random.Range(0, voices.Length - 1)];
		GetComponent<AudioSource>().PlayOneShot(voice);
	}
}
