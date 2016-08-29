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

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"The world is a harsh place. Everybody has to make decisions. Some decisions matter, some don’t. Some seem to matter and don’t matter others don’t seem to be relevant but change the course of your life. Choose your own destiny and maybe you find that there is some purpose in this life.",
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Begin adventure";
			options.yes.resolve = delegate() {
				return new string[] {
					"Maybe the other choice would have been better."
				};
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

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You feel your body creaking."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Get older";
			options.yes.resolve = delegate() {
				state.player.oldAge = true;

				return new string[] {
					"There's nothing that can stop it."
				};
			};

			options.no.title = "Get younger";
			options.no.resolve = delegate() {
				state.player.oldAge = true;

				return new string[] {
					"Laws of physics are preventing you."
				};
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

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You've lived a long life, but Death catches up with us all."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Reminisce";
			options.yes.resolve = delegate() {
				state.die ();
				return new string[] {
					"You see all the encounters in your life, while everything fades away."
				};
			};

			options.no.title = "Say";
			options.no.resolve = delegate() {
				state.die ();
				return new string[] {
					"You try to say something meaningful, but there is no meaning beyond death."
				};
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
				"While walking during a windy night you encounter a young woman crying under a nearby tree. She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			if (encounters == 0) {
				options.yes.title = "Step Closer";
				options.yes.resolve = delegate() {
					state.player.deliriousVisions = true;

					Rand.InsertBetween (state.deck, 4, 6, this);
					Rand.InsertBetween (state.deck, 4, 6, state.world.DeliriousVisions);

					return new string[] {
						"She notices you when you are only a few steps from her. She briefly looks towards you through her tears and continues mumbling. While trying to figure out what to do you start to understand fragments of the children's story she is mumbling. You walk away unable to comfort her. You wonder what happened to her."
					};
				};

				options.no.title = "Walk away";
				options.no.resolve = delegate() {
					return new string[] {
						"You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. She is not there anymore. What happened to her, you wonder... You decide not to bother yourself with this matter anymore."
					};
				};
				return options;
			}

			options.yes.title = "Step Closer";
			options.yes.resolve = delegate() {
				state.player.deliriousVisions = false;
				state.player.peaceOfMind = true;

				return new string[] {
					"How long has she been here, you wonder... Upon walking closer she acknowledges your presence with a nod and stops sobbing. The wind clears as she slowly sags into the tree."
				};
			};

			options.no.title = "Walk away";
			options.no.resolve = delegate() {
				return new string[] {
					"You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. She is not there anymore. You feel uneasy and lonely."
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

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			List<string> paras = new List<string> ();

			paras.Add (
				"You wake up. Or did you? You are covered in sweat. You wake up. Are you even alive? What is going on? You wake up. Sun shines through the small hole in tavern wall. Tavern keeper tells you that you had been rambling for three days straight in high fever. You were brought here by a friend of yours who paid for a whole week in advance. You have no friends in this town."
			);

			if (state.player.flu) {
				paras.Add (
					"You feel that shivers plaguing you are also gone."
				);
			}
			if (state.player.deliriousVisions) {
				paras.Add (
					"You think about some of the visions you had and you are fairly certain you were talking with the lady you found sobbing under the tree. She might have been sad that you left, but this is just speculation. Your memories are not clear enough to tell for sure."
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

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"After traveling for miles you see a stubby post leaning in the haze.",

				"It has two signs nailed to it. One points to the forest with huge creeping trees. The other points towards a swamp, with a gleaming light in the distance."
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
				state.player.creepingTerror = true;
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

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"After traveling for miles you see a stubby post leaning in the haze.",

				"It has two signs nailed to it. One points to the gloomy town. The other points towards a swamp, with a gleaming light in the distance."
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

			options.no.title = "Town";
			options.no.resolve = delegate() {
				Rand.InsertCards (state.deck, 2, state.world.AllTown ());
				return new string[] {
					"You arrive in the town."
				};
			};

			return options;
		}
	}

	public class ForkTownForest: Card
	{
		public ForkTownForest ()
		{
			title = "Fork";
			environment = "";
			image = "Fork";
		}

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"After traveling for miles you see a stubby post leaning in the haze.",
				"It has two signs nailed to it. One points to the forest with huge creeping trees. The other points towards a gloomy town."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Town";
			options.yes.resolve = delegate() {
				return new string[] {
					"You arrive at the town."
				};
			};

			options.no.title = "Forest";
			options.no.resolve = delegate() {
				state.player.creepingTerror = true;
				return new string[] {
					"You start walking into the forest. The trees ascend and block out the light leaving you in the dark."
				};
			};

			return options;
		}
	}

	public class Hut: Card
	{
		public Hut ()
		{
			title = "Hut";
			environment = "Swamp";
			image = "Hut";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"Hut with gleaming lights."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Knock";
			options.yes.resolve = delegate() {
				state.player.flu = false;
				state.player.hut = true;

				return new string[] {
					"Upon knocking on the door it jumps open. From the other side you are greeted by a jolly old man with long white beard. He pulls you in and forces you to sit down on an ancient but comfortable bed. Then he runs to the back room and returns with a huge wooden cup. He assures you that this tea is made from the best herbs this swamp harnesses. You sip your tea as you watch this peculiar old man jump around and caress his beard non-stop."
				};
			};

			options.no.title = "Pass";
			options.no.resolve = delegate() {
				return new string[] {
					"You hear weird thumps from the house and quicken your steps. You wonder what is going on inside."
				};
			};

			return options;
		}
	}

	public class Frog: Card
	{
		public Frog ()
		{
			title = "Frog";
			environment = "Swamp";
			image = "Frog";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"Placing foot after foot on the swamp road you notice a small slimy frog jumping around."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Let it live";
			options.yes.resolve = delegate() {
				return new string[] {
					"You leave it and see him happily jumping in a puddle."
				};
			};

			options.no.title = "Step on it";
			options.no.resolve = delegate() {
				state.player.stickyBoots = true;
				return new string[] {
					"With a forceful jump you ascend to the sky and fall towards the frog. The frog trembles in horror. The frog is crushed leaving sticky resin on your boots."
				};
			};

			return options;
		}
	}

	public class Wagon: Card
	{
		public Wagon ()
		{
			title = "Wagon";
			environment = "Forest";
			image = "Wagon";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You notice a broken wagon in the dirt. A small old man is slowly tasking away trying to fix a broken wheel spike. The old man looks very angry."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Help";
			options.yes.resolve = delegate() {
				// avoid encountering again
				encounters += 10;

				state.player.creepingTerror = false;
				while (state.deck.Count > 0 && state.deck [0].environment == "Forest") {
					state.deck.RemoveAt (0);
				}

				Rand.InsertCards (state.deck, 2, state.world.AllTown ());
				return new string[] {
					"The old man is thankful for the help and gives you a lift to the next town."
				};
			};

			options.no.title = "Walk past";
			options.no.resolve = delegate() {
				return new string[] {
					"You walk past the man. He shouts \"Good riddance, there's no need for people like you.\"",
					"Looking back he is still trying to get things fixed."
				};
			};

			return options;
		}
	}

	public class SickMan: Card
	{
		public SickMan ()
		{
			title = "Sick Man";
			environment = "Town";
			image = "Sick Man";
		}

		public override bool applicable (State state)
		{
			return encounters < 2;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"Walking on a cobblestone street you come across a man. He can barely stand straight. He asks people to help him, but no one does."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Help";
			options.yes.resolve = delegate() {
				state.player.flu = true;
				Rand.InsertBetween (state.deck, 2, 4, state.world.Shivers);

				return new string[] {
					"You go near the man and support his weight, helping him to walk to the hospital. The doctors take over from there. The man thanks you and stumbles into the hospital.",
					"You feel a slight shiver coming over you."
				};
			};

			options.no.title = "Avoid";
			options.no.resolve = delegate() {
				return new string[] {
					"You do as everyone else and avoid him."
				};
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
			image = "Shivers";
		}

		public override bool applicable (State state)
		{
			return state.player.flu;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You feel shivers throughout your body and start to cough. The weakness starts setting in and you are not sure whether you can go on."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Hope";
			options.yes.resolve = delegate() {
				Rand.InsertBetween (state.deck, 2, 4, state.world.CoughingBlood);

				return new string[] {
					"Hopefully it's nothing serious."
				};
			};

			options.no.title = "Death";
			options.no.resolve = delegate() {
				Rand.InsertBetween (state.deck, 2, 4, state.world.CoughingBlood);
				state.player.depression = true;

				return new string[] {
					"You feel like there's nothing more you can do.",
					"What comes, must come."
				};
			};

			return options;
		}
	}

	public class CoughingBlood: Card
	{
		public CoughingBlood ()
		{
			title = "Blood Cough";
			environment = "";
			image = "Blood Cough";
		}

		public override bool applicable (State state)
		{
			return state.player.flu;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"It seems things are taking a turn for the worse. You seem to be coughing up blood."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Swallow";
			options.yes.resolve = delegate() {
				Rand.InsertBetween (state.deck, 2, 4, state.world.DeathByFlu);
				return new string[] {
					"This causes you to cough even more. It was not a good idea."
				};
			};

			options.no.title = "Spit out";
			options.no.resolve = delegate() {
				Rand.InsertBetween (state.deck, 2, 4, state.world.DeathByFlu);
				return new string[] {
					"You spit leaving a large blood stain on the ground."
				};
			};

			return options;
		}
	}

	public class DeathByFlu: Card
	{
		public DeathByFlu ()
		{
			title = "DeathByFlu";
			environment = "";
			image = "Death";
		}

		public override bool applicable (State state)
		{
			return state.player.flu;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"The strength as left your body and you fall to the ground, seeing some people passing by.",
				"You ask for help, but no-one is willing to risk the same fate as you.",
				"The world slowly fades away."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Last breath";
			options.yes.resolve = delegate() {
				state.die ();
				return new string[] {
					"You breathe out last time."
				};
			};

			options.no.title = "Close eyes";
			options.no.resolve = delegate() {
				state.die ();
				return new string[] {
					"You close your eyes."
				};
			};

			return options;
		}
	}

	public class Archeologist: Card
	{
		public Archeologist ()
		{
			title = "Archeologist";
			environment = "Town";
			image = "Archeologist";
		}

		public override bool applicable (State state)
		{
			return (encounters < 3) && !state.player.map && state.player.depression;
		}

		public override string[] describe (State state)
		{
			//TODO: write different text for non-depression
			return new string[] {
				"A gentleman carrying a briefcase approaches you.",
				"\"Are you alright? You seem to be aimlessly looking for a way out of your life.\"",
				"You don't know what the correct answer is.",
				"\"I've recently discovered mentions of an old Artifact that gave people back their life. I hadn't much luck, maybe you have more.\"",
				"The man offers you a dirty looking map having a picture of a rock on it."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Take map";
			options.yes.resolve = delegate() {
				state.player.map = true;

				return new string[] {
					"You take the map. The gentleman continues his walk."
				};
			};

			options.no.title = "Leave";
			options.no.resolve = delegate() {
				return new string[] {
					"You take offence what the man said and simply leave."
				};
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
			image = "Corpse";
		}

		public override bool applicable (State state)
		{
			return encounters < 2;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You notice a fly on a corpse lying beside the road. How he got there is anyone’s guess. Probably flew in from the swamp."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[] {
					"Poking the corpse did not bring him back to life. What a shame."
				};
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[] {
					"The fly flies away angrily."
				};
			};

			return options;
		}
	}

	public class MysteriousRock: Card
	{
		public MysteriousRock ()
		{
			title = "Mysterious Rock";
			environment = "";
			image = "Mysterious Rock";
		}

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			if (state.player.map) {
				return new string[] {
					"You notice the similarity of the rock to the map that the Archeologist gave you. There seem to be strange symbols that can be twisted."
				};
			}

			return new string[] {
				"You might be imagining things, but it seems that this rock is humming? Maybe it has a mind of itself?"
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			if (state.player.map && state.player.jasmine && state.player.dianne && state.player.noemi) {
				options.yes.title = "Open";
				options.yes.resolve = delegate() {
					state.deck.Insert (0, state.world.BrokenClockwork);
					return new string[] {
						"The rock slowly creaks and opens. The hidden passageway below the rock becomes visible.",
						"You slowly descend into the dark room."
					};
				};

				options.no.title = "Leave";
				options.no.resolve = delegate() {
					return new string[] {
						"You leave waving the rock goodbye. It seemed so friendly."
					};
				};

				return options;
			}

			if (state.player.map) {
				options.yes.title = "Touch";
				options.yes.resolve = delegate() {
					return new string[] {
						"You feel around the rock, but it doesn't respond to your advances."
					};
				};

				options.no.title = "Leave";
				options.no.resolve = delegate() {
					return new string[] {
						"You leave waving the rock goodbye. It seemed so friendly."
					};
				};

				return options;
			}

			options.yes.title = "Poke";
			options.yes.resolve = delegate() {
				return new string[] {
					"Poking the rock did not do anything you wouldn’t expect. What did you expect?"
				};
			};

			options.no.title = "Magic";
			options.no.resolve = delegate() {
				return new string[] {
					"You don’t know how to do magic. At least you tried.",
					"Must count for something, right?"
				};
			};

			return options;
		}
	}

	public class BrokenClockwork: Card
	{
		public BrokenClockwork ()
		{
			title = "Broken Clockwork";
			environment = "";
			image = "Broken Clockwork";
		}

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"Entering the room you notice a strange artifact placed on a pedestal.",
				"You slowly approach it, it seems that debri from the ceiling has damaged the device and broken off some pieces."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			options.no.title = "Leave";
			options.no.resolve = delegate() {
				return new string[] {
					"You decided that strange devices are better not played with and leave."
				};
			};

			options.yes.title = "Repair";
			if (state.player.stickyBoots) {
				options.yes.resolve = delegate() {
					state.deck.Insert (0, state.world.Clockwork);

					return new string[] {
						"You are able to fit the pieces together and the wheels inside the device start turning.",
						"A slight glow starts to eminate from the device."
					};
				};
			} else {
				options.yes.resolve = delegate() {
					return new string[] {
						"You are unable to make the pieces stick to each other properly. It seems some glue is needed."
					};
				};
			}

			return options;
		}
	}

	public class Clockwork: Card
	{
		public Clockwork ()
		{
			title = "Clockwork";
			environment = "";
			image = "Clockwork";
		}

		public override bool applicable (State state)
		{
			return true;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"Glowing device that is humming. There is a strange button on it."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Use";
			options.yes.resolve = delegate() {
				return new string[] {
					""
				};
			};

			options.no.title = "Place back";
			options.no.resolve = delegate() {
				return new string[] {
					""
				};
			};

			return options;
		}
	}

	// 3-Girls use "Girl" as title to space them out
	public class Noemi: Card
	{
		public Noemi ()
		{
			title = "Girl";
			environment = "";
			image = "Noemi";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[] {
					""
				};
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[] {
					""
				};
			};

			return options;
		}
	}

	public class Jasmine: Card
	{
		public Jasmine ()
		{
			title = "Girl";
			environment = "";
			image = "Jasmine";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[] {
					""
				};
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[] {
					""
				};
			};

			return options;
		}
	}

	public class Dianne: Card
	{
		public Dianne ()
		{
			title = "Girl";
			environment = "";
			image = "Dianne";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[] {
					""
				};
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[] {
					""
				};
			};

			return options;
		}
	}

	public class LiteralDeath: Card
	{
		public LiteralDeath ()
		{
			title = "Literal Death";
			environment = "";
			image = "Literal Death";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "";
			options.yes.resolve = delegate() {
				return new string[] {
					""
				};
			};

			options.no.title = "";
			options.no.resolve = delegate() {
				return new string[] {
					""
				};
			};

			return options;
		}
	}

}