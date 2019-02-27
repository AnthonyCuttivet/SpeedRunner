using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsController : MonoBehaviour {

	public Player player;

	public Transform checkpoints;
	private List<Transform> checkPointsList = new List<Transform>();

	public int lastCheckPoint = 0;

	public float respawnHeight = 4.5f;

	// Use this for initialization
	void Start () {
		//get all checkpoints coordinates
		foreach(Transform child in checkpoints){
			checkPointsList.Add(child);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SendToLastCheckPoint(){
		if(lastCheckPoint == 0){
			player.transform.position = player.spawnPosition;
		}else{
			player.transform.position = new Vector3(checkPointsList[lastCheckPoint - 1].transform.position.x,(checkPointsList[lastCheckPoint - 1].transform.position.y - respawnHeight),0);
		}
	}
}
