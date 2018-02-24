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

    public void PopulateServers()
    {
        string serverListText = "";

        networkControl.client.GetComponent<Client>().servers.ForEach((server) =>
        {
            int index = networkControl.client.GetComponent<Client>().servers.FindIndex(s => s.IP == server.IP && s.Name == server.Name);

            serverListText += index + " " + server.Name + System.Environment.NewLine;
        });

        scrollView.content.GetChild(0).GetComponent<Text>().text = serverListText;
    }

    public void Connected()
    {
        scrollView.enabled = false;
        connectButton.enabled = false;
        selectServerText.enabled = false;
        serverNumber.enabled = false;
    }
}
