// import setAttributes from '../utils/setAttributes'
// import normalizeColor from '../utils/normalizeColor'

/**
 * Create SVGTextElement from an annotation definition.
 * This is used for anntations of type `textbox`.
 *
 * @param {Object} a The annotation definition
 * @return {SVGTextElement} A text to be rendered
 */
export default function renderText(a) {
  // const text = document.createElementNS('http://www.w3.org/2000/svg', 'text')

  // setAttributes(text, {
  //   x: a.x,
  //   y: a.y + parseInt(a.size, 10),
  //   fill: normalizeColor(a.color || '#000'),
  //   fontSize: a.size
  // })
  // text.innerHTML = a.content
  // const textStrings = a.content.split(' ')
  // console.log('texts', textStrings)

  // textStrings.forEach(function(value, index) {
  //   var ts = document.createElementNS('http://www.w3.org/2000/svg', 'tspan')
  //   var tn = document.createTextNode(value)
  //   ts.setAttributeNS(null, 'dy', (index) ? '1.2em' : 0)
  //   ts.setAttributeNS(null, 'x', a.x)
  //   ts.setAttributeNS(null, 'text-anchor', 'start')
  //   ts.appendChild(tn)
  //   text.appendChild(ts)
  // })

  // return text
  var p = document.createElement('p')
  p.style.fontSize = `${a.size}px`
  p.style.lineHeight = '1.5'
  p.style.color = a.color || 'f00'
  var tn = document.createTextNode(a.content)
  const foreignObject = document.createElementNS('http://www.w3.org/2000/svg', 'foreignObject')
  // if (a.width < 150) {
  //   foreignObject.setAttribute('width', a.width)
  // } else {
  //   foreignObject.setAttribute('width', '150')
  // }
  foreignObject.setAttribute('width', a.width + 5)
  foreignObject.setAttribute('height', a.height)
  foreignObject.setAttribute('x', a.x)
  foreignObject.setAttribute('y', a.y)
  foreignObject.setAttribute('stroke', a.color || '#f00')

  p.appendChild(tn)

  foreignObject.appendChild(p)

  return foreignObject
}
