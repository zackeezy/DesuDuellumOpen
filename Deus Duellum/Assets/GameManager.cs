using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text you;
	public Text them;

	public Button serverButton;
	public Button clientButton;

	public InputField yourMessage;
	public Button submit;

	private Client client;
	private Server server;

	private bool isClient;

	void Start(){
		Button btn1 = serverButton.GetComponent<Button>();
        btn1.onClick.AddListener(ServerOnClick);

		Button btn2 = clientButton.GetComponent<Button>();
        btn2.onClick.AddListener(ClientOnClick);

		Button btn3 = submit.GetComponent<Button>();
        btn3.onClick.AddListener(SendMessage);
	}

	public void ClientOnClick(){
		client = new Client();
		isClient = true;

		DisableButtons();
	}

	public void ServerOnClick(){
		server = new Server();
		isClient = false;

		DisableButtons();
	}

	public void SendMessage(){
		if(isClient){
			Client you = client;
		}
		else{
			Server you = server;
		}


	}

	private void DisableButtons(){
		clientButton.interactable = false;
		serverButton.interactable = false;
	}
}
