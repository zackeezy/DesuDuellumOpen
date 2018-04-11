using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public int player = 1;
    public Button newNetButton;
    public Button joinNetButton;

    private GameObject Logo = null;

    void Awake()
    {
        //Logo = GameObject.FindGameObjectWithTag("Finish");
        //if (Logo)
        //{
        //    LeanTween.alpha(Logo, 0f, 4f).setOnComplete(LoadMainMenu);
        //}
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    void Update(){

    }

    public void LoadByIndex(int sceneIndex)
    {
        if (sceneIndex == 8)
        {
            CharacterSelect selection = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSelect>();
            selection.PlayClicked = true;
        }
        //do a cool scene transition
        AutoFade.LoadLevel(sceneIndex, 1, 1, Color.black);
        //SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNetworkConnection(bool server)
    {
        InputField nameInput = GameObject.FindGameObjectWithTag("network").GetComponent<InputField>();
        if (nameInput)
        {
            string name = nameInput.text;
            if (name != "")
            {
                PlayerPrefs.SetString("name", name);
                if (server)
                {
                    PlayerPrefs.SetInt("server", 1);
                    AutoFade.LoadLevel(3, 1, 1, Color.black);
                }
                else
                {
                    PlayerPrefs.SetInt("server", 0);
                    AutoFade.LoadLevel(3, 1, 1, Color.black);
                }
            }
        }
    }

    public void EnableNetworkButtons()
    {
        string servername = GetComponent<InputField>().text;
        if (servername != "")
        {
            newNetButton.interactable = true;
            joinNetButton.interactable = true;
        }
        else
        {
            newNetButton.interactable = false;
            joinNetButton.interactable = false;
        }
    }
}
