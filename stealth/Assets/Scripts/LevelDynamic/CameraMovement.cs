﻿using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float smooth=1.5f;

	private Transform player;		//position of the player
	private Vector3 relCameraPos;
	private float relCameraPosMag;
	private Vector3 newPos;		//new position of camera

	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f; 		//to reduce the mangitude between camera and player in order to have camera view at the top of player's head instead of feet.

	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 standardPos = player.position + relCameraPos;
		Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;
		Vector3[] checkPoints = new Vector3[5];

		checkPoints [0] = standardPos;
		checkPoints [1] = Vector3.Lerp (standardPos, abovePos, 0.25f);
		checkPoints [2] = Vector3.Lerp (standardPos, abovePos, 0.5f);
		checkPoints [3] = Vector3.Lerp (standardPos, abovePos, 0.75f);
		checkPoints [4] = abovePos;

		for(int iii=0;iii<checkPoints.Length;iii++)
		{
			if(ViewingPosCheck(checkPoints[iii]))
			{
				break;
			}
		}
		transform.position=Vector3.Lerp (transform.position,newPos,smooth*Time.deltaTime);
		SmoothLookAt ();
	}
	
	bool ViewingPosCheck(Vector3 checkPos)
	{
		RaycastHit hit;
		if (Physics.Raycast (checkPos, player.position - checkPos, out hit, relCameraPosMag))
		{
			if(hit.transform!=player)
			{
				return false;
			}
		}
		newPos = checkPos;
		return true;
	}
	void SmoothLookAt()
	{
		Vector3 relPlayerPosition = player.position - transform.position;
		Quaternion lookAtRotation = Quaternion.LookRotation (relPlayerPosition, Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
}
