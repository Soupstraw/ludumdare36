function pick (cards) {
  return cards[cards.length * Math.random() | 0]
}

function between (low, high) {
  return (Math.random() * (high - low) + low) | 0
}

function pickcards (N, into, from) {
  var clones = from.slice()
  for (var i = 0; i < N; i++) {
    if (clones.length <= 0) {
      return
    }
    into.unshift(clones[i])
    clones.splice(i, 1)
  }
}

function basic_description (lines) {
  return function (game) {
    return paragraphs(lines)
  }
}

function paragraphs (lines) {
  return lines.join('\n\n')
}

function basic_options (opts) {
  return function (game) {
    return {
      yes: new Effect(opts.yes),
      no: new Effect(opts.no)
    }
  }
}

var Journey = new Card({
  title: 'Journey',
  image: '../Art/Empty.png',
  describe: basic_description([
    'Your journey begins here and there are many choices to be made.',
    'You can take the path to your left or right.'
  ]),
  options: basic_options({
    yes: {
      option: 'Start',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You start your journey wondering what might happen.'
        ])
      }
    },
    no: {
      option: 'Begin',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You begin your hoping for a better future.'
        ])
      }
    }
  })
})

var Aging = new Card({
  title: "Aging",
  image: '../Art/Empty.png',
  describe: basic_description([
    'Your journey begins here and there are many choices to be made.',
    'You can take the path to your left or right.'
  ]),
  options: basic_options({
    yes: {
      option: 'Start',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You start your journey wondering what might happen.'
        ])
      }
    },
    no: {
      option: 'Begin',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You begin your hoping for a better future.'
        ])
      }
    }
  })
})

var DeathByAging = new Card({
  title: "Death",
  image: '../Art/Empty.png',
  describe: basic_description([
    'Death catches up with us all.'
  ]),
  options: basic_options({
    yes: {
      option: 'Reminisce',
      resolve: function (game, card, effect) {
        game.deck = [];
        return paragraphs([
          'You remember all the encounters in your life while everything fades away.'
        ])
      }
    },
    no: {
      option: 'Say',
      resolve: function (game, card, effect) {
        game.deck = [];
        return paragraphs([
          'You try to say something meaningful, but there is no meaning beyond death.'
        ])
      }
    }
  })
})

var GhostlyLady = new Card({
  title: 'Ghostly Lady',
  image: '../Art/GhostlyLady.png',
  environment: null,
  describe: basic_description([
    'While walking during a windy night you encounter a young woman crying under a nearby tree. She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something.'
  ]),
  options: basic_options({
    yes: {
      option: 'Step Closer',
      resolve: function (game, card, effect) {
        game.player.buff['Delirious Visions'] = true
        game.insertAt(between(4, 6), GhostlyLady2.clone())
        game.insertAt(between(4, 6), DeliriousVisions.clone())

        return paragraphs(["She notices you when you are only a few steps from her. She briefly looks towards you through her tears and continues mumbling. While trying to figure out what to do you start to understand fragments of the children's story she is mumbling. You walk away unable to comfort her. You wonder what happened to her."])
      }
    },
    no: {
      option: 'Walk away',
      resolve: function (game, card, effect) {
        return paragraphs(['You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. She is not there anymore. What happened to her, you wonder… You decide not to bother yourself with this matter anymore.'])
      }
    }
  })
})

var GhostlyLady2 = new Card({
  title: 'Ghostly Lady',
  image: '../Art/GhostlyLady.png',
  environment: null,
  describe: basic_description([
    'While walking during a windy night you encounter a young woman crying under a nearby tree. She has a ghastly halo surrounding her, as if she is not from this world. Through her delirious mumbles you hear her sobbing about something.'
  ]),
  options: basic_options({
    yes: {
      option: 'Step Closer',
      resolve: function (game, card, effect) {
        delete game.player.buff['Delirious Visions']
        game.player.buff['Peace of mind'] = true

        return paragraphs(['How long has she been here, you wonder.. Upon walking closer she acknowledges your presence with a nod and stops sobbing. The wind clears as she slowly sags into the tree.'])
      }
    },
    no: {
      option: 'Walk away',
      resolve: function (game, card, effect) {
        return paragraphs(['You turn away and start walking away from her. You feel the wind becoming stronger and you glance back at the tree where the woman was sitting. She is not there anymore. You feel uneasy and lonely.'])
      }
    }
  })
})

var DeliriousVisions = new Card({
  title: 'Delirious Visions',
  image: '../Art/DeliriousVisions.png',
  environment: null,
  describe: function (game) {
    var lines = [
      'You wake up. Or did you? You are covered in sweat. You wake up. Are you even alive? What is going on? You wake up. Sun shines through the small hole in tavern wall. Tavern keeper tells you that you had been rambling for three days straight in high fever. You were brought here by a friend of yours who paid for a whole week in advance. You have no friends in this town.'
    ]
    if (game.player.buff['Delirious Visions']) {
      lines.push('You think about some of the visions you had and you are fairly certain you were talking with the lady you found sobbing under the tree. She might have been sad that you left, but this is just speculation. Your memories are not clear enough to tell for sure.')
    }
    return paragraphs(lines)
  },
  options: basic_options({
    yes: {
      option: 'Mumble',
      resolve: function (game, card, effect) {
        delete game.player.buff['Delirious Visions']
        game.deck.shift()
        game.deck.shift()
        game.deck.shift()

        return paragraphs([
          'You try to remember words, but they escape you and you end up eloquently saying... "brrrrha"'
        ])
      }
    },
    no: {
      option: 'Thank',
      resolve: function (game, card, effect) {
        delete game.player.buff['Delirious Visions']
        game.deck.shift()
        game.deck.shift()
        game.deck.shift()

        return paragraphs([
          'You thank the tavern keeper and go on your merry way.'
        ])
      }
    }
  })
})

var Fork = new Card({
  title: 'Fork',
  image: '../Art/Empty.png',
  environment: null,
  describe: basic_description([
    'After traveling for miles you see a stubby post leaning in the haze.',
    'It has two signs nailed to it. One points to the forest with huge creeping trees. The other points towards a swamp, with a gleaming light in the distance.'
  ]),
  options: basic_options({
    yes: {
      option: 'Swamp',
      resolve: function (game, card, effect) {
        pickcards(2, game.deck, SwampCards)
        return paragraphs([
          'You start walking towards the light while the fog slowly descends.'
        ])
      }
    },
    no: {
      option: 'Forest',
      resolve: function (game, card, effect) {
        pickcards(2, game.deck, ForestCards)
        return paragraphs([
          'You start walking into the forest. The trees ascend and block out the light leaving you in the dark.'
        ])
      }
    }
  })
})

var Hut = new Card({
  title: 'Hut',
  image: '../Art/Hut.png',
  environment: null,
  describe: basic_description([
    'Hut with gleaming lights.'
  ]),
  options: basic_options({
    yes: {
      option: 'Knock',
      resolve: function (game, card, effect) {
        delete game.player.buff['Flu']
        game.player.buff['Hut'] = true

        return paragraphs([
          'Upon knocking on the door it jumps open. From the other side you are greeted by a jolly old man with long white beard. He pulls you in and forces you to sit down on an ancient but comfortable bed. Then he runs to the back room and returns with a huge wooden cup. He assures you that this tea is made from the best herbs this swamp harnesses. You sip your tea as you watch this peculiar old man jump around and caress his beard non-stop.'
        ])
      }
    },
    no: {
      option: 'Pass',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You hear weird thumps from the house and quicken your steps. You wonder what is going on inside.'
        ])
      }
    }
  })
})

var Frog = new Card({
  title: 'Frog',
  image: '../Art/Frog.png',
  environment: 'Swamp',
  describe: basic_description([
    'Placing foot after foot on the swamp road you notice a small slimy frog jumping around.'
  ]),
  options: basic_options({
    yes: {
      option: 'Let it live',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You leave it and see him happily jumping in a puddle.'
        ])
      }
    },
    no: {
      option: 'Step on it',
      resolve: function (game, card, effect) {
        game.player.buff['Sticky Boots'] = true
        return paragraphs([
          'With a forceful jump you ascend to the sky and fall towards the frog. The frog trembles in horror. The frog is crushed leaving sticky resin on your boots.'
        ])
      }
    }
  })
})

var Wagon = new Card({
  title: 'Wagon',
  image: '../Art/Wagon.png',
  environment: 'Forest',
  describe: basic_description([
    'You notice a broken wagon in the dirt. A small old man is slowly tasking away trying to fix a broken wheel spike. The old man looks very angry.'
  ]),
  options: basic_options({
    yes: {
      option: 'Help',
      resolve: function (game, card, effect) {
        delete game.player.buff['Creeping Terror']
        while(game.deck[0].environment === 'Forest'){
          game.deck.shift()
        }
        pickcards(2, game.deck, TownCards)
        return paragraphs([
          'The old man is thankful for the help and gives you a lift to the next town.'
        ])
      }
    },
    no: {
      option: 'Walk past',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You walk past the man. He shouts "Good riddance, there\'s no need for people like you."',
          'Looking back he is still trying to get things fixed.'
        ])
      }
    }
  })
})

var SickMan = new Card({
  title: 'Sick Man',
  image: '../Art/Empty.png',
  environment: 'Town',
  describe: basic_description([
    'Walking on a cobblestone street you come across a man. He can barely stand straight. He asks people to help him, but no one does.'
  ]),
  options: basic_options({
    yes: {
      option: 'Help',
      resolve: function (game, card, effect) {
        game.player.buff['Flu'] = true
        game.insertAt(between(2, 4), Shivers.clone())

        return paragraphs([
          'You go near the man and support his weight, helping him to walk to the hospital. The doctors take over from there. The man thanks you and stumbles into the hospital.',
          'You feel a slight shiver coming over you.'
        ])
      }
    },
    no: {
      option: 'Avoid',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You do as everyone else and avoid him.'
        ])
      }
    }
  })
})

var Shivers = new Card({
  title: 'Shivers',
  image: '../Art/DeliriousVisions.png',
  environment: null,
  describe: basic_description([
    'You feel shivers throughout your body and start to cough. The weakness starts setting in and you are not sure whether you can go on.'
  ]),
  options: basic_options({
    yes: {
      option: 'Hope',
      resolve: function (game, card, effect) {
        game.insertAt(between(4, 6), DeathShivers.clone())

        return paragraphs([
          "Hopefully it's nothing serious."
        ])
      }
    },
    no: {
      option: 'Death',
      resolve: function (game, card, effect) {
        game.player.buff['Depression'] = true
        game.insertAt(between(4, 6), DeathShivers.clone())

        return paragraphs([
          "You feel like there's nothing more you can do.",
          'What comes, must come.'
        ])
      }
    }
  })
})

var DeathShivers = new Card({
  title: 'Death',
  image: '../Art/Death.png',
  environment: null,
  describe: basic_description([
    'The strength as left your body and you fall to the ground, seeing some people passing by. No-one is willing to risk the same fate as you.',
    'The world slowly fades away.'
  ]),
  options: basic_options({
    yes: {
      option: 'Last breath',
      resolve: function (game, card, effect) {
        game.deck = []
        return paragraphs([
          'You breathe out last time.'
        ])
      }
    },
    no: {
      option: 'Close eyes',
      resolve: function (game, card, effect) {
        game.deck = []
        return paragraphs([
          'You close your eyes.'
        ])
      }
    }
  })
})

var Archeologist = new Card({
  title: 'Archeologist',
  image: '../Art/Archeologist.png',
  environment: 'Town',
  describe: basic_description([
    'A gentleman carrying a briefcase approaches you.',
    '"Are you alright? You seem to be aimlessly looking for a way out of your life."',
    "You don't know what the correct answer is.",
    '"I\'ve recently discovered mentions of an old Artifact that gave people back their life. I hadn\'t much luck, maybe you have more."',
    'The man offers you a map.'
  ]),
  options: basic_options({
    yes: {
      option: 'Take map',
      resolve: function (game, card, effect) {
        game.player.buff['Map'] = true

        return paragraphs([
          'You take the map. The gentleman continues his walk.'
        ])
      }
    },
    no: {
      option: 'Leave',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You take offence what the man said and simply leave.'
        ])
      }
    }
  })
})

var Corpse = new Card({
  title: 'Corpse',
  image: '../Art/Corpse.png',
  environment: null,
  describe: basic_description([
    'You notice a fly on a corpse lying beside the road. How he got there is anyone’s guess. Probably flew in from the swamp.'
  ]),
  options: basic_options({
    yes: {
      option: 'Poke',
      resolve: function (game, card, effect) {
        game.player.buff['Corpse Poker'] = true
        return paragraphs([
          'Poking the corpse did not bring him back to life. What a shame.'
        ])
      }
    },
    no: {
      option: 'Shoo',
      resolve: function (game, card, effect) {
        return paragraphs([
          'The fly flies away angrily.'
        ])
      }
    }
  })
})

var MysteriousRock = new Card({
  title: 'Mysterious Rock',
  image: '../Art/Stone.png',
  environment: null,
  describe: function (game) {
    if (game.player.buff['Map']) {
      return paragraphs([
        'You notice the similarity of the rock to the map that the Archeologist gave you. There seem to be strange symbols that can be twisted.'
      ])
    }

    return paragraphs([
      'You might be imagining things, but it seems that this rock is humming? Maybe it has a mind of itself?'
    ])
  },
  options: function (game) {
    if (game.player.buff['Map'] &&
      game.player.buff['Jasmine'] &&
      game.player.buff['Dianne'] &&
      game.player.buff['Noemi']) {
      return {
        yes: {
          option: 'Open',
          resolve: function (game, card, effect) {
            game.deck.unshift(BrokenClockwork.clone())
            return paragraphs([
              'The rock slowly creaks and opens. The hidden passageway below the rock becomes visible.',
              'You slowly descend into the dark room.'
            ])
          }
        },
        no: {
          option: 'Leave',
          resolve: function (game, card, effect) {
            return paragraphs([
              'You leave waving the rock goodbye. It seemed so friendly.'
            ])
          }
        }
      }
    }

    if (game.player.buff['Map']) {
      return {
        yes: {
          option: 'Touch',
          resolve: function (game, card, effect) {
            return paragraphs([
              'You are unable to figure out the correct combination to make something happen.'
            ])
          }
        },
        no: {
          option: 'Leave',
          resolve: function (game, card, effect) {
            return paragraphs([
              'You leave waving the rock goodbye. It seemed so friendly.'
            ])
          }
        }
      }
    }

    return {
      yes: {
        option: 'Poke',
        resolve: function (game, card, effect) {
          return paragraphs([
            'Poking the rock did not do anything you wouldn’t expect. What did you expect?'
          ])
        }
      },
      no: {
        option: 'Magic',
        resolve: function (game, card, effect) {
          return paragraphs([
            'You don’t know how to do magic. At least you tried. Must count for something, right?'
          ])
        }
      }
    }
  }
})

var BrokenClockwork = new Card({
  title: 'Broken Clockwork',
  image: '../Art/Empty.png',
  environment: null,
  describe: function (game) {
    return paragraphs([
      'Entering the room you notice a strange artifact placed on a pedestal.',
      'You slowly approach it, it seems that debri from the ceiling has damaged the device and broken off some pieces.'
    ])
  },
  options: basic_options({
    yes: {
      option: 'Repair',
      resolve: function (game, card, effect) {
        if (game.player.buff['Sticky Boots']) {
          delete game.player.buff['Sticky Boots']
          game.deck.unshift(Clockwork.clone())

          return paragraphs([
            'You are able to fit the pieces together and the wheels inside the device start turning.',
            'A slight glow starts to eminate from the device.'
          ])
        }

        return paragraphs([
          'You are unable to make the pieces stick to each other properly. It seems some glue is needed.'
        ])
      }
    },
    no: {
      option: 'Leave',
      resolve: function (game, card, effect) {
        return paragraphs([
          'You decided that strange devices are better not played with and leave.'
        ])
      }
    }
  })
})

var Clockwork = new Card({
  title: 'Clockwork',
  image: '../Art/Empty.png',
  environment: null,
  describe: basic_description([
    'Glowing device that is humming. There is a strange button on it.'
  ]),
  options: basic_options({
    yes: {
      option: 'Use',
      resolve: function (game, card, effect) {
        return paragraphs([
          '<TODO>'
        ])
      }
    },
    no: {
      option: 'Place back',
      resolve: function (game, card, effect) {
        return paragraphs([
          '<TODO>'
        ])
      }
    }
  })
})

var Noemi = new Card({
  title: 'Noemi',
  image: '../Art/Noemi.png',
  environment: 'Swamp',
  describe: function (game) {
    var lines = []

    lines.push('From a distance you hear mild and playful screams of joy. Some local girls must be playing in the swamp pool. You haven’t seen anyone for miles and thought that you are completely lost by now so you decide to ask your way. With this in mind you set your course towards the shrieks. To your surprise you only find one young lady in a pool. More chillingly this young lady is poking a dead body. She seems to be very thrilled with what she is doing.')

    if (game.player.buff['Corpse Poker']) {
      lines.push('You think that you’ve already tried poking a corpse before. Didn’t bring her back to life. You wonder what happened with the fly.')
    }

    return paragraphs(lines)
  },
  options: basic_options({
    yes: {
      option: 'Cough politely',
      resolve: function (game, card, effect) {
        game.player.buff['Noemi'] = true

        var lines = []
        lines.push('She jumps slightly and turns around. She does not seem to mind that she has no clothes. There does not seem to be any clothes around the pool either. She gladly agrees to show you the way. Her movements are sharp. She has a distinct aroma surrounding her that reminds you for some reason nutmeg even though they are nothing alike.')

        if (game.player.buff['Hut']) {
          lines.push('Hmm… I think you have already met my godfather. He has a long white beard. He told me good things about you.')
        }

        lines.push('She tells that her name is Noemi. She murmurs about some ancient technology that allows you to give eternal love and for some unexpected reason gives you a lecture on the anatomy of the heart. You feel enlightened but also relieved to hit the road again.')

        return paragraphs(lines)
      }
    },
    no: {
      option: 'Walk away',
      resolve: function (game, card, effect) {
        var lines = []
        lines.push('You turn around and try to walk away without making a noise. Then you feel a light tap on your shoulder. A little spooked out you tell the naked, corpse-poking lady about your journey. She gladly agrees to show you the way. Her movements are sharp. She has a distinct aroma surrounding her that reminds you for some reason nutmeg even though they smell nothing alike.')

        if (game.player.buff['Hut']) {
          lines.push('Hmm… I think you have already met my godfather. He has a long white beard. He told me good things about you.')
        }

        lines.push('She tells that her name is Noemi. She murmurs about some ancient technology that allows you to give eternal love and for some unexpected reason gives you a lecture on the anatomy of the heart. You feel enlightened but also relieved to hit the road again.')

        return paragraph(lines)
      }
    }
  })
})

var Jasmine = new Card({
  title: 'Jasmine',
  image: '../Art/Jasmine.png',
  environment: 'Forest',
  describe: function (game) {
    var lines = []
    lines.push("Walking a peaceful forest road admiring the centuries old trees you hear distant singing. This appealing voice oddly rings in your ears. Charmed by it you step towards it.")
    if (game.player.buff["Peace of mind"] || game.player.buff["Delirious Visions"]) {
      lines.push("Walking closer, you can make out some of this ancient language she is singing in. Reminds you of a children’s story you’ve heard somewhere.")
    }

    lines.push("You reach the edge of a glade. In the center there is a lady singing on top of a rock. Around her there is a circle of dead bodies. She doesn’t seem to bother. Her eyes seem to be cried out.")
    
    if (game.player.buff["Corpse Poker"]) {
      lines.push("Interestingly enough one of the bodies looks familiar. It is still not alive. There is a fly sitting on top of it. ")
    }
    return paragraphs(lines)
  },
  options: basic_options({
    yes: {
      option: 'Step closer',
      resolve: function (game, card, effect) {
        game.player.buff['Jasmine'] = true
        var lines = []
        if (!game.player.buff["Peace of mind"]) {
          lines.push("You step out of the protective shadows onto the soft grass. The lady sitting on the stone keeps singing even though she definitely noticed you. Walking closer you feel choking sensation. You want to stop walking but the singing forces you towards her. You fall next to other dead bodies. You feel tired.")
          game.insertAt(0, Death)
        } else {
          lines.push("You step out of the protective shadows onto the soft grass. The lady sitting on the stone keeps singing even though she definitely noticed you. Walking closer you feel choking sensation. Then she stops singing. You gasp for air. She starts speaking with a beautiful voice that reminds you of birds singing:")
          lines.push('"I am happy to see you again traveler. Last time we didn’t talk, I was overwhelmed with my grief. Your compassion made me understand that I have been in this world for too long. You see, I am an eternal being. A spirit if you wish. I have many faces, you saw one of mine under the tree sobbing."')
          lines.push("She tells you many things about the forest and the meaning of love. She tells you something about the combination of love and technology. It makes little sense to you but you feel like this infromation is going to be useful some day. She sends you off. You realize that sometimes bad things in life can bring experiences that you otherwise might have missed.")
        }
        return paragraphs(lines)
      }
    },
    no: {
      option: 'Walk away',
      resolve: function (game, card, effect) {
        var lines = []
        lines.push("You feel that it was a wise decision to leave. No need to end up like the poor folk on the ground.")

        if (game.player.buff["Depression"]) {
          lines.push("You feel that it was a wise decision to leave. No need to end up like the poor folk on the ground. ")
        }
        return paragraph(lines)
      }
    }
  })
})

var Dianne = new Card({
  title: 'Dianne',
  image: '../Art/Dianne.png',
  environment: 'Forest',
  describe: function (game) {
    var lines = []

    return paragraphs(lines)
  },
  options: basic_options({
    yes: {
      option: 'Cough politely',
      resolve: function (game, card, effect) {
        game.player.buff['Dianne'] = true

        var lines = []

        return paragraphs(lines)
      }
    },
    no: {
      option: 'Walk away',
      resolve: function (game, card, effect) {
        var lines = []

        return paragraph(lines)
      }
    }
  })
})

var SwampCards = [Hut, Frog, Corpse, MysteriousRock, Noemi]
var ForestCards = [Wagon, Corpse, MysteriousRock, Jasmine]
var TownCards = [SickMan, Archeologist, Corpse, MysteriousRock, Dianne]

var RandomCards = [
  GhostlyLady, Fork, Hut, Frog, Corpse, MysteriousRock, Wagon, SickMan
]