function between (low, high) {
  return (Math.random() * (high - low) + low) | 0
}

function pickcards (N, into, from) {
  // TODO: randomize picking
  for (var i = 0; i < N; i++) {
    if (i >= from.length) {
      return
    }
    into.unshift(from[i].clone())
  }
}

var GhostlyLady = new Card({
  title: 'Ghostly Lady',
  img: '../Art/GhostlyLady.png',
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
  img: '../Art/GhostlyLady.png',
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
  img: '../Art/DeliriousVisions.png',
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
  img: '../Art/Empty.png',
  environment: null,
  desc: 'After traveling for miles you see a stubby post leaning in the haze.\nIt has two signs nailed to it. One points to the forest with huge creeping trees. The other points towards a swamp, with a gleaming light in the distance.',
  yes: new Effect({
    option: 'Swamp',
    desc: 'You start walking towards the light while the fog slowly descends.',
    resolve: function (game, card, effect) {
      pickcards(2, game.deck, SwampCards)
    }
  }),
  no: new Effect({
    option: 'Forest',
    desc: 'You start walking into the forest. The trees ascend and block out the light leaving you in the dark.',
    resolve: function (game, card, effect) {
      pickcards(2, game.deck, ForestCards)
    }
  })
})

var Hut = new Card({
  title: 'Hut',
  img: '../Art/Hut.png',
  environment: 'Swamp',
  desc: 'Hut with gleaming lights.',
  yes: new Effect({
    option: 'Knock',
    desc: 'Upon knocking on the door it jumps open. From the other side you are greeted by a jolly old man with long white beard. He pulls you in and forces you to sit down on an ancient but comfortable bed. Then he runs to the back room and returns with a huge wooden cup. He assures you that this tea is made from the best herbs this swamp harnesses. You sip your tea as you watch this peculiar old man jump around and caress his beard non-stop.',
    resolve: function (game, card, effect) {
      delete game.player.buff['Flu']
      game.player.buff['Hut'] = true
    }
  }),
  no: new Effect({
    option: 'Pass',
    desc: 'You hear weird thumps from the house and quicken your steps. You wonder what is going on inside.',
    resolve: function (game, card, effect) {}
  })
})

var Frog = new Card({
  title: 'Frog',
  img: '../Art/Frog.png',
  environment: 'Swamp',
  desc: 'Placing foot after foot on the swamp road you notice a small slimy frog jumping around.',
  yes: new Effect({
    option: 'Let it live',
    desc: 'You leave it and see him happily jumping in a puddle.',
    resolve: function (game, card, effect) {}
  }),
  no: new Effect({
    option: 'Step on it',
    desc: 'With a forceful jump you ascend to the sky and fall towards the frog. The frog trembles in horror. The frog is crushed leaving sticky resin on your boots.',
    resolve: function (game, card, effect) {
      game.player.buff['Sticky Boots'] = true
    }
  })
})

var Wagon = new Card({
  title: 'Wagon',
  img: '../Art/Wagon.png',
  environment: 'Forest',
  desc: 'You notice a broken wagon in the dirt. A small old man is slowly tasking away trying to fix a broken wheel spike. The old man looks very angry.',
  yes: new Effect({
    option: 'Help',
    desc: 'The old man is thankful for the help and gives you a lift to the next town.',
    resolve: function (game, card, effect) {
      delete game.player.buff['Creeping Terror']
      while(game.deck[0].environment === 'Forest'){
        game.deck.shift()
      }
      pickcards(2, game.deck, TownCards)
    }
  }),
  no: new Effect({
    option: 'Walk past',
    desc: 'You walk past the man. He shouts "Good riddance, there\'s no need for people like you."\n\nLooking back he is still trying to get things fixed.',
    resolve: function (game, card, effect) { }
  })
})

var SickMan = new Card({
  title: 'Sick Man',
  img: '../Art/Empty.png',
  environment: 'Town',
  desc: 'Walking on a cobblestone street you come across a man. He can barely stand straight. He asks people to help him, but no one does.',
  yes: new Effect({
    option: 'Help',
    desc: 'You go near the man and support his weight, helping him to walk to the hospital. The doctors take over from there. The man thanks you and stumbles into the hospital.\n\nYou feel a slight shiver coming over you.',
    resolve: function (game, card, effect) {
      game.player.buff['Flu'] = true;
      game.insertAt(between(2, 4), Shivers.clone())
    }
  }),
  no: new Effect({
    option: 'Avoid',
    desc: 'You do as everyone else and avoid him.',
    resolve: function (game, card, effect) { }
  })
})

var Shivers = new Card({
  title: 'Shivers',
  img: '../Art/DeliriousVisions.png',
  environment: null,
  desc: 'You feel shivers throughout your body and start to cough. The weakness starts setting in and you are not sure whether you can go on.',
  yes: new Effect({
    option: 'Hope',
    desc: 'Hopefully it\'s nothing serious.',
    resolve: function (game, card, effect) {
      game.insertAt(between(4, 6), DeathShivers.clone())
    }
  }),
  no: new Effect({
    option: 'Death',
    desc: 'You feel like there\'s nothing more you can do.\n\nWhat comes, must come.',
    resolve: function (game, card, effect) {
      game.player.buff["Depression"] = true;
      game.insertAt(between(4, 6), DeathShivers.clone())
    }
  })
})

var DeathShivers = new Card({
  title: 'Death',
  img: '../Art/Death.png',
  environment: null,
  desc: 'The strength as left your body and you fall to the ground, seeing some people passing by. No-one is willing to risk the same fate as you.\n\nThe world slowly fades away.',
  yes: new Effect({
    option: 'Last breath',
    desc: 'You breathe out once more.',
    resolve: function (game, card, effect) {
      game.deck = [];
    }
  }),
  no: new Effect({
    option: 'Close eyes',
    desc: 'You close your eyes.',
    resolve: function (game, card, effect) {
      game.deck = [];
    }
  })
})

var Archeologist = new Card({
  title: 'Archeologist',
  img: '../Art/Archeologist.png',
  environment: "Town",
  desc: 'A gentleman carrying a briefcase approaches you. "Are you alright? You seem to be aimlessly looking for a way out of your life."\n\nYou don\'t know what the correct answer is.\n\n"I\'ve recently discovered mentions of an old Artifact that gave people back their life. I hadn\'t much luck, maybe you have more."\n\nThe man offers you a map.',
  yes: new Effect({
    option: 'Take map',
    desc: 'You take the map. The gentleman continues his walk.',
    resolve: function (game, card, effect) {
      game.player.buff["Map"] = true;
    }
  }),
  no: new Effect({
    option: 'Leave',
    desc: 'You take offence what the man said and simply leave.',
    resolve: function (game, card, effect) {
    }
  })
})

var Corpse = new Card({
  title: 'Corpse',
  img: '../Art/Corpse.png',
  environment: "Town",
  desc: '',
  yes: new Effect({
    option: 'Poke',
    desc: 'Poking the corpse did not bring him back to life. What a shame.',
    resolve: function (game, card, effect) {}
  }),
  no: new Effect({
    option: 'Shoo',
    desc: 'The fly flies away angrily.',
    resolve: function (game, card, effect) {}
  })
})

var MysteriousRock = new Card({
  title: 'Mysterious Rock',
  img: '../Art/Stone.png',
  environment: null,
  desc: 'You might be imagining things, but it seems that this rock is humming? Maybe it has a mind of itself?',
  yes: new Effect({
    option: 'Poke',
    desc: 'Poking the rock did not do anything you wouldn’t expect. What did you expect?',
    resolve: function (game, card, effect) {}
  }),
  no: new Effect({
    option: 'Magic',
    desc: 'You don’t know how to do magic. At least you tried. Must count for something, right?',
    resolve: function (game, card, effect) {}
  })
})

var MysteriousRock = new Card({
  title: 'Mysterious Rock',
  img: '../Art/Stone.png',
  environment: null,
  desc: 'You might be imagining things, but it seems that this rock is humming? Maybe it has a mind of itself?',
  yes: new Effect({
    option: 'Poke',
    desc: 'Poking the rock did not do anything you wouldn’t expect. What did you expect?',
    resolve: function (game, card, effect) {}
  }),
  no: new Effect({
    option: 'Magic',
    desc: 'You don’t know how to do magic. At least you tried. Must count for something, right?',
    resolve: function (game, card, effect) {}
  })
})

var MysteriouserRock = new Card({
  title: 'Mysteriouser Rock',
  img: '../Art/Stone.png',
  environment: null,
  desc: 'You notice the similarity of the rock to the map that the Archeologist gave you. There seem to be strange symbols that can be twisted.',
  yes: new Effect({
    option: 'Poke',
    desc: 'You are unable to figure out the correct combination to make something happen.',
    resolve: function (game, card, effect) {}
  }),
  no: new Effect({
    option: 'Leave',
    desc: 'You leave waving the rock goodbye. It seemed so friendly.',
    resolve: function (game, card, effect) {}
  })
})

var MysteriouserRock2 = new Card({
  title: 'Mysteriouser Rock',
  img: '../Art/Stone.png',
  environment: null,
  desc: 'You notice the similarity of the rock to the map that the Archeologist gave you. There seem to be strange symbols that can be twisted.',
  yes: new Effect({
    option: 'Open',
    desc: 'The rock slowly creaks and opens. The hidden passageway below the rock becomes visible.\n\nYou slowly descend into the dark room.',
    resolve: function (game, card, effect) {
      game.deck.unshift(BrokenClockwork.clone())
    }
  }),
  no: new Effect({
    option: 'Leave',
    desc: 'You leave waving the rock goodbye. It seemed so friendly.',
    resolve: function (game, card, effect) {}
  })
})

var BrokenClockwork =new Card({
  title: 'Broken Clockwork',
  img: '../Art/Empty.png',
  environment: null,
  desc: 'Entering the room you notice a strange artifact placed on a pedestal.\n\nYou slowly approach it, it seems that debri from the ceiling has damaged the device and broken off some pieces.',
  yes: new Effect({
    option: 'Repair',
    desc: 'You are unable to make the pieces stick to each other properly. It seems some glue is needed.',
    resolve: function (game, card, effect) {
      
    }
  }),
  no: new Effect({
    option: 'Leave',
    desc: 'You decided that strange devices are better not played with and leave.',
    resolve: function (game, card, effect) {}
  })
})

var BrokenClockwork2 =new Card({
  title: 'Broken Clockwork',
  img: '../Art/Empty.png',
  environment: null,
  desc: 'Entering the room you notice a strange artifact placed on a pedestal.\n\nYou slowly approach it, it seems that debri from the ceiling has damaged the device and broken off some pieces.',
  yes: new Effect({
    option: 'Repair',
    desc: 'You are able to fit the pieces together and the wheels inside the device start turning.\n\nA slight glow starts to eminate from the device.',
    resolve: function (game, card, effect) {
      game.deck.unshift(Clockwork.clone())
    }
  }),
  no: new Effect({
    option: 'Leave',
    desc: 'You decided that strange devices are better left alone.',
    resolve: function (game, card, effect) {}
  })
})

var Clockwork = new Card({
  title: 'Clockwork',
  img: '../Art/Empty.png',
  environment: null,
  desc: 'Glowing device that is humming. There is a strange button on it.',
  yes: new Effect({
    option: 'Use',
    desc: '',
    resolve: function (game, card, effect) {
      game.deck.unshift(Clockwork.clone())
    }
  }),
  no: new Effect({
    option: 'Place Back',
    desc: '',
    resolve: function (game, card, effect) {}
  })
})

var SwampCards = [Hut, Frog, Corpse, MysteriousRock]
var ForestCards = [Wagon, Corpse, MysteriousRock]
var TownCards = [SickMan, Archeologist, Corpse, MysteriousRock]

var BaseCards = [GhostlyLady, Fork]
