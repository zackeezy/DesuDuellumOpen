using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

	public void ClientScene()
    {
        SceneManager.LoadScene("Client");
    }

    public void ServerScene()
    {
        SceneManager.LoadScene("Server");
    }
}
