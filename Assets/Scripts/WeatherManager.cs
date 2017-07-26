using System;
using UnityEngine;
using DigitalRuby.RainMaker;

public class WeatherManager : MonoBehaviour
{
    public GameObject rainObject;

    public float transitionTime = 5;

    private bool raining;
    private GameObject currentWeatherObject;
    private float elapsedTime;
    private RainScript2D rainScript;

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
    }
    
    // Update is called once per frame
    void Update ()
    {
        elapsedTime += Time.deltaTime;

        float transitionRatio = Math.Min(elapsedTime / transitionTime, 1);
        rainScript.RainIntensity = raining ? transitionRatio : 1 - transitionRatio;
    }

    public bool isRaining()
    {
        return raining;
    }

    public void set_rain()
    {
        if (raining)
            return;

        raining = true;
        elapsedTime = 0;
    }

    public void set_sun()
    {
        if (!raining)
            return;

        raining = false;
        elapsedTime = 0;
    }
}
