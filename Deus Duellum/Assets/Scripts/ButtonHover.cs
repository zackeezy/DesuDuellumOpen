using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour {

    private Vector3 oldScale;

	// Use this for initialization
	void Start () {
        oldScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void MouseEnter()
    {
        //mouse enters the space
        //Debug.Log("entered");
        Vector3 newscale = oldScale*1.1f;
        LeanTween.scale(gameObject, newscale, .05f);
    }

    public void MouseExit()
    {
        //Debug.Log("exited");
        //mouse exits the space
        LeanTween.scale(gameObject, oldScale, .05f);
    }
}
