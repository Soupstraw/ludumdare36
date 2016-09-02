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
	public UnityEngine.UI.Text NoTitle;
	public UnityEngine.UI.Text OptionDescription;

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

		if (cardAnimator.state == CardAnimator.State.History) {
			UpdateStory ();
			cardSounds.NewStoryCard ();
			cardAnimator.SetTargetState (CardAnimator.State.Image);
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
				} else if (cardAnimator.state == CardAnimator.State.Option) {
					cardSounds.NewStoryCard ();
					cardAnimator.SetTargetState (CardAnimator.State.History);
				}
			}

			if (target == cardAnimator.YesCard || target == cardAnimator.NoCard) {
				if (cardAnimator.state == CardAnimator.State.Description) {
					if (target == cardAnimator.YesCard) {
						ChooseYes ();
					}
					if (target == cardAnimator.NoCard) {
						ChooseNo ();
					}

					cardSounds.SelectOption ();
					cardAnimator.SetTargetState (CardAnimator.State.Option);
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
		} else {
			StoryTitle.text = "";	
			renderer.material.mainTexture = tex;
		}
	}

	void UpdateStory ()
	{
		OptionDescription.gameObject.SetActive (false);
		StoryTitle.gameObject.SetActive (true);
		StoryFace.gameObject.SetActive (true);

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
		OptionDescription.gameObject.SetActive (true);
		StoryTitle.gameObject.SetActive (false);
		StoryFace.gameObject.SetActive (false);

		if (state.deckEmpty ()) {
			OptionDescription.text = "Maybe this time things will go differently.";
			SetupNewGame ();
			return;
		}

		result = state.yes ();
		OptionDescription.text = result.description;
	}

	void ChooseNo ()
	{
		OptionDescription.gameObject.SetActive (true);
		StoryTitle.gameObject.SetActive (false);
		StoryFace.gameObject.SetActive (false);

		if (state.deckEmpty ()) {
			OptionDescription.text = "You can leave all your worries behind now.";
			Application.Quit ();
			return;
		}

		result = state.no ();
		OptionDescription.text = result.description;
	}
}
