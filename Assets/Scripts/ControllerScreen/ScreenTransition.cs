using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenTransition : MonoBehaviour {

	public Animator animator;

	public float waitTime = 3.0f;

	private int levelToLoad;

	void Start(){
		if(waitTime != -1){
			StartCoroutine(ExecuteAfterTime(waitTime));
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void FadeToLevel(int levelIndex){
		levelToLoad = levelIndex;
		animator.SetTrigger("fadeOut");
		Debug.Log("Fading to level " + levelIndex);
	}

	public void OnFadeComplete(){
		Debug.Log("Fade Complete to " + levelToLoad);
		SceneManager.LoadScene(levelToLoad);
	}

	 private IEnumerator ExecuteAfterTime(float time){
		yield return new WaitForSeconds(time);
	
		// After delay is up
		FadeToLevel(1);

	}
}
