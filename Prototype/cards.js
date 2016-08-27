
var GhostlyLady = new Card({
  N: 1,
  title: "Ghostly Lady",
  desc: "While walking during a windy night you encounter a young woman crying under a nearby tree. She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something.\n\nWould you like to step closer?.",
  no: new Effect({
    option: "No",
    desc: "You wander ahead leaving the lady sobbing.",
    resolve: null
  }),
  yes: new Effect({
    option: "Yes",
    desc: "She notices you when you are only a few steps from her. She briefly looks towards you through her tears and continues mumbling. While trying to figure out what to do you start to understand fragments of the children's story she is mumbling. You walk away unable to comfort her. You wonder what happened to her.",
    resolve: function(game, card, effect){
        game.player.buff["delirious-visions"] = true;
        game.deck.unshift(GhostlyLady2.clone());
    }
  })
});

var GhostlyLady2 = new Card({
  N: 1,
  title: "Ghostly Lady",
  desc: "How long has she been here, you wonder...\n\nUpon walking closer she acknowledges your presence with a nod and stops sobbing.\n\nWould you like to approach closer?",
  no: new Effect({
    option: "No",
    desc: "You move on feeling empty inside.",
    resolve: null
  }),
  yes: new Effect({
    option: "Yes",
    desc: "She notices you when you are only a few steps from her. The wind clears when she slowly sags into the tree.",
    resolve: function(game, card, effect){
      delete game.player.buff["delirious-visions"];
      game.player.buff["peace-of-mind"] = true;  
    }
  })
});

var BaseCards = [GhostlyLady]
