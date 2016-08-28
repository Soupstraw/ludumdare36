using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	Game.State state;

	void Start(){
		state = new Game.State ();
		state.setup ();
	}

	void OnEnable(){
		CardInteraction.OnChoice += Choose;
	}

	void OnDisable(){
		CardInteraction.OnChoice -= Choose;
	}

	public void Choose(int choice){
		if (choice == 0) {
			state.yes ();
		} else {
			state.no ();
		}
	}
}
