using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardInteraction : MonoBehaviour {

	public ChoicePanel choicePanel;
	public Text descriptionText;

	public delegate void ChoiceAction(int choice);
	public static event ChoiceAction OnChoice;

	public delegate void CardAction ();
	public static event CardAction OnCardPushedAside;
	// Swipe sensitivity
	public float swipeSensitivity = 1.0f;

	[Space]
	// Max rotation angle before the card gets stuck
	public float maxRotation = 80.0f;
	// Max distance the card can move
	public float maxDeviation = 1.0f;

	[Header ("Movement properties")]
	public float movementLerpFactor = 0.7f;
	// Rotation lerp factor
	public float rotationLerpFactor = 0.7f;
	//  Stabilization lerp factor
	public float stabilizationSpeed = 1.0f;

	[Header ("Stability thersholds")]
	// Minimum angle for the card to be considered stable
	public float stableRotationThreshold = 1.0f;
	// Minimum deviation from the center for the card to be considered stable
	public float stablePositionThreshold = 1.0f;

	// Variables related to description states (swiping up/down)
	[Header ("Description state")]
	// How long should be the delay before card is automatically swiped up
	public float descriptionDelay = 3.0f;
	// default Y coordinate for the card while description is visible
	public float descMaxY = 1.0f;
	// 
	public float descMovementLerpFactor = 0.2f;
	// How far up should the card to pe pulled to move to DESC_STABILIZE state (swiping up)
	public float descInThreshold = 0.2f;
	// How far up should the card to pe pulled to move to DESC_OUT state (swiping down)
	public float descOutThreshold = 0.2f;
	// Movement sensitivity up/down
	public float descMovementSensitivity = 0.1f;
	// how close does the card have to be to max Y for it to be considered stable
	public float descStableThreshold = 0.1f;


	private float degreesPerPixel = 1.0f;

	private bool frontActive = true;
	private bool asideOnRight = true;

	private Vector2 clickPos;

	private CardState cardState = CardState.DESC_DELAY;

	private bool buttonHeld = false;

	private enum CardState{
		STABLE,
		PREFLIP,
		STABILIZING,
		ASIDE,
		WAITING_FOR_EVENT,
		DESC_DELAY,
		DESC_STABILIZE,
		DESC_IN,
		DESC_OUT
	}

	// Use this for initialization
	void Start () {
		degreesPerPixel = Screen.width * swipeSensitivity;
		StartCoroutine (DescDelayCoroutine());
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

		if (Input.GetButtonDown ("Fire1")) {
			clickPos = Input.mousePosition;
			buttonHeld = true;
		} else if (Input.GetButtonUp ("Fire1")) {
			buttonHeld = false;
		}

		if (cardState == CardState.STABILIZING) {
			StabilizeRotation ();
		} else if (cardState == CardState.ASIDE) {
			MoveAside ();
		} else if (cardState == CardState.STABLE || cardState == CardState.PREFLIP) {
			if (Input.GetButtonUp ("Fire1") && cardState == CardState.PREFLIP) {
				float dx = -clickPos.x + Input.mousePosition.x;
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
					if (Mathf.Abs (transform.parent.position.x) <= stablePositionThreshold) {
						DescMoveTo (-(clickPos.y - Input.mousePosition.y) * descMovementSensitivity / Screen.height);
						if (transform.parent.position.y > descInThreshold) {
							cardState = CardState.DESC_STABILIZE;
							return;
						}
					}
					RotateTo ((-clickPos.x + Input.mousePosition.x) / Screen.width * swipeSensitivity);
				} else {
					StabilizeRotation ();
				}
			}
		} else if (cardState == CardState.DESC_STABILIZE) {
			StabilizeDesc ();
		} else if (cardState == CardState.DESC_IN) {
			if (Input.GetButton ("Fire1")) {
				DescMoveTo (descMaxY - ((clickPos.y - Input.mousePosition.y) * descMovementSensitivity / Screen.height));
				if (transform.parent.position.y < descMaxY - descOutThreshold && cardState == CardState.DESC_IN) {
					cardState = CardState.DESC_OUT;
				}
			} else {
				StabilizeDesc ();
			}
		} else if (cardState == CardState.DESC_OUT) {
			StabilizeDesc ();
		} else if (cardState == CardState.DESC_DELAY) {
			if (Input.GetButton ("Fire1")) {
				DescMoveTo (-(clickPos.y - Input.mousePosition.y) * descMovementSensitivity / Screen.height);
				if (transform.parent.position.y > descInThreshold) {
					cardState = CardState.DESC_STABILIZE;
					return;
				}
			} else {
				StabilizeDesc ();
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

	public IEnumerator DescDelayCoroutine(){
		yield return new WaitForSeconds(descriptionDelay);
		if (cardState == CardState.DESC_DELAY) {
			cardState = CardState.DESC_STABILIZE;
		}
		yield return null;
	}

	private void DescMoveTo(float y){
		transform.parent.position = Vector3.Lerp (transform.parent.position, new Vector3(0, Mathf.Clamp(y, 0, descMaxY)), descMovementLerpFactor * Time.deltaTime);
	}

	private void RotateTo(float x){
		choicePanel.target = x;
		//Debug.Log ("Rotating");
		float stableRotation;
		if (frontActive) {
			stableRotation = 0;
		} else {
			stableRotation = 180;
		}

		Quaternion targetRot;
		Vector3 targetPos;
		if (x * degreesPerPixel > maxRotation) {
			cardState = CardState.PREFLIP;
			targetRot = Quaternion.Euler (new Vector3(0, stableRotation + maxRotation, 0));
			targetPos = new Vector3(maxDeviation, 0, 0);
		} else if (x * degreesPerPixel < -maxRotation) {
			cardState = CardState.PREFLIP;
			targetRot = Quaternion.Euler (new Vector3 (0, stableRotation - maxRotation, 0));
			targetPos = new Vector3(-maxDeviation, 0, 0);
		} else {
			float maxSwipe = maxRotation / degreesPerPixel;

			cardState = CardState.STABLE;
			targetRot = Quaternion.Euler (new Vector3(0, stableRotation + x * degreesPerPixel, 0));
			targetPos = new Vector3(maxDeviation * x / maxSwipe, 0, 0);
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, rotationLerpFactor * Time.deltaTime);
		transform.parent.position = Vector3.Lerp (transform.parent.position, targetPos, movementLerpFactor * Time.deltaTime);
	}

	private void MoveAside(){
		choicePanel.target = 0;


		Quaternion targetRot;
		Vector3 targetPos;
		if (asideOnRight) {
			targetRot = Quaternion.Euler (0, GetStableRotation() + maxRotation, 0);
			targetPos = new Vector3 (maxDeviation, 0, 0);
		} else {
			targetRot = Quaternion.Euler (0, GetStableRotation() - maxRotation, 0);
			targetPos = new Vector3 (-maxDeviation, 0, 0);
		}

		transform.parent.position = Vector3.Lerp (transform.parent.position, targetPos, movementLerpFactor * Time.deltaTime);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, rotationLerpFactor * Time.deltaTime);
	}

	private void Flip(){
		buttonHeld = false;
		frontActive = !frontActive;
		cardState = CardState.STABILIZING;
	}

	private void StabilizeDesc(){
		if (cardState == CardState.DESC_IN || cardState == CardState.DESC_STABILIZE) {
			transform.parent.position = Vector3.Lerp (transform.parent.position, new Vector3 (0, descMaxY), descMovementLerpFactor * Time.deltaTime);
			if (cardState == CardState.DESC_STABILIZE && descMaxY - transform.parent.position.y < descStableThreshold) {
				cardState = CardState.DESC_IN;
			}
		} else if (cardState == CardState.DESC_OUT) {
			transform.parent.position = Vector3.Lerp (transform.parent.position, new Vector3 (0, 0), descMovementLerpFactor * Time.deltaTime);
			if (Vector3.Distance (transform.parent.position, Vector3.zero) <= stablePositionThreshold) {
				cardState = CardState.STABLE;
				SetTextAlpha (0);
			}
		}

		Quaternion targetRot = Quaternion.Euler (0, GetStableRotation(), 0);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, stabilizationSpeed * Time.deltaTime);

		float r = transform.parent.position.y / descMaxY;
		choicePanel.target = r;
		SetTextAlpha (r);
	}

	private void StabilizeRotation(){
		choicePanel.target = 0;
		Quaternion targetRot = Quaternion.Euler (0, GetStableRotation(), 0);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, stabilizationSpeed * Time.deltaTime);
		transform.parent.position = Vector3.Lerp (transform.parent.position, Vector3.zero, movementLerpFactor * Time.deltaTime);
		if (Quaternion.Angle (transform.rotation, targetRot) < stableRotationThreshold) {
			cardState = CardState.STABLE;
		}
	}

	private void SetTextAlpha(float alpha){
		Color c = descriptionText.color;
		descriptionText.color = new Color(c.r, c.g, c.b, alpha);
	}

	private float GetStableRotation(){
		float stableRotation;
		if (frontActive) {
			stableRotation = 0;
		} else {
			stableRotation = 180;
		}
		return stableRotation;
	}
}
