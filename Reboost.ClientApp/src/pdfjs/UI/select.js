/* eslint-disable no-empty */
/* eslint-disable no-unused-vars */
import PDFJSAnnotate from '../PDFJSAnnotate';
import appendChild from '../render/appendChild';
import { addEventListener } from './event';
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
} from './utils';

let _enabled = false;
let _type;
let overlay;
let originY;
let originX;
let _rects;
let _svg;
let _target;
let _annotation;
let _selectedText;
let _topPos;
let _endPos;
let _inputHeight;
let _order;
/**
 * Get the current window selection as rects
 *
 * @return {Array} An Array of rects
 */
function getSelectionRects() {
	try {
		let selection = window.getSelection();
		let range = selection.getRangeAt(0);
		let rects = range.getClientRects();

		if (rects.length > 0 &&
        rects[0].width > 0 &&
        rects[0].height > 0) {
			return rects;
		}
	} catch (e) {}
  
	return null;
}

/**
 * Handle document.mousedown event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMousedown(e) {
	let svg;
	if (_type === 'select' || !(svg = findSVGAtPoint(e.clientX, e.clientY))) {
		// hide text bool buttons
		if(!e.target.classList.contains('textToolBtn') && 
        !e.target.classList.contains('svg-inline--fa') &&
        !e.target.parentElement.classList.contains('svg-inline--fa')){
			let textTool = document.getElementById('textTool');
			textTool.style.visibility = 'hidden';
		}
		// Hide add new comment form
		let newCommentWrapper = document.getElementById('add-new-comment');
		if(newCommentWrapper && newCommentWrapper.style.display != 'none'){
			let validTarget = hasAParentWithClass(e.target, 'add-new-comment');
			let comment = document.getElementById('comment-text-area').value;
			if(!validTarget && comment.replace(/\s/g, '').length == 0)
				cancelCommentText();
		}
		// Unselect highlight comment
		if(!e.target.classList.contains('comment-highlight-selected')){
			let validTarget = hasAParentWithClass(e.target, 'comment-card-selected');
			if(!validTarget)
				unselectHighlightComment();
		}
		return;
	}

	let rect = svg.getBoundingClientRect();
	originY = e.clientY;
	originX = e.clientX;

	overlay = document.createElement('div');
	overlay.style.position = 'absolute';
	overlay.style.top = `${originY - rect.top}px`;
	overlay.style.left = `${originX - rect.left}px`;
	overlay.style.border = `3px solid ${BORDER_COLOR}`;
	overlay.style.borderRadius = '3px';
	svg.parentNode.appendChild(overlay);
  
	document.addEventListener('mousemove', handleDocumentMousemove);
	disableUserSelect();
}

function hasAParentWithClass(element, className) {
	for(let i = 0; i <= 5; i++){
		if (element.classList.contains(className)) 
			return true;
		else
			element = element.parentNode;
	}
	return false;
	// if (element.classList.contains(className)) return true;
	// return element.parentNode && hasAParentWithClass(element.parentNode, className);
}

/**
 * Handle document.mousemove event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMousemove(e) {
	let svg = overlay.parentNode.querySelector('svg.annotationLayer');
	let rect = svg.getBoundingClientRect();

	if (originX + (e.clientX - originX) < rect.right) {
		overlay.style.width = `${e.clientX - originX}px`;
	}

	if (originY + (e.clientY - originY) < rect.bottom) {
		overlay.style.height = `${e.clientY - originY}px`;
	}
}

/**
 * Handle document.mouseup event
 *
 * @param {Event} e The DOM event to handle
 */
function handleDocumentMouseup(e) {
	if(e.target.parentElement.classList.contains('textLayer')){
    
		let rects;
		if (_type === 'select' && (rects = getSelectionRects())) {
    
			let text = window
				.getSelection()
				.toString()
				.trim();
			// Display text tool bar if text is selected.
			if (text) {
				let textTool = document.getElementById('textTool');
				let posX = rects[rects.length - 1].left + (rects[rects.length - 1].width / 2);
				let posY = rects[rects.length - 1].top + 20;
				textTool.style =
          'position: absolute; top: ' +
          posY +
          'px; left: ' +
          posX +
          'px;';

				_rects = rects;
				_svg = findSVGAtPoint(rects[0].left, rects[0].top);
				_target = e.target;
				_selectedText = text;
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
		let selection = window.getSelection();
		selection.removeAllRanges();
		if (overlay && overlay.parentNode) {
			overlay.parentNode.removeChild(overlay);
			overlay = null;
			document.removeEventListener('mousemove', handleDocumentMousemove);
		}
	}
}

/**
 * Save a highlight annotation
 */
export async function highlightText(type) {
	let svg = _svg; //findSVGAtPoint(rects[0].left, rects[0].top);
	let rects = _rects;
	let annotation;

	if (!svg) {
		return;
	}

	let boundingRect = svg.getBoundingClientRect();
  
	let color = 'FFFF00';
	if(type == 'strikeout')
		color = 'FF0000';
	// Initialize the annotation
	annotation = {
		type,
		color,
		rectangles: [...rects].map((r) => {
			let offset = 0;
			if(type == 'strikeout')
				offset = r.height / 2;
			return scaleDown(svg, {
				y: (r.top + offset) - boundingRect.top,
				x: r.left - boundingRect.left,
				width: r.width,
				height: r.height
			});
		}).filter((r) => r.width > 0 && r.height > 0 && r.x > -1 && r.y > -1)
	};
  
  
	// Short circuit if no rectangles exist
	if (annotation.rectangles.length === 0) {
		return;
	}

	if(annotation.rectangles.length > 2){
		annotation.rectangles = removeMinHeigthRects(annotation.rectangles);
	}

	let { documentId, pageNumber } = getMetadata(svg);

	annotation.top = parseInt(_target.style.top.substring(0, _target.style.top.length - 2));
	annotation.pageNum = pageNumber;
	annotation.pageHeight = svg.getAttribute('height');
	_annotation = annotation;
  
  
	// Add the annotation
	await PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
		.then((annotation) => {
			appendChild(svg, annotation);
			hideTextToolBar();
		});
}

function removeMinHeigthRects(arr) {
	let min = Math.min(...arr.map(a => a.height));
	return arr.filter(a => a.height != min);
}

function hideTextToolBar(){
	let textTool = document.getElementById('textTool');
	textTool.style.visibility = 'hidden';
}

function updateCommentPositions(){
	let commentCards = document.querySelectorAll('.comment-card');
	let commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']");
	let highlightArr = Array.prototype.slice.call(commentHighlights);
	// Sort the highlights
	highlightArr.sort(compareTopAttributes);
	// Get order of the current highlight
	_order = highlightArr.findIndex(item => {
		return item.dataset.pdfAnnotateId == _annotation.uuid;
	});

	if(!hasEnoughSpace(commentCards)){
		// Update comment positions

		// get add new form's top position
		let svg = _svg;
		let rectTop = parseInt(_target.style.top.substring(0, _target.style.top.length - 2));
		let svgHeight = svg.getAttribute('height');
		let svgPageNum = svg.getAttribute('data-pdf-annotate-page');
		let svgTop = 0;
		if(svgPageNum > 1)
			svgTop += ((svgPageNum - 1) * svgHeight);
		let topPos = svgTop + rectTop - 35;
		let endPos = topPos + 105;

		if(_order == 0){
			// Move down all comment cards
			//moveDownCommentCards(commentCards, 0, commentCards.length - 1, topPos, endPos);
			moveDownFromEndPos(commentCards, 0, endPos);
		} else if(_order == highlightArr.length - 1){
			// Move up all comment cards
			//moveUpCommentCards(commentCards, commentCards.length - 1, 0, topPos, endPos);
			moveUpFromTopPos(commentCards, commentCards.length - 1, topPos);
		} else {
			// Move up commentCards[0] -> commentCards[i - 1]
			//moveUpCommentCards(commentCards, _order - 1, 0, topPos, endPos)
			let upperTop = parseInt(commentCards[_order - 1].style.top.substring(0, commentCards[_order - 1].style.top.length - 2));
			let upperEnd = upperTop + commentCards[_order - 1].offsetHeight;
			// Check if move up is necessary
			if(upperEnd > (topPos - 10)){
				moveUpFromTopPos(commentCards, _order - 1, topPos);
			} else {
				// Check if move down is necessary
				let commentId = commentCards[_order - 1].getAttribute('highlight-id');
				let g = getHighlightByCommentId(commentId);
				let gTop = parseInt(g.getAttribute('top')) - 35;
				if(g.getAttribute('page-num') > 1)
					gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height'));
				if(upperTop < gTop){
					moveDownToTopPos(commentCards, _order - 1, topPos);
				}
			}
			// Move down commentCards[i] -> commentCards[commentCards.length - 1]
			//moveDownCommentCards(commentCards, _order, commentCards.length, topPos, endPos);
			let lowerTop = parseInt(commentCards[_order].style.top.substring(0, commentCards[_order].style.top.length - 2));
			if(lowerTop < (endPos + 10)){
				moveDownFromEndPos(commentCards, _order, endPos);
			} else {
				// Check if move up is necessary
				let commentId = commentCards[_order].getAttribute('highlight-id');
				let g = getHighlightByCommentId(commentId);
				let gTop = parseInt(g.getAttribute('top')) - 35;
				if(g.getAttribute('page-num') > 1)
					gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height'));
				if(lowerTop > gTop){
					moveUpToEndPos(commentCards, _order, endPos);
				}
			}
		}
	}
}

export function updatePositionsAfterCommentAdded(){
	let newComment = document.querySelector(".comment-card[highlight-id='"+ _annotation.uuid +"']");
	let commentCards = document.querySelectorAll('.comment-card');
	let endPos = parseInt(newComment.getAttribute('top-position')) + newComment.offsetHeight;
	if(_order < (commentCards.length - 1))
		moveUpToEndPos(commentCards, _order + 1, endPos);
	newComment.addEventListener('click', handleCommentCardClick);
}

export function updatePositionsAfterCommentDeleted(){

}

function moveUpFromTopPos(commentCards, startIndex, topPos){
	// Get start index card's position
	let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
	let adjEnd = adjTop + commentCards[startIndex].offsetHeight;
	// Move the start index card up first
	commentCards[startIndex].style.top = adjTop - (adjEnd - topPos) - 10 + 'px';
	commentCards[startIndex].setAttribute('top-position', adjTop - (adjEnd - topPos) - 10);
	if(startIndex > 0){
		for(let m = startIndex - 1; m >= 0; m--){
			let previousTop = parseInt(commentCards[m + 1].style.top.substring(0, commentCards[m + 1].style.top.length - 2));
			let thisTop = parseInt(commentCards[m].style.top.substring(0, commentCards[m].style.top.length - 2));
			let thisEnd = thisTop + commentCards[m].offsetHeight;
			if(thisEnd > (previousTop - 10)){
				commentCards[m].style.top = thisTop - (thisEnd - previousTop) - 10 + 'px';
				commentCards[m].setAttribute('top-position', thisTop - (thisEnd - previousTop) - 10);
			} else {
				break;
			}
		}
	}
}

function moveUpToEndPos(commentCards, startIndex, endPos){

	let commentId = commentCards[startIndex].getAttribute('highlight-id');
	let g = getHighlightByCommentId(commentId);
	let gTop = parseInt(g.getAttribute('top')) - 35;
	if(g.getAttribute('page-num') > 1)
		gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height'));

	let desiredTop = gTop >= (endPos + 10) ? gTop : (endPos + 10) ;
	// Move start index card up
	commentCards[startIndex].style.top = desiredTop + 'px';
	commentCards[startIndex].setAttribute('top-position', desiredTop);

	// let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
	// commentCards[startIndex].style.top = adjTop - (adjTop - endPos) + 10 + "px";
	// commentCards[startIndex].setAttribute("top-position", adjTop - (adjTop - endPos) + 10);

	if(commentCards.length > (startIndex + 1)){

		for(let n = startIndex + 1; n < commentCards.length; n++){
			let previousTop = parseInt(commentCards[n - 1].style.top.substring(0, commentCards[n - 1].style.top.length - 2));
			let previousEnd = previousTop + commentCards[n - 1].offsetHeight;
			let thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2));
      
			// Check if move down is necessary
			if(thisTop > (previousEnd + 10)){
				let thisCommentId = commentCards[n].getAttribute('highlight-id');
				let thisG = getHighlightByCommentId(thisCommentId);
				let thisGTop = parseInt(thisG.getAttribute('top')) - 35;
				if(thisG.getAttribute('page-num') > 1)
					thisGTop += ((thisG.getAttribute('page-num') - 1) * thisG.getAttribute('page-height'));
				if(thisTop > thisGTop){
					let thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10) ;
					commentCards[n].style.top = thisDesiredTop + 'px';
					commentCards[n].setAttribute('top-position', thisDesiredTop);
				}
			} else {
				break;
			}
		}
	}
}

function moveDownToTopPos(commentCards, startIndex, topPos){
	// Get start index card's position
	let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
	let adjEnd = adjTop + commentCards[startIndex].offsetHeight;
	// Check if move down is necessary
	let commentId = commentCards[startIndex].getAttribute('highlight-id');
	let g = getHighlightByCommentId(commentId);
	let gTop = parseInt(g.getAttribute('top')) - 35;
	if(g.getAttribute('page-num') > 1)
		gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height'));
	// Only move cards down if the upper card is higher that its highlight
	if(adjTop < gTop ){
		let desiredTop = gTop <= (adjTop + (topPos - adjEnd) - 10) ? gTop : (adjTop + (topPos - adjEnd) - 10) ;
		// Move upper adjacent card down
		commentCards[startIndex].style.top = desiredTop + 'px';
		commentCards[startIndex].setAttribute('top-position', desiredTop);
		// Move other cards down
		if(startIndex > 0){
			for(let n = startIndex - 1; n >= 0; n--){
				let previousTop = parseInt(commentCards[n + 1].style.top.substring(0, commentCards[n + 1].style.top.length - 2));
				//let previousEnd = previousTop + commentCards[n + 1].offsetHeight;
				let thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2));
				let thisEnd = thisTop + commentCards[n].offsetHeight;
				// Check if move down is necessary
				if(thisEnd < (previousTop - 10)){
					let thisCommentId = commentCards[n].getAttribute('highlight-id');
					let thisG = getHighlightByCommentId(thisCommentId);
					let thisGTop = parseInt(thisG.getAttribute('top')) - 35;
					if(thisG.getAttribute('page-num') > 1)
						thisGTop += ((thisG.getAttribute('page-num') - 1) * thisG.getAttribute('page-height'));
					if(thisTop < thisGTop){
						let thisDesiredTop = thisGTop <= (thisTop + (previousTop - thisEnd) - 10) ? thisGTop : (thisTop + (previousTop - thisEnd) - 10) ;
						commentCards[n].style.top = thisDesiredTop + 'px';
						commentCards[n].setAttribute('top-position', thisDesiredTop);
					}
				} else {
					break;
				}
			}
		}
	}
  
}

function moveDownFromEndPos(commentCards, startIndex, endPos){
	// Move the start index card down first
	//let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
	commentCards[startIndex].style.top = endPos + 10 + 'px';
	commentCards[startIndex].setAttribute('top-position', endPos + 10);
	// Move other cards down ward
	if(commentCards.length > (startIndex + 1)){
		for(let j = startIndex + 1; j < commentCards.length; j++){
			let previousTop = parseInt(commentCards[j - 1].style.top.substring(0, commentCards[j - 1].style.top.length - 2));
			let previousEnd = previousTop + commentCards[j - 1].offsetHeight;
			let thisTop = parseInt(commentCards[j].style.top.substring(0, commentCards[j].style.top.length - 2));
			if(thisTop < (previousEnd + 10)){
				commentCards[j].style.top = thisTop + (previousEnd - thisTop) + 10 + 'px';
				commentCards[j].setAttribute('top-position', thisTop + (previousEnd - thisTop) + 10 );
			} else {
				break;
			}
		}
	}
}

function moveUpCommentCards(commentCards, startIndex, endIndex, topPos, endPos){
	// Get adjacent card's position
	let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
	let adjEnd = adjTop + commentCards[startIndex].offsetHeight;
	// Check if move up is necessary
	if(adjEnd > (topPos - 10)){
		// Move the upper adjacent card up first
		commentCards[startIndex].style.top = adjTop - (adjEnd - topPos) - 10 + 'px';
		commentCards[startIndex].setAttribute('top-position', adjTop - (adjEnd - topPos) - 10);
		// Move other cards upward
		if(startIndex > 0){
			for(let m = startIndex - 1; m >= 0; m--){
				let previousTop = parseInt(commentCards[m + 1].style.top.substring(0, commentCards[m + 1].style.top.length - 2));
				let thisTop = parseInt(commentCards[m].style.top.substring(0, commentCards[m].style.top.length - 2));
				let thisEnd = thisTop + commentCards[m].offsetHeight;
				if(thisEnd > (previousTop - 10)){
					commentCards[m].style.top = thisTop - (thisEnd - previousTop) - 10 + 'px';
					commentCards[m].setAttribute('top-position', thisTop - (thisEnd - previousTop) - 10);
				} else {
					break;
				}
			}
		}
	} else {
		// Check if move down is necessary
		let commentId = commentCards[startIndex].getAttribute('highlight-id');
		let g = getHighlightByCommentId(commentId);
		let gTop = parseInt(g.getAttribute('top')) - 35;

		if(adjTop < gTop){
			let desiredTop = gTop <= (adjTop + (topPos - adjEnd) - 10) ? gTop : (adjTop + (topPos - adjEnd) - 10) ;
			// Move upper adjacent card down
			commentCards[startIndex].style.top = desiredTop + 'px';
			commentCards[startIndex].setAttribute('top-position', desiredTop);
			// Move other cards down
			if(startIndex > 0){
				for(let n = startIndex - 1; n >= 0; n--){
					let previousTop = parseInt(commentCards[n + 1].style.top.substring(0, commentCards[n + 1].style.top.length - 2));
					let previousEnd = previousTop + commentCards[n + 1].offsetHeight;
					let thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2));
					let thisEnd = thisTop + commentCards[n].offsetHeight;
					// Check if move down is necessary
					if(thisEnd < (previousTop - 10)){
						let thisCommentId = commentCards[n].getAttribute('highlight-id');
						let thisG = getHighlightByCommentId(thisCommentId);
						let thisGTop = parseInt(thisG.getAttribute('top')) - 35;
						if(thisTop < thisGTop){
							let thisDesiredTop = thisGTop <= (thisTop + (previousTop - thisEnd) - 10) ? gTop : (thisTop + (previousTop - thisEnd) - 10) ;
							commentCards[n].style.top = thisDesiredTop + 'px';
							commentCards[n].setAttribute('top-position', thisDesiredTop);
						}
					} else {
						break;
					}
				}
			}
		}
	}
  
}

function moveDownCommentCards(commentCards, startIndex, endIndex, topPos, endPos){
	// Move the upper adjacent card down first
	let adjTop = parseInt(commentCards[startIndex].style.top.substring(0, commentCards[startIndex].style.top.length - 2));
	if(adjTop < (endPos + 10)){
		commentCards[startIndex].style.top = endPos + 10 + 'px';
		commentCards[startIndex].setAttribute('top-position', endPos + 10);

		// Move other cards down ward
		if(commentCards.length > (startIndex + 1)){
			for(let j = startIndex + 1; j < commentCards.length; j++){
				let previousTop = parseInt(commentCards[j - 1].style.top.substring(0, commentCards[j - 1].style.top.length - 2));
				let previousEnd = previousTop + commentCards[j - 1].offsetHeight;
				let thisTop = parseInt(commentCards[j].style.top.substring(0, commentCards[j].style.top.length - 2));
				if(thisTop < (previousEnd + 10)){
					commentCards[j].style.top = thisTop + (previousEnd - thisTop) + 10 + 'px';
					commentCards[j].setAttribute('top-position', thisTop + (previousEnd - thisTop) + 10 );
				}
			}
		}
	} else {
		// Check if move up is necessary
		let commentId = commentCards[startIndex].getAttribute('highlight-id');
		let g = getHighlightByCommentId(commentId);
		let gTop = parseInt(g.getAttribute('top')) - 35;

		if(adjTop > gTop){
			let desiredTop = gTop >= (endPos + 10) ? gTop : (endPos + 10) ;
			// Move lower adjacent card up
			commentCards[startIndex].style.top = desiredTop + 'px';
			commentCards[startIndex].setAttribute('top-position', desiredTop);
			// Move other cards up
			if(commentCards.length > (startIndex + 1)){
				for(let n = startIndex + 1; n < commentCards.length; n++){
					let previousTop = parseInt(commentCards[n - 1].style.top.substring(0, commentCards[n - 1].style.top.length - 2));
					let previousEnd = previousTop + commentCards[n - 1].offsetHeight;
					let thisTop = parseInt(commentCards[n].style.top.substring(0, commentCards[n].style.top.length - 2));
					//let thisEnd = thisTop + commentCards[n].offsetHeight;
					// Check if move down is necessary
					if(thisTop > (previousEnd + 10)){
						let thisCommentId = commentCards[n].getAttribute('highlight-id');
						let thisG = getHighlightByCommentId(thisCommentId);
						let thisGTop = parseInt(thisG.getAttribute('top')) - 35;
						if(thisTop > thisGTop){
							let thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10) ;
							commentCards[n].style.top = thisDesiredTop + 'px';
							commentCards[n].setAttribute('top-position', thisDesiredTop);
						}
            
					} else {
						break;
					}
				}
			}
		}
	}
}

function compareTopAttributes(a, b){
	let top1 = parseInt(a.getAttribute('top'));
	if(a.getAttribute('page-num') > 1)
		top1 += ((a.getAttribute('page-num') - 1) * a.getAttribute('page-height'));
  
	let top2 = parseInt(b.getAttribute('top'));
	if(b.getAttribute('page-num') > 1)
		top2 += ((b.getAttribute('page-num') - 1) * b.getAttribute('page-height'));
	if (top1 > top2) return 1;
	if (top2 > top1) return -1;
	if (top2 == top1) {
		// compare order attributes
		let order1 = parseInt(a.getAttribute('order'));
		let order2 = parseInt(b.getAttribute('order'));
		if (order1 > order2) return 1;
		if (order2 > order1) return -1;
	}
	return 0;
}

function hasEnoughSpace(commentCards){
	if(commentCards.length > 0){
		let svg = _svg;
		let rectTop = parseInt(_target.style.top.substring(0, _target.style.top.length - 2));
		let svgHeight = svg.getAttribute('height');
		let svgPageNum = svg.getAttribute('data-pdf-annotate-page');
    
		let svgTop = 0;
		if(svgPageNum > 1)
			svgTop += ((svgPageNum - 1) * svgHeight);
		let topPos = svgTop + rectTop - 35;
		let endPos = topPos + 105;

		// Check first and last position
		let firstTopPos = parseInt(commentCards[0].getAttribute('top-position'));
		let lastEndPos = parseInt(commentCards[commentCards.length - 1].getAttribute('top-position')) + 
                      parseInt(commentCards[commentCards.length - 1].offsetHeight);

		if(endPos < firstTopPos || topPos > lastEndPos)
			return true;
		if(commentCards.length > 1){
			for(let i = 0; i < commentCards.length - 1; i++){
				let thisTopPos = parseInt(commentCards[i].getAttribute('top-position'));
				let thisEndPos = thisTopPos + commentCards[i].offsetHeight;
  
				let nextTopPos = parseInt(commentCards[i + 1].getAttribute('top-position'));
				//let nextEndPos = nextTopPos + commentCards[i + 1].offsetHeight;
  
				if(topPos > thisEndPos && endPos < nextTopPos){
					return true;
				}
			}
		}
    
		return false;
	}
	return true;
}


function updateCommentsAfterSave(){

}

function getHighlightByCommentId(commentId){
	return  document.querySelector("g[data-pdf-annotate-id='"+ commentId +"']");
}

// Add comment tab to the pdf viewer
function displayAddNewCommentForm(){
	let newCommentWrapper = document.getElementById('add-new-comment');

	newCommentWrapper.style.visibility = 'visible';

	let svg = _svg;
	let rectTop = parseInt(_target.style.top.substring(0, _target.style.top.length - 2));
	let svgHeight = svg.getAttribute('height');
	let svgPageNum = svg.getAttribute('data-pdf-annotate-page');
  
	let svgTop = 0;
	if(svgPageNum > 1)
		svgTop += ((svgPageNum - 1) * svgHeight);
	let topPos = svgTop + rectTop - 35;
	newCommentWrapper.style =
    'position: absolute; right: 5px; top: ' + topPos + 'px;';
	hideTextToolBar();
	let textArea = document.getElementById('comment-text-area');
	textArea.focus();
	_inputHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2));

	textArea.addEventListener('keydown', function (e){
		let commentCards = document.querySelectorAll('.comment-card');
		if(commentCards.length > 0){
			setTimeout(function(){ 
				let newHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2));
				let newCommentWrapper = document.getElementById('add-new-comment');
				let topPos = parseInt(newCommentWrapper.style.top.substring(0, newCommentWrapper.style.top.length - 2));
				let endPos = topPos + newCommentWrapper.offsetHeight;
				if(_inputHeight < newHeight){
					if(_order <= commentCards.length - 1 )
						moveDownFromEndPos(commentCards, _order, endPos);
				} else if(_inputHeight > newHeight){
					if(_order < commentCards.length)
						moveUpToEndPos(commentCards, _order, endPos);
				}
				_inputHeight = newHeight;
			}, 100);
		}
	});

}

export function cancelCommentText(){
	let { documentId, pageNumber } = getMetadata(_svg);
	let newCommentWrapper = document.getElementById('add-new-comment');
	let topPos = parseInt(newCommentWrapper.style.top.substring(0, newCommentWrapper.style.top.length - 2));
	let endPos  = topPos + newCommentWrapper.offsetHeight;
	newCommentWrapper.style.display = 'none';
	let commentCards = document.querySelectorAll('.comment-card');
	if(commentCards.length > 0){
		if(_order <= (commentCards.length - 1)){
			moveUpToEndPos(commentCards, _order, topPos);
			if(_order > 0){
				let orderTopPos = parseInt(commentCards[_order].style.top.substring(0, commentCards[_order].style.top.length - 2)); 
				moveDownToTopPos(commentCards, _order - 1, orderTopPos);
			}
		}
		if(_order == commentCards.length){
			moveDownToTopPos(commentCards, _order - 1, endPos);
		}
	}
	// Remove the annotation
	PDFJSAnnotate.getStoreAdapter().deleteAnnotation(documentId, _annotation.uuid)
		.then((isDeleted) => {
			if(isDeleted){
				//remove highlight in UI
				document.querySelector("g[data-pdf-annotate-id='"+ _annotation.uuid +"']").remove();
				_annotation = null;
			}
      
		});
}

export async function addCommentText(commentContent, documentId){
	let commentWrapper = document.getElementById('add-new-comment');
	let topPos = parseInt(commentWrapper.style.top.substring(0, commentWrapper.style.top.length - 2));
	let annotation = _annotation;
	return await PDFJSAnnotate.getStoreAdapter().addComment(documentId,
		annotation,
		commentContent,
		_selectedText,
		topPos);
}

export async function commentText(){
	// Highlight the text first
	await highlightText('comment-highlight');
	// Update positions of comment boxes
	updateCommentPositions();
	// Display add new form
	displayAddNewCommentForm();
	event.stopPropagation();
}
/**
 * Enable rect behavior
 */
export function enableSelect(type) {
	_type = type;
  
	if (_enabled) { return; }

	_enabled = true;
	document.addEventListener('mouseup', handleDocumentMouseup);
	document.addEventListener('mousedown', handleDocumentMousedown);
	document.addEventListener('keyup', handleDocumentKeyup);
	addEventListener('annotation:click', handleCommentAnnotationClick);

	let commentCards = document.querySelectorAll('.comment-card');
	commentCards.forEach(item => {
		item.addEventListener('click', handleCommentCardClick);
	});
  
	//document.getElementById("restore").addEventListener("click", restoreCommentPositions);
	window.addEventListener('beforeunload', restoreCommentPositions);
}

function handleCommentCardClick(){
	let highlightId = this.getAttribute('highlight-id');
	let highlight = document.querySelector("g[data-pdf-annotate-id='"+ highlightId +"']");
	handleCommentAnnotationClick(highlight);
}

function unselectHighlightComment(){
	let selectedHighlight = document.getElementsByClassName('comment-highlight-selected');
	if(selectedHighlight.length > 0)
		selectedHighlight[0].classList.remove('comment-highlight-selected');
	let selectedComment = document.getElementsByClassName('comment-card-selected');
	if(selectedComment.length > 0)
		selectedComment[0].classList.remove('comment-card-selected');
}

async function restoreCommentPositions(e){
	let documentId = document.getElementById('viewer').getAttribute('document-id');
	let highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']");
	handleCommentAnnotationClick(highlights[0]);
	let comments = await updateCommentAnnotations(documentId);
	console.log(comments);
	await PDFJSAnnotate.getStoreAdapter().updateComments(documentId, comments);
}

async function updateCommentAnnotations(documentId){
	let comments = await PDFJSAnnotate.getStoreAdapter().getComments(documentId);
	comments.forEach(comment => {
		let thisCard = document.querySelector(".comment-card[highlight-id='"+ comment.uuid +"']");
		comment.topPosition = parseInt(thisCard.getAttribute('top-position'));
	});
	return comments;
}

function handleCommentAnnotationClick(target){
	let text = window
		.getSelection()
		.toString()
		.trim();
	// Display text tool bar if text is selected.
	if (!text){
		let type = target.getAttribute('data-pdf-annotate-type');
		if(type == 'comment-highlight'){
			let selectedHighlight = document.getElementsByClassName('comment-highlight-selected');
			if(selectedHighlight.length > 0)
				selectedHighlight[0].classList.remove('comment-highlight-selected');
			target.classList.add('comment-highlight-selected');

			let commentCards = document.querySelectorAll('.comment-card');
			let commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']");
			let highlightArr = Array.prototype.slice.call(commentHighlights);
			// Sort the highlights
			highlightArr.sort(compareTopAttributes);
			// Get order of the current highlight
			_order = highlightArr.findIndex(item => {
				return item.dataset.pdfAnnotateId == target.getAttribute('data-pdf-annotate-id');
			});
      
			let gTop = parseInt(target.getAttribute('top')) - 35;
			let svgHeight = parseInt(target.getAttribute('page-height'));
			let svgPageNum = parseInt(target.getAttribute('page-num'));
			if(svgPageNum > 1)
				gTop += ((svgPageNum - 1) * svgHeight);

			let cTop = parseInt(commentCards[_order].style.top.substring(0, commentCards[_order].style.top.length - 2));
      
			// remove current selected comment card class if any
			let selected = document.getElementsByClassName('comment-card-selected');
			if(selected.length > 0)
				selected[0].classList.remove('comment-card-selected');
			// Set this card as selected
			commentCards[_order].classList.add('comment-card-selected');

			if(cTop != gTop){
				// Move the comment up to gTop
				commentCards[_order].style.top = gTop + 'px';
				commentCards[_order].setAttribute('top-position', gTop);

        
				let selected = document.getElementsByClassName('comment-card-selected');
				if(selected.length > 0)
					selected[0].classList.remove('comment-card-selected');
				commentCards[_order].classList.add('comment-card-selected');
				let endPos = gTop + commentCards[_order].offsetHeight;
				if(cTop > gTop){
					// Move up other uppen comments
					if(_order > 0)
						moveUpFromTopPos(commentCards, _order - 1, gTop);
					// Move up other lower comments
					if(_order < commentCards.length - 1)
						moveUpToEndPos(commentCards, _order + 1, endPos);
				} else if(cTop < gTop){
					// Move down other uppen comments
					if(_order > 0)
						moveDownToTopPos(commentCards, _order - 1, gTop);
					// Move down other lower comments
					if(_order < commentCards.length - 1)
						moveDownFromEndPos(commentCards, _order + 1, endPos);
				}
			}
		}
	}
}
/**
 * Disable rect behavior
 */
export function disableSelect() {
	if (!_enabled) { return; }

	_enabled = false;
	document.removeEventListener('mouseup', handleDocumentMouseup);
	document.removeEventListener('mousedown', handleDocumentMousedown);
	document.removeEventListener('keyup', handleDocumentKeyup);
}

