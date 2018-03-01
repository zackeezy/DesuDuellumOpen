using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOpen : MonoBehaviour {

    public GameObject SelectedPanel = null;

    public void toggleOpen(GameObject panel)
    {
        if (SelectedPanel != null)
        {
            SelectedPanel.SetActive(false);
        }
        if (SelectedPanel != panel)
        {
            panel.SetActive(!panel.activeSelf);
        }
        if (panel.activeSelf)
        {
            SelectedPanel = panel;
        }
        else
        {
            SelectedPanel = null;
            //Debug.Log("deselect");
        }
    }
}
