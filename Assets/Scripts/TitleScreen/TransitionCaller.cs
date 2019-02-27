using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCaller : MonoBehaviour {

	public GameObject LevelChanger;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CallTransition(int levelToLoad){
		LevelChanger.GetComponent<ScreenTransition>().FadeToLevel(levelToLoad);
	}
}
