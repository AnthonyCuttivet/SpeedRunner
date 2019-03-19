using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public int maxJumps = 2;

	[HideInInspector]
	public int remainingJumps;
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;
	float defaultSpeed;
	public float sprintSpeed = 8;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	[HideInInspector]
	public bool grounded = false;

	[HideInInspector]
	public Vector3 spawnPosition;

	public Transform dynamicPlatforms;
	
	private List<Transform> dynamicPlatformsOriginalPositions = new List<Transform>();

	void Start() {

		remainingJumps = maxJumps;
		defaultSpeed = moveSpeed;

		//player original position
		spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

		//get all dynamic platforms
		foreach(Transform child in dynamicPlatforms){
			dynamicPlatformsOriginalPositions.Add(child);
		}
	}

	void Update() {
		CalculateVelocity ();
		HandleWallSliding ();

		controller.Move (velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}

		if(controller.collisions.below){ // check if player is on the ground
			grounded = true;
		}else{
			grounded = false;
		}

		if(grounded){
			remainingJumps = maxJumps;
		}
	}

	public void ResetLevel(){
		velocity.y = -velocity.y;
		//reset player position
		this.transform.position = spawnPosition;
		Debug.Log(velocity);
		velocity = Vector3.zero;
		//reset player jumps
		remainingJumps = maxJumps;
		//reset player checkpoint count
		this.GetComponent<CheckpointsController>().lastCheckPoint = 0;
		//reset all platforms position
		int i = 0;
		foreach(Transform child in dynamicPlatforms){
			child.GetComponent<PlatformController>().ResetPlatform();
			i++;
		}
	}

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void OnSprintInputDown(){
		moveSpeed = sprintSpeed;
	}

	public void OnSprintInputUp(){
		moveSpeed = defaultSpeed;
	}

	public void OnJumpInputDown() {
		if (wallSliding) { // walljump
			if (wallDirX == directionalInput.x) { //player jumps against the wall
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) { // player jumps without any direction
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {							   // player jumps off the wall
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		} else if (controller.collisions.below) { //if player is on the ground
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
				remainingJumps --;
			}
		}
		
		if(grounded == false && remainingJumps > 0){ // if player is in the air and have at least one jump available
			velocity.y = maxJumpVelocity;
			remainingJumps --;
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}
		

	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}
