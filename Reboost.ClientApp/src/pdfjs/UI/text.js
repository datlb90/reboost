import PDFJSAnnotate from '../PDFJSAnnotate'
import appendChild from '../render/appendChild'
import {
  // BORDER_COLOR,
  findSVGAtPoint,
  getMetadata,
  scaleDown
} from './utils'

let _enabled = false
let input
let _textSize = 12
let _textColor = 'ff0000'

/**
 * Handle document.mouseup event
 *
 * @param {Event} e The DOM event to handle
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

  const page = document.getElementById('pageContainer1')
  const rect = page.getBoundingClientRect()
  const x = e.clientX - rect.left
  const w = rect.width - x

  const { viewport } = getMetadata(findSVGAtPoint(e.clientX, e.clientY))

  input = document.createElement('span')
  input.setAttribute('id', 'pdf-annotate-text-input')
  input.style.maxWidth = `${w}px`
  // input.setAttribute('placeholder', 'Enter text')
  input.setAttribute('contenteditable', true)
  input.setAttribute('display', 'block')
  // input.style.border = `3px solid ${BORDER_COLOR}`
  input.style.outline = 'none'
  // input.style.borderRadius = '3px'
  input.style.position = 'absolute'
  input.style.color = localStorage.getItem('colorChosen') || _textColor
  // input.style.textSize = (_textSize * viewport.scale) + 'px'
  input.style.fontSize = `${_textSize * viewport.scale}px`

  // if (inWidth < 150) {
  //   input.style.width = inWidth + 'px'
  // } else {
  //   input.style.width = '150px'
  // }
  input.style.top = `${e.clientY}px`
  input.style.left = `${e.clientX}px`

  input.addEventListener('blur', handleInputBlur)
  input.addEventListener('keyup', handleInputKeyup)
  input.addEventListener('keydown', handleInputKeydown)

  document.body.appendChild(input)
  input.focus()

  input.innerText = 'text'
  document.execCommand('selectAll', false, null)
}

/**
 * Handle input.blur event
 */
function handleInputBlur() {
  saveText()
}

/**
 * Handle input.keyup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleInputKeyup(e) {
  if (e.keyCode === 27) {
    closeInput()
  } else if (e.keyCode === 13) {
    saveText()
  }
}

function handleInputKeydown(e) {
  if (e.keyCode === 13) {
    e.preventDefault()
  }
}

/**
 * Save a text annotation from input
 */
function saveText() {
  if (input.innerText.trim().length > 0) {
    const clientX = parseInt(input.style.left, 10)
    const clientY = parseInt(input.style.top, 10)
    const svg = findSVGAtPoint(clientX, clientY)
    if (!svg) {
      return
    }
    const { documentId, pageNumber } = getMetadata(svg)
    const rect = svg.getBoundingClientRect()
    const w = input.clientWidth
    const annotation = Object.assign({
      type: 'textbox',
      size: _textSize,
      color: localStorage.getItem('colorChosen') || _textColor,
      content: input.innerText.trim()
    }, scaleDown(svg, {
      x: clientX - rect.left,
      y: clientY - rect.top,
      width: w,
      height: input.clientHeight
    })
    )

    PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
      .then((annotation) => {
        appendChild(svg, annotation)
      })
  }

  closeInput()
}

/**
 * Close the input
 */
function closeInput() {
  if (input) {
    input.removeEventListener('blur', handleInputBlur)
    input.removeEventListener('keyup', handleInputKeyup)
    document.body.removeChild(input)
    input = null
  }
}

/**
 * Set the text attributes
 *
 * @param {Number} textSize The size of the text
 * @param {String} textColor The color of the text
 */
export function setText(textSize = 12, textColor = '000000') {
  _textSize = parseInt(textSize, 10)
  _textColor = textColor
}

/**
 * Enable text behavior
 */
export function enableText() {
  if (_enabled) { return }

  _enabled = true
  document.addEventListener('mouseup', handleDocumentMouseup)
}

/**
 * Disable text behavior
 */
export function disableText() {
  if (!_enabled) { return }

  _enabled = false
  document.removeEventListener('mouseup', handleDocumentMouseup)
}

