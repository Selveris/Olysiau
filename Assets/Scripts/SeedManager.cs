using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour {

    public float lightNecessity;
    public float maxWater;
    public float floodResist;
    public float drieResist;
    public float haverestTime;
    public GameObject weatherZone;

    public GameObject seed;
    public GameObject plant;
    public GameObject maturePlant;

    private float actualWater;
    private float recievedLight;
    private bool flooded;
    private bool dried;
    private float readySince;
    private WeatherManager manager;

    // Use this for initialization
    void Start()
    {
        actualWater = 0;
        recievedLight = 0;

        flooded = false;
        dried = false;

        manager = weatherZone.GetComponent<WeatherManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;

        handleWeather(t);

        if(actualWater > maxWater)
        {
            flooded = true;
            if(actualWater - maxWater > floodResist)
            {
                die();
            }
        }else if(actualWater <= 0)
        {
            dried = true;
            if(-actualWater > drieResist)
            {
                die();
            }
        }
        else
        {
            flooded = false;
            dried = false;
        }

        if(recievedLight >= lightNecessity)
        {
            readyToHaverest();
        }

        updateGrowth();
    }

    private void handleWeather(float time)
    {
        if (manager.isRaining())
        {
            actualWater += time;
        }
        else
        {
            actualWater -= time;
            if(actualWater >= 0)
            {
                recievedLight += time;
            }
        }
    }
    private void updateGrowth()
    {
        float growth = recievedLight / lightNecessity;
    }

    private void readyToHaverest()
    {

    }

    private void die()
    {

    }
}
