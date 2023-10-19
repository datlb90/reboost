/* eslint-disable no-empty */
/* eslint-disable no-unused-vars */
import PDFJSAnnotate from '../PDFJSAnnotate'
import appendChild from '../render/appendChild'
import { addEventListener } from './event'
import {
  BORDER_COLOR,
  disableUserSelect,
  enableUserSelect,
  findSVGAtPoint,
  getMetadata,
  getOffset,
  scaleDown,
  scaleUp,
  getAnnotationRect
} from './utils'

let _enabled = false
let _type
let overlay
let originY
let originX
let _rects
let _svg
let _target
let _annotation
let _selectedText
let _topPos
let _endPos
let _inputHeight
let _order
/**
 * Get the current window selection as rects
 *
 * @return {Array} An Array of rects
 */
function getSelectionRects() {
  try {
    const selection = window.getSelection()
    const range = selection.getRangeAt(0)
    const rects = range.getClientRects()

    if (rects.length > 0 &&
        rects[0].width > 0 &&
        rects[0].height > 0) {
      return rects
    }
  } catch (e) {}

  return null
}

/**
 * Handle document.mousedown event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMousedown(e) {
  let svg
  if (_type === 'select' || !(svg = findSVGAtPoint(e.clientX, e.clientY))) {
    // hide text bool buttons
    if (!e.target.classList.contains('textToolBtn') &&
        !e.target.classList.contains('svg-inline--fa') &&
        !e.target.parentElement.classList.contains('svg-inline--fa')) {
      const textTool = document.getElementById('textTool')
      textTool.style.visibility = 'hidden'
    }
    // Hide add new comment form
    const newCommentWrapper = document.getElementById('add-new-comment')
    if (newCommentWrapper && newCommentWrapper.style.display != 'none') {
      const validTarget = hasAParentWithClass(e.target, 'add-new-comment')
      const comment = document.getElementById('comment-text-area').value
      if (!validTarget && comment.replace(/\s/g, '').length == 0) { cancelCommentText() }
    }
    // Unselect highlight comment
    if (!e.target.classList.contains('comment-highlight-selected')) {
      const validTarget = hasAParentWithClass(e.target, 'comment-card-selected')
      if (!validTarget) { unselectHighlightComment() }
    }
    return
  }

  const rect = svg.getBoundingClientRect()
  originY = e.clientY
  originX = e.clientX

  overlay = document.createElement('div')
  overlay.style.position = 'absolute'
  overlay.style.top = `${originY - rect.top}px`
  overlay.style.left = `${originX - rect.left}px`
  overlay.style.border = `2px solid ${BORDER_COLOR}`
  overlay.style.borderRadius = '3px'
  svg.parentNode.appendChild(overlay)

  document.addEventListener('mousemove', handleDocumentMousemove)
  disableUserSelect()
}

function hasAParentWithClass(element, className) {
  for (let i = 0; i <= 5; i++) {
    if (element.classList.contains(className)) { return true } else { element = element.parentNode }
  }
  return false
  // if (element.classList.contains(className)) return true;
  // return element.parentNode && hasAParentWithClass(element.parentNode, className);
}

/**
 * Handle document.mousemove event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMousemove(e) {
  const svg = overlay.parentNode.querySelector('svg.annotationLayer')
  const rect = svg.getBoundingClientRect()

  if (originX + (e.clientX - originX) < rect.right) {
    overlay.style.width = `${e.clientX - originX}px`
  }

  if (originY + (e.clientY - originY) < rect.bottom) {
    overlay.style.height = `${e.clientY - originY}px`
  }
}

/**
 * Handle document.mouseup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMouseup(e) {
  if (e.target.parentElement.classList.contains('textLayer')) {
    let rects
    if (_type === 'select' && (rects = getSelectionRects())) {
      const text = window
        .getSelection()
        .toString()
        .trim()
      // Display text tool bar if text is selected.
      if (text) {
        const textTool = document.getElementById('textTool')
        const posX = rects[rects.length - 1].left + (rects[rects.length - 1].width / 2)
        const posY = rects[rects.length - 1].top + 20
        textTool.style =
          'position: absolute; top: ' +
          posY +
          'px; left: ' +
          posX +
          'px;'

        _rects = rects
        _svg = findSVGAtPoint(rects[0].left, rects[0].top)
        _target = e.target
        _selectedText = text
      } else {
        // Display different tool bar
      }
    }
  }
}

/**
 * Handle document.keyup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentKeyup(e) {
  // Cancel rect if Esc is pressed
  if (e.keyCode === 27) {
    const selection = window.getSelection()
    selection.removeAllRanges()
    if (overlay && overlay.parentNode) {
      overlay.parentNode.removeChild(overlay)
      overlay = null
      document.removeEventListener('mousemove', handleDocumentMousemove)
    }
  }
}

/**
 * Save a highlight annotation
 */
export async function highlightText(type) {
  const svg = _svg // findSVGAtPoint(rects[0].left, rects[0].top);
  const rects = _rects

  if (!svg) {
    return
  }

  const boundingRect = svg.getBoundingClientRect()

  let color = 'FFFF00'
  if (type == 'strikeout') { color = 'FF0000' }
  // Initialize the annotation
  const annotation = {
    type,
    color,
    rectangles: [...rects].map((r) => {
      let offset = 0
      if (type == 'strikeout') { offset = r.height / 2 }
      return scaleDown(svg, {
        y: (r.top + offset) - boundingRect.top,
        x: r.left - boundingRect.left,
        width: r.width,
        height: r.height
      })
    }).filter((r) => r.width > 0 && r.height > 0 && r.x > -1 && r.y > -1)
  }

  // Short circuit if no rectangles exist
  if (annotation.rectangles.length === 0) {
    return
  }

  if (annotation.rectangles.length > 2) {
    annotation.rectangles = removeMinHeigthRects(annotation.rectangles)
  }

  const { documentId, pageNumber } = getMetadata(svg)

  annotation.top = parseInt(_target.style.top.substring(0, _target.style.top.length - 2))
  annotation.pageNum = pageNumber
  annotation.pageHeight = svg.getAttribute('height')
  _annotation = annotation

  // Add the annotation
  await PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
    .then((annotation) => {
      appendChild(svg, annotation)
      hideTextToolBar()
    })
}

function removeMinHeigthRects(arr) {
  const min = Math.min(...arr.map(a => a.height))
  return arr.filter(a => a.height != min)
}

function hideTextToolBar() {
  const textTool = document.getElementById('textTool')
  textTool.style.visibility = 'hidden'
}

function updateCommentPositions() {
  const commentCards = document.querySelectorAll('.comment-card')
  const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
  const highlightArr = Array.prototype.slice.call(commentHighlights)
  // Sort the highlights
  highlightArr.sort(compareTopAttributes)
  // Get order of the current highlight
  _order = highlightArr.findIndex(item => {
    return item.dataset.pdfAnnotateId == _annotation.uuid
  })

  if (!hasEnoughSpace(commentCards)) {
    // Update comment positions

    // get add new form's top position
    const svg = _svg
    const rectTop = parseInt(_target.style.top.substring(0, _target.style.top.length - 2))
    const svgHeight = svg.getAttribute('height')
    const svgPageNum = svg.getAttribute('data-pdf-annotate-page')
    let svgTop = 0
    if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
    const topPos = svgTop + rectTop - 35
    const endPos = topPos + 105

    if (_order == 0) {
      // Move down all comment cards
      // moveDownCommentCards(commentCards, 0, commentCards.length - 1, topPos, endPos);
      moveDownFromEndPos(commentCards, 0, endPos)
    } else if (_order == highlightArr.length - 1) {
      // Move up all comment cards
      // moveUpCommentCards(commentCards, commentCards.length - 1, 0, topPos, endPos);
      moveUpFromTopPos(commentCards, commentCards.length - 1, topPos)
    } else {
      // Move up commentCards[0] -> commentCards[i - 1]
      // moveUpCommentCards(commentCards, _order - 1, 0, topPos, endPos)
      const upperTop = parseInt(commentCards[_order - 1].style.top.substring(0, commentCards[_order - 1].style.top.length - 2))
      const upperEnd = upperTop + commentCards[_order - 1].offsetHeight
      // Check if move up is necessary
      if (upperEnd > (topPos - 10)) {
        moveUpFromTopPos(commentCards, _order - 1, topPos)
      } else {
        // Check if move down is necessary
        const commentId = commentCards[_order - 1].getAttribute('highlight-id')
        const g = getHighlightByCommentId(commentId)
        let gTop = parseInt(g.getAttribute('top')) - 35
        if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
        if (upperTop < gTop) {
          moveDownToTopPos(commentCards, _order - 1, topPos)
        }
      }
      // Move down commentCards[i] -> commentCards[commentCards.length - 1]
      // moveDownCommentCards(commentCards, _order, commentCards.length, topPos, endPos);
      const lowerTop = parseInt(commentCards[_order].style.top.substring(0, commentCards[_order].style.top.length - 2))
      if (lowerTop < (endPos + 10)) {
        moveDownFromEndPos(commentCards, _order, endPos)
      } else {
        // Check if move up is necessary
        const commentId = commentCards[_order].getAttribute('highlight-id')
        const g = getHighlightByCommentId(commentId)
        let gTop = parseInt(g.getAttribute('top')) - 35
        if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
        if (lowerTop > gTop) {
          moveUpToEndPos(commentCards, _order, endPos)
        }
      }
    }
  }
}

export function updatePositionsAfterCommentAdded() {
  const newComment = document.querySelector(".comment-card[highlight-id='" + _annotation.uuid + "']")
  const commentCards = document.querySelectorAll('.comment-card')
  const endPos = parseInt(newComment.getAttribute('top-position')) + newComment.offsetHeight
  if (_order < (commentCards.length - 1)) { moveUpToEndPos(commentCards, _order + 1, endPos) }
  newComment.addEventListener('click', handleCommentCardClick)
}

export function updatePositionsAfterCommentDeleted() {

}

function moveUpFromTopPos(commentCards, startIndex, topPos) {
  // Get start index card's position
  const adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2))
  const adjEnd = adjTop + commentCards[startIndex].offsetHeight
  // Move the start index card up first
  commentCards[startIndex].style.top = adjTop - (adjEnd - topPos) - 10 + 'px'
  commentCards[startIndex].setAttribute('top-position', adjTop - (adjEnd - topPos) - 10)
  if (startIndex > 0) {
    for (let m = startIndex - 1; m >= 0; m--) {
      const previousTop = parseInt(commentCards[m + 1].style.top.substring(0, commentCards[m + 1].style.top.length - 2))
      const thisTop = parseInt(commentCards[m].style.top.substring(0, commentCards[m].style.top.length - 2))
      const thisEnd = thisTop + commentCards[m].offsetHeight
      if (thisEnd > (previousTop - 10)) {
        commentCards[m].style.top = thisTop - (thisEnd - previousTop) - 10 + 'px'
        commentCards[m].setAttribute('top-position', thisTop - (thisEnd - previousTop) - 10)
      } else {
        break
      }
    }
  }
}

function moveUpToEndPos(commentCards, startIndex, endPos) {
  const commentId = commentCards[startIndex].getAttribute('highlight-id')
  const g = getHighlightByCommentId(commentId)
  let gTop = parseInt(g.getAttribute('top')) - 35
  if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }

  const desiredTop = gTop >= (endPos + 10) ? gTop : (endPos + 10)
  // Move start index card up
  commentCards[startIndex].style.top = desiredTop + 'px'
  commentCards[startIndex].setAttribute('top-position', desiredTop)

  // let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
  // commentCards[startIndex].style.top = adjTop - (adjTop - endPos) + 10 + "px";
  // commentCards[startIndex].setAttribute("top-position", adjTop - (adjTop - endPos) + 10);

  if (commentCards.length > (startIndex + 1)) {
    for (let n = startIndex + 1; n < commentCards.length; n++) {
      const previousTop = parseInt(commentCards[n - 1].style.top.substring(0, commentCards[n - 1].style.top.length - 2))
      const previousEnd = previousTop + commentCards[n - 1].offsetHeight
      const thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2))

      // Check if move down is necessary
      if (thisTop > (previousEnd + 10)) {
        const thisCommentId = commentCards[n].getAttribute('highlight-id')
        const thisG = getHighlightByCommentId(thisCommentId)
        let thisGTop = parseInt(thisG.getAttribute('top')) - 35
        if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * thisG.getAttribute('page-height')) }
        if (thisTop > thisGTop) {
          const thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10)
          commentCards[n].style.top = thisDesiredTop + 'px'
          commentCards[n].setAttribute('top-position', thisDesiredTop)
        }
      } else {
        break
      }
    }
  }
}

function moveDownToTopPos(commentCards, startIndex, topPos) {
  // Get start index card's position
  const adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2))
  const adjEnd = adjTop + commentCards[startIndex].offsetHeight
  // Check if move down is necessary
  const commentId = commentCards[startIndex].getAttribute('highlight-id')
  const g = getHighlightByCommentId(commentId)
  let gTop = parseInt(g.getAttribute('top')) - 35
  if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
  // Only move cards down if the upper card is higher that its highlight
  if (adjTop < gTop) {
    const desiredTop = gTop <= (adjTop + (topPos - adjEnd) - 10) ? gTop : (adjTop + (topPos - adjEnd) - 10)
    // Move upper adjacent card down
    commentCards[startIndex].style.top = desiredTop + 'px'
    commentCards[startIndex].setAttribute('top-position', desiredTop)
    // Move other cards down
    if (startIndex > 0) {
      for (let n = startIndex - 1; n >= 0; n--) {
        const previousTop = parseInt(commentCards[n + 1].style.top.substring(0, commentCards[n + 1].style.top.length - 2))
        // let previousEnd = previousTop + commentCards[n + 1].offsetHeight;
        const thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2))
        const thisEnd = thisTop + commentCards[n].offsetHeight
        // Check if move down is necessary
        if (thisEnd < (previousTop - 10)) {
          const thisCommentId = commentCards[n].getAttribute('highlight-id')
          const thisG = getHighlightByCommentId(thisCommentId)
          let thisGTop = parseInt(thisG.getAttribute('top')) - 35
          if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * thisG.getAttribute('page-height')) }
          if (thisTop < thisGTop) {
            const thisDesiredTop = thisGTop <= (thisTop + (previousTop - thisEnd) - 10) ? thisGTop : (thisTop + (previousTop - thisEnd) - 10)
            commentCards[n].style.top = thisDesiredTop + 'px'
            commentCards[n].setAttribute('top-position', thisDesiredTop)
          }
        } else {
          break
        }
      }
    }
  }
}

function moveDownFromEndPos(commentCards, startIndex, endPos) {
  // Move the start index card down first
  // let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
  commentCards[startIndex].style.top = endPos + 10 + 'px'
  commentCards[startIndex].setAttribute('top-position', endPos + 10)
  // Move other cards down ward
  if (commentCards.length > (startIndex + 1)) {
    for (let j = startIndex + 1; j < commentCards.length; j++) {
      const previousTop = parseInt(commentCards[j - 1].style.top.substring(0, commentCards[j - 1].style.top.length - 2))
      const previousEnd = previousTop + commentCards[j - 1].offsetHeight
      const thisTop = parseInt(commentCards[j].style.top.substring(0, commentCards[j].style.top.length - 2))
      if (thisTop < (previousEnd + 10)) {
        commentCards[j].style.top = thisTop + (previousEnd - thisTop) + 10 + 'px'
        commentCards[j].setAttribute('top-position', thisTop + (previousEnd - thisTop) + 10)
      } else {
        break
      }
    }
  }
}

function moveUpCommentCards(commentCards, startIndex, endIndex, topPos, endPos) {
  // Get adjacent card's position
  const adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2))
  const adjEnd = adjTop + commentCards[startIndex].offsetHeight
  // Check if move up is necessary
  if (adjEnd > (topPos - 10)) {
    // Move the upper adjacent card up first
    commentCards[startIndex].style.top = adjTop - (adjEnd - topPos) - 10 + 'px'
    commentCards[startIndex].setAttribute('top-position', adjTop - (adjEnd - topPos) - 10)
    // Move other cards upward
    if (startIndex > 0) {
      for (let m = startIndex - 1; m >= 0; m--) {
        const previousTop = parseInt(commentCards[m + 1].style.top.substring(0, commentCards[m + 1].style.top.length - 2))
        const thisTop = parseInt(commentCards[m].style.top.substring(0, commentCards[m].style.top.length - 2))
        const thisEnd = thisTop + commentCards[m].offsetHeight
        if (thisEnd > (previousTop - 10)) {
          commentCards[m].style.top = thisTop - (thisEnd - previousTop) - 10 + 'px'
          commentCards[m].setAttribute('top-position', thisTop - (thisEnd - previousTop) - 10)
        } else {
          break
        }
      }
    }
  } else {
    // Check if move down is necessary
    const commentId = commentCards[startIndex].getAttribute('highlight-id')
    const g = getHighlightByCommentId(commentId)
    const gTop = parseInt(g.getAttribute('top')) - 35

    if (adjTop < gTop) {
      const desiredTop = gTop <= (adjTop + (topPos - adjEnd) - 10) ? gTop : (adjTop + (topPos - adjEnd) - 10)
      // Move upper adjacent card down
      commentCards[startIndex].style.top = desiredTop + 'px'
      commentCards[startIndex].setAttribute('top-position', desiredTop)
      // Move other cards down
      if (startIndex > 0) {
        for (let n = startIndex - 1; n >= 0; n--) {
          const previousTop = parseInt(commentCards[n + 1].style.top.substring(0, commentCards[n + 1].style.top.length - 2))
          const previousEnd = previousTop + commentCards[n + 1].offsetHeight
          const thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2))
          const thisEnd = thisTop + commentCards[n].offsetHeight
          // Check if move down is necessary
          if (thisEnd < (previousTop - 10)) {
            const thisCommentId = commentCards[n].getAttribute('highlight-id')
            const thisG = getHighlightByCommentId(thisCommentId)
            const thisGTop = parseInt(thisG.getAttribute('top')) - 35
            if (thisTop < thisGTop) {
              const thisDesiredTop = thisGTop <= (thisTop + (previousTop - thisEnd) - 10) ? gTop : (thisTop + (previousTop - thisEnd) - 10)
              commentCards[n].style.top = thisDesiredTop + 'px'
              commentCards[n].setAttribute('top-position', thisDesiredTop)
            }
          } else {
            break
          }
        }
      }
    }
  }
}

function moveDownCommentCards(commentCards, startIndex, endIndex, topPos, endPos) {
  // Move the upper adjacent card down first
  const adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2))
  if (adjTop < (endPos + 10)) {
    commentCards[startIndex].style.top = endPos + 10 + 'px'
    commentCards[startIndex].setAttribute('top-position', endPos + 10)

    // Move other cards down ward
    if (commentCards.length > (startIndex + 1)) {
      for (let j = startIndex + 1; j < commentCards.length; j++) {
        const previousTop = parseInt(commentCards[j - 1].style.top.substring(0, commentCards[j - 1].style.top.length - 2))
        const previousEnd = previousTop + commentCards[j - 1].offsetHeight
        const thisTop = parseInt(commentCards[j].style.top.substring(0, commentCards[j].style.top.length - 2))
        if (thisTop < (previousEnd + 10)) {
          commentCards[j].style.top = thisTop + (previousEnd - thisTop) + 10 + 'px'
          commentCards[j].setAttribute('top-position', thisTop + (previousEnd - thisTop) + 10)
        }
      }
    }
  } else {
    // Check if move up is necessary
    const commentId = commentCards[startIndex].getAttribute('highlight-id')
    const g = getHighlightByCommentId(commentId)
    const gTop = parseInt(g.getAttribute('top')) - 35

    if (adjTop > gTop) {
      const desiredTop = gTop >= (endPos + 10) ? gTop : (endPos + 10)
      // Move lower adjacent card up
      commentCards[startIndex].style.top = desiredTop + 'px'
      commentCards[startIndex].setAttribute('top-position', desiredTop)
      // Move other cards up
      if (commentCards.length > (startIndex + 1)) {
        for (let n = startIndex + 1; n < commentCards.length; n++) {
          const previousTop = parseInt(commentCards[n - 1].style.top.substring(0, commentCards[n - 1].style.top.length - 2))
          const previousEnd = previousTop + commentCards[n - 1].offsetHeight
          const thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2))
          // let thisEnd = thisTop + commentCards[n].offsetHeight;
          // Check if move down is necessary
          if (thisTop > (previousEnd + 10)) {
            const thisCommentId = commentCards[n].getAttribute('highlight-id')
            const thisG = getHighlightByCommentId(thisCommentId)
            const thisGTop = parseInt(thisG.getAttribute('top')) - 35
            if (thisTop > thisGTop) {
              const thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10)
              commentCards[n].style.top = thisDesiredTop + 'px'
              commentCards[n].setAttribute('top-position', thisDesiredTop)
            }
          } else {
            break
          }
        }
      }
    }
  }
}

function compareTopAttributes(a, b) {
  let top1 = parseInt(a.getAttribute('top'))
  if (a.getAttribute('page-num') > 1) { top1 += ((a.getAttribute('page-num') - 1) * a.getAttribute('page-height')) }

  let top2 = parseInt(b.getAttribute('top'))
  if (b.getAttribute('page-num') > 1) { top2 += ((b.getAttribute('page-num') - 1) * b.getAttribute('page-height')) }
  if (top1 > top2) return 1
  if (top2 > top1) return -1
  if (top2 == top1) {
    // compare order attributes
    const order1 = parseInt(a.getAttribute('order'))
    const order2 = parseInt(b.getAttribute('order'))
    if (order1 > order2) return 1
    if (order2 > order1) return -1
  }
  return 0
}

function hasEnoughSpace(commentCards) {
  if (commentCards.length > 0) {
    const svg = _svg
    const rectTop = parseInt(_target.style.top.substring(0, _target.style.top.length - 2))
    const svgHeight = svg.getAttribute('height')
    const svgPageNum = svg.getAttribute('data-pdf-annotate-page')

    let svgTop = 0
    if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
    const topPos = svgTop + rectTop - 35
    const endPos = topPos + 105

    // Check first and last position
    const firstTopPos = parseInt(commentCards[0].getAttribute('top-position'))
    const lastEndPos = parseInt(commentCards[commentCards.length - 1].getAttribute('top-position')) +
                      parseInt(commentCards[commentCards.length - 1].offsetHeight)

    if (endPos < firstTopPos || topPos > lastEndPos) { return true }
    if (commentCards.length > 1) {
      for (let i = 0; i < commentCards.length - 1; i++) {
        const thisTopPos = parseInt(commentCards[i].getAttribute('top-position'))
        const thisEndPos = thisTopPos + commentCards[i].offsetHeight

        const nextTopPos = parseInt(commentCards[i + 1].getAttribute('top-position'))
        // let nextEndPos = nextTopPos + commentCards[i + 1].offsetHeight;

        if (topPos > thisEndPos && endPos < nextTopPos) {
          return true
        }
      }
    }

    return false
  }
  return true
}

function updateCommentsAfterSave() {

}

function getHighlightByCommentId(commentId) {
  return document.querySelector("g[data-pdf-annotate-id='" + commentId + "']")
}

// Add comment tab to the pdf viewer
function displayAddNewCommentForm() {
  const newCommentWrapper = document.getElementById('add-new-comment')
  newCommentWrapper.style.visibility = 'visible'

  const svg = _svg
  const rectTop = parseInt(_target.style.top.substring(0, _target.style.top.length - 2))
  const svgHeight = svg.getAttribute('height')
  const svgPageNum = svg.getAttribute('data-pdf-annotate-page')

  let svgTop = 0
  if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
  const topPos = svgTop + rectTop - 35
  newCommentWrapper.style =
    'position: absolute; right: 5px; top: ' + topPos + 'px;'
  hideTextToolBar()
  const textArea = document.getElementById('comment-text-area')
  textArea.focus()
  _inputHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2))

  textArea.addEventListener('keydown', function(e) {
    const commentCards = document.querySelectorAll('.comment-card')
    if (commentCards.length > 0) {
      setTimeout(function() {
        const newHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2))
        const newCommentWrapper = document.getElementById('add-new-comment')
        const topPos = parseInt(newCommentWrapper.style.top.substring(0, newCommentWrapper.style.top.length - 2))
        const endPos = topPos + newCommentWrapper.offsetHeight
        if (_inputHeight < newHeight) {
          if (_order <= commentCards.length - 1) { moveDownFromEndPos(commentCards, _order, endPos) }
        } else if (_inputHeight > newHeight) {
          if (_order < commentCards.length) { moveUpToEndPos(commentCards, _order, endPos) }
        }
        _inputHeight = newHeight
      }, 100)
    }
  })
}

export function cancelCommentText() {
  const { documentId, pageNumber } = getMetadata(_svg)
  const newCommentWrapper = document.getElementById('add-new-comment')
  const topPos = parseInt(newCommentWrapper.style.top.substring(0, newCommentWrapper.style.top.length - 2))
  const endPos = topPos + newCommentWrapper.offsetHeight
  newCommentWrapper.style.display = 'none'
  const commentCards = document.querySelectorAll('.comment-card')
  if (commentCards.length > 0) {
    if (_order <= (commentCards.length - 1)) {
      moveUpToEndPos(commentCards, _order, topPos)
      if (_order > 0) {
        const orderTopPos = parseInt(commentCards[_order].style.top.substring(0, commentCards[_order].style.top.length - 2))
        moveDownToTopPos(commentCards, _order - 1, orderTopPos)
      }
    }
    if (_order == commentCards.length) {
      moveDownToTopPos(commentCards, _order - 1, endPos)
    }
  }
  // Remove the annotation
  PDFJSAnnotate.getStoreAdapter().deleteAnnotation(documentId, _annotation.uuid)
    .then((isDeleted) => {
      if (isDeleted) {
        // remove highlight in UI
        document.querySelector("g[data-pdf-annotate-id='" + _annotation.uuid + "']").remove()
        _annotation = null
      }
    })
}

export async function addCommentText(commentContent, documentId) {
  const commentWrapper = document.getElementById('add-new-comment')
  const topPos = parseInt(commentWrapper.style.top.substring(0, commentWrapper.style.top.length - 2))
  const annotation = _annotation
  return await PDFJSAnnotate.getStoreAdapter().addComment(documentId,
    annotation,
    commentContent,
    _selectedText,
    topPos)
}

export async function commentText() {
  // Highlight the text first
  await highlightText('comment-highlight')
  // Update positions of comment boxes
  updateCommentPositions()
  // Display add new form
  displayAddNewCommentForm()
  event.stopPropagation()
}
/**
 * Enable rect behavior
 */
export function enableSelect(type) {
  _type = type
  if (_enabled) { return }

  _enabled = true
  document.addEventListener('mouseup', handleDocumentMouseup)
  document.addEventListener('mousedown', handleDocumentMousedown)
  document.addEventListener('keyup', handleDocumentKeyup)
  addEventListener('annotation:click', handleCommentAnnotationClick)

  const commentCards = document.querySelectorAll('.comment-card')
  commentCards.forEach(item => {
    item.addEventListener('click', handleCommentCardClick)
  })

  // document.getElementById("restore").addEventListener("click", restoreCommentPositions);
  window.addEventListener('beforeunload', restoreCommentPositions)
}

function handleCommentCardClick() {
  const highlightId = this.getAttribute('highlight-id')
  const highlight = document.querySelector("g[data-pdf-annotate-id='" + highlightId + "']")
  handleCommentAnnotationClick(highlight)
}

function unselectHighlightComment() {
  const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
  if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
  const selectedComment = document.getElementsByClassName('comment-card-selected')
  if (selectedComment.length > 0) { selectedComment[0].classList.remove('comment-card-selected') }
}

async function restoreCommentPositions(e) {
  const documentId = document.getElementById('viewer').getAttribute('document-id')
  const highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
  handleCommentAnnotationClick(highlights[0])
  const comments = await updateCommentAnnotations(documentId)
  console.log(comments)
  await PDFJSAnnotate.getStoreAdapter().updateComments(documentId, comments)
}

async function updateCommentAnnotations(documentId) {
  const comments = await PDFJSAnnotate.getStoreAdapter().getComments(documentId)
  comments.forEach(comment => {
    const thisCard = document.querySelector(".comment-card[highlight-id='" + comment.uuid + "']")
    comment.topPosition = parseInt(thisCard.getAttribute('top-position'))
  })
  return comments
}

function handleCommentAnnotationClick(target) {
  const text = window
    .getSelection()
    .toString()
    .trim()
  // Display text tool bar if text is selected.
  if (!text) {
    const type = target.getAttribute('data-pdf-annotate-type')
    if (type == 'comment-highlight') {
      const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
      if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
      target.classList.add('comment-highlight-selected')

      const commentCards = document.querySelectorAll('.comment-card')
      const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const highlightArr = Array.prototype.slice.call(commentHighlights)
      // Sort the highlights
      highlightArr.sort(compareTopAttributes)
      // Get order of the current highlight
      _order = highlightArr.findIndex(item => {
        return item.dataset.pdfAnnotateId == target.getAttribute('data-pdf-annotate-id')
      })

      let gTop = parseInt(target.getAttribute('top')) - 34
      const svgHeight = parseInt(target.getAttribute('page-height'))
      const svgPageNum = parseInt(target.getAttribute('page-num'))
      if (svgPageNum > 1) { gTop += ((svgPageNum - 1) * svgHeight) }

      const cTop = parseInt(commentCards[_order].style.top.substring(0, commentCards[_order].style.top.length - 2))

      // remove current selected comment card class if any
      const selected = document.getElementsByClassName('comment-card-selected')
      if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
      // Set this card as selected
      commentCards[_order].classList.add('comment-card-selected')

      if (cTop != gTop) {
        // Move the comment up to gTop
        commentCards[_order].style.top = gTop + 'px'
        commentCards[_order].setAttribute('top-position', gTop)

        const selected = document.getElementsByClassName('comment-card-selected')
        if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
        commentCards[_order].classList.add('comment-card-selected')
        const endPos = gTop + commentCards[_order].offsetHeight
        if (cTop > gTop) {
          // Move up other uppen comments
          if (_order > 0) { moveUpFromTopPos(commentCards, _order - 1, gTop) }
          // Move up other lower comments
          if (_order < commentCards.length - 1) { moveUpToEndPos(commentCards, _order + 1, endPos) }
        } else if (cTop < gTop) {
          // Move down other uppen comments
          if (_order > 0) { moveDownToTopPos(commentCards, _order - 1, gTop) }
          // Move down other lower comments
          if (_order < commentCards.length - 1) { moveDownFromEndPos(commentCards, _order + 1, endPos) }
        }
      }
    }
  }
}
/**
 * Disable rect behavior
 */
export function disableSelect() {
  if (!_enabled) { return }

  _enabled = false
  document.removeEventListener('mouseup', handleDocumentMouseup)
  document.removeEventListener('mousedown', handleDocumentMousedown)
  document.removeEventListener('keyup', handleDocumentKeyup)
}

