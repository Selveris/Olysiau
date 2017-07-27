using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour {

    public GameObject sequenceOutput;

    private int nbreOfSymbol;
    private Symbol[] symbols;
    private int[] sequence;
    private int index;
    private Dictionary<int, Symbol> dic;

    private SymbolGetter sg;


    // Use this for initialization
    void Start () {
        sg = sequenceOutput.GetComponent<SymbolGetter>();
        dic = new Dictionary<int, Symbol>();

        nbreOfSymbol = 5;

        string[] s = { "q", "w", "e", "a", "s", "d" };
        for (int i = 0; i<s.Length; i++)
        {
            dic.Add(i, createSymbol(s[i]));
        }

        reset();
        disableOutput();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool addSymbol(int symbolID)
    {
        if(symbolID == sequence[index])
        {
            //activate symbol and change sprite
            Symbol s = symbols[index];
            s.activate();
            updateSymbolSprite(index, s.getSprite());

            index += 1;
            if(index == nbreOfSymbol)
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

    private Symbol[] symbolsFromSequence(int[] seq)
    {
        Symbol[] s = new Symbol[nbreOfSymbol];
        for(int i=0; i<nbreOfSymbol; i++)
        {
            s[i] = dic[seq[i]];
        }

        return s;
    }

    private void reset()
    {
        index = 0;
        sequence = generateRandomSequence();
        symbols = symbolsFromSequence(sequence);

        for(int i = 0; i<nbreOfSymbol; i++)
        {
            updateSymbolSprite(i, dic[sequence[i]].getSprite());
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
