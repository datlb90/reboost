/**
 * HTTP client.
 *
 * @see {@link https://github.com/axios/axios}
 * @module utils/axios
 */
import _ from 'lodash'
import axios from 'axios'
import * as urlUtils from '@/utils/url'
import store from '../store'

import { Notification } from 'element-ui'

const config = {
  baseURL: urlUtils.getBaseApiURL(),
  timeout: 100000
}

const instance = axios.create(config)

// Add a request interceptor
instance.interceptors.request.use(
  function(config) {
    store.dispatch('loading/startLoading')
    const accessToken = store.state.auth.user.token
    console.log(accessToken)
    if (!_.isEmpty(accessToken)) {
      config.headers.Authorization = `Bearer ${accessToken}`
    }
    return config
  },
  function(error) {
    store.dispatch('loading/startLoading')
    return Promise.reject(error)
  }
)

instance.interceptors.response.use(
  response => {
    store.dispatch('loading/doneLoading')
    return response
  },
  error => {
    store.dispatch('loading/doneLoading')

    // Check for errorHandle config
    if (error.config.hasOwnProperty('errorHandle') && error.config.errorHandle === false) {
      return Promise.reject(error)
    }

    // Handling error
    if (error.response) {
      let message = ''
	  if (error.response.data.message) { message += error.response.data.message + ' ' }
	  if (error.response.data.title) { message += error.response.data.title + ' ' }
      if (error.response.data.modelState) { message += error.response.data.modelState.invalid_grant + ' ' }
      if (error.response.data.innerException) { message += error.response.data.innerException.exceptionMessage }

      Notification.error({
        title: 'Error',
        message: message
      })
    }
  }
)

// axios methods
const methods = {
  /**
	 * request.
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  request(config) {
    throw new Error('Not Implemented')
  },

  /**
	 * get.
	 * @param {String} url Url
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  get(url, config) {
    return instance.get(url, config)
  },

  /**
	 * delete.
	 * @param {String} url Url
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  delete(url, config) {
    return instance.delete(url, config)
  },

  /**
	 * head.
	 * @param {String} url Url
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  head(url, config) {
    throw new Error('Not Implemented')
  },

  /**
	 * options.
	 * @param {String} url Url
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  options(url, config) {
    throw new Error('Not Implemented')
  },

  /**
	 * post.
	 * @param {String} url Url
	 * @param {Object} data Data
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  post(url, data, config) {
    return instance.post(url, data, config)
  },

  /**
	 * put.
	 * @param {String} url Url
	 * @param {Object} data Data
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  put(url, data, config) {
    return instance.put(url, data, config)
  },

  /**
	 * patch.
	 * @param {String} url Url
	 * @param {Object} data Data
	 * @param {Object} config Config
	 * @return {Promise} Promise
	 * @see {@link https://github.com/mzabriskie/axios#instance-methods}
	 */
  patch(url, data, config) {
    throw new Error('Not Implemented')
  }
}

export default methods

/**
 * Set access token.
 *
 * @param {String} token access token
 */
// export const setAccessToken = token => {
//   accessToken = token
// }

/**
 * Set error handler for axios instance.
 *
 * @param {Function} handler handler fucntion
 */
// export const setErrorHander = handler => {
//   errorHandler = handler
// }
