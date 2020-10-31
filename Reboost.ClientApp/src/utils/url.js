/**
 * URL utilities.
 * @module utils/url-utils
 */

/**
 * Get base URL.
 *
 * @return {String} URL
 */
export const getBaseURL = () => {
  const url = window.location.href
  const arr = url.split('/')
  const protocol = arr[0]
  // const protocol = process.env.NODE_ENV === 'production' ? 'https:' :'http:';
  const host = process.env.NODE_ENV === 'production' ? window.location.host : 'localhost:6990'
  return `${protocol}//${host}`
}

/**
 * Get base URL of REST service.
 *
 * @return {String} URL
 */
export const getBaseApiURL = () => {
  const apiEndpoint = process.env.VUE_APP_API_ENDPOINT
  return `${getBaseURL()}/${apiEndpoint}`
}
