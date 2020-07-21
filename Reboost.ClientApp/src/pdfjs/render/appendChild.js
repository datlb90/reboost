import objectAssign from 'object-assign';
import renderLine from './renderLine';
import renderPath from './renderPath';
import renderPoint from './renderPoint';
import renderRect from './renderRect';
import renderText from './renderText';
//import { wrap } from 'module';

const isFirefox = /firefox/i.test(navigator.userAgent);

/**
 * Get the x/y translation to be used for transforming the annotations
 * based on the rotation of the viewport.
 *
 * @param {Object} viewport The viewport data from the page
 * @return {Object}
 */
function getTranslation(viewport) {
	let x;
	let y;

	// Modulus 360 on the rotation so that we only
	// have to worry about four possible values.
	switch(viewport.rotation % 360) {
		case 0:
			x = y = 0;
			break;
		case 90:
			x = 0;
			y = (viewport.width / viewport.scale) * -1;
			break;
		case 180:
			x = (viewport.width / viewport.scale) * -1;
			y = (viewport.height / viewport.scale) * -1;
			break;
		case 270:
			x = (viewport.height / viewport.scale) * -1;
			y = 0;
			break;
	}

	return { x, y };
}

/**
 * Transform the rotation and scale of a node using SVG's native transform attribute.
 *
 * @param {Node} node The node to be transformed
 * @param {Object} viewport The page's viewport data
 * @return {Node}
 */
function transform(node, viewport) {
	let trans = getTranslation(viewport);

	// Let SVG natively transform the element
	node.setAttribute('transform', `scale(${viewport.scale}) rotate(${viewport.rotation}) translate(${trans.x}, ${trans.y})`);
  
	// Manually adjust x/y for nested SVG nodes
	if (!isFirefox && node.nodeName.toLowerCase() === 'svg') {
		node.setAttribute('x', parseInt(node.getAttribute('x'), 10) * viewport.scale);
		node.setAttribute('y', parseInt(node.getAttribute('y'), 10) * viewport.scale);

		let x = parseInt(node.getAttribute('x', 10));
		let y = parseInt(node.getAttribute('y', 10));
		let width = parseInt(node.getAttribute('width'), 10);
		let height = parseInt(node.getAttribute('height'), 10);
		let path = node.querySelector('path');
		let svg = path.parentNode;
   
		// Scale width/height
		[node, svg, path, node.querySelector('rect')].forEach((n) => {
			n.setAttribute('width', parseInt(n.getAttribute('width'), 10) * viewport.scale);
			n.setAttribute('height', parseInt(n.getAttribute('height'), 10) * viewport.scale);
		});

		// Transform path but keep scale at 100% since it will be handled natively
		transform(path, objectAssign({}, viewport, { scale: 1 }));
    
		switch(viewport.rotation % 360) {
			case 90:
				node.setAttribute('x', viewport.width - y - width);
				node.setAttribute('y', x);
				svg.setAttribute('x', 1);
				svg.setAttribute('y', 0);
				break;
			case 180:
				node.setAttribute('x', viewport.width - x - width);
				node.setAttribute('y', viewport.height - y - height);
				svg.setAttribute('y', 2);
				break;
			case 270:
				node.setAttribute('x', y);
				node.setAttribute('y', viewport.height - x - height);
				svg.setAttribute('x', -1);
				svg.setAttribute('y', 0);
				break;
		}
	}

	return node;
}

/**
 * Append an annotation as a child of an SVG.
 *
 * @param {SVGElement} svg The SVG element to append the annotation to
 * @param {Object} annotation The annotation definition to render and append
 * @param {Object} viewport The page's viewport data
 * @return {SVGElement} A node that was created and appended by this function
 */
export default function appendChild(svg, annotation, viewport) {
	if (!viewport) {
		viewport = JSON.parse(svg.getAttribute('data-pdf-annotate-viewport'));
	}
  
	let child;
	switch (annotation.type) {
		case 'area':
		case 'comment-highlight':
		case 'highlight':
			child = renderRect(annotation);
			break;
		case 'strikeout':
			child = renderLine(annotation);
			break;
		case 'point':
			child = renderPoint(annotation);
			break;
		case 'textbox':
			child = renderText(annotation);
			break;
		case 'drawing':
			child = renderPath(annotation);
			break;
	}

	// If no type was provided for an annotation it will result in node being null.
	// Skip appending/transforming if node doesn't exist.
	if (child) {
		// Set attributes
		child.setAttribute('data-pdf-annotate-id', annotation.uuid);
		child.setAttribute('data-pdf-annotate-type', annotation.type);
		child.setAttribute('aria-hidden', true);


		// INSERT the group element into its CORRECT order, not before its wrapper
		let wrapperNode = findWrapperNode(svg, child);
		if(wrapperNode != null)
			wrapperNode.before(transform(child, viewport));
		else
			svg.appendChild(transform(child, viewport));
	}

	return child;
}

function isArrayInArray(arr, childArr){
	let isWrapped = true;
	let arrString = arr.toString();
	for(let i = 0; i < childArr.length; i++){
		if(!arrString.includes(childArr[i])){
			isWrapped = false;
			break;
		}
	}
	return isWrapped;
}

function findWrapperNode(svg, node){
	let rects = node.childNodes;
	if(rects.length > 0){
		let wrapperRects = svg.querySelectorAll("rect[y='" + rects[0].getAttribute('y') + "']");
		if(wrapperRects.length > 0){
			if(rects.length == 1){
				if(wrapperRects[0].parentNode.childNodes.length == 1){
					let rectX = parseInt(rects[0].getAttribute('x'));
					let rectWidth = parseInt(rects[0].getAttribute('width'));
					let wrapperX = parseInt(wrapperRects[0].getAttribute('x'));
					let wrapperWidth = parseInt(wrapperRects[0].getAttribute('width'));
					if(rectX < wrapperX && rectWidth > wrapperWidth)
						return null;
				} 
			} else {
				// node has more than 1 rects
				let wrapper =  null;
				let rectsY = Array.prototype.slice.call(rects).map(function(r) { return r.getAttribute('y'); });
				// check all parents of wrapperRects
				for(let i = 0; i < wrapperRects.length; i++){
					let item = wrapperRects[i];
					let wrapperChildRects = item.parentNode.childNodes;
					if(rects.length <= wrapperChildRects.length){
						let wrapperY = Array.prototype.slice.call(wrapperChildRects).map(function(r) { return r.getAttribute('y'); });
						if(isArrayInArray(wrapperY, rectsY)){
							wrapper = item.parentNode;
							break;
						}
					}
				}
				return wrapper;
			}
			return wrapperRects[0].parentNode;
		} 
	}
	return null;
}
