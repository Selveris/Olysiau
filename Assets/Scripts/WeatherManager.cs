using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public GameObject sunObject;
    public GameObject rainObject;

    private bool raining;
    private GameObject currentWeatherObject;
    private float time;

    // Use this for initialization
    void Start ()
    {
        DigitalRuby.RainMaker.RainScript2D rainScript = rainObject.GetComponent<DigitalRuby.RainMaker.RainScript2D>();
        rainScript.RainIntensity = 1;

        // prevent the rain from colliding the trigger box
        rainScript.CollisionMask &= ~(1 << LayerMask.NameToLayer("WheaterZone"));

        raining = false;
        set_sun();
    }
    
    // Update is called once per frame
    void Update ()
    {
        time += Time.deltaTime;
    }

    public bool isRaining()
    {
        return raining;
    }

    public void set_rain()
    {
        raining = true;
        switchWeather();
    }

    public void set_sun()
    {
        raining = false;
        switchWeather();
    }

    private void switchWeather()
    {
        time = 0;

        Destroy(currentWeatherObject);

        GameObject newWeatherObject = raining ? rainObject : sunObject;
        Transform parent = transform.Find("WeatherSpawn");
        currentWeatherObject = Instantiate<GameObject>(newWeatherObject, parent);
    }
}
