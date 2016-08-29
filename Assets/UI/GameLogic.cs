using UnityEngine;
using System.Collections;
using Game;

public class GameLogic : MonoBehaviour
{

	public UnityEngine.UI.Text descriptionText;
	public UnityEngine.UI.Text resultText;
	public UnityEngine.UI.Text choiceYesText;
	public UnityEngine.UI.Text choiceNoText;

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
		descriptionText.text = state.currentDescription;

		choiceNoText.text = state.currentOptions.no.title;
		choiceYesText.text = state.currentOptions.yes.title;
	}
}
