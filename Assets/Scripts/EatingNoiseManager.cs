using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingNoiseManager : MonoBehaviour {

    private AudioClip[] eatings;

	// Use this for initialization
	void Start () {
		eatings = Resources.LoadAll<AudioClip>("Sounds/FXs/EatPlant/");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayEatings()
	{
		AudioClip eating = eatings[Random.Range(0, eatings.Length - 1)];
		GetComponent<AudioSource>().PlayOneShot(eating);
	}
}
