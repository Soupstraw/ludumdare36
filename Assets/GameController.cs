using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
	public UnityEngine.UI.Text StoryTitle;
	public UnityEngine.UI.Text StoryDescription;

	public UnityEngine.GameObject StoryFace;
	public UnityEngine.Texture2D BlankCard;

	public UnityEngine.UI.Text YesTitle;
	public UnityEngine.UI.Text YesDescription;

	public UnityEngine.UI.Text NoTitle;
	public UnityEngine.UI.Text NoDescription;

	public CardAnimator cardAnimator;
	public CardSounds cardSounds;
	public AmbientSounds ambientSounds;
	public AmbientBackground ambientBackground;

	private Game.State state;
	private Game.Card.Choice result;

	// Use this for initialization
	void Start ()
	{
		cardAnimator = GetComponent<CardAnimator> ();
		cardSounds = GetComponent<CardSounds> ();
		ambientSounds = GetComponent<AmbientSounds> ();
		ambientBackground = GetComponent<AmbientBackground> ();

		SetupNewGame ();
		UpdateStory ();
	}

	void SetupNewGame ()
	{
		state = new Game.State ();
		state.setup ();
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

		// handle click
		if (!Input.GetButtonUp ("Fire1")) {
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

					cardSounds.FlipStoryCard ();
					UpdateOptionTitles ();
				} else if (cardAnimator.state == CardAnimator.State.Description) {
					cardAnimator.SetTargetState (CardAnimator.State.Image);
					cardSounds.FlipStoryCard ();
				}
			}

			if (target == cardAnimator.YesCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					ChooseYes ();

					cardSounds.SelectOption ();
					cardAnimator.SetTargetState (CardAnimator.State.Yes);
				} else if (cardAnimator.state == CardAnimator.State.Yes) {
					UpdateStory ();

					cardSounds.NewStoryCard ();
					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}

			if (target == cardAnimator.NoCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					ChooseNo ();

					cardSounds.SelectOption ();
					cardAnimator.SetTargetState (CardAnimator.State.No);
				} else if (cardAnimator.state == CardAnimator.State.No) {
					UpdateStory ();

					cardSounds.NewStoryCard ();
					cardAnimator.SetTargetState (CardAnimator.State.Image);
				}
			}
		}
	}

	void SetStory (String title, String image, String description)
	{
		StoryDescription.text = description;

		if (image == "") {
			image = title;
		}

		String imagePath = "Card/" + image;
		Texture2D tex = Resources.Load <Texture2D> (imagePath);

		Renderer renderer = StoryFace.GetComponent<Renderer> ();
		if (tex == null) {
			StoryTitle.text = title;
			renderer.material.mainTexture = BlankCard;
			
			//StoryFace.
			//StoryFaceMaterial.mainTexture = StoryFaceMissing;
		} else {
			StoryTitle.text = "";	
			renderer.material.mainTexture = tex;
			//StoryFaceMaterial.mainTexture = tex;
		}
	}

	void UpdateStory ()
	{
		if (state.deckEmpty ()) {
			SetStory (
				"Death",
				"Death",
				"Your journey has come to an end."
			);
			return;
		}

		SetStory (
			state.currentCard.title,
			state.currentCard.image,
			state.currentDescription
		);

		ambientSounds.ChangeEnvironment (state.currentCard.environment);
		ambientBackground.ChangeEnvironment (state.currentCard.environment);
	}

	void UpdateOptionTitles ()
	{
		if (state.deckEmpty ()) {
			YesTitle.text = "Try again";
			NoTitle.text = "Quit";

			return;
		}

		YesTitle.text = state.currentOptions.yes.title;
		NoTitle.text = state.currentOptions.no.title;
	}


	void ChooseYes ()
	{
		if (state.deckEmpty ()) {
			YesDescription.text = "Maybe this time things will go differently.";
			SetupNewGame ();
			return;
		}

		result = state.yes ();
		YesDescription.text = result.description;
	}

	void ChooseNo ()
	{
		if (state.deckEmpty ()) {
			NoDescription.text = "You can leave all your worries behind now.";
			Application.Quit ();
			return;
		}

		result = state.no ();
		NoDescription.text = result.description;
	}
}
