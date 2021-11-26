export default {
  install(Vue) {
    Vue.mixin({
      methods: {
        messageTranslate(message) {
          return this.$ml.get(message)
        },
        messageTranslates(page, message) {
          return this.$ml.get(page)[message]
        },
        constantTranslate(type, message) {
          // console.log(this.$ml.get('constant')[type][message.toUpperCase()], message.toUpperCase())
          return this.$ml.get('constant')[type][message]
        }
      }
    })
  }
}
