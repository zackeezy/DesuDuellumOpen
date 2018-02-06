using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOpen : MonoBehaviour {

    private bool isOpen = false;
    public GameObject toToggle = null;

    public void toggleOpen()
    {
        isOpen = !isOpen;
        setVisibility();
    }

    private void setVisibility()
    {
        if (toToggle != null)
        {
            toToggle.SetActive(isOpen);
        }
    }
}
