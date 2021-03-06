﻿using System;
using System.Collections.Generic;

namespace Game
{
	public static class Rand
	{
		private static Random rng = new Random ();

		public static Card Pick (List<Card> cards)
		{
			int index = rng.Next (cards.Count);
			Card card = cards [index];
			cards.RemoveAt (index);
			return card;
		}

		public static void InsertCards (List<Card> deck, int n, List<Card> from)
		{
			for (int i = 0; i < n; i++) {
				deck.Insert (0, Pick (from));
			}
		}

		public static void InsertBetween (List<Card> deck, int low, int high, Card card)
		{
			int position = rng.Next (high - low) + low;
			deck.Insert (position, card);
		}

		public static string PickText (string[] texts)
		{
			int index = rng.Next (texts.Length);
			return texts [index];
		}
	}

	public static class Text
	{
		public static string Sanitize (string[] lines)
		{
			List<string> result = new List<string> ();
			for (int i = 0; i < lines.Length; i++) {
				string trimmed = lines [i].Trim ();
				if (trimmed.Length > 0) {
					result.Add (trimmed);
				}
			}
			return String.Join ("\n\n", result.ToArray ());
		}
	}
}