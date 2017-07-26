using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour {

    public string name;
    public float lightNecessity;
    public float maxWater;
    public float floodResist;
    public float drieResist;
    public float haverestTime;
    public GameObject weatherZone;
    public Transform spawn;

    public GameObject seed;
    public GameObject plant;
    public GameObject maturePlant;

    private float actualWater;
    private float recievedLight;
    private bool flooded;
    private bool dried;
    private bool ready;
    private float readySince;
    private enum GrowthState { SEED, PLANT, MATURE};
    private GrowthState actualGrowth;
    private SpriteRenderer renderer;

    private WeatherManager weatherManager;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        weatherManager = weatherZone.GetComponent<WeatherManager>();
        if (weatherManager == null)
            Debug.LogError("Script 'WeatherManager' not found in WeatherZone");

        restart();
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
                renderer.color = new Color(3, 146, 201);
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
                renderer.color = new Color(201, 146, 3);
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
            flooded = false;
            dried = false;
        }
    }

    private void updateGrowth()
    {
        float growth = recievedLight / lightNecessity;

        if(actualGrowth != GrowthState.PLANT && growth >= 0.25f && growth <= 0.67f)
        {
            actualGrowth = GrowthState.PLANT;
            updateSprite();
        }else if(actualGrowth != GrowthState.MATURE && growth > 0.67f)
        {
            actualGrowth = GrowthState.MATURE;
            updateSprite();
        }

    }

    private void updateSprite()
    {
        renderer.sprite = (Sprite) Resources.Load("Sprites/" + name + actualGrowth.ToString());
    }

    private void die()
    {
        GameManager.gm.OnePlantDied(gameObject);
    }

    private void restart()
    {
        actualWater = 0;
        recievedLight = 0;

        flooded = false;
        dried = false;
        ready = false;

        actualGrowth = GrowthState.SEED;
        updateSprite();
    }
}
