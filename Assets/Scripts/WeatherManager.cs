using System;
using UnityEngine;
using DigitalRuby.RainMaker;

public class WeatherManager : MonoBehaviour
{
    public GameObject rainObject;

    public float transitionTime = 5;

    public GameObject backRendererRain;
    public GameObject backRendererSun;
    public GameObject frontRendererRain;
    public GameObject frontRendererSun;

    public enum Weather { sun, rain };

    private bool raining;
    private GameObject currentWeatherObject;
    private float elapsedTime;
    private RainScript2D rainScript;

    private float alphaRain;
    private float alphaSun;
    private bool fadeToSun;
    private bool fadeToRain;

    private SpriteRenderer brRain;
    private SpriteRenderer brSun;
    private SpriteRenderer frRain;
    private SpriteRenderer frSun;

    // Use this for initialization
    void Start ()
    {
        rainObject = Instantiate(rainObject, gameObject.transform);
        rainScript = rainObject.GetComponent<RainScript2D>();
        rainScript.FollowCamera = false;

        // prevent the rain from colliding the trigger box
        rainScript.CollisionMask &= ~(1 << LayerMask.NameToLayer("WheaterZone"));

        set_sun();
        elapsedTime = transitionTime;

        alphaRain = 0;
        alphaSun = 1;
        fadeToSun = false;
        fadeToRain = false;

        brRain = backRendererRain.GetComponent<SpriteRenderer>();
        brSun = backRendererSun.GetComponent<SpriteRenderer>();
        frRain = frontRendererRain.GetComponent<SpriteRenderer>();
        frSun = frontRendererSun.GetComponent<SpriteRenderer>();

    }
    
    // Update is called once per frame
    void Update ()
    {
        elapsedTime += Time.deltaTime;

        float transitionRatio = Math.Min(elapsedTime / transitionTime, 1);
        rainScript.RainIntensity = raining ? transitionRatio : 1 - transitionRatio;

        if (fadeToSun || fadeToRain)
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

            changeAlpha(brRain, alphaRain);
            changeAlpha(frRain, alphaRain);
            changeAlpha(brSun, alphaSun);
            changeAlpha(frSun, alphaSun);
        }
    }

    private void fadeBackground(Weather toWeather)
    {
        if (toWeather == Weather.sun)
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

    public bool isRaining()
    {
        return raining;
    }

    private void set_rain()
    {
        if (raining)
            return;
        
        elapsedTime = 0;
    }

    private void set_sun()
    {
        if (!raining)
            return;
        
        elapsedTime = 0;
    }

    public void change_weather()
    {
        if (raining)
        {
            set_sun();
            fadeBackground(Weather.sun);
        }
        else
        {
            set_rain();
            fadeBackground(Weather.rain);
        }
        raining = !raining;
    }
}
