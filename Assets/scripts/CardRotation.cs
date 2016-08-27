﻿using UnityEngine;
using System.Collections;

public class CardRotation : MonoBehaviour {

	public delegate void ChoiceAction(int choice);
	public static event ChoiceAction OnChoice;

	public float degreesPerPixel = 1.0f;
	public float positionChangeFactor = 1.0f;

	public float rotationLerpFactor = 0.7f;

	public float stabilizationSpeed = 1.0f;
	public float maxRot = 80.0f;
	public float stableRotationThreshold = 1.0f;
	public float StablePositionThreshold = 1.0f;

	private bool frontActive = true;

	private Vector2 lastMousePos;

	private CardState cardState;
	private bool buttonHeld = false;

	private enum CardState{
		STABLE,
		PREFLIP,
		STABILIZING
	}

	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		/*if (Input.touchCount > 0) {
			Touch touch = Input.touches [0];
			switch (touch.phase) {
			case TouchPhase.Moved:
				RotateBy (touch.deltaPosition.x);
				break;
			case TouchPhase.Ended:
				StabilizeRotation ();
				break;
			}
		}*/

		if (cardState == CardState.STABILIZING) {
			StabilizeRotation ();
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				lastMousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				buttonHeld = true;
			}

			if (Input.GetButtonUp ("Fire1") && cardState == CardState.PREFLIP) {
				float dx = lastMousePos.x - Input.mousePosition.x;
				if (dx < 0) {
					Flip (0);
				} else {
					Flip (1);
				}
			}

			if (Input.GetButton ("Fire1") && buttonHeld) {
				RotateTo (lastMousePos.x - Input.mousePosition.x);
			} else {
				StabilizeRotation ();
			}
		}
	}

	private void RotateTo(float dx){
		float stableRotation;
		if (frontActive) {
			stableRotation = 0;
		} else {
			stableRotation = 180;
		}

		Quaternion targetRot;
		if (dx * degreesPerPixel > maxRot) {
			cardState = CardState.PREFLIP;
			targetRot = Quaternion.Euler (new Vector3(0, stableRotation + maxRot, 0));
		} else if (dx * degreesPerPixel < -maxRot) {
			cardState = CardState.PREFLIP;
			targetRot = Quaternion.Euler (new Vector3 (0, stableRotation - maxRot, 0));
		} else {
			targetRot = Quaternion.Euler (new Vector3(0, stableRotation + dx * degreesPerPixel, 0));
			cardState = CardState.STABLE;
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, rotationLerpFactor);
	}

	private void Flip(int choice){
		Debug.Log ("Choice: " + choice);
		buttonHeld = false;
		frontActive = !frontActive;
		cardState = CardState.STABILIZING;
		if (OnChoice != null) {
			OnChoice (choice);
		}
	}

	private void StabilizeRotation(){
		Quaternion targetRot;
		if (frontActive) {
			targetRot = Quaternion.Euler (0, 0, 0);
		} else {
			targetRot = Quaternion.Euler (0, 180, 0);
		}
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, stabilizationSpeed);
		if (Quaternion.Angle (transform.rotation, targetRot) < stableRotationThreshold) {
			cardState = CardState.STABLE;
		}
	}
}
