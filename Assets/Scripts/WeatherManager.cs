using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    public GameObject sun;
    public GameObject rainingCloud;

    private bool raining;
    private Transform weatherSpawn;
    private GameObject lastInst;

	// Use this for initialization
	void Start () {
        weatherSpawn = transform.Find("WeatherSpawn");
        raining = false;
        set_sun();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isRaining()
    {
        return raining;
    }

    public void set_rain()
    {
        raining = true;
        instantiateWeather(rainingCloud);
    }

    public void set_sun()
    {
        raining = false;
        instantiateWeather(sun);
    }

    private void instantiateWeather(GameObject o)
    {
        Destroy(lastInst);
        lastInst = Instantiate<GameObject>(o, weatherSpawn);
        lastInst.transform.SetParent(transform, true);
    }
}
