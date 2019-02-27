using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	
	public Text timertext;
	float time = 0f;

	// Use this for initialization
	void Start () {
		
	}

	public void ResetTimer(){
		time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		timertext.text = "Timer : " + time.ToString("F2") + "s" + 
						"\nR : Restart";/*\nKeys : Move\nLeftShift : Sprint\nSpace : Jump\nWallSlide : Automatic\nWalljump : jump while on wallSlide\nInner WallJump : WallSlide jump while key facing wall";*/
	}
}
