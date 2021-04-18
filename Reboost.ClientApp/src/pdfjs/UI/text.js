import PDFJSAnnotate from '../PDFJSAnnotate'
import appendChild from '../render/appendChild'
import { fireEvent } from './event'
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
let svg = null
let clientClickX = 0
let clientClickY = 0
let annotation = null
let content = null
let isChangeContent = false

/**
 * Handle document.mouseup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMouseup(e) {
  const rectTool = document.getElementById('rectTool')
  if (rectTool.style.position == 'absolute' && rectTool.style.visibility != 'hidden') {
    return
  }
  const overlay = document.getElementById('pdf-annotate-edit-overlay')
  if (overlay) {
    // overlay.parentNode.removeChild(overlay)
    return
  }
  hideRectToolBar()
  clientClickX = e.clientX
  clientClickY = e.clientY
  var isToolBar = false
  Array.prototype.slice.call(e.path).forEach(r => {
    if (r.tagName == 'DIV' && r.getAttribute('id') == 'tool-bar') {
      isToolBar = true
    }
  })

  if (input || !findSVGAtPoint(e.clientX, e.clientY) || isToolBar) {
    return
  }
  svg = findSVGAtPoint(e.clientX, e.clientY)
  if (!svg) {
    return
  }
  const { documentId, pageNumber, viewport } = getMetadata(svg)
  // const { viewport } = getMetadata(findSVGAtPoint(e.clientX, e.clientY))

  const page = document.getElementById('pageContainer' + `${pageNumber}`)
  const rect = page.getBoundingClientRect()
  const x = e.clientX - rect.left
  const w = rect.width - x
  const y = e.clientY - rect.top

  input = document.createElement('pre')
  input.setAttribute('id', 'pdf-annotate-text-input')
  input.style.maxWidth = `${w}px`
  // input.setAttribute('placeholder', 'Enter text')
  input.setAttribute('contenteditable', true)
  input.setAttribute('display', 'block')
  // input.style.border = `3px solid ${BORDER_COLOR}`
  input.style.outline = 'none'
  // input.style.borderRadius = '3px'
  input.style.position = 'absolute'
  input.style.color = localStorage.getItem(`${documentId}/color`) || _textColor
  // input.style.textSize = (_textSize * viewport.scale) + 'px'
  input.style.fontSize = `${_textSize * viewport.scale}px`

  // if (inWidth < 150) {
  //   input.style.width = inWidth + 'px'
  // } else {
  //   input.style.width = '150px'
  // }
  input.style.top = `${y}px`
  input.style.left = `${x}px`

  input.addEventListener('blur', handleInputBlur)
  input.addEventListener('keyup', handleInputKeyup)
  input.addEventListener('keydown', handleInputKeydown)

  // document.body.appendChild(input)

  document.querySelector(`div[data-page-number='${pageNumber}']`).appendChild(input)

  input.focus()

  input.innerText = 'Enter Text Here'
  document.execCommand('selectAll', false, null)
}

/**
 * Handle input.blur event
 */
async function handleInputBlur() {
  await saveText()
  fireEvent('text:disable', 'text')
}

function hideRectToolBar() {
  const rectTool = document.getElementById('rectTool')
  rectTool.style.visibility = 'hidden'
}

/**
 * Handle input.keyup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleInputKeyup(e) {
  isChangeContent = true
  if (e.keyCode === 27) {
    closeInput()
    fireEvent('text:disable', 'text')
  } else if (e.keyCode === 13) {
    // saveText()
  }
}

function handleInputKeydown(e) {
  isChangeContent = true
  if (e.keyCode === 13) {
    // e.preventDefault()
  }
}

/**
 * Save a text annotation from input
 */
async function saveText() {
  if (input.innerText.trim().length > 0 && isChangeContent) {
    const clientX = clientClickX
    const clientY = clientClickY
    if (!svg) {
      return
    }
    const { documentId, pageNumber } = getMetadata(svg)
    const rect = svg.getBoundingClientRect()
    const w = input.clientWidth
    content = input.innerText.trim()
    for (let i = 0; i < content.length - 1; i++) {
      if (content[i] == '\n' && content[i + 1] == '\n') {
        var tempStr = content.slice(0, i)
        tempStr = tempStr + ' '
        tempStr = tempStr + content.slice(i + 1, content.length)
        content = tempStr
        i++
      }
    }
    annotation = Object.assign({
      type: 'textbox',
      size: _textSize,
      color: localStorage.getItem(`${documentId}/color`) || _textColor,
      content: content
    }, scaleDown(svg, {
      x: clientX - rect.left,
      y: clientY - rect.top,
      width: w,
      height: input.offsetHeight
    })
    )
    await PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
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
    // document.body.removeChild(input)
    const { pageNumber } = getMetadata(svg)
    document.querySelector(`div[data-page-number='${pageNumber}']`).removeChild(input)
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
  document.querySelectorAll(`.textLayer`).forEach(txt => {
    txt.style.userSelect = 'none'
  })
  document.addEventListener('mouseup', handleDocumentMouseup)
}

/**
 * Disable text behavior
 */
export function disableText() {
  if (!_enabled) { return }
  _enabled = false
  document.querySelectorAll(`.textLayer`).forEach(txt => {
    txt.style.userSelect = 'auto'
  })
  document.removeEventListener('mouseup', handleDocumentMouseup)
}

/**
 * Check enable status
 */
export function isEnabling() {
  return _enabled
}

