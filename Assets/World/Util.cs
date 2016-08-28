using System;
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
	}
}