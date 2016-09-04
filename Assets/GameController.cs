using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
	public UnityEngine.UI.Text StoryTitle;

	public UnityEngine.GameObject StoryFace;
	public UnityEngine.Texture2D BlankCard;

	public UnityEngine.UI.Text YesTitle;
	public UnityEngine.UI.Text NoTitle;

	public UnityEngine.UI.Text FrontText;
	public UnityEngine.UI.Text BackText;

	private CardController cardController;
	private CardSounds cardSounds;
	private AmbientSounds ambientSounds;
	private AmbientBackground ambientBackground;

	private Game.State state;
	private Game.Card.Choice result;

	// Use this for initialization
	void Start ()
	{
		cardController = GetComponent<CardController> ();
		cardSounds = GetComponent<CardSounds> ();
		ambientSounds = GetComponent<AmbientSounds> ();
		ambientBackground = GetComponent<AmbientBackground> ();

		cardController.OnTrigger += HandleTrigger;

		SetupNewGame ();
		UpdateStory ();
	}

	void SetupNewGame ()
	{
		state = new Game.State ();
		state.setup ();
	}

	void HandleTrigger (int choice)
	{
		switch (cardController.state) {
		case CardController.State.Image:
			cardSounds.FlipStoryCard ();
			cardController.SetState (CardController.State.Description);
			break;
		
		case CardController.State.Description:
		case CardController.State.Image2:
			if (choice == -1) {
				ChooseNo ();
				cardSounds.SelectOption ();

				if (cardController.state == CardController.State.Description) {
					cardController.SetState (CardController.State.Option);
				} else {
					cardController.SetState (CardController.State.Option2);
				}
			} else if (choice == 1) {
				ChooseYes ();
				cardSounds.SelectOption ();

				if (cardController.state == CardController.State.Description) {
					cardController.SetState (CardController.State.Option);
				} else {
					cardController.SetState (CardController.State.Option2);
				}
			} else {
				cardSounds.FlipStoryCard ();

				if (cardController.state == CardController.State.Description) {
					cardController.SetState (CardController.State.Image2);
				} else {
					cardController.SetState (CardController.State.Description);
				}
			}
			break;
		
		case CardController.State.Option:
		case CardController.State.Option2:
			cardSounds.NewStoryCard ();
			cardController.SetState (CardController.State.Dismiss);
			break;
		
		case CardController.State.Dismiss:
			break;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!cardController.Animating () && cardController.state == CardController.State.Dismiss) {
			UpdateStory ();
			cardSounds.NewStoryCard ();
			cardController.SetState (CardController.State.Image);
			return;
		}
	}

	void SetStory (String title, String image, String description)
	{
		BackText.text = description;

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
		UpdateOptionTitles ();

		FrontText.gameObject.SetActive (false);
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
		String text = "";
		if (state.deckEmpty ()) {
			text = "Maybe this time things will go differently.";
			SetupNewGame ();
		} else {
			result = state.yes ();
			text = result.description;
		}

		UpdateOptionText (text);
	}

	void ChooseNo ()
	{
		String text = "";
		if (state.deckEmpty ()) {
			text = "You can leave all your worries behind now.";
			Application.Quit ();
		} else {
			result = state.no ();
			text = result.description;
		}

		UpdateOptionText (text);
	}

	private void UpdateOptionText (String text)
	{
		// change the text on the other side of the card
		if (cardController.state == CardController.State.Image ||
		    cardController.state == CardController.State.Image2) {
			// Front is facing us
			BackText.text = text;
		} else {
			// Back is facing us
			FrontText.gameObject.SetActive (true);
			StoryTitle.gameObject.SetActive (false);
			StoryFace.gameObject.SetActive (false);

			FrontText.text = text;
		}
	}
}
