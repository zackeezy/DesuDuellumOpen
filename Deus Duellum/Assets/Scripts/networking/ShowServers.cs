//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ShowServers : MonoBehaviour {

//    public NetworkControl netcontroller;
//    //public GameObject content;
//    public Text serverlist;

//	// Use this for initialization
//	void Start () {

//	}

//	// Update is called once per frame
//	void Update () {
//        try
//        {
//            PlayerInfo[] servers = netcontroller.GetServerListFromClient();

//            serverlist.text = "";

//            foreach (PlayerInfo info in servers)
//            {
//                serverlist.text += info.Name + "\n";
//            }
//        }
//        catch(Exception e)
//        {
//            Debug.Log(e.Message);
//        }
//	}
//}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowServers : MonoBehaviour
{

    private int _updateCount = 200;

    public GameObject playButton;
    public GameObject prefabButton;
    public GameObject ParentPanel;
    public GameObject selection;
    private Text selectionText;

    public NetworkControl netcontroller;
    //public GameObject content;
    //public Text serverlist;

    // Use this for initialization
    void Start()
    {
        selectionText = selection.GetComponent<Text>();
    }

    private void ButtonClicked(int number, string name)
    {
        Debug.Log("BButton Clicked = " + number);
        selectionText.text = name;
        Button play = playButton.GetComponent<Button>();
        play.interactable = true;
        //netcontroller.ServerSelected(number);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (_updateCount >= 200)
            {
                Debug.Log("Searching for Games.");
                foreach (Transform child in ParentPanel.transform)
                {
                    Destroy(child.gameObject);
                }
                PlayerInfo[] servers = netcontroller.GetServerListFromClient(true);

                for (int i = 0; i < servers.Length; i++)
                //for (int i = 0; i < 5; i++)
                {
                    GameObject goButton = (GameObject)Instantiate(prefabButton);
                    goButton.transform.SetParent(ParentPanel.GetComponent<RectTransform>(), false);
                    goButton.transform.localScale = new Vector3(1, 1, 1);
                    goButton.GetComponentInChildren<Text>().text = servers[i].Name;
                    //goButton.GetComponentInChildren<Text>().text = "Button " + i;
                    Button tempButton = goButton.GetComponent<Button>();

                    //int tempInt = i;
                    string temp = tempButton.transform.GetChild(0).GetComponent<Text>().text;
                    tempButton.onClick.AddListener(() => ButtonClicked(i, temp));
                }
                _updateCount = 0;
            }
            else
            {
                _updateCount++;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
