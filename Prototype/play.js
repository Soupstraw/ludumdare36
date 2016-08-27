var content = document.getElementById('content')
var game = new Game(BaseCards)
game.reset()

function renderBuffs (root) {
  var player = game.player
  var buffs = []
  for (var name in player.buff) {
    buffs.push(E('buff', {}, [name]))
  }
  root.appendChild(E('buffs', {}, buffs))
}

function showActiveCard () {
  var root = content
  root.innerHTML = ''
  renderBuffs(root)

  var card = game.activeCard
  if (card == null) {
    root.appendChild(E('card', {}, [
      Et('img', 'card-image', {src:"../Art/death.png"}, []),
      E('card-content', {}, [
        E('card-title', {}, ['End of the road']),
        E('card-desc', {}, ['Every life ends at some point.'])
      ])
    ]))
    return
  }

  console.log(card.img);
  root.appendChild(E('card', {}, [
    Et('img', 'card-image', {src:card.img}, []),
    E('card-content', {}, [
      E('card-title', {}, [card.title]),
      E('card-desc', {}, [card.desc])
    ])
  ]))

  root.appendChild(E('options', {}, [
    E('option', {
      onclick: function () {
        game.no()
        showResolution()
      }
    }, [card.no.option]),
    E('option', {
      onclick: function () {
        game.yes()
        showResolution()
      }
    }, [card.yes.option])
  ]))
}
function showResolution () {
  var root = content
  root.innerHTML = ''
  renderBuffs(root)

  var resolution = game.lastResolution
  var card = resolution.card
  var effect = resolution.effect
  root.appendChild(E('card', {}, [
    Et('img', 'card-image', {src:card.img}, []),
    E('card-content', {}, [
      E('card-title', {}, [card.title]),
      E('card-desc', {}, [effect.desc])
    ])
  ]))

  root.appendChild(E('options', {}, [
    E('option', {
      onclick: showActiveCard
    }, ['Continue'])
  ]))
}

showActiveCard()
