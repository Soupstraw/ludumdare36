function between (low, high) {
  return (Math.random() * (high - low) + low) | 0
}

var GhostlyLady = new Card({
  title: 'Ghostly Lady',
  img: "../Art/ghastly ladi.png",
  environment: null,
  desc: 'While walking during a windy night you encounter a young woman crying under a nearby tree. She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something.',
  yes: new Effect({
    option: 'Step Closer',
    desc: "She notices you when you are only a few steps from her. She briefly looks towards you through her tears and continues mumbling. While trying to figure out what to do you start to understand fragments of the children's story she is mumbling. You walk away unable to comfort her. You wonder what happened to her.",
    resolve: function (game, card, effect) {
      game.player.buff['Delirious Visions'] = true
      game.insertAt(between(4, 6), GhostlyLady2.clone())
      game.insertAt(between(4, 6), DeliriousVisions.clone())
    }
  }),
  no: new Effect({
    option: 'Walk away',
    desc: 'You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. She is not there anymore. What happened to her, you wonder… You decide not to bother yourself with this matter anymore.',
    resolve: null
  })
})

var GhostlyLady2 = new Card({
  title: 'Ghostly Lady',
  img: "../Art/ghastly ladi.png",
  environment: null,
  desc: 'While walking during a windy night you encounter a young woman crying under a nearby tree. She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something.',
  yes: new Effect({
    option: 'Step Closer',
    desc: 'How long has she been here, you wonder.. Upon walking closer she acknowledges your presence with a nod and stops sobbing. The wind clears as she slowly sags into the tree.',
    resolve: function (game, card, effect) {
      delete game.player.buff['Delirious Visions']
      game.player.buff['Peace of mind'] = true
    }
  }),
  no: new Effect({
    option: 'Walk away',
    desc: ' You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. She is not there anymore. You feel uneasy and lonely.',
    resolve: null
  })
})

var DeliriousVisions = new Card({
  title: 'Delirious Visions',
  img: "../Art/visionnss.png",
  environment: null,
  desc: 'You wake up. Or did you? You are covered in sweat. You wake up. Are you even alive? What is going on? You wake up. Sun shines through the small hole in tavern wall. Tavern keeper tells you that you had been rambling for three days straight in high fever. You were brought here by a friend of yours who paid for a whole week in advance. You have no friends in this town.\n\nYou think about some of the visions you had and you are fairly certain you were talking with the lady you found sobbing under the tree. She might have been sad that you left, but this is just speculation. Your memories are not clear enough to tell for sure.',
  yes: new Effect({
    option: 'Mumble',
    desc: 'You try to remember words, but they escape you and you end up eloquently saying... "brrrrha"',
    resolve: function (game, card, effect) {
      delete game.player.buff['Delirious Visions']

      game.deck.shift()
      game.deck.shift()
      game.deck.shift()
    }
  }),
  no: new Effect({
    option: 'Thank',
    desc: 'You thank the tavern keeper and go on your merry way.',
    resolve: function (game, card, effect) {
      delete game.player.buff['Delirious Visions']

      game.deck.shift()
      game.deck.shift()
      game.deck.shift()
    }
  })
})

var Fork = new Card({
  title: 'Fork',
  img: "#",
  environment: null,
  desc: 'After traveling for miles you see a stubby post leaning in the haze.\nIt has two signs nailed to it. One points to the forest with huge creeping trees. The other points towards a swamp, with a gleaming light in the distance.',
  yes: new Effect({
    option: 'Swamp',
    desc: 'You start walking towards the light while the fog slowly descends.',
    resolve: function (game, card, effect) {}
  }),
  no: new Effect({
    option: 'Forest',
    desc: 'You start walking into the forest. The trees ascend and block out the light leaving you in the dark.',
    resolve: function (game, card, effect) {}
  })
})

var Hut = new Card({
  title: 'Hut',
  environment: "Swamp",
  desc: 'Hut with gleaming lights.',
  yes: new Effect({
    option: 'Knock',
    desc: 'Upon knocking on the door it jumps open. From the other side you are greeted by a jolly old man with long white beard. He pulls you in and forces you to sit down on an ancient but comfortable bed. Then he runs to the back room and returns with a huge wooden cup. He assures you that this tea is made from the best herbs this swamp harnesses. You sip your tea as you watch this peculiar old man jump around and caress his beard non-stop.',
    resolve: function (game, card, effect) {
      delete game.player.buff["Flu"];
      game.player.buff["Hut"] = true;
    }
  }),
  no: new Effect({
    option: 'Pass',
    desc: 'You hear weird thumps from the house and quicken your steps. You wonder what is going on inside.',
    resolve: function (game, card, effect) {}
  })
})

var Hut = new Card({
  title: 'Hut',
  environment: "Swamp",
  desc: 'Hut with gleaming lights.',
  yes: new Effect({
    option: 'Knock',
    desc: 'Upon knocking on the door it jumps open. From the other side you are greeted by a jolly old man with long white beard. He pulls you in and forces you to sit down on an ancient but comfortable bed. Then he runs to the back room and returns with a huge wooden cup. He assures you that this tea is made from the best herbs this swamp harnesses. You sip your tea as you watch this peculiar old man jump around and caress his beard non-stop.',
    resolve: function (game, card, effect) {
      delete game.player.buff["Flu"];
      game.player.buff["Hut"] = true;
    }
  }),
  no: new Effect({
    option: 'Pass',
    desc: 'You hear weird thumps from the house and quicken your steps. You wonder what is going on inside.',
    resolve: function (game, card, effect) {}
  })
})

var SwampCards = [Hut]
var ForestCards = []
var TownCards = []
var BaseCards = [GhostlyLady, Fork]
