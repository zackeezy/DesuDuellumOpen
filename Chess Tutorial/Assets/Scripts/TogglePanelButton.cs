using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanelButton : MonoBehaviour {
    //https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/panels-panes-windows
    public void TogglePanel (GameObject panel){
        panel.SetActive(!panel.activeSelf);
    }
}
