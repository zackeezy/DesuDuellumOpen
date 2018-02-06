using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWinPanel : MonoBehaviour {
    public void WinPanel(GameObject WinPanel)
    {
        WinPanel.SetActive(!WinPanel.activeSelf);
    }
}
