using UnityEngine;
using System.Collections;

public class CardInteraction : MonoBehaviour {

	public delegate void ChoiceAction(int choice);
	public static event ChoiceAction OnChoice;

	public delegate void CardAction ();
	public static event CardAction OnCardPushedAside;
	// Swipe sensitivity
	public float swipeSensitivity = 1.0f;

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



	private float degreesPerPixel = 1.0f;

	private bool frontActive = true;
	private bool asideOnRight = true;

	private Vector2 clickPos;

	private CardState cardState = CardState.WAITING_FOR_EVENT;

	private bool buttonHeld = false;

	private enum CardState{
		STABLE,
		PREFLIP,
		STABILIZING,
		ASIDE,
		WAITING_FOR_EVENT
	}

	// Use this for initialization
	void Start () {
		degreesPerPixel = Screen.width * swipeSensitivity;
	}

	void OnEnable(){
		SlidingScrollPanel.OnDialogDismissed += DialogDismissed;
		FadingPanel.OnDialogDismissed += DialogDismissed;
	}

	void OnDisable(){
		SlidingScrollPanel.OnDialogDismissed -= DialogDismissed;
		FadingPanel.OnDialogDismissed -= DialogDismissed;
	}
		
	// Update is called once per frame
	void Update () {
		if (cardState == CardState.STABILIZING) {
			StabilizeRotation ();
		} else if(cardState == CardState.ASIDE) {
			MoveAside ();
		} else if(cardState == CardState.STABLE || cardState == CardState.PREFLIP){
			if (Input.GetButtonDown ("Fire1")) {
				clickPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				buttonHeld = true;
			}

			if (Input.GetButtonUp ("Fire1") && cardState == CardState.PREFLIP) {
				float dx = clickPos.x - Input.mousePosition.x;
				if (OnCardPushedAside != null) {
					OnCardPushedAside ();
				}
				cardState = CardState.ASIDE;

				if (dx < 0) {
					asideOnRight = false;
				} else {
					asideOnRight = true;
				}
				if (OnChoice != null) {
					OnChoice ((int)Mathf.Sign (dx));
				}
				Debug.Log ("Choice: " + (dx < 0 ? 0 : 1));
			} else {
				if (Input.GetButton ("Fire1") && buttonHeld) {
					RotateTo (clickPos.x - Input.mousePosition.x);
				} else {
					StabilizeRotation ();
				}
			}
		}
	}

	public void DialogDismissed(){
		if (cardState == CardState.WAITING_FOR_EVENT) {
			cardState = CardState.STABILIZING;
		} else if (cardState == CardState.ASIDE) {
			Flip ();
		}
	}

	private void RotateTo(float dx){
		//Debug.Log ("Rotating");
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

	private void MoveAside(){

		float stableRotation;
		if (frontActive) {
			stableRotation = 0;
		} else {
			stableRotation = 180;
		}

		Quaternion targetRot;
		Vector3 targetPos;
		if (asideOnRight) {
			targetRot = Quaternion.Euler (0, stableRotation + maxRotation, 0);
			targetPos = new Vector3 (maxDeviation, 0, 0);
		} else {
			targetRot = Quaternion.Euler (0, stableRotation - maxRotation, 0);
			targetPos = new Vector3 (-maxDeviation, 0, 0);
		}

		transform.parent.position = Vector3.Lerp (transform.parent.position, targetPos, movementLerpFactor);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, rotationLerpFactor);
	}

	private void Flip(){
		Debug.Log ("Flipping.");
		buttonHeld = false;
		frontActive = !frontActive;
		cardState = CardState.STABILIZING;
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
