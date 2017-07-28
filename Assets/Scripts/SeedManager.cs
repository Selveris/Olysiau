﻿using UnityEngine;

public class SeedManager : MonoBehaviour {

    public string name;
    public float lightNecessity;
    public float maxWater;
    public float floodResist;
    public float drieResist;
    public float haverestTime;
    public GameObject weatherZone;

    private float actualWater;
    private float recievedLight;
    private bool flooded;
    private bool dried;
    private bool ready;
    private bool healthy;
    private float growth;

    private float minLocalY;
    private float travelDistanceY;

    private SpriteRenderer renderer;
    private WeatherManager weatherManager;

    private int resetCount = 0;

    // Use this for initialization
    void Start()
    {
        lightNecessity = Random.Range(30, 50);
        maxWater = System.Math.Max(Random.Range(30, 50) - 3 * resetCount, 6);

        floodResist = System.Math.Max(21 - resetCount, 4);
        drieResist = System.Math.Max(21 - resetCount, 4);

        renderer = GetComponent<SpriteRenderer>();
        weatherManager = weatherZone.GetComponent<WeatherManager>();
        if (weatherManager == null)
            Debug.LogError("Script 'WeatherManager' not found in WeatherZone");

        restart();

        float spriteExtentY = renderer.sprite.bounds.extents.y;
        minLocalY = -4f / 5f * spriteExtentY;
        travelDistanceY = spriteExtentY - minLocalY;

        updateGrowth();
    }

    // Update is called once per frame
    void Update()
    {

        float t = Time.deltaTime;

        handleWeather(t);

        handleExtrema();

        if(recievedLight >= lightNecessity)
        {
            ready = true;
            if(recievedLight - lightNecessity > haverestTime)
            {
                restart();
            }
        }

        updateGrowth();
    }

    public bool isReady()
    {
        return ready;
    }

    public bool haverest()
    {
        if (ready)
        {
            restart();
            return true;
        }

        return false;
    }

    private void handleWeather(float time)
    {
        if (weatherManager.isRaining())
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
    private void handleExtrema()
    {
        //flooded
        if (actualWater > maxWater)
        {
            if (!flooded)
            {
                renderer.color = new Color(3f/256f, 146f/256f, 201f/256f, 1f);
            }

            flooded = true;
            if (actualWater - maxWater > floodResist)
            {
                die();
            }
        }
        //dried
        else if (actualWater <= 0)
        {
            if (!dried)
            {
                renderer.color = new Color(201f/256f, 146f/256f, 3f/256f, 1f);
            }

            dried = true;
            if (-actualWater > drieResist)
            {
                die();
            }
        }
        //in order
        else
        {
            if(flooded || dried)
            {
                renderer.color = new Color(1f, 1f, 1f, 1f);
            }
            flooded = false;
            dried = false;
            
        }
    }

    private void updateGrowth()
    {
        float growth = Mathf.Min(recievedLight / lightNecessity, 1);
        gameObject.transform.localPosition = new Vector3(0, minLocalY + growth * travelDistanceY, 0);
    }

    private void updateSprite()
    {
        string spe = healthy ? "" : "_unhealthy";
        renderer.sprite = Resources.Load<Sprite>("Sprites/" + name + spe);
    }

    private void die()
    {
        print(GameManager.gm);
        GameManager.gm.OnePlantDied(gameObject);
    }

    private void restart()
    {
        actualWater = maxWater/3;
        recievedLight = 0;

        flooded = false;
        dried = false;
        healthy = !flooded && !dried;
        ready = growth >= 1;

        ++resetCount;

        updateSprite();
    }
}
