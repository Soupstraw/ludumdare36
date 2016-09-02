using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
	public UnityEngine.UI.Text StoryTitle;
	public UnityEngine.UI.Text StoryDescription;

	public UnityEngine.Material StoryFaceMaterial;
	public UnityEngine.Texture2D StoryFaceMissing;

	public UnityEngine.UI.Text YesTitle;
	public UnityEngine.UI.Text YesDescription;

	public UnityEngine.UI.Text NoTitle;
	public UnityEngine.UI.Text NoDescription;

	private CardAnimator cardAnimator;

	private Game.State state;
	private Game.Card.Choice result;

	// Use this for initialization
	void Start ()
	{
		cardAnimator = GetComponent<CardAnimator> ();

		SetupNewGame ();
	}

	void SetupNewGame ()
	{
		state = new Game.State ();
		state.setup ();

		UpdateStory ();
		UpdateOptionTitles ();
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
			// there must be a clearer way to implement null checks in C#???
			if (hit.collider == null) {
				return;
			}
			if (hit.collider.gameObject == null) {
				return;
			}
			if (hit.collider.gameObject.transform.parent == null) {
				return;
			}

			GameObject target = hit.collider.gameObject.transform.parent.gameObject;

			if (target == null) {
				return;
			}

			if (target == cardAnimator.StoryCard) {
				if (cardAnimator.state == CardAnimator.State.Image) {
					cardAnimator.SetTargetState (CardAnimator.State.Description);
					UpdateOptionTitles ();
				} else if (cardAnimator.state == CardAnimator.State.Description) {
					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}

			if (target == cardAnimator.YesCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					ChooseYes ();

					cardAnimator.SetTargetState (CardAnimator.State.Yes);
				} else if (cardAnimator.state == CardAnimator.State.Yes) {
					UpdateStory ();

					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}

			if (target == cardAnimator.NoCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					ChooseNo ();

					cardAnimator.SetTargetState (CardAnimator.State.No);
				} else if (cardAnimator.state == CardAnimator.State.No) {
					UpdateStory ();

					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}
		}
	}

	void ChooseYes ()
	{
		result = state.yes ();
		YesDescription.text = result.description;
	}

	void ChooseNo ()
	{
		result = state.no ();
		NoDescription.text = result.description;
	}

	void UpdateStory ()
	{
		// TODO: update texture
		StoryDescription.text = state.currentDescription;

		String imageName = "Card/" + state.currentCard.image;
		Texture2D tex = Resources.Load <Texture2D> (imageName);
		if (tex == null) {
			StoryFaceMaterial.mainTexture = StoryFaceMissing;
		} else {
			StoryFaceMaterial.mainTexture = tex;
		}
	}

	void UpdateOptionTitles ()
	{
		YesTitle.text = state.currentOptions.yes.title;
		NoTitle.text = state.currentOptions.no.title;
	}
}
