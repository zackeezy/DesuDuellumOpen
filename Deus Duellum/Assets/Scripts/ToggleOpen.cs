using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOpen : MonoBehaviour {

    public void toggleOpen(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
