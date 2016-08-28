using System;
using System.Collections.Generic;

namespace Game
{
	public class Journey: Card
	{
		public Journey ()
		{
			title = "Journey";
			environment = "";
			image = "Journey";
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"The world is a harsh place. " +
				"Everybody has to make decisions. " +
				"Some decisions matter, some don’t. " +
				"Some seem to matter and don’t matter others don’t seem to be relevant but change the course of your life. " +
				"Choose your own destiny and maybe you find that there is some purpose in this life.",
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Begin adventure";
			options.yes.resolve = delegate() {
				return new string[]{ "Maybe the other choice would have been better." };
			};
				
			options.no.title = "Begin adventure";
			options.no.resolve = delegate() {
				return new string[]{ "Good choice." };
			};

			return options;
		}
	}

	public class Ageing: Card
	{
		public Ageing ()
		{
			title = "Ageing";
			environment = "";
			image = "Ageing";
		}

		public override string[] describe (State state)
		{
			return new string[]{ "You feel your body creaking." };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Get older";
			options.yes.resolve = delegate() {
				state.player.oldAge = true;

				return new string[]{ "There's nothing that can stop it." };
			};

			options.no.title = "Get younger";
			options.no.resolve = delegate() {
				state.player.oldAge = true;

				return new string[]{ "Laws of physics are preventing you." };
			};

			return options;
		}
	}

	public class DeathByAging: Card
	{
		public DeathByAging ()
		{
			title = "Death by Ageing";
			environment = "";
			image = "Death";
		}

		public override string[] describe (State state)
		{
			return new string[]{ "You've lived a long life, but Death catches up with us all." };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Reminisce";
			options.yes.resolve = delegate() {
				state.die ();
				return new string[]{ "You see all the encounters in your life, while everything fades away." };
			};

			options.no.title = "Say";
			options.no.resolve = delegate() {
				state.die ();
				return new string[]{ "You try to say something meaningful, but there is no meaning beyond death." };
			};

			return options;
		}
	}

	public class GhostlyLady: Card
	{
		public GhostlyLady ()
		{
			title = "Ghostly Lady";
			environment = "";
			image = "Ghostly Lady";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"While walking during a windy night you encounter a young woman crying under a nearby tree. " +
				"She has a ghastly halo surrounding her, as if she is not from this world. " +
				"Through her delirious mumbles you hear her sobbing about something."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			if (encounters > 0) {
				options.yes.title = "Step Closer";
				options.yes.resolve = delegate() {
					state.player.deliriousVisions = false;
					state.player.peaceOfMind = true;

					return new string[] {
						"How long has she been here, you wonder... " +
						"Upon walking closer she acknowledges your presence with a nod and stops sobbing. " +
						"The wind clears as she slowly sags into the tree."
					};
				};

				options.no.title = "Walk away";
				options.no.resolve = delegate() {
					return new string[] {
						"You turn away and start walking away from her. " +
						"You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. " +
						"She is not there anymore. " +
						"You feel uneasy and lonely."
					};
				};

				return options;
			}

			options.yes.title = "Step Closer";
			options.yes.resolve = delegate() {
				state.player.deliriousVisions = true;

				Rand.InsertBetween (state.deck, 4, 6, this);
				Rand.InsertBetween (state.deck, 4, 6, state.world.DeliriousVisions);

				return new string[] {
					"She notices you when you are only a few steps from her. " +
					"She briefly looks towards you through her tears and continues mumbling. " +
					"While trying to figure out what to do you start to understand fragments of the children's story she is mumbling. " +
					"You walk away unable to comfort her. " +
					"You wonder what happened to her."
				};
			};

			options.no.title = "Walk away";
			options.no.resolve = delegate() {
				return new string[] {
					"You turn away and start walking away from her. " +
					"You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. " +
					"She is not there anymore. " +
					"What happened to her, you wonder... " +
					"You decide not to bother yourself with this matter anymore."
				};
			};

			return options;
		}
	}

	public class DeliriousVisions: Card
	{
		public DeliriousVisions ()
		{
			title = "Delirious Visions";
			environment = "";
			image = "Delirious Visions";
		}

		public override string[] describe (State state)
		{
			List<string> paras = new List<string> ();

			paras.Add (
				"You wake up. Or did you? You are covered in sweat. " +
				"You wake up. Are you even alive? What is going on? You wake up. " +
				"Sun shines through the small hole in tavern wall. " +
				"Tavern keeper tells you that you had been rambling for three days straight in high fever. " +
				"You were brought here by a friend of yours who paid for a whole week in advance. " +
				"You have no friends in this town."
			);

			if (state.player.flu) {
				paras.Add ("You feel that shivers plaguing you are also gone.");
			}
			if (state.player.deliriousVisions) {
				paras.Add (
					"You think about some of the visions you had and you are fairly certain you were " +
					"talking with the lady you found sobbing under the tree. " +
					"She might have been sad that you left, but this is just speculation. " +
					"Your memories are not clear enough to tell for sure."
				);
			}

			return paras.ToArray ();
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Mumble";
			options.yes.resolve = delegate() {
				state.player.deliriousVisions = false;
				state.player.flu = false;

				state.deck.RemoveAt (0);
				state.deck.RemoveAt (0);
				state.deck.RemoveAt (0);

				return new string[] {
					"You try to remember words, but they escape you and you end up eloquently saying... \"brrrrha\""
				};
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				state.player.deliriousVisions = false;
				state.player.flu = false;

				state.deck.RemoveAt (0);
				state.deck.RemoveAt (0);
				state.deck.RemoveAt (0);

				return new string[] {
					"You thank the tavern keeper and go on your merry way."
				};
			};

			return options;
		}
	}

	public class ForkSwampForest: Card
	{
		public ForkSwampForest ()
		{
			title = "Fork";
			environment = "";
			image = "Fork";
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"After traveling for miles you see a stubby post leaning in the haze.",

				"It has two signs nailed to it. " +
				"One points to the forest with huge creeping trees. " +
				"The other points towards a swamp, with a gleaming light in the distance."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Swamp";
			options.yes.resolve = delegate() {
				Rand.InsertCards (state.deck, 2, state.world.AllSwamp ());
				return new string[] {
					"You start walking towards the light while the fog slowly descends."
				};
			};

			options.no.title = "Forest";
			options.no.resolve = delegate() {
				Rand.InsertCards (state.deck, 2, state.world.AllForest ());
				return new string[] {
					"You start walking towards the light while the fog slowly descends."
				};
			};

			return options;
		}
	}

	public class ForkSwampTown: Card
	{
		public ForkSwampTown ()
		{
			title = "Fork";
			environment = "";
			image = "Fork";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class ForkTownForest: Card
	{
		public ForkTownForest ()
		{
			title = "ForkTownForest";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Hut: Card
	{
		public Hut ()
		{
			title = "Hut";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Frog: Card
	{
		public Frog ()
		{
			title = "Frog";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Wagon: Card
	{
		public Wagon ()
		{
			title = "Wagon";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class SickMan: Card
	{
		public SickMan ()
		{
			title = "SickMan";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Shivers: Card
	{
		public Shivers ()
		{
			title = "Shivers";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class DeathByShivers: Card
	{
		public DeathByShivers ()
		{
			title = "DeathByShivers";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Archeologist: Card
	{
		public Archeologist ()
		{
			title = "Archeologist";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Corpse: Card
	{
		public Corpse ()
		{
			title = "Corpse";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class MysteriousRock: Card
	{
		public MysteriousRock ()
		{
			title = "MysteriousRock";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class BrokenClockwork: Card
	{
		public BrokenClockwork ()
		{
			title = "BrokenClockwork";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Clockwork: Card
	{
		public Clockwork ()
		{
			title = "Clockwork";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Noemi: Card
	{
		public Noemi ()
		{
			title = "Noemi";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Jasmine: Card
	{
		public Jasmine ()
		{
			title = "Jasmine";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

	public class Dianne: Card
	{
		public Dianne ()
		{
			title = "Dianne";
			environment = "";
			image = "";
		}

		public override string[] describe (State state)
		{
			return new string[]{ };
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[]{ };
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[]{ };
			};

			return options;
		}
	}

}