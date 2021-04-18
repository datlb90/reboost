import setAttributes from '../utils/setAttributes'

const SIZE = 25
const D = 'M499.968 214.336q-113.832 0 -212.877 38.781t-157.356 104.625 -58.311 142.29q0 62.496 39.897 119.133t112.437 97.929l48.546 27.9 -15.066 53.568q-13.392 50.778 -39.06 95.976 84.816 -35.154 153.45 -95.418l23.994 -21.204 31.806 3.348q38.502 4.464 72.54 4.464 113.832 0 212.877 -38.781t157.356 -104.625 58.311 -142.29 -58.311 -142.29 -157.356 -104.625 -212.877 -38.781z'

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
    'page-num': a.pageNum,
    'page-height': a.pageHeight,
    'order': order
  })

  setAttributes(innerSVG, {
    width: SIZE,
    height: SIZE,
    x: 0,
    y: (SIZE * 0.05) * -1,
    viewBox: '0 0 1000 1000'
  })

  setAttributes(rect, {
    width: SIZE,
    height: SIZE,
    stroke: '#000',
    fill: '#ff0'
  })

  setAttributes(path, {
    d: D,
    strokeWidth: 50,
    stroke: '#000',
    fill: '#fff'
  })

  innerSVG.appendChild(path)
  outerSVG.appendChild(rect)
  outerSVG.appendChild(innerSVG)

  return outerSVG
}
