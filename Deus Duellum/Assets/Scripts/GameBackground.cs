﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBackground : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        //Debug.Log("this function works....?");
        ToggleOpen panelManager = GameObject.FindGameObjectWithTag("panelManager").GetComponent<ToggleOpen>();
        panelManager.ClosePanel();        
    }
}