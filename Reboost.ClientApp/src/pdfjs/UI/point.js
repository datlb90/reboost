import PDFJSAnnotate from '../PDFJSAnnotate'
import { fireEvent } from '../UI/event'
import appendChild from '../render/appendChild'
import {
  BORDER_COLOR,
  findSVGAtPoint,
  getMetadata,
  scaleDown
} from './utils'

let _enabled = false
let input

/**
 * Handle document.mouseup event
 *
 * @param {Event} The DOM event to be handled
 */
function handleDocumentMouseup(e) {
  var isToolBar = false
  Array.prototype.slice.call(e.path).forEach(r => {
    if (r.tagName == 'DIV' && r.getAttribute('id') == 'tool-bar') {
      isToolBar = true
    }
  })

  if (input || !findSVGAtPoint(e.clientX, e.clientY) || isToolBar) {
    return
  }

  input = document.createElement('input')
  input.setAttribute('id', 'pdf-annotate-point-input')
  input.setAttribute('placeholder', 'Enter note')
  input.setAttribute('top', `${e.clientY}`)
  input.style.border = `3px solid ${BORDER_COLOR}`
  input.style.borderRadius = '3px'
  input.style.position = 'absolute'
  input.style.top = `${e.clientY}px`
  input.style.left = `${e.clientX}px`

  savePoint()

  // input.addEventListener('blur', handleInputBlur)
  // input.addEventListener('keyup', handleInputKeyup)

  // document.body.appendChild(input)
  // input.focus()
}

// /**
//  * Handle input.blur event
//  */
// function handleInputBlur() {
//   savePoint()
// }

// /**
//  * Handle input.keyup event
//  *
//  * @param {Event} e The DOM event to handle
//  */
// function handleInputKeyup(e) {
//   if (e.keyCode === 27) {
//     closeInput()
//   } else if (e.keyCode === 13) {
//     savePoint()
//   }
// }

/**
 * Save a new point annotation from input
 */
function savePoint() {
  const clientX = parseInt(input.style.left, 10)
  const clientY = parseInt(input.style.top, 10)
  const svg = findSVGAtPoint(clientX, clientY)
  if (!svg) {
    return
  }
  // format top with scale 100%
  const rect = svg.getBoundingClientRect()
  const { documentId, pageNumber, viewport } = getMetadata(svg)
  const annotation = Object.assign({
    type: 'point',
    pageNum: pageNumber,
    pageHeight: svg.getAttribute('height') / viewport.scale
  }, scaleDown(svg, {
    x: clientX - rect.left,
    y: clientY - rect.top
  })
  )
  annotation.top = annotation.y
  annotation.left = annotation.x
  console.log('annoation', annotation)
  // const commentWrapper = document.getElementById('add-new-comment')
  // const topPos = parseInt(commentWrapper.style.top.substring(0, commentWrapper.style.top.length - 2))
  PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation, true)
    .then((annotation) => {
      appendChild(svg, annotation)
      fireEvent('annotation:insertNoteComment', svg, annotation)
    })

  closeInput()
  disablePoint()
}

/**
 * Close the input element
 */
function closeInput() {
  // input.removeEventListener('blur', handleInputBlur)
  // input.removeEventListener('keyup', handleInputKeyup)
  // document.body.removeChild(input)
  input = null
}

/**
 * Enable point annotation behavior
 */
export function enablePoint() {
  if (_enabled) { return }

  _enabled = true
  document.addEventListener('mouseup', handleDocumentMouseup)
}

/**
 * Disable point annotation behavior
 */
export function disablePoint() {
  if (!_enabled) { return }

  _enabled = false
  document.removeEventListener('mouseup', handleDocumentMouseup)
}
