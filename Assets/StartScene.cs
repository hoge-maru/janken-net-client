using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {

	
	void OnGUI(){
		if(GUI.Button (new Rect (180,180,100,100), "Game Start")){
			Application.LoadLevel("janken-game-scene");
		}

	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
