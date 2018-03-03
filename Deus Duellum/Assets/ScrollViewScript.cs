using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScript : MonoBehaviour {

    public ScrollRect scrollView;
    public Button connectButton;
    public Text selectServerText;
    public InputField serverNumber;
    public NetworkControl networkControl;

    private void Start()
    {
        this.name = "ScrollView";
    }

    public void Connected()
    {
        scrollView.enabled = false;
        connectButton.enabled = false;
        selectServerText.enabled = false;
        serverNumber.enabled = false;
    }
}
