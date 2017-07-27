﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour {

    public GameObject sequenceOutput;

    private int nbreOfSymbol;
    private int[] sequence;
    private int index;
    private Dictionary<int, Symbol> dic;

    private SymbolGetter sg;


    // Use this for initialization
    void Start () {
        sg = sequenceOutput.GetComponent<SymbolGetter>();

        nbreOfSymbol = 5;

        string[] s = { "q", "w", "e", "a", "s", "d" };
        for (int i = 0; i<s.Length; i++)
        {
            dic.Add(i, createSymbol(s[i]));
        }

        reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool addSymbol(int symbolID)
    {
        if(symbolID == sequence[index])
        {
            //activate symbol and change sprite
            Symbol s = dic[sequence[index]];
            s.activate();
            updateSymbolSprite(index, s.getSprite());

            index += 1;
            if(index == nbreOfSymbol)
            {
                gameObject.GetComponent<PlayerController>().ChangeWeather();
                reset();
            }
            return true;
        }

        return false;
    }

    public void activateOutput()
    {
        sequenceOutput.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void disableOutput()
    {
        sequenceOutput.GetComponent<SpriteRenderer>().enabled = false;
    }

    private Symbol createSymbol(string key)
    {
        Sprite active = Resources.Load<Sprite>("Sprites/Symbols/" + key + "_active");
        Sprite inactive = Resources.Load<Sprite>("Sprites/Symbols/" + key + "_inactive");

        return new Symbol(active, inactive);
    }

    private int[] generateRandomSequence()
    {
        int[] s = new int[nbreOfSymbol];

        for(int i = 0; i<nbreOfSymbol; i++)
        {
            s[i] = Random.Range(0, nbreOfSymbol-1);
        }

        return s;
    }

    private void reset()
    {
        index = 0;
        sequence = generateRandomSequence();

        for(int i = 0; i<nbreOfSymbol; i++)
        {
            updateSymbolSprite(i, dic[i].getSprite());
        }
    }

    private void updateSymbolSprite(int index, Sprite sprite)
    {
        sg.getSymbol(index + 1).getComponent<SpriteRenderer>().Sprite = sprite;
    }


    public class Symbol
    {
        private bool isActive;

        private Sprite active;
        private Sprite inactive;

        public Symbol(Sprite active, Sprite inactive)
        {
            this.active = active;
            this.inactive = inactive;
            isActive = false;
        }

        public void activate()
        {
            isActive = true;
        }

        public Sprite getSprite()
        {
            return isActive ? active : inactive;
        }
    }
}