using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolGetter : MonoBehaviour {

    public GameObject symbol1;
    public GameObject symbol2;
    public GameObject symbol3;
    public GameObject symbol4;
    public GameObject symbol5;

    private GameObject[] symbols;

    // Use this for initialization
    void Start () {
		symbols = new GameObject[]{ symbol1, symbol2, symbol3, symbol4, symbol5};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject getSymbol(int id)
    {
        if(id < 1 || id > 5)
        {
            Debug.LogError("Try to access to an inexistant Symbol");
        }

        return symbols[id - 1];
    }
}
