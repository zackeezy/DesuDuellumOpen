using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour {
	public List<string> chatHistory = new List<string>();

	private string currentMessage;

	private void OnGUI(){
		GUILayout.BeginHorizontal(GUILayout.Width(250));
		currentMessage = GUILayout.TextField(currentMessage);
		if(GUILayout.Button("Send")){
			if(!string.isNullOrEmpty(currentMessage.Trim())){
				networkView.RPC("ChatMessage", RPCMode.AllBuffered, new object[] { currentMessage });
				ChatMessage = string.Empty;
			}
		}
		GUILayout.EndHorizontal();

		foreach(string c in chatHistory){
			GUILayout.Label(c);
		}
	}

	[RPC]
	public void ChatMessage(string message){
		chatHistory.Add(message);
	}
}
