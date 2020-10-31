process.env.VUE_APP_API_ENDPOINT = 'api';
process.env.VUE_APP_CLIENT_SECRET = '6rujMjHV2aNKaRf+E/GaYE5UjiDrmt7D/rthexlyZg12bjnC7GB55sTgDFCqPiwLhnjPF/Iwx9KQwS+QRRW5/w==';
process.env.VUE_APP_CLIENT_ID = 'EdvisionWebClient';
const port = process.env.port || process.env.npm_config_port || 3011; 
module.exports = {
	devServer: {
		port: port,
		open: true
	},
	outputDir: '../Reboost.WebApi/wwwroot'
};