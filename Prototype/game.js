function pick (array) {
  return array[array.length * Math.random() | 0]
}

function nop () {}

function Card (props) {
  this.img = props.img || "#"
  this.final = props.final || false
  this.title = props.title
  this.desc = props.desc
  this.yes = props.yes.clone()
  this.no = props.no.clone()
  this.specialize = props.specialize || null
}
Card.prototype = {
  clone: function () {
    if (this.specialize) {
      return this.specialize()
    }
    return new Card(this)
  }
}

function Player () {
  this.buff = {}
}

function Effect (props) {
  this.option = props.option
  this.desc = props.desc
  this.resolve = props.resolve || nop
}
Effect.prototype = {
  clone: function () { return new Effect(this) }
}

function Game (cards) {
  this.cards = cards
  this.resolved = []
  this.deck = []
  this.player = new Player()
}

Game.prototype = {
  get activeCard() {
    return this.deck[0]
  },
  get lastResolution() {
    return this.resolved[this.resolved.length - 1]
  },
  reset: function () {
    this.player = new Player()
    this.resolved = []
    this.deck = []
    for (var i = 0; i < this.cards.length; i++) {
      var card = this.cards[i]
      this.deck.push(this.cards[i].clone())
    }
  },
  insertAt: function(n, card){
    if(n >= this.deck.length){
      this.deck.push(card);
    } else {
      this.deck.splice(n, 0, card);
    }
  },
  select: function (option) {
    var card = this.deck.shift()
    var effect = card[option]
    this.resolved.push({
      card: card,
      selected: option,
      effect: effect
    })
    effect.resolve(this, card, effect)
    return effect
  },
  yes: function () { return this.select('yes'); },
  no: function () { return this.select('no'); }
}
