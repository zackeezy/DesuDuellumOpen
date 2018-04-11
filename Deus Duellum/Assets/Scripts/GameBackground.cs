using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBackground : MonoBehaviour {

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

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
        if (!EventSystem.current.IsPointerOverGameObject()) {
            ToggleOpen panelManager = GameObject.FindGameObjectWithTag("panelManager").GetComponent<ToggleOpen>();
            panelManager.ClosePanel();
        }     
    }
}
