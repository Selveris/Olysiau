using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFade : MonoBehaviour {

    public GameObject nightVersion;
    public GameObject dayVersion;

    private float alphaNight;
    private float alphaDay;
    private bool fadeToNight;
    private bool fadeToDay;

    private SpriteRenderer srNight;
    private SpriteRenderer srDay;

	// Use this for initialization
	void Start () {
        alphaNight = 0;
        alphaDay = 1;
        fadeToNight = false;
        fadeToDay = false;

        srNight = nightVersion.GetComponent<SpriteRenderer>();
        srDay = dayVersion.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(fadeToDay || fadeToNight)
        {
            float dt = Time.deltaTime;

            if (fadeToDay)
            {
                alphaDay -= dt;
                alphaNight += dt;

                if (alphaDay >= 1)
                {
                    alphaDay = 1;
                    alphaNight = 0;
                    fadeToDay = false;
                }
            }
            if (fadeToNight)
            {
                alphaNight -= dt;
                alphaDay += dt;

                if (alphaNight >= 1)
                {
                    alphaNight = 1;
                    alphaDay = 0;
                    fadeToNight = false;
                }
            }

            changeAlpha(srNight, alphaNight);
            changeAlpha(srDay, alphaDay);
        }
        
	}

    public void fadeImage(bool toDay)
    {
        if(isDay)
        {
            fadeToDay = true;
            fadeToNight = false;
        }
        else
        {
            fadeToDay = false;
            fadeToNight = true;
        }
    }

    private void changeAlpha(SpriteRenderer sr, float a)
    {
        Color prev = sr.color;
        prev.a = a;
        sr.color = prev;
    }
}
