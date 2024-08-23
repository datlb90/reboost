process.env.VUE_APP_API_ENDPOINT = 'api'
process.env.VUE_APP_CLIENT_SECRET = '6rujMjHV2aNKaRf+E/GaYE5UjiDrmt7D/rthexlyZg12bjnC7GB55sTgDFCqPiwLhnjPF/Iwx9KQwS+QRRW5/w=='
process.env.VUE_APP_CLIENT_ID = 'EdvisionWebClient'
const port = process.env.port || process.env.npm_config_port || 3011

var PrerenderSpaPlugin = require('prerender-spa-plugin')
var path = require('path')
const Renderer = PrerenderSpaPlugin.PuppeteerRenderer

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
        new PrerenderSpaPlugin({
          // Absolute path to compiled SPA
          staticDir: path.resolve(__dirname, '../Reboost.WebApi/wwwroot'),
          // List of routes to prerender
          routes: ['/', '/pricing', '/sample/feedback/basic'],

          // Configure Puppeteer for better rendering of pages with dynamic content
          renderer: new Renderer({
            headless: true,
            renderAfterDocumentEvent: 'render-event', // Wait for 'render-event' to be dispatched
            renderAfterTime: 5000 // Add a delay to ensure everything is fully loaded (optional)
          })
        })
      ]
    }
  }
}
