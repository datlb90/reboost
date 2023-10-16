import setAttributes from '../utils/setAttributes'

const SIZE = 25
// const D = 'M499.968 214.336q-113.832 0 -212.877 38.781t-157.356 104.625 -58.311 142.29q0 62.496 39.897 119.133t112.437 97.929l48.546 27.9 -15.066 53.568q-13.392 50.778 -39.06 95.976 84.816 -35.154 153.45 -95.418l23.994 -21.204 31.806 3.348q38.502 4.464 72.54 4.464 113.832 0 212.877 -38.781t157.356 -104.625 58.311 -142.29 -58.311 -142.29 -157.356 -104.625 -212.877 -38.781z'
// const D = 'M448 348.106V80c0-26.51-21.49-48-48-48H48C21.49 32 0 53.49 0 80v351.988c0 26.51 21.49 48 48 48h268.118a48 48 0 0 0 33.941-14.059l83.882-83.882A48 48 0 0 0 448 348.106zm-128 80v-76.118h76.118L320 428.106zM400 80v223.988H296c-13.255 0-24 10.745-24 24v104H48V80h352z'
const D = 'M312 320h136V56c0-13.3-10.7-24-24-24H24C10.7 32 0 42.7 0 56v400c0 13.3 10.7 24 24 24h264V344c0-13.2 10.8-24 24-24zm129 55l-98 98c-4.5 4.5-10.6 7-17 7h-6V352h128v6.1c0 6.3-2.5 12.4-7 16.9z'

/**
 * Create SVGElement from an annotation definition.
 * This is used for anntations of type `comment`.
 *
 * @param {Object} a The annotation definition
 * @return {SVGElement} A svg to be rendered
 */
export default function renderPoint(a) {
  const outerSVG = document.createElementNS('http://www.w3.org/2000/svg', 'svg')
  const innerSVG = document.createElementNS('http://www.w3.org/2000/svg', 'svg')
  const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect')
  const path = document.createElementNS('http://www.w3.org/2000/svg', 'path')
  let order = 1
  const otherPoints = document.querySelectorAll("svg[top='" + a.top + "']")
  if (otherPoints.length > 0) {
    const groupsArr = Array.prototype.slice.call(otherPoints)
    const max = Math.max.apply(Math, groupsArr.map(function(o) { return parseInt(o.getAttribute('order')) }))
    order = max + 1
  }
  setAttributes(outerSVG, {
    width: SIZE,
    height: SIZE,
    x: a.x,
    y: a.y,
    top: a.top,
    left: a.left,
    'page-num': a.pageNum,
    'page-height': a.pageHeight,
    'order': order
  })

  // <svg class="svg-inline--fa fa-sticky-note fa-w-14 toolbar-icon" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="sticky-note" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M312 320h136V56c0-13.3-10.7-24-24-24H24C10.7 32 0 42.7 0 56v400c0 13.3 10.7 24 24 24h264V344c0-13.2 10.8-24 24-24zm129 55l-98 98c-4.5 4.5-10.6 7-17 7h-6V352h128v6.1c0 6.3-2.5 12.4-7 16.9z"></path></svg>

  setAttributes(innerSVG, {
    width: SIZE,
    height: SIZE,
    x: 0,
    y: (SIZE * 0.05) * -1,
    viewBox: '0 0 448 512',
    style: 'color: #419efe;',
    // viewBox: '0 0 1000 1000',
    class: 'svg-inline--fa fa-sticky-note fa-w-14'

  })

  setAttributes(rect, {
    width: SIZE,
    height: SIZE,
    // stroke: '#000',
    fill: '#fff'
  })

  setAttributes(path, {
    d: D,
    // width: SIZE,
    // height: SIZE,
    // strokeWidth: 50,
    // stroke: '#000',
    fill: 'currentColor'
  })

  innerSVG.appendChild(path)
  outerSVG.appendChild(rect)
  outerSVG.appendChild(innerSVG)
  return outerSVG
}
