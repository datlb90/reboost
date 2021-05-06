import setAttributes from '../utils/setAttributes'
import normalizeColor from '../utils/normalizeColor'

/**
 * Create SVGRectElements from an annotation definition.
 * This is used for anntations of type `area` and `highlight`.
 *
 * @param {Object} a The annotation definition
 * @return {SVGGElement|SVGRectElement} A group of all rects to be rendered
 */
export default function renderRect(a) {
  if (a.type === 'highlight' || a.type === 'comment-highlight') {
    const group = document.createElementNS('http://www.w3.org/2000/svg', 'g')
    let order = 1

    const otherGroups = document.querySelectorAll("g[top='" + parseInt(a.top) + "']")
    if (otherGroups.length > 0) {
      const groupsArr = Array.prototype.slice.call(otherGroups)
      const max = Math.max.apply(Math, groupsArr.map(function(o) { return parseInt(o.getAttribute('order')) }))
      order = max + 1
    }

    setAttributes(group, {
      fill: normalizeColor(a.color || '#ff0'),
      fillOpacity: 0.2,
      top: parseInt(a.top),
      left: parseInt(a.left),
      pageNum: a.pageNum,
      pageHeight: a.pageHeight,
      order: order
    })

    a.rectangles.forEach((r) => {
      group.appendChild(createRect(r))
    })

    return group
  } else {
    const rect = createRect(a)
    setAttributes(rect, {
      stroke: normalizeColor(a.color || '#ff0000'),
      fill: 'none',
      top: a.y,
      left: a.left,
      'page-num': a.pageNum,
      'page-height': a.pageHeight
    })

    return rect
  }
}

function createRect(r) {
  const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect')

  setAttributes(rect, {
    x: r.x,
    y: r.y,
    width: r.width,
    height: r.height
  })

  return rect
}
