/* eslint-disable no-unused-vars */
import PDFJSAnnotate from '../PDFJSAnnotate'

import appendChild from '../render/appendChild'
import { fireEvent } from '../UI/event'
import {
  addEventListener,
  removeEventListener
} from './event'
import {
  BORDER_COLOR,
  disableUserSelect,
  enableUserSelect,
  findSVGContainer,
  findSVGAtPoint,
  getAnnotationRect,
  getMetadata,
  scaleDown,
  scaleUp
} from './utils'

let _enabled = false
let isDragging = false
let startScroll
let parentNode
let overlay
let input
let svgEdit
let dragOffsetX, dragOffsetY, dragStartX, dragStartY
let editingAnnotation
let isEditing = false
const OVERLAY_BORDER_SIZE = 3

/**
 * Create an overlay for editing an annotation.
 *
 * @param {Element} target The annotation element to apply overlay for
 */
function createEditOverlay(target) {
  destroyEditOverlay()
  if (isEditing) {
    return
  }
  if (!target) {
    return
  }
  overlay = document.createElement('div')
  // const anchor = document.createElement('a')
  const parentNode = findSVGContainer(target).parentNode
  const id = target.getAttribute('data-pdf-annotate-id')
  const rect = getAnnotationRect(target)
  const styleLeft = rect.left - OVERLAY_BORDER_SIZE
  const styleTop = rect.top - OVERLAY_BORDER_SIZE

  overlay.setAttribute('id', 'pdf-annotate-edit-overlay')
  overlay.setAttribute('data-target-id', id)
  overlay.style.boxSizing = 'content-box'
  overlay.style.position = 'absolute'
  overlay.style.top = `${styleTop}px`
  overlay.style.left = `${styleLeft}px`
  overlay.style.width = `${rect.width}px`
  overlay.style.height = `${rect.height}px`
  overlay.style.border = `${OVERLAY_BORDER_SIZE}px solid ${BORDER_COLOR}`
  overlay.style.borderRadius = `${OVERLAY_BORDER_SIZE}px`

  // anchor.innerHTML = '×'
  // anchor.setAttribute('href', 'javascript://')
  // anchor.style.background = '#fff'
  // anchor.style.borderRadius = '20px'
  // anchor.style.border = '1px solid #bbb'
  // anchor.style.color = '#bbb'
  // anchor.style.fontSize = '16px'
  // // anchor.style.padding = '2px'
  // anchor.style.textAlign = 'center'
  // anchor.style.textDecoration = 'none'
  // anchor.style.position = 'absolute'
  // anchor.style.top = '-13px'
  // anchor.style.right = '-13px'
  // anchor.style.width = '20px'
  // anchor.style.height = '20px'
  // anchor.style.lineHeight = 1.3

  // overlay.appendChild(anchor)
  parentNode.appendChild(overlay)
  document.addEventListener('click', handleDocumentClick)
  document.addEventListener('keyup', handleDocumentKeyup)
  document.addEventListener('mousedown', handleDocumentMousedown)
  // anchor.addEventListener('click', deleteAnnotation)
  // anchor.addEventListener('mouseover', () => {
  //   anchor.style.color = '#35A4DC'
  //   anchor.style.borderColor = '#999'
  //   anchor.style.boxShadow = '0 1px 1px #ccc'
  // })
  // anchor.addEventListener('mouseout', () => {
  //   anchor.style.color = '#bbb'
  //   anchor.style.borderColor = '#bbb'
  //   anchor.style.boxShadow = ''
  // })
  // overlay.addEventListener('mouseover', () => {
  //   if (!isDragging) { anchor.style.display = '' }
  // })
  // overlay.addEventListener('mouseout', () => {
  //   anchor.style.display = 'none'
  // })
}

/**
 * Destroy the edit overlay if it exists.
 */
function destroyEditOverlay() {
  const overlayDoc = document.getElementById('pdf-annotate-edit-overlay')
  if (overlayDoc && typeof (overlayDoc) != 'undefined') {
    overlayDoc.parentNode.removeChild(overlayDoc)
    overlay = null
  }

  document.removeEventListener('click', handleDocumentClick)
  document.removeEventListener('keyup', handleDocumentKeyup)
  document.removeEventListener('mousedown', handleDocumentMousedown)
  document.removeEventListener('mousemove', handleDocumentMousemove)
  document.removeEventListener('mouseup', handleDocumentMouseup)
  enableUserSelect()
}

/**
 * Delete currently selected annotation
 */
function deleteAnnotation() {
  if (!overlay) { return }

  const annotationId = overlay.getAttribute('data-target-id')
  const nodes = document.querySelectorAll(`[data-pdf-annotate-id="${annotationId}"]`)
  const svg = overlay.parentNode.querySelector('svg.annotationLayer')
  const { documentId } = getMetadata(svg);

  [...nodes].forEach((n) => {
    n.parentNode.removeChild(n)
  })

  PDFJSAnnotate.getStoreAdapter().deleteAnnotation(documentId, annotationId)

  destroyEditOverlay()
}
export function deleteAnnotations() {
  const overlayDoc = document.getElementById('pdf-annotate-edit-overlay')
  if (!overlayDoc) { return }

  const annotationId = overlayDoc.getAttribute('data-target-id')
  const nodes = document.querySelectorAll(`[data-pdf-annotate-id="${annotationId}"]`)
  const svg = overlayDoc.parentNode.querySelector('svg.annotationLayer')
  const { documentId } = getMetadata(svg);

  [...nodes].forEach((n) => {
    n.parentNode.removeChild(n)
  })

  PDFJSAnnotate.getStoreAdapter().deleteAnnotation(documentId, annotationId)

  destroyEditOverlay()
}
/**
 * Handle document.click event
 *
 * @param {Event} e The DOM event that needs to be handled
 */
function handleDocumentClick(e) {
  if (!findSVGAtPoint(e.clientX, e.clientY)) { return }

  // Remove current overlay
  const overlay = document.getElementById('pdf-annotate-edit-overlay')
  if (overlay) {
    if (isDragging || e.target === overlay) {
      return
    }
    const rectTool = document.getElementById('rectTool')
    if (rectTool.style.position != 'absolute' || (rectTool.style.position == 'absolute' && rectTool.style.visibility == 'hidden')) {
      destroyEditOverlay()
    }
  }
}

/**
 * Handle document.keyup event
 *
 * @param {Event} e The DOM event that needs to be handled
 */
function handleDocumentKeyup(e) {
  if (overlay && e.keyCode === 46 &&
      e.target.nodeName.toLowerCase() !== 'textarea' &&
      e.target.nodeName.toLowerCase() !== 'input') {
    deleteAnnotation()
  }
}

/**
 * Handle document.mousedown event
 *
 * @param {Event} e The DOM event that needs to be handled
 */
function handleDocumentMousedown(e) {
  if (e.target !== overlay) { return }

  const annotationId = overlay.getAttribute('data-target-id')
  parentNode = overlay.parentNode
  const target = document.querySelector(`[data-pdf-annotate-id="${annotationId}"]`)
  const type = target.getAttribute('data-pdf-annotate-type')

  // const { viewport } = getMetadata(svgEdit)

  if (type === 'highlight' || type === 'strikeout') { return }
  if (type == 'textbox') {
    isDragging = true
    dragOffsetX = e.clientX
    dragOffsetY = e.clientY
    dragStartX = overlay.offsetLeft
    dragStartY = overlay.offsetTop

    overlay.style.background = 'rgba(255, 255, 255, 0.7)'
    overlay.style.cursor = 'move'
    // overlay.querySelector('a').style.display = 'none'
    const svg = overlay.parentNode.querySelector('svg.annotationLayer')
    const { viewport } = getMetadata(svg)
    const targetAnno = document.querySelector(`[data-pdf-annotate-id="${annotationId}"]`)
    if (targetAnno.childNodes.length > 1) {
      targetAnno.removeChild(targetAnno.childNodes[1])
    }

    const pre = targetAnno.firstChild

    pre.style.fontSize = 12 * viewport.scale + 'px'
    overlay.appendChild(pre)
    startScroll = document.getElementById('viewerContainer').scrollTop
    document.getElementById('viewerContainer').addEventListener('scroll', handleDocumentScroll)
    document.addEventListener('mousemove', handleDocumentMousemove)
    document.addEventListener('mouseup', handleDocumentMouseup)

    disableUserSelect()
  } else if (type == 'area' || type == 'comment-area' || type === 'point') {
    isDragging = true
    dragOffsetX = e.clientX
    dragOffsetY = e.clientY
    dragStartX = overlay.offsetLeft
    dragStartY = overlay.offsetTop

    overlay.style.background = 'rgba(255, 255, 255, 0.7)'
    overlay.style.cursor = 'move'
    // overlay.querySelector('a').style.display = 'none'
    const svg = overlay.parentNode.querySelector('svg.annotationLayer')
    const { viewport } = getMetadata(svg)
    if (type == 'point') {
      const trans = getTranslation(viewport)
      target.setAttribute('transform', `scale(1) rotate(${viewport.rotation}) translate(${trans.x}, ${trans.y})`)
    }
    overlay.appendChild(target)
    console.log('target', target)
    document.getElementById('viewerContainer').addEventListener('scroll', handleDocumentScroll)
    document.addEventListener('mousemove', handleDocumentMousemove)
    document.addEventListener('mouseup', handleDocumentMouseup)
    disableUserSelect()
  }
}

/**
 * Handle document.mousemove event
 *
 * @param {Event} e The DOM event that needs to be handled
 */
function handleDocumentMousemove(e) {
  const rect = parentNode.getBoundingClientRect()
  const y = (dragStartY + (e.clientY - dragOffsetY))
  const x = (dragStartX + (e.clientX - dragOffsetX))
  dragOffsetY = e.clientY
  dragOffsetX = e.clientX
  const minY = 0
  const maxY = rect.height
  const minX = 0
  const maxX = rect.width
  if (y > minY && y + overlay.offsetHeight < maxY) {
    overlay.style.top = `${y}px`
    dragStartY = y
  }

  if (x > minX && x + overlay.offsetWidth < maxX) {
    overlay.style.left = `${x}px`
    dragStartX = x
  }
}
function handleDocumentScroll(e) {
  if (isDragging) {
    const scrollDistance = document.getElementById('viewerContainer').scrollTop - startScroll
    const rect = parentNode.getBoundingClientRect()
    const y = dragStartY + scrollDistance
    const x = dragStartX
    const minY = 0
    const maxY = rect.height
    const minX = 0
    const maxX = rect.width
    if (y > minY && y + overlay.offsetHeight < maxY) {
      overlay.style.top = `${y}px`
      dragStartY = y
    }

    if (x > minX && x + overlay.offsetWidth < maxX) {
      overlay.style.left = `${x}px`
      dragStartX = x
    }
    startScroll = document.getElementById('viewerContainer').scrollTop
  }
}

/**
 * Handle document.mouseup event
 *
 * @param {Event} e The DOM event that needs to be handled
 */
function handleDocumentMouseup(e) {
  const annotationId = overlay.getAttribute('data-target-id')
  const target = document.querySelectorAll(`[data-pdf-annotate-id="${annotationId}"]`)
  const type = target[0].getAttribute('data-pdf-annotate-type')
  const svg = overlay.parentNode.querySelector('svg.annotationLayer')
  const { documentId, viewport } = getMetadata(svg)

  // overlay.querySelector('a').style.display = ''

  function getDelta(propX, propY) {
    return calcDelta(parseInt(target[0].getAttribute(propX), 10), parseInt(target[0].getAttribute(propY), 10))
  }

  function calcDelta(x, y) {
    return {
      deltaX: OVERLAY_BORDER_SIZE + scaleDown(svg, { x: overlay.offsetLeft }).x - x,
      deltaY: OVERLAY_BORDER_SIZE + scaleDown(svg, { y: overlay.offsetTop }).y - y
    }
  }

  PDFJSAnnotate.getStoreAdapter().getAnnotation(documentId, annotationId).then(async(annotation) => {
    const oldAnnotation = Object.assign({}, annotation)
    if (['textbox'].indexOf(type) > -1) {
      const { deltaX, deltaY } = getDelta('x', 'y');
      [...target].forEach((t, i) => {
        if (deltaY !== 0) {
          const modelY = parseInt(t.getAttribute('y'), 10) + deltaY
          const viewY = modelY

          t.setAttribute('y', viewY)
          if (annotation.rectangles) {
            annotation.rectangles[i].y = modelY
          } else if (annotation.y) {
            annotation.y = modelY
          }
        }
        if (deltaX !== 0) {
          const modelX = parseInt(t.getAttribute('x'), 10) + deltaX
          const viewX = modelX

          t.setAttribute('x', viewX)
          if (annotation.rectangles) {
            annotation.rectangles[i].x = modelX
          } else if (annotation.x) {
            annotation.x = modelX
          }
        }
      })
      // } else if (type === 'strikeout') {
      //   let { deltaX, deltaY } = getDelta('x1', 'y1');
      //   [...target].forEach(target, (t, i) => {
      //     if (deltaY !== 0) {
      //       t.setAttribute('y1', parseInt(t.getAttribute('y1'), 10) + deltaY);
      //       t.setAttribute('y2', parseInt(t.getAttribute('y2'), 10) + deltaY);
      //       annotation.rectangles[i].y = parseInt(t.getAttribute('y1'), 10);
      //     }
      //     if (deltaX !== 0) {
      //       t.setAttribute('x1', parseInt(t.getAttribute('x1'), 10) + deltaX);
      //       t.setAttribute('x2', parseInt(t.getAttribute('x2'), 10) + deltaX);
      //       annotation.rectangles[i].x = parseInt(t.getAttribute('x1'), 10);
      //     }
      //   });
    } else if (type == 'area') {
      const { deltaX, deltaY } = getDelta('x', 'y');
      [...target].forEach((t, i) => {
        if (deltaY !== 0) {
          const modelY = parseInt(t.getAttribute('y'), 10) + deltaY
          const viewY = modelY

          t.setAttribute('y', viewY)
          t.setAttribute('top', viewY)
          if (annotation.rectangles) {
            annotation.rectangles[i].y = modelY
          } else if (annotation.y) {
            annotation.y = modelY
            annotation.top = modelY
          }
        }
        if (deltaX !== 0) {
          const modelX = parseInt(t.getAttribute('x'), 10) + deltaX
          const viewX = modelX

          t.setAttribute('x', viewX)
          t.setAttribute('left', viewX)
          if (annotation.rectangles) {
            annotation.rectangles[i].x = modelX
          } else if (annotation.x) {
            annotation.x = modelX
            annotation.left = modelX
          }
        }
      })
    } else if (type == 'point') {
      const { deltaX, deltaY } = getDelta('x', 'y');
      [...target].forEach((t, i) => {
        if (deltaY !== 0) {
          const modelY = parseInt(t.getAttribute('y'), 10) + deltaY
          const viewY = modelY

          t.setAttribute('y', viewY * viewport.scale)
          t.setAttribute('top', viewY)
          if (annotation.rectangles) {
            annotation.rectangles[i].y = modelY
          } else if (annotation.y) {
            annotation.y = modelY
            annotation.top = modelY
          }
        }
        if (deltaX !== 0) {
          const modelX = parseInt(t.getAttribute('x'), 10) + deltaX
          const viewX = modelX

          t.setAttribute('x', viewX * viewport.scale)
          t.setAttribute('left', viewX)
          if (annotation.rectangles) {
            annotation.rectangles[i].x = modelX
          } else if (annotation.x) {
            annotation.x = modelX
            annotation.left = modelX
          }
        }
      })
    } else if (type == 'comment-area') {
      const { deltaX, deltaY } = getDelta('x', 'y');
      [...target].forEach((t, i) => {
        if (deltaY !== 0) {
          const modelY = parseInt(t.getAttribute('y'), 10) + deltaY
          const viewY = modelY

          t.setAttribute('y', viewY)
          if (annotation.rectangles) {
            annotation.rectangles[i].y = modelY
          } else if (annotation.y) {
            annotation.y = modelY
            annotation.top = modelY
          }
        }
        if (deltaX !== 0) {
          const modelX = parseInt(t.getAttribute('x'), 10) + deltaX
          const viewX = modelX

          t.setAttribute('x', viewX)
          if (annotation.rectangles) {
            annotation.rectangles[i].x = modelX
          } else if (annotation.x) {
            annotation.x = modelX
            annotation.left = modelX
          }
        }
      })
    } else if (type === 'drawing') {
      const rect = scaleDown(svg, getAnnotationRect(target[0]))
      const [originX, originY] = annotation.lines[0]
      let { deltaX, deltaY } = calcDelta(originX, originY)

      // origin isn't necessarily at 0/0 in relation to overlay x/y
      // adjust the difference between overlay and drawing coords
      deltaY += (originY - rect.top)
      deltaX += (originX - rect.left)

      annotation.lines.forEach((line, i) => {
        const [x, y] = annotation.lines[i]
        annotation.lines[i][0] = x + deltaX
        annotation.lines[i][1] = y + deltaY
      })

      target[0].parentNode.removeChild(target[0])
      appendChild(svg, annotation)
    }

    await PDFJSAnnotate.getStoreAdapter().editAnnotation(documentId, annotationId, annotation, undefined, oldAnnotation)
    if (type == 'comment-area' || type == 'point') {
      fireEvent('comment:updateCommentPositionAfterEditAnnotation', annotationId)
    }
  })
  if (type == 'textbox') {
    overlay.firstChild.style.fontSize = '12px'
    document.querySelectorAll(`[data-pdf-annotate-id="${annotationId}"]`)[0].appendChild(overlay.firstChild)
  } else if (type == 'area' || type == 'comment-area' || type == 'point') {
    svg.appendChild(overlay.firstChild)
  }

  setTimeout(() => {
    isDragging = false
  }, 0)

  overlay.style.background = ''
  overlay.style.cursor = ''
  document.getElementById('viewerContainer').addEventListener('scroll', handleDocumentScroll)
  document.removeEventListener('mousemove', handleDocumentMousemove)
  document.removeEventListener('mouseup', handleDocumentMouseup)
  enableUserSelect()
}

/**
 * Handle annotation.click event
 *
 * @param {Element} e The annotation element that was clicked
 */
function handleAnnotationClick(target) {
  const newCommentWrapper = document.getElementById('add-new-comment')
  if (newCommentWrapper && newCommentWrapper.style.display != 'none') {
    return
  }
  createEditOverlay(target)
}

/**
 * Enable edit mode behavior.
 */
export function enableEdit() {
  if (_enabled) { return }

  _enabled = true
  addEventListener('annotation:click', handleAnnotationClick)
}
/**
 * Enable edit mode behavior.
 */
export function editTextBox(e, svg) {
  const annotationId = e.getAttribute('data-pdf-annotate-id')
  const target = document.querySelector(`[data-pdf-annotate-id="${annotationId}"]`)
  destroyEditOverlay()
  editingAnnotation = target
  svgEdit = svg
  target.style.visibility = 'hidden'
  const { documentId, pageNumber, viewport } = getMetadata(svgEdit)
  const page = document.getElementById('pageContainer' + `${pageNumber}`)
  const rect = page.getBoundingClientRect()
  const x = target.getAttribute('x') * viewport.scale
  const w = rect.width - x
  const y = target.getAttribute('y') * viewport.scale

  const _textColor = parseInt(window.getComputedStyle(target.firstChild).color, 10)
  const _textSize = parseInt(window.getComputedStyle(target.firstChild).fontSize, 10)

  input = document.createElement('pre')
  input.setAttribute('id', 'pdf-annotate-text-input')
  input.style.maxWidth = `${w}px`
  input.setAttribute('contenteditable', true)
  input.setAttribute('id', 'pdf-annotate-text-input')
  input.setAttribute('display', 'block')
  input.setAttribute('onfocus', "document.execCommand('selectAll',false,null)")
  input.style.outline = 'none'
  input.style.position = 'absolute'
  input.style.color = localStorage.getItem(`${documentId}/color`) || _textColor
  input.style.fontSize = `${_textSize * viewport.scale}px`

  input.style.top = `${y}px`
  input.style.left = `${x}px`

  input.addEventListener('blur', handleInputBlur)
  input.addEventListener('keyup', handleInputKeyup)
  input.addEventListener('keydown', handleInputKeydown)

  document.querySelector(`div[data-page-number='${pageNumber}']`).appendChild(input)

  input.focus()
  selectText('pdf-annotate-text-input')
  input.innerHTML = target.firstChild.innerHTML

  isEditing = true
}

function selectText(id) {
  var sel, range
  var el = document.getElementById(id) // get element id
  if (window.getSelection && document.createRange) { // Browser compatibility
	  sel = window.getSelection()
	  if (sel.toString() == '') { // no text selection
		 window.setTimeout(function() {
        range = document.createRange() // range object
        range.selectNodeContents(el) // sets Range
        sel.removeAllRanges() // remove all ranges from selection
        sel.addRange(range)// add Range to a Selection.
      }, 1)
	  }
  } else if (document.selection) { // older ie
    sel = document.selection.createRange()
    if (sel.text == '') { // no text selection
      range = document.body.createTextRange()// Creates TextRange object
      range.moveToElementText(el)// sets Range
      range.select() // make selection.
    }
  }
}

/**
 * Disable edit mode behavior.
 */
export function disableEdit() {
  destroyEditOverlay()

  if (!_enabled) { return }

  _enabled = false
  removeEventListener('annotation:click', handleAnnotationClick)
}
async function handleInputBlur(e) {
  if (editingAnnotation && editingAnnotation.firstChild.textContent != input.textContent) {
    if (!svgEdit) { svgEdit = findSVGAtPoint(parseInt(input.getAttribute('left'), 10), parseInt(input.getAttribute('top'), 10)) }

    const { documentId, viewport } = getMetadata(svgEdit)
    const uuid = editingAnnotation.getAttribute('data-pdf-annotate-id')
    await PDFJSAnnotate.getStoreAdapter().getAnnotation(documentId, uuid).then(async rs => {
      const previousAnno = Object.assign({}, rs)
      rs.content = e.target.innerText

      for (let i = 0; i < rs.content.length - 1; i++) {
        if (rs.content[i] == '\n' && rs.content[i + 1] == '\n') {
          var tempStr = rs.content.slice(0, i)
          tempStr = tempStr + ' '
          tempStr = tempStr + rs.content.slice(i + 1, rs.content.length)
          rs.content = tempStr
          i++
        }
      }

      rs.width = scaleDown(svgEdit, {
        width: e.target.clientWidth,
        height: e.target.clientHeight
      }).width

      rs.height = scaleDown(svgEdit, {
        width: e.target.clientWidth,
        height: e.target.clientHeight
      }).height

      editingAnnotation.setAttribute('width', rs.width + 5 + 'px')
      editingAnnotation.setAttribute('height', rs.height + 'px')

      await PDFJSAnnotate.getStoreAdapter().editAnnotation(documentId, uuid, rs, undefined, previousAnno).then(r => {
        editingAnnotation.firstChild.innerText = e.target.innerText
      })
    })
  }
  closeInput()
}
/**
 * Handle input.keyup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleInputKeyup(e) {
  if (e.keyCode === 27) {
    closeInput()
  }
}

function handleInputKeydown(e) {

}

function closeInput() {
  if (input) {
    if (!svgEdit) { svgEdit = findSVGAtPoint(parseInt(input.getAttribute('left'), 10), parseInt(input.getAttribute('top'), 10)) }
    input.removeEventListener('blur', handleInputBlur)
    input.removeEventListener('keyup', handleInputKeyup)
    const { pageNumber } = getMetadata(svgEdit)
    document.querySelector(`div[data-page-number='${pageNumber}']`).removeChild(input)
    svgEdit = null
    input = null
    editingAnnotation.style.visibility = 'visible'
    editingAnnotation = null
    isEditing = false
  }
}
function getTranslation(viewport) {
  let x
  let y

  // Modulus 360 on the rotation so that we only
  // have to worry about four possible values.
  switch (viewport.rotation % 360) {
    case 0:
      x = y = 0
      break
    case 90:
      x = 0
      y = (viewport.width / viewport.scale) * -1
      break
    case 180:
      x = (viewport.width / viewport.scale) * -1
      y = (viewport.height / viewport.scale) * -1
      break
    case 270:
      x = (viewport.height / viewport.scale) * -1
      y = 0
      break
  }

  return { x, y }
}
