using UnityEngine;
using System.Collections;
using Game;

public class GameLogic : MonoBehaviour {

	public UnityEngine.UI.Text descriptionText;
	public UnityEngine.UI.Text resultText;
	public UnityEngine.UI.Text choiceYesText;
	public UnityEngine.UI.Text choiceNoText;

	private string[] resultStrings;

	private Game.State state;

	void Start(){
		state = new Game.State ();
		state.setup ();
		UpdateTexts ();
	}

	void OnEnable(){
		CardInteraction.OnChoice += Choose;
		FadingPanel.OnDialogDismissed += UpdateTexts;
	}

	void OnDisable(){
		CardInteraction.OnChoice -= Choose;
		FadingPanel.OnDialogDismissed -= UpdateTexts;
	}

	public void Choose(int choice){
		if (choice == 0) {
			resultStrings = state.currentOptions.yes.resolve();
			state.yes ();
		} else {
			resultStrings = state.currentOptions.no.resolve();
			state.no ();
		}
		resultText.text = resultStrings [0];
	}

	public void UpdateTexts(){
		descriptionText.text = state.currentDescription;

		choiceNoText.text = state.currentOptions.no.title;
		choiceYesText.text = state.currentOptions.yes.title;
	}
}
