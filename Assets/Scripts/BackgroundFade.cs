using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFade : MonoBehaviour {

    public GameObject backgroundRain;
    public GameObject backgroundSun;

    public enum Weather { sun, rain };

    private float alphaRain;
    private float alphaSun;
    private bool fadeToSun;
    private bool fadeToRain;

    private SpriteRenderer srRain;
    private SpriteRenderer srSun;

	// Use this for initialization
	void Start () {
        alphaRain = 0;
        alphaSun = 1;
        fadeToSun = false;
        fadeToRain = false;

        srRain = backgroundRain.GetComponent<SpriteRenderer>();
        srSun = backgroundSun.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(fadeToSun || fadeToRain)
        {
            float dt = Time.deltaTime;

            if (fadeToSun)
            {
                alphaSun -= dt;
                alphaRain += dt;

                if (alphaSun >= 1)
                {
                    alphaSun = 1;
                    alphaRain = 0;
                    fadeToSun = false;
                }
            }
            if (fadeToRain)
            {
                alphaRain -= dt;
                alphaSun += dt;

                if (alphaRain >= 1)
                {
                    alphaRain = 1;
                    alphaSun = 0;
                    fadeToRain = false;
                }
            }

            changeAlpha(srRain, alphaRain);
            changeAlpha(srSun, alphaSun);
        }
        
	}

    public void fadeBackground(Weather toWeather)
    {
        if(toWeather == Weather.sun)
        {
            fadeToSun = true;
            fadeToRain = false;
        }
        else
        {
            fadeToSun = false;
            fadeToRain = true;
        }
    }

    private void changeAlpha(SpriteRenderer sr, float a)
    {
        Color prev = sr.color;
        prev.a = a;
        sr.color = prev;
    }
}
