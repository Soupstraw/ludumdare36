using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
	private CardAnimator cardAnimator;

	// Use this for initialization
	void Start ()
	{
		cardAnimator = GetComponent<CardAnimator> ();
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 position = Input.mousePosition;
		Vector3 relative = position;
		relative.x = 2.0f * (relative.x / Screen.width) - 1f;
		relative.y = 2.0f * (relative.y / Screen.height) - 1f;
		relative.Normalize ();

		cardAnimator.SetTilt (relative);

		if (cardAnimator.animating ()) {
			return;
		}

		bool trigger = Input.GetButtonDown ("Fire1");
		if (!trigger) {
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			GameObject target = hit.collider.gameObject.transform.parent.gameObject;

			if (target == cardAnimator.StoryCard) {
				if (cardAnimator.state == CardAnimator.State.Image) {
					cardAnimator.SetTargetState (CardAnimator.State.Description);
				} else if (cardAnimator.state == CardAnimator.State.Description) {
					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}

			if (target == cardAnimator.YesCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					cardAnimator.SetTargetState (CardAnimator.State.Yes);
				} else if (cardAnimator.state == CardAnimator.State.Yes) {
					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}

			if (target == cardAnimator.NoCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					cardAnimator.SetTargetState (CardAnimator.State.No);
				} else if (cardAnimator.state == CardAnimator.State.No) {
					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}
		}
	}
}
