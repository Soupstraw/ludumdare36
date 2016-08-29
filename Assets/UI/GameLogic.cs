using UnityEngine;
using System.Collections;
using Game;

public class GameLogic : MonoBehaviour
{

	public UnityEngine.UI.Text descriptionText;
	public UnityEngine.UI.Text resultText;
	public UnityEngine.UI.Text choiceYesText;
	public UnityEngine.UI.Text choiceNoText;

	public UnityEngine.Material front;
	public UnityEngine.Material back;

	private Card.Choice result;

	private Game.State state;

	void Start ()
	{
		state = new Game.State ();
		state.setup ();
		UpdateTexts ();
	}

	void OnEnable ()
	{
		CardInteraction.OnChoice += Choose;
		FadingPanel.OnDialogDismissed += UpdateTexts;
	}

	void OnDisable ()
	{
		CardInteraction.OnChoice -= Choose;
		FadingPanel.OnDialogDismissed -= UpdateTexts;
	}

	public void Choose (int choice)
	{
		if (choice == 1) {
			result = state.yes ();
		} else {
			result = state.no ();
		}

		resultText.text = result.description;
	}

	public void UpdateTexts ()
	{
		if (state.currentCard != null) {
			Texture cardface = FindByName (state.currentCard.image);
			front.mainTexture = cardface;
			back.mainTexture = cardface;
		}

		descriptionText.text = state.currentDescription;

		choiceNoText.text = state.currentOptions.no.title;
		choiceYesText.text = state.currentOptions.yes.title;
	}

	private Texture FindByName (string name)
	{
		for (int i = 0; i < Cards.Length; i++) {
			if (Cards [i].name == name) {
				return Cards [i];
			}
		}

		Debug.LogAssertion ("Missing texture for: " + name);
		return Empty;
	}

	[Header ("Cards")]
	public UnityEngine.Texture Empty;
	public UnityEngine.Texture[] Cards;
}
