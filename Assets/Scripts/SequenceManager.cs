using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour {

    public GameObject sequenceOutput;

    private int symbolCount = 5;
    private Symbol[] symbols;
    private int[] sequence;
    private int index;
    private List<Symbol> symbolList = new List<Symbol>();

    private SymbolGetter sg;


    // Use this for initialization
    void Start () {
        sg = sequenceOutput.GetComponent<SymbolGetter>();

        foreach (char key in "qweasd")
            symbolList.Add(createSymbol(key));

        reset();
        disableOutput();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool addSymbol(int symbolID)
    {
        if(symbolID - 1 == sequence[index])
        {
            //activate symbol and change sprite
            Symbol s = symbols[index];
            s.activate();
            updateSymbolSprite(index, s.getSprite());

            index += 1;
            if(index == symbolCount)
            {
                gameObject.GetComponent<PlayerController>().CompleteSequence();
                reset();
            }
            return true;
        }

        return false;
    }

    public void activateOutput()
    {
        sequenceOutput.SetActive(true);
    }

    public void disableOutput()
    {
        sequenceOutput.SetActive(false);
    }

    private Symbol createSymbol(char key)
    {
        Sprite active = Resources.Load<Sprite>("Sprites/Symbols/" + key + "_active");
        Sprite inactive = Resources.Load<Sprite>("Sprites/Symbols/" + key + "_inactive");

        return new Symbol(active, inactive);
    }

    private int[] generateRandomSequence()
    {
        int[] randomSequence = new int[symbolCount];

        for(int i = 0; i < symbolCount; i++)
            randomSequence[i] = Random.Range(0, symbolList.Count);

        return randomSequence;
    }

    private Symbol[] symbolsFromSequence(int[] seq)
    {
        Symbol[] s = new Symbol[symbolCount];
        for(int i=0; i<symbolCount; i++)
        {
            s[i] = symbolList[seq[i]].newInstance();
        }

        return s;
    }

    public void reset()
    {
        index = 0;
        sequence = generateRandomSequence();
        symbols = symbolsFromSequence(sequence);

        for(int i = 0; i<symbolCount; i++)
        {
            updateSymbolSprite(i, symbolList[sequence[i]].getSprite());
        }
    }

    private void updateSymbolSprite(int index, Sprite sprite)
    {
        sg.getSymbol(index + 1).GetComponent<SpriteRenderer>().sprite = sprite;
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

        public Symbol newInstance()
        {
            return new Symbol(active, inactive);
        }

        public Sprite getSprite()
        {
            return isActive ? active : inactive;
        }
    }
}
