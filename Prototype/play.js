var content = document.getElementById('content')
var game = new Game(RandomCards)
game.reset()

function renderBuffs (root) {
  var player = game.player
  var buffs = []
  for (var name in player.buff) {
    buffs.push(E('buff', {}, [name]))
  }
  root.appendChild(E('buffs', {}, buffs))
}

function mkCard (props) {
  return E('card', {}, [
    E('card-image', {}, [
      Et('img', '', {src: props.img}, [])
    ]),
    E('card-content', {}, [
      E('card-title', {}, [props.title]),
      E('card-desc', {}, [props.desc])
    ])
  ])
}

function showActiveCard () {
  var root = content
  root.innerHTML = ''
  renderBuffs(root)

  var card = game.activeCard
  if (card == null) {
    root.appendChild(mkCard({
      title: 'End of the road',
      desc: 'Every life ends at some point.',
      img: '../art 2048/death.png'
    }))
    return
  }

  console.log("Active Card", card)

  var desc = game.desc
  root.appendChild(mkCard({
    title: card.title,
    img: card.image,
    desc: desc
  }))

  var options = game.options
  root.appendChild(E('options', {}, [
    E('option', {
      onclick: function () {
        game.no()
        showResolution()
      }
    }, [options.no.option]),
    E('option', {
      onclick: function () {
        game.yes()
        showResolution()
      }
    }, [options.yes.option])
  ]))
}
function showResolution () {
  var root = content
  root.innerHTML = ''
  renderBuffs(root)

  var resolution = game.lastResolution
  var card = resolution.card
  var effect = resolution.effect
  root.appendChild(mkCard({
    title: resolution.card.title,
    desc: resolution.desc,
    img: card.image
  }))

  root.appendChild(E('options', {}, [
    E('option', {
      onclick: showActiveCard
    }, ['Continue'])
  ]))
}

showActiveCard()
