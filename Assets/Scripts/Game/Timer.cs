using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	
	public Text timertext;
	public float time = 0f;
	public bool finished = false;

	public bool hasPrinted = false;
	private bool hasSent = false;

	private List<float> laps = new List<float>();

	// Use this for initialization
	void Start () {
		
	}

	public void ResetTimer(){
		time = 0f;
	}

	public void addLap(int checkPoint){
		if(laps.Count == checkPoint){
			laps[checkPoint-1] = Mathf.Round(time * 1000f) / 1000f;
		}else{
			laps.Insert((checkPoint - 1),Mathf.Round(time * 1000f) / 1000f);
		}
	}

	public void finish(){
		laps.Insert(2,Mathf.Round(time * 1000f) / 1000f);
		finished = true;
	}

	public void printLaps(){
		if(hasPrinted == false){
			foreach(float lap in laps)
			{
				Debug.Log(lap);
			}
			hasPrinted = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(finished != true){
			time += Time.deltaTime;
		}
		if(finished == true && hasSent == false){
			sendScore();
			hasSent = true;
		}
		timertext.text = "Timer : " + time.ToString("F2") + "s" + 
						"\nR : Restart";/*\nKeys : Move\nLeftShift : Sprint\nSpace : Jump\nWallSlide : Automatic\nWalljump : jump while on wallSlide\nInner WallJump : WallSlide jump while key facing wall";*/
	}

	void sendScore(){
        string url = "https://anthonycuttivet.com/GOTY2019MIM/leaderboard.php";
        WWWForm form = new WWWForm();
        form.AddField("t1", laps[0].ToString());
		form.AddField("t2", laps[1].ToString());
		form.AddField("tt", laps[2].ToString());
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www){
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
	        Debug.Log("WWW Error: " + www.error);
        }
    }
}
