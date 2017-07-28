using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour {

    private AudioClip[] footsteps;

	// Use this for initialization
	void Start () {
        footsteps = Resources.LoadAll<AudioClip>("Sounds/FXs/Footsteps/");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayFootstep () {
        AudioClip footstep = footsteps[Random.Range(0, footsteps.Length - 1)];
        GetComponent<AudioSource>().PlayOneShot(footstep);
    }
}
