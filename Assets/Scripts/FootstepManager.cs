using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour {

    private AudioClip[] footsteps;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        footsteps = Resources.LoadAll<AudioClip>("Sounds/FXs/Footsteps/");
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayFootstep () {
        AudioClip footstep = footsteps[Random.Range(0, footsteps.Length - 1)];
        audioSource.PlayOneShot(footstep, 0.5f);
    }
}
