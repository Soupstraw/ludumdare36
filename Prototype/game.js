function pick (array) {
  return array[array.length * Math.random() | 0]
}

function nop () {}

function Card (props) {
  this.image = props.image || ''
  this.title = props.title
  this.describe = props.describe
  this.options = props.options
  this.environment = props.environment
  this.applicable = props.applicable || function () { return true; }
}

Card.prototype = {
  clone: function () {
    return new Card(this)
  }
}

function Player () {
  this.buff = {}
}

function Effect (props) {
  this.option = props.option
  this.resolve = props.resolve || nop
}
Effect.prototype = {
  clone: function () { return new Effect(this) }
}

function Game (random) {
  this.random = random
  this.resolved = []
  this.deck = []
  this.player = new Player()
}

Game.prototype = {
  get activeCard() {
    if(this.deck.length == 0){
      //this.deck.unshift(pick(this.random))
    }
    return this.deck[0]
  },
  get desc() {
    return this.activeCard.describe(this)
  },
  get options() {
    return this.activeCard.options(this)
  },
  get lastResolution() {
    return this.resolved[this.resolved.length - 1]
  },
  reset: function () {
    this.player = new Player()
    this.resolved = []
    this.deck = []
  },
  insertAt: function (n, card) {
    if (n >= this.deck.length) {
      this.deck.push(card)
    } else {
      this.deck.splice(n, 0, card)
    }
  },
  select: function (option) {
    var card = this.deck.shift()
    var options = card.options(this)
    var effect = options[option]
    var desc = effect.resolve(this, card, effect)

    this.resolved.push({
      card: card,
      desc: desc,
      selected: option,
      effect: effect
    })

    return this.lastResolution
  },
  yes: function () { return this.select('yes'); },
  no: function () { return this.select('no'); }
}
