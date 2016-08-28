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

function mkCard(props){
  return E('card', {}, [
      E('card-image', {}, [
        Et('img', '', {src:props.img}, []),
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
      title: "End of the road",
      desc: "Every life ends at some point.",
      img: "../Art/death.png"
    }))
    return
  }

  root.appendChild(mkCard(card));

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
  root.appendChild(mkCard({
    title: card.title,
    desc: effect.desc,
    img: card.img
  }));
    
  root.appendChild(E('options', {}, [
    E('option', {
      onclick: showActiveCard
    }, ['Continue'])
  ]))
}

showActiveCard()
