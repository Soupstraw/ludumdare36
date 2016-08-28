var Template = new Card({
  title: '',
  image: '../Art/Empty.png',
  environment: null,
  describe: basic_description([
    ''
  ]),
  options: basic_options({
    yes: {
      option: 'Yes',
      resolve: function (game, card, effect) {
        return paragraphs([
          'Yes'
        ])
      }
    },
    no: {
      option: 'No',
      resolve: function (game, card, effect) {
        return paragraphs([
          'No'
        ])
      }
    }
  })
})
