function pick (array) {
  return array[array.length * Math.random() | 0]
}

function pickremove(array){
  var i = array.length * Math.random() | 0;
  var el = array[i]
  array.splice(i, 1)
  return el
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
  reset: function () {
    this.player = new Player()
    this.resolved = []
    this.deck = []
    
    var partial = [];
    for(var i = 0; i < 30; i++){
      if(partial.length == 0){
        partial = this.random.slice()
      }
      this.deck.push(pickremove(partial))
    }

    this.deck[0] = Journey
    this.deck[15] = Aging
    this.deck[29] = DeathByAging
  },
  get activeCard() {
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

    this.dropDuplicates()

    return this.lastResolution
  },
  dropDuplicates: function(){
    var avoid = this.resolved.slice(this.resolved.length-3)
    while(this.deck.length > 0){
      var card = this.deck[0]
      
      // drop cards that are not applicable
      if(!card.applicable(this)){
        this.deck.shift()
        continue
      }
      
      // drop cards that have recently been drawn
      var skip = false
      for(var k = 0; k < avoid.length; k++){
        if(avoid[k].card.title === card.title){
          skip = true
        }
      }
      if(!skip){
        break
      }
      this.deck.shift()
    }
  },
  yes: function () { return this.select('yes'); },
  no: function () { return this.select('no'); }
}
