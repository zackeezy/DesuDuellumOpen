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

    public void ClosePanel()
    {
        if (SelectedPanel != null)
        {
            //Debug.Log("clicked on the thing");
            SelectedPanel.SetActive(false);
            SelectedPanel = null;
        }
    }

    public void CloseQuit()
    {
        //to close the quit game panel when click on the gear button
        if(SelectedPanel != null && SelectedPanel == GameObject.FindGameObjectWithTag("QuitPanel"))
        {
            Debug.Log("quit panel was open, close it");
            //quit panel was open, close it
            SelectedPanel.SetActive(false);
            SelectedPanel = null;
        }
    }
}
