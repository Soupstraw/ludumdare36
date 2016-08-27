using UnityEngine;
using System.Collections;

public class CardRotation : MonoBehaviour {

	public delegate void ChoiceAction(int choice);
	public static event ChoiceAction OnChoice;

	// Rotation degrees per pixels swiped
	public float degreesPerPixel = 1.0f;

	// Max rotation angle before the card gets stuck
	public float maxRotation = 80.0f;
	// Max distance the card can move
	public float maxDeviation = 1.0f;

	public float movementLerpFactor = 0.7f;
	// Rotation lerp factor
	public float rotationLerpFactor = 0.7f;
	//  Stabilization lerp factor
	public float stabilizationSpeed = 1.0f;

	// Minimum angle for the card to be considered stable
	public float stableRotationThreshold = 1.0f;
	// Minimum deviation from the center for the card to be considered stable
	public float stablePositionThreshold = 1.0f;

	// Is the front of the card currently visible?
	private bool frontActive = true;

	private Vector2 clickPos;

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
				clickPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				buttonHeld = true;
			}

			if (Input.GetButtonUp ("Fire1") && cardState == CardState.PREFLIP) {
				float dx = clickPos.x - Input.mousePosition.x;
				if (dx < 0) {
					Flip (0);
				} else {
					Flip (1);
				}
			}

			if (Input.GetButton ("Fire1") && buttonHeld) {
				RotateTo (clickPos.x - Input.mousePosition.x);
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
		Vector3 targetPos;
		if (dx * degreesPerPixel > maxRotation) {
			cardState = CardState.PREFLIP;
			targetRot = Quaternion.Euler (new Vector3(0, stableRotation + maxRotation, 0));
			targetPos = new Vector3(maxDeviation, 0, 0);
		} else if (dx * degreesPerPixel < -maxRotation) {
			cardState = CardState.PREFLIP;
			targetRot = Quaternion.Euler (new Vector3 (0, stableRotation - maxRotation, 0));
			targetPos = new Vector3(-maxDeviation, 0, 0);
		} else {
			float maxSwipe = maxRotation / degreesPerPixel;

			cardState = CardState.STABLE;
			targetRot = Quaternion.Euler (new Vector3(0, stableRotation + dx * degreesPerPixel, 0));
			targetPos = new Vector3(maxDeviation * dx / maxSwipe, 0, 0);
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, rotationLerpFactor);
		transform.parent.position = Vector3.Lerp (transform.parent.position, targetPos, movementLerpFactor);
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
		transform.parent.position = Vector3.Lerp (transform.parent.position, Vector3.zero, movementLerpFactor);
		if (Quaternion.Angle (transform.rotation, targetRot) < stableRotationThreshold) {
			cardState = CardState.STABLE;
		}
	}
}
