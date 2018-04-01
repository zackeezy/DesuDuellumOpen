using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDescription : MonoBehaviour {
    public Transform popupText;
    public bool displayInfo = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseOver()
    {
        GetComponent<TextMesh>().text = "emote";
    }
    private void OnMouseExit()
    {
        GetComponent<TextMesh>().text = "";
    }
}
