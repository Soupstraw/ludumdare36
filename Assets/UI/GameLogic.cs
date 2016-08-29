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

	public string deathDescription = "You are dead";
	public string deathOptionYes = "Try again";
	public string deathOptionNo = "Quit";
	public string deathResult = "You chose to try again";

	public SoundManager soundmanager;

	private Card.Choice result;

	private Game.State state;
	private string lastEnvironment;

	void Start ()
	{
		SetupNewGame ();
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

	private void SetupNewGame(){
		state = new Game.State ();
		state.setup ();
		UpdateTexts ();
	}

	public void Choose (int choice)
	{
		if (state.deckEmpty ()) {
			if (choice == 1) {
				SetupNewGame ();
				return;
			} else {
				Application.Quit ();
				return;
			}
		}

		if (choice == 1) {
			result = state.yes ();
		} else {
			result = state.no ();
		}

		resultText.text = result.description;
	}

	public void UpdateTexts ()
	{
		if (state.deckEmpty ()) {
			front.mainTexture = Death;
			back.mainTexture = Death;
			choiceYesText.text = deathOptionYes;
			choiceNoText.text = deathOptionNo;
			descriptionText.text = deathDescription;
			resultText.text = deathResult;
			return;
		}

		if (state.currentCard != null) {
			Texture cardface = FindByName (state.currentCard.image);
			front.mainTexture = cardface;
			back.mainTexture = cardface;

			if (state.currentCard.environment != lastEnvironment) {
				switch (state.currentCard.environment) {

				case "Forest":
					soundmanager.ChangeAmbient (SoundManager.ambientSoundInfo.Forest);
					break;
				case "Swamp":
					soundmanager.ChangeAmbient (SoundManager.ambientSoundInfo.Swamp);
					break;
				case "Town":
					soundmanager.ChangeAmbient (SoundManager.ambientSoundInfo.Town);
					break;
				}
			}
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
	public UnityEngine.Texture Death;
	public UnityEngine.Texture[] Cards;
}
