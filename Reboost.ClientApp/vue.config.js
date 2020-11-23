process.env.VUE_APP_API_ENDPOINT = 'api'
process.env.VUE_APP_CLIENT_SECRET = '6rujMjHV2aNKaRf+E/GaYE5UjiDrmt7D/rthexlyZg12bjnC7GB55sTgDFCqPiwLhnjPF/Iwx9KQwS+QRRW5/w=='
process.env.VUE_APP_CLIENT_ID = 'EdvisionWebClient'
const port = process.env.port || process.env.npm_config_port || 3011

var PrerenderSpaPlugin = require('prerender-spa-plugin')
var path = require('path')

module.exports = {
  devServer: {
    port: port,
    open: true
  },
  outputDir: '../Reboost.WebApi/wwwroot',
  configureWebpack: config => {
    if (process.env.NODE_ENV !== 'production') return

    return {
      plugins: [
        new PrerenderSpaPlugin(
          // Absolute path to compiled SPA
          path.resolve(__dirname, 'dist'),
          // List of routes to prerender
          ['/', '/about'],
        )
      ]
    }
  }
}
