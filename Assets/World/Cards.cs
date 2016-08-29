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
				"The world is a harsh place. Everybody has to make decisions.",
				
				"Some decisions matter, some don’t. Some seem to matter and don’t matter others don’t seem to be relevant but change the course of your life.",

				"Choose your own destiny and maybe you find that there is some purpose in this life.",
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
				"You haven't even noticed how the time has caught up with you. You think, what else can I do with my life."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Get older";
			options.yes.resolve = delegate() {
				state.player.oldAge = true;

				return new string[] {
					"You notice that you don't move as fast as you did. You wonder, when you will stop moving altogether."
				};
			};

			options.no.title = "Get younger";
			options.no.resolve = delegate() {
				state.player.oldAge = true;

				return new string[] {
					"Laws of nature are preventing you."
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
			return encounters < 2;
		}

		public override string[] describe (State state)
		{
			if (encounters == 0) {
				return new string[] {
					"While walking during a windy night you encounter a young woman crying under a nearby tree.",
					"She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something."
				};
			}
			return new string[] {
				"You notice a familiar crying women under a nearby tree.",
				"She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			if (encounters == 0) {
				options.yes.title = "Step Closer";
				options.yes.resolve = delegate() {
					state.player.ghostlyLady = true;
					state.player.deliriousVisions = true;

					Rand.InsertBetween (state.deck, 4, 6, this);
					Rand.InsertBetween (state.deck, 4, 6, state.world.DeliriousVisions);

					return new string[] {
						"She notices you when you are only a few steps from her. She briefly looks towards you through her tears and continues mumbling.",
						"While trying to figure out what to do you start to understand fragments of the children's story she is mumbling.",
						"You walk away unable to comfort her. You wonder what happened to her."
					};
				};

				options.no.title = "Walk away";
				options.no.resolve = delegate() {
					return new string[] {
						"You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting.",
						"She is not there anymore. What happened to her, you wonder...",
						"You decide not to bother yourself with this matter anymore."
					};
				};
				return options;
			}

			options.yes.title = "Step Closer";
			options.yes.resolve = delegate() {
				state.player.deliriousVisions = false;
				state.player.peaceOfMind = true;

				return new string[] {
					"How long has she been here, you wonder...",
					"Upon walking closer she acknowledges your presence with a nod and stops sobbing.",
					"The wind clears as she slowly sags into the tree."
				};
			};

			options.no.title = "Walk away";
			options.no.resolve = delegate() {
				return new string[] {
					"You turn away and start walking away from her.",
					"You feel the wind becoming stronger and you glance back at the tree where the woman was sitting.",
					"She is not there anymore. You feel uneasy and lonely."
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
			return state.player.flu || state.player.deliriousVisions;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You wake up. Or did you? You are covered in sweat. You wake up. Are you even alive? What is going on?",
				"You wake up. Sun shines through the small hole in tavern wall. Tavern keeper tells you that you had been rambling for three days straight in high fever.",
				"You were brought here by a friend of yours who paid for a whole week in advance.",
				"You have no friends in this town.",

				state.player.flu ? "You feel that shivers plaguing you are also gone." : "",

				state.player.deliriousVisions ? "You think about some of the visions you had and you are fairly certain you were talking with the lady you found sobbing under the tree. She might have been sad that you left, but this is just speculation. Your memories are not clear enough to tell for sure." : ""
			};
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
					"You try to remember words, but they escape you and you end up eloquently saying... \"bxrrrrha\""
				};
			};

			options.no.title = "Thank";
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


	public static class Fork
	{
		public static string[] Sign = new string[] {
			"You notice a worn out post barely holding onto two signs.",
			"After walking for miles you encounter a fork in the road. A post holds two signs.",
			"A post holding two signs starts to appear from a thick fog.",
			"After walking for miles you notice a post leaning with two signs."
		};

		public static string[] ForestSign = new string[] {
			"Text beaten by the weather, points towards the forest.",
			"The text isn't readable, but it seems to point towards the forest."
		};
		public static string[] ForestWalk = new string[] {
			"You start walking into the forest. The trees ascend and block out the light leaving you in the dark.",
			"You take the path leading into a thick forest.",
		};

		public static string[] TownSign = new string[] {
			"One points towards a Town that lightly glows in the darkness.",
			"One text seems to indicate that there is a town nearby."
		};
		public static string[] TownWalk = new string[] {
			"You start walking towards the town.",
			"You decide to try your luck in the town.",
			"You take your first steps and wonder what will lye ahead.",
		};
		
		public static string[] SwampSign = new string[] {
			"One points towards a swamp, with a gleaming light in the distance.",
			"It seems to that direction is a swamp. It doesn't look very appealing though."
		};
		public static string[] SwampWalk = new string[] {
			"You start walking towards the light while the fog slowly descends.",
			"You decided the take the muddy road towards the swamp.",
		};
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
				Rand.PickText (Fork.Sign),
				Rand.PickText (Fork.SwampSign),
				Rand.PickText (Fork.ForestSign)
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Swamp";
			options.yes.resolve = delegate() {
				Rand.InsertCards (state.deck, 2, state.world.AllSwamp ());
				return new string[] {
					Rand.PickText(Fork.SwampWalk)
				};
			};

			options.no.title = "Forest";
			options.no.resolve = delegate() {
				state.player.creepingTerror = true;
				Rand.InsertCards (state.deck, 2, state.world.AllForest ());
				return new string[] {
					Rand.PickText(Fork.ForestWalk)
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
				Rand.PickText (Fork.Sign),
				Rand.PickText (Fork.SwampSign),
				Rand.PickText (Fork.TownSign)
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Swamp";
			options.yes.resolve = delegate() {
				Rand.InsertCards (state.deck, 2, state.world.AllSwamp ());
				return new string[] {
					Rand.PickText(Fork.SwampWalk)
				};
			};

			options.no.title = "Town";
			options.no.resolve = delegate() {
				Rand.InsertCards (state.deck, 2, state.world.AllTown ());
				return new string[] {
					Rand.PickText(Fork.TownWalk)
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
				Rand.PickText (Fork.Sign),
				Rand.PickText (Fork.TownSign),
				Rand.PickText (Fork.ForestSign)
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Town";
			options.yes.resolve = delegate() {
				return new string[] {
					Rand.PickText(Fork.TownWalk)
				};
			};

			options.no.title = "Forest";
			options.no.resolve = delegate() {
				state.player.creepingTerror = true;
				return new string[] {
					Rand.PickText(Fork.ForestWalk)
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
					"Upon knocking on the door it jumps open. From the other side you are greeted by a jolly old man with long white beard.",
					"He pulls you in and forces you to sit down on an ancient but comfortable bed. Then he runs to the back room and returns with a huge wooden cup. He assures you that this tea is made from the best herbs this swamp harnesses.",
					"You sip your tea as you watch this peculiar old man jump around and caress his beard non-stop."
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
					"With a forceful jump you ascend to the sky and fall towards the frog.",
					"The frog trembles in horror.",
					"The frog is crushed leaving sticky resin on your boots."
				};
			};

			return options;
		}
	}

	public class Sheep: Card
	{
		public Sheep ()
		{
			title = "Sheep";
			environment = "Forest";
			image = "Sheep";
		}

		public override bool applicable (State state)
		{
			return encounters < 2;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"You notice a sheep lost in the forest.",
				"He seems to have an intergalatic quality."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Poke";
			options.yes.resolve = delegate() {
				return new string[] {
					"You poke the sheep. He pops out of existance. Maybe it is better this way?",
					"He felt like he wasn't from this universe."
				};
			};

			options.no.title = "Shear";
			options.no.resolve = delegate() {
				return new string[] {
					"You have shorn the sheep. Somewhere some aliens are happy."
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
			if (encounters == 0) {
				return new string[] {
					"You notice a broken wagon in the dirt.",
					"A small old man is slowly tasking away trying to fix a broken wheel spike.",
					"The old man looks very angry."
				};
			}
			return new string[] {
				"You notice a familiar broken wagon in the dirt.",
				"The old man is still trying to fix the broken wheel.",
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
				if (encounters == 0) {
					return new string[] {
						"You walk past the man. He shouts \"Good riddance, there's no need for people like you.\"",
						"Looking back he is still trying to get things fixed."
					};
				}
				return new string[] {
					"You try to avoid him, but he still notices you \"Still not helping? What is wrong with people these days?\"",
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
			return encounters < 2 && !state.player.flu;
		}

		public override string[] describe (State state)
		{
			if (encounters == 0) {
				return new string[] {
					"Walking on a cobblestone street you come across a man. He can barely stand straight. He asks people to help him, but no one does."
				};
			}
			return new string[] {
				"You notice a man stumbling and looking sickly. He can barely stand straight.",
				"He is begging people to help him, but no one does."
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
					"You go near the man and support his weight, helping him to walk to the hospital. The doctors take over from there.",
					"The man thanks you and stumbles into the hospital.",
					"You feel a slight shiver coming over you."
				};
			};

			options.no.title = "Avoid";
			options.no.resolve = delegate() {
				if (encounters == 0) {
					return new string[] {
						"You do as everyone else and avoid him."
					};
				}
				return new string[] {
					"You do as everyone else and try best to avoid him."
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
				encounters >= 1 ? "This looks like a bad omen." : "",
				"You feel shivers throughout your body and start to cough. The weakness starts setting in and you are not sure whether you can go on."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Hope";
			options.yes.resolve = delegate() {
				Rand.InsertBetween (state.deck, 1, 3, state.world.CoughingBlood);

				return new string[] {
					"Hopefully it's nothing serious."
				};
			};

			options.no.title = "Death";
			options.no.resolve = delegate() {
				Rand.InsertBetween (state.deck, 1, 3, state.world.CoughingBlood);
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
				Rand.InsertBetween (state.deck, 1, 3, state.world.DeathByFlu);
				return new string[] {
					"This causes you to cough even more. It was not a good idea."
				};
			};

			options.no.title = "Spit out";
			options.no.resolve = delegate() {
				Rand.InsertBetween (state.deck, 1, 3, state.world.DeathByFlu);
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
					"You breathe out the last time."
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
			return (encounters < 3) && !state.player.map;
		}

		public override string[] describe (State state)
		{
			if (state.player.depression) {
				return new string[] {
					"A gentleman carrying a briefcase approaches you.",
					"\"Are you alright? You seem to be aimlessly looking for a way out of your life.\"",
					"You don't know what the correct answer is.",
					"\"Decades ago I found mentions of an old Artifact that gave people a reason to live. I hadn't much luck, maybe you have more.\"",
					"The man offers you a dirty looking map having a picture of a rock on it. There is probably multiple rocks in the world, you understand why he has had some difficulty."
				};
			} else {
				return new string[] {
					"A gentleman carrying a briefcase approaches you. He offers to have a word with you nearby."
				};
			}
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			if (state.player.depression) {
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
						"You take offence by what the man said and simply leave. You have a reason to live already!"
					};
				};
			} else {
				options.yes.title = "Accept his offer";
				options.yes.resolve = delegate() {
					return new string[] {
						"He tells about how much he has learned in his life by his suffering.",
						"Does suffering in life really bring you knowledge, you wonder."
					};
				};

				options.no.title = "Leave";
				options.no.resolve = delegate() {
					return new string[] {
						"You don't care much about what he has to say, you continue your path."
					};
			
				};
			}

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
			options.yes.title = "Poke";
			options.yes.resolve = delegate() {
				return new string[] {
					"Poking the corpse did not bring him back to life. What a shame.",
					"You hope to see him around."
				};
			};

			options.no.title = "Shoo";
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
					"You notice the similarity of the rock to the map that the Archeologist gave you.",
					"There seem to be strange symbols that can be twisted."
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
				options.yes.title = "Twist the symbols";
				options.yes.resolve = delegate() {
					state.deck.Insert (0, state.world.BrokenClockwork);
					return new string[] {
						"You twist the symbols and the rock starts to hum. You keep on turning and the humming becomes louder, you feel that the teachings of Naomi, Dianne and Jasmine are helping.",
						"The rock violently explodes as the humming reaches its climax and reveals a hidden path below it. Taking a moment to think about what just happened you realize that the rock is dead now, you wish it didn't have to be like this.",
						"Well, at least this is how magic feels like.",
						"You slowly descend into the dark room below."
					};
				};

				options.no.title = "Leave";
				options.no.resolve = delegate() {
					return new string[] {
						"You leave waving the rock goodbye. It seemed so friendly.",
						"Maybe it would have liked if you twisted its symbols?"
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
						"You leave waving the rock goodbye. It seemed so friendly.",
						"Rock looks, weeping, as you walk away."
					};
				};

				return options;
			}

			options.yes.title = "Poke";
			options.yes.resolve = delegate() {
				return new string[] {
					"Poking the rock did not do anything you wouldn’t expect.",
					"What did you expect?"
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
				"You slowly approach it, it seems that debri from the ceiling has damaged the device and broken off some pieces.",
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
						"You are unable to make the pieces stick to each other properly. You check your worn-out boots, there is no glue there...",
						"Why is there no glue on your boots?",
						"You walk away wondering where to get glue onto your boots."
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
				"You wonder what to do with this device. Was this the goal of your adventure? Or was the journey itself the goal?",
				"You have explored this world in its entirety but there might be hidden paths yet to be explored, more frogs to step on, more little girls to talk to, more ways to die, more rocks to mingle with.",
				"However, now, you take a step back and put this device under your jacket. You feel happier than before",
				"Maybe this sense of accomplishment itself was the goal? However you feel satisfied."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "The End";
			options.yes.resolve = delegate() {
				state.deck.Insert (0, state.world.DeathByAging);
				return new string[] {
					"This game was made by:\n\n Silver Kontus \n Egon Elbre \n Joonatan Samuel \n Joosep Jääger \n Ott Adermann \n Edvin Aedma \n"
				};
			};

			options.no.title = "The End";
			options.no.resolve = delegate() {
				state.deck.Insert (0, state.world.DeathByAging);
				return new string[] {
					"This game was made by:\n\n Silver Kontus \n Egon Elbre \n Joonatan Samuel \n Joosep Jääger \n Ott Adermann \n Edvin Aedma \n"
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
			environment = "Swamp";
			image = "Noemi";
		}

		public override bool applicable (State state)
		{
			return encounters < 1;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"From a distance you hear mild and playful screams of joy. Some local girls must be playing in the swamp pool. You haven’t seen anyone for miles and think that you are completely lost by now so you decide to ask your way.",
				"With this in mind you set your course towards the shrieks. To your surprise you only find one young lady in a pool.",
				"More chillingly this young lady is poking a dead body. She seems to be very thrilled with what she is doing.",

				state.player.corpsePoker ? "You think that you’ve already tried poking a corpse before. Didn’t bring her back to life. You wonder what happened with the fly." : ""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Cough politely";
			options.yes.resolve = delegate() {
				state.player.noemi = true;

				return new string[] {
					"She jumps slightly and turns around. She does not seem to mind that she has no clothes. There do not seem to be any clothes around the pool either.",
					"She gladly agrees to show you the way. Her movements are sharp. She has a distinct aroma surrounding her that for some reason reminds you of nutmeg even though they are nothing alike.",

					state.player.hut ? "Hmm… I think you have already met my godfather. He has a long white beard. He told me good things about you." : "",

					"She says that her name is Noemi. She murmurs about some ancient technology that allows you to give eternal love and for some unexpected reason gives you a lecture on the anatomy of the heart.",
					"You feel enlightened but also relieved to hit the road again."
				};
			};

			options.no.title = "Walk away";
			options.no.resolve = delegate() {
				state.player.noemi = true;

				return new string[] {
					"You turn around and try to walk away without making a noise. Then you feel a light tap on your shoulder. A little spooked out you tell the naked, corpse-poking lady about your journey.",
					"She gladly agrees to show you the way. Her movements are sharp. She has a distinct aroma surrounding her that for some reason reminds you of nutmeg even though they smell nothing alike.",

					state.player.hut ? "Hmm… I think you have already met my godfather. He has a long white beard. He told me good things about you." : "",

					"She tells that her name is Noemi. She murmurs about some ancient technology that allows you to give eternal love and for some unexpected reason gives you a lecture on the anatomy of the heart.",
					"You feel enlightened but also relieved to hit the road again."
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
			environment = "Forest";
			image = "Jasmine";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"Walking on a peaceful forest road admiring the centuries old trees you hear distant singing. This appealing voice oddly rings in your ears. Charmed by it you step towards it.",

				state.player.peaceOfMind || state.player.ghostlyLady ? "Walking closer, you can make out some of this ancient language she is singing in. Reminds you of a children’s story you’ve heard somewhere." : "",

				"You reach the edge of a glade. In the center there is a lady singing on top of a rock. Around her there is a circle of dead bodies. She doesn’t seem to care. Her eyes seem to be cried out.",

				state.player.corpsePoker ? "Interestingly enough one of the bodies looks familiar. It is still not alive. There is a fly sitting on top of it." : ""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "Step closer";
			if (state.player.peaceOfMind) {
				options.yes.resolve = delegate() {
					return new string[] {
						"You step out of the protective shadows onto the soft grass. The lady sitting on the stone keeps singing even though she definitely noticed you.",

						"Walking closer you feel a choking sensation. Then she stops singing. You gasp for air. She starts speaking with a beautiful voice that reminds you of birds singing:",

						"\"I am happy to see you again traveler. Last time we didn’t talk, I was overwhelmed with my grief. Your compassion made me understand that I have been in this world for too long.\"",

						"\"You see, I am an eternal being. A spirit if you wish. I have many faces, you saw one of mine under the tree sobbing.\"",

						"She tells you many things about the forest and the meaning of love. She tells you something about the combination of love and technology.",

						"It makes little sense to you but you feel like this infromation is going to be useful some day. She sends you off.",

						"You realize that sometimes bad things in life can bring experiences that you otherwise might have missed."
					};
				};
			} else {
				options.yes.resolve = delegate() {
					state.die ();
					return new string[] {
						"You step out of the protective shadows onto the soft grass. The lady sitting on the stone keeps singing even though she definitely noticed you.",
						
						"Walking closer you feel a choking sensation. You want to stop walking but the singing forces you towards her.",

						"You fall next to the other dead bodies. You feel tired."
					};
				};
			}

			options.no.title = "Walk away";
			options.no.resolve = delegate() {
				return new string[] {
					"You feel that it was a wise decision to leave. No need to end up like the poor folk on the ground.",
					state.player.depression ? "What is the meaning of life anyways." : ""
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
			environment = "Town";
			image = "Dianne";
		}

		public override bool applicable (State state)
		{
			return encounters < 3;
		}

		public override string[] describe (State state)
		{
			return new string[] {
				"While walking on a street you hear a manly scream. As you keep walking with the crowd you see the man who screamed. He is lying dead on the ground.",
				"A beautiful woman standing next to him. Her skin is pale and looks like she doesn’t go outside too often.",
				"Nobody seems to care about that the man is bleeding. As you pass her you hear a whisper: \"It was his time\".",

				state.player.seenDeath ? "It reminds you of the encounter with the Death, your knees start to tremble." : ""
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();
			options.yes.title = "What do you mean?";
			options.yes.resolve = delegate() {
				if (state.player.seenDeath) {
					return new string[] {
						"She looks through you. \"I’ve seen you before,\" she says after a moment that seemed forever. Her voice rings in your ears.",
						
						state.player.jasmine ? "This effect reminds you of Jasmine, but it is more disturbing." : "",
						
						"You want to turn your head but then realize it is just an instinct because of the sound echoing inside your head. Gathering yourself you ask about the man who is still on the ground spewing blood everywhere. But then realize your stupidity. It was just his time to die. Sometimes it is time. After a while she grabs your hand and you go sit on top of a roof.",
						"She sits you down and tells you to shut your mouth before she starts talking. She doesn’t have to do this but for some reason she does so you shut your mouth and listen. She tells you about how different situations in life bring different opportunities.",
						"Experiences that first seem to be negative turn out to open doors unexpectedly. She then proceeds to teach you about clocks and their inner workings. She says something peculiar about how clocks are very similar to life and love. But as you don’t understand you can’t remember the wording exactly. She stops speaking.",
						"You blink and she is not there anymore."
					};
				}

				state.die ();
				return new string[] {
					"She says: \"I admire your bravery\". After a moment that seemed to last forever. Her voice rings in your ears.",
					state.player.jasmine ? "This effect reminds you of Jasmine, but it is more disturbing." : "",
					"Then she reveals a dagger and stabs you between your ribs. You feel your heart trying to pump against the crude edge of the dagger. The world goes blank.",
					!state.player.depression ? "Yet, everything was going so well." : ""
				};
			};

			options.no.title = "Keep walking";
			options.no.resolve = delegate() {
				if (state.player.seenDeath) {
					return new string[] {
						"This whole situation reminded you meeting the Death. He had said that it was not your time. Lets keep it that way."
					};
				}

				return new string[] {
					"I don’t want it to be my time you think to yourself and quicken your steps."
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
				"One moment you are walking, next moment the day turns black. The sun just vanished. The temperature dropped several notches. Some primal part of your mind kicks in and you feel threatened.",
				"You try to run but there is no power in your legs. With a silent thump you fall on your knees. A dark figure appears before you.",
				"It is hard to tell if he is a fiction of your imagination or really exists. Suddenly you understand.",
				"He is Death."
			};
		}

		public override Options options (State state)
		{
			Options options = new Options ();

			options.yes.title = "Accept Death";
			options.no.title = "Plead for your life";

			if (state.player.oldAge) {
				options.yes.resolve = delegate() {
					state.player.seenDeath = true;
					state.die ();
					return new string[] {
						"Death comes toward you. The world begins to swirl..."
					};
				};

				options.no.resolve = delegate() {
					state.player.seenDeath = true;
					state.die ();
					return new string[] {
						"Death comes toward you. The world begins to swirl..You feel deep feeling of disgust. Was it your emotion or not, you don’t know. But does it matter?"
					};
				};
			} else {
				options.yes.resolve = delegate() {
					state.player.seenDeath = true;
					return new string[] {
						"Death comes toward you. The world begins to swirl and then stops. You understand that Death admires your bravery.",
						"You think \"It’s not your time\" and then realize that it wasn’t your thought.",
						state.player.depression ? "You wish he had took you. He brought comfort to your misery." : ""
					};
				};

				options.no.resolve = delegate() {
					state.player.seenDeath = true;
					return new string[] {
						"Death comes toward you. The world begins to swirl and then stops. You feel deep feeling of disgust. Was it your emotion or not, you don’t know. But does it matter?",
						"You think \"It’s not your time\" and then realize that it wasn’t your thought. ",
						state.player.depression ? "Why did you even plead you actually wanted to die, didn’t you?" : ""
					};
				};
			}

			return options;
		}
	}

}