using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public abstract class Card
	{
		public string title = "";
		public string image = "";
		public string environment = "";
		public int encounters = 0;

		// applicable checks whether the card should be shown
		public abstract bool applicable (State state);

		// describes the current situation for this card
		public abstract string[] describe (State state);

		// lists options available here
		public abstract Options options (State state);

		// Option describes a single choice that a person can make on a card
		public struct Option
		{
			// title of the option
			public string title;
			// callback to resolve the effect
			public OnResolve resolve;

			public delegate string[] OnResolve ();
		}

		// options is the two choices a player can make in a particular situation
		public struct Options
		{
			public Option yes;
			public Option no;
		}

		// class for tracking choices that the player made throughout the game
		public class Choice
		{
			public Card card;
			public Option option;
			public string selected;
			public string description;
		}
	}

	// Player lists all the possible buffs
	public class Player
	{
		public bool hut = false;
		public bool flu = false;
		public bool stickyBoots = false;
		public bool creepingTerror = false;
		public bool depression = false;
		public bool map = false;
		public bool corpsePoker = false;

		public bool oldAge = false;

		public bool jasmine = false;
		public bool dianne = false;
		public bool noemi = false;
		public bool seenDeath = false;

		public bool ghostlyLady = false;
		public bool deliriousVisions = false;
		public bool peaceOfMind = false;
	}

	// State manages and updates the game state
	public class State
	{
		public Player player = new Player ();
		public List<Card> deck = new List<Card> ();
		public List<Card.Choice> history = new List<Card.Choice> ();
		public World world = new World ();

		public Card currentCard = null;
		public string currentDescription = "";
		public Card.Options currentOptions = new Card.Options ();
		public Card.Choice lastChoice = null;

		public void setup ()
		{
			deck.Clear ();
			history.Clear ();

			world = new World ();

			List<Card> encounters = world.AllEncounters ();
			for (int i = 0; i < 30; i++) {
				if (encounters.Count == 0) {
					encounters = world.AllEncounters ();
				}
				deck.Add (Rand.Pick (encounters));
			}

			deck [deck.Count - 5] = world.Ageing;
			deck [deck.Count - 1] = world.LiteralDeath;

			updateCurrentCard (world.Journey);
		}

		public bool deckEmpty ()
		{
			return deck.Count == 0;
		}

		private void updateCurrentCard (Card card)
		{
			currentCard = card;
			if (card == null) {
				currentDescription = "";
				currentOptions = new Card.Options ();
				return;
			}
			currentDescription = Text.Sanitize (card.describe (this));
			currentOptions = card.options (this);
		}

		private Card.Choice choose (string selected, Card.Option option)
		{
			currentCard.encounters++;

			Card.Choice choice = new Card.Choice ();
			choice.card = currentCard;
			choice.description = Text.Sanitize (option.resolve ());
			choice.option = option;
			choice.selected = selected;

			history.Add (choice);

			avoidDuplicates ();

			// advance to the next card
			lastChoice = choice;
			if (deck.Count == 0) {
				updateCurrentCard (null);
			} else {
				Card card = deck [0];
				deck.RemoveAt (0);
				updateCurrentCard (card);
			}

			return choice;
		}

		private List<Card.Choice> lastChoices (int n)
		{
			int low = history.Count - 4;
			int high = low + 4;
			if (low < 0) {
				low = 0;
			}
			if (high >= history.Count) {
				high = history.Count;
			}
			return history.GetRange (low, high - low);
		}

		private void avoidDuplicates ()
		{
			if (history.Count == 0) {
				return;
			}

			List<Card.Choice> avoid = lastChoices (5);

			while (deck.Count > 0) {
				Card candidate = deck [0];

				// remove cards that shouldn't be shown more
				if (!candidate.applicable (this)) {
					deck.RemoveAt (0);
					continue;
				}

				// avoid repeating same card that has been shown recently
				bool skip = false;
				for (int k = 0; k < avoid.Count; k++) {
					if (avoid [k].card.title == candidate.title) {
						skip = true;
						break;
					}
				}

				if (skip) {
					deck.RemoveAt (0);
					continue;
				}
				break;
			}
		}

		public void die ()
		{
			deck.Clear ();
		}

		public Card.Choice yes ()
		{
			return choose ("yes", currentCard.options (this).yes);
		}

		public Card.Choice no ()
		{
			return choose ("no", currentCard.options (this).no);
		}
	}
}
