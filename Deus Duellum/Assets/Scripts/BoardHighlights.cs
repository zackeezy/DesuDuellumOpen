using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighlights : MonoBehaviour {

	public static BoardHighlights Instance{ get; set;}
	public GameObject highlightPrefab;
	private List<GameObject> highlights;

	private void Start(){
		Instance = this;
		highlights = new List<GameObject> ();
	}

	private GameObject GetHighlightObject(){
		GameObject gameObject = highlights.Find (g => !g.activeSelf);

		if (gameObject == null) {
			gameObject = Instantiate (highlightPrefab);
			highlights.Add (gameObject);
		}
		return gameObject;
	}
	public void HighlightAllowedMoves(bool[,]moves){
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if (moves [i, j]) {
					GameObject gameObject = GetHighlightObject ();
					gameObject.SetActive (true);
					gameObject.transform.position = new Vector3 (i+0.5f, 0.005f, j+0.5f);
				}
			}
		}
	}

	public void HideHighlights(){
		foreach (GameObject gameObject in highlights) {
			gameObject.SetActive (false);
		}
	}
}
