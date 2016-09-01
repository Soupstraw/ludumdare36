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
		if (!cardAnimator.animating ()) {
			if (cardAnimator.state == CardAnimator.State.Yes) {
				cardAnimator.SetTargetState (CardAnimator.State.Image);
			} else if (cardAnimator.state == CardAnimator.State.Image) {
				cardAnimator.SetTargetState (CardAnimator.State.Description);
			} else if (cardAnimator.state == CardAnimator.State.Description) {
				//cardAnimator.SetTargetState (CardAnimator.State.Yes);
			}
		}
	}
}
