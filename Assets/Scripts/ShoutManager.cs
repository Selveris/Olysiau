using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutManager : MonoBehaviour {

    private AudioClip[] voices;
    private AudioClip[] cries;
    private AudioClip thunder;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        voices = Resources.LoadAll<AudioClip>("Sounds/FXs/Dance/");
        cries = Resources.LoadAll<AudioClip>("Sounds/FXs/CriDouleur");
        thunder = Resources.Load<AudioClip>("Sounds/FXs/Thunder/thunder1");
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayVoices()
	{
		AudioClip voice = voices[Random.Range(0, voices.Length - 1)];
        audioSource.PlayOneShot(voice);
	}

    public void PlayCries(){
        AudioClip cry = cries[Random.Range(0, cries.Length - 1)];
        audioSource.PlayOneShot(cry);
    }

    public void PlayThunder(){
        audioSource.PlayOneShot(thunder);
    }
}
