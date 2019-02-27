using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {


	Player player;

	public Timer timer;

	[HideInInspector]
	public bool triggerLock = false;

	void Start(){
		player = GetComponent<Player> ();
	}

	void Update () {
		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		StateManager.previousDI = directionalInput;
		player.SetDirectionalInput(directionalInput);

		/* Jump */
		if (Input.GetButtonDown("Jump")) {
			player.OnJumpInputDown ();
		}else if (Input.GetButtonUp("Jump")) {
			player.OnJumpInputUp ();
		}

		/* Keyboard Sprint */
		if(Input.GetButtonDown("Sprint")){
			StateManager.wasSprintDown = true;
			player.OnSprintInputDown();
		}else if(Input.GetButtonUp("Sprint")){
			StateManager.wasSprintDown = false;
			player.OnSprintInputUp();
		}

		/* Xbox Sprint */
		if (Input.GetAxis("RT") == 1) {
			player.OnSprintInputDown();
			triggerLock = false;
		}
		if (Input.GetAxis("RT") == 0 && triggerLock == false) {
			player.OnSprintInputUp();
			triggerLock = true;
		}

		/* Return to last visited checkpoint */
		if(Input.GetKeyDown(KeyCode.Backspace)){
			this.GetComponent<CheckpointsController>().SendToLastCheckPoint();
		}

		/* Reset level */
		if (Input.GetButtonDown("Reset")) {
			Time.timeScale = 1f;
			timer.ResetTimer();
			player.ResetLevel();
		}
	}
}
