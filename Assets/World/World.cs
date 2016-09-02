using System;
using System.Collections.Generic;

namespace Game
{
	// World contains all instances of cards in the current world
	public class World
	{
		public Journey Journey = new Journey ();
		public Ageing Ageing = new Ageing ();
		public GhostlyLady GhostlyLady = new GhostlyLady ();
		public DeliriousVisions DeliriousVisions = new DeliriousVisions ();
		public ForkSwampForest ForkSwampForest = new ForkSwampForest ();
		public ForkSwampTown ForkSwampTown = new ForkSwampTown ();
		public ForkTownForest ForkTownForest = new ForkTownForest ();
		public Hut Hut = new Hut ();
		public Frog Frog = new Frog ();
		public Sheep Sheep = new Sheep ();
		public Wagon Wagon = new Wagon ();
		public SickMan SickMan = new SickMan ();
		public Shivers Shivers = new Shivers ();
		public CoughingBlood CoughingBlood = new CoughingBlood ();
		public DeathByFlu DeathByFlu = new DeathByFlu ();
		public Archeologist Archeologist = new Archeologist ();
		public Corpse Corpse = new Corpse ();
		public MysteriousRock MysteriousRock = new MysteriousRock ();
		public BrokenClockwork BrokenClockwork = new BrokenClockwork ();
		public Clockwork Clockwork = new Clockwork ();
		public Noemi Noemi = new Noemi ();
		public Jasmine Jasmine = new Jasmine ();
		public Dianne Dianne = new Dianne ();
		public LiteralDeath LiteralDeath = new LiteralDeath ();

		public List<Card> AllEncounters ()
		{
			return new List<Card> {
				GhostlyLady, Hut, Frog, Sheep, Corpse, MysteriousRock, Wagon, SickMan,
				ForkSwampForest, ForkSwampTown, ForkTownForest,
				Noemi, Jasmine, Dianne,
				DeliriousVisions,
				LiteralDeath
			};
		}

		public List<Card> AllTown ()
		{
			return new List<Card> {
				SickMan, Archeologist, Dianne, ForkSwampForest, ForkSwampTown, ForkTownForest
			};
		}

		public List<Card> AllForest ()
		{
			return new List<Card> {
				Wagon, Sheep, MysteriousRock, Jasmine, ForkSwampForest, ForkSwampTown, ForkTownForest
			};
		}

		public List<Card> AllSwamp ()
		{
			return new List<Card> {
				Hut, Frog, MysteriousRock, Noemi, ForkSwampForest, ForkSwampTown, ForkTownForest
			};
		}
	}
}