/* eslint-disable no-unused-vars */
/* eslint-disable no-constant-condition */
import PDFJSAnnotate from '../PDFJSAnnotate'
import renderScreenReaderHints from '../a11y/renderScreenReaderHints'
import { DefaultTextLayerFactory } from 'pdf-dist/web/pdf_viewer.js'
// Template for creating a new page
const PAGE_TEMPLATE = `
  <div style="visibility: hidden;" class="page" data-loaded="false">
    <div class="canvasWrapper">
      <canvas></canvas>
		</div>
		<svg class="annotationLayer"></svg>
		<div class="textLayer"></div>
  </div>
`

/**
 * Create a new page to be appended to the DOM.
 *
 * @param {Number} pageNumber The page number that is being created
 * @return {HTMLElement}
 */
export function createPage(pageNumber) {
  const temp = document.createElement('div')
  temp.innerHTML = PAGE_TEMPLATE

  const page = temp.children[0]
  const canvas = page.querySelector('canvas')

  page.setAttribute('id', `pageContainer${pageNumber}`)
  page.setAttribute('data-page-number', pageNumber)

  canvas.mozOpaque = true
  canvas.setAttribute('id', `page${pageNumber}`)

  return page
}

/**
 * Render a page that has already been created.
 *
 * @param {Number} pageNumber The page number to be rendered
 * @param {Object} renderOptions The options for rendering
 * @return {Promise} Settled once rendering has completed
 *  A settled Promise will be either:
 *    - fulfilled: [pdfPage, annotations]
 *    - rejected: Error
 */
export async function renderPage(pageNumber, renderOptions) {
  const {
    documentId,
    pdfDocument,
    scale,
    rotate
  } = renderOptions

  // Load the page and annotations
  return Promise.all([
    pdfDocument.getPage(pageNumber),
    PDFJSAnnotate.getAnnotations(documentId, pageNumber)
  ]).then(async([pdfPage, annotations]) => {
    const page = document.getElementById(`pageContainer${pageNumber}`)
    const svg = page.querySelector('.annotationLayer')
    const canvas = page.querySelector('.canvasWrapper canvas')
    const canvasContext = canvas.getContext('2d', { alpha: false })
    const viewport = pdfPage.getViewport({ scale, rotate })
    const transform = scalePage(pageNumber, viewport, canvasContext)
    await pdfPage.render({ canvasContext, viewport, transform }).promise
    await PDFJSAnnotate.render(svg, viewport, annotations).promise
    const textContent = await pdfPage.getTextContent({ normalizeWhitespace: true })
    // Render text layer for a11y of text content
    const textLayer = page.querySelector('.textLayer')
    const textLayerFactory = new DefaultTextLayerFactory()
    const textLayerBuilder = textLayerFactory.createTextLayerBuilder(textLayer, pageNumber - 1, viewport)
    textLayerBuilder.setTextContent(textContent)
    textLayerBuilder.render()
    // Enable a11y for annotations
    // Timeout is needed to wait for `textLayerBuilder.render`
    setTimeout(() => {
      try {
        renderScreenReaderHints(annotations.annotations)
      } catch (e) {
        console.log(e)
      }
    })

    return [pdfPage, annotations]
    // return pdfPage.getTextContent({ normalizeWhitespace: true })
    // 	.then((textContent) => {
    // 	return new Promise((resolve, reject) => {
    // 		// Render text layer for a11y of text content
    // 		let textLayer = page.querySelector(`.textLayer`);
    // 		let textLayerFactory = new DefaultTextLayerFactory();
    // 		let textLayerBuilder = textLayerFactory.createTextLayerBuilder(textLayer, pageNumber - 1, viewport);
    // 		textLayerBuilder.setTextContent(textContent);
    // 		textLayerBuilder.render();

    // 		// Enable a11y for annotations
    // 		// Timeout is needed to wait for `textLayerBuilder.render`
    // 		setTimeout(() => {
    // 			try {
    // 				renderScreenReaderHints(annotations.annotations);
    // 				resolve();
    // 			} catch (e) {
    // 				reject(e);
    // 			}
    // 		});
    // 	});
    // });

    // // Render the page
    // return Promise.all([
    // 	pdfPage.render({ canvasContext, viewport, transform }),
    // 	PDFJSAnnotate.render(svg, viewport, annotations)
    // ]).then(() => {
    // 	// Text content is needed for a11y, but is also necessary for creating
    // 	// highlight and strikeout annotations which require selecting text.
    // 	return pdfPage.getTextContent({ normalizeWhitespace: true }).then((textContent) => {
    // 		return new Promise((resolve, reject) => {
    // 			// Render text layer for a11y of text content
    // 			let textLayer = page.querySelector(`.textLayer`);
    // 			let textLayerFactory = new DefaultTextLayerFactory();
    // 			let textLayerBuilder = textLayerFactory.createTextLayerBuilder(textLayer, pageNumber - 1, viewport);
    // 			textLayerBuilder.setTextContent(textContent);
    // 			textLayerBuilder.render();

    // 			// Enable a11y for annotations
    // 			// Timeout is needed to wait for `textLayerBuilder.render`
    // 			setTimeout(() => {
    // 				try {
    // 					renderScreenReaderHints(annotations.annotations);
    // 					resolve();
    // 				} catch (e) {
    // 					reject(e);
    // 				}
    // 			});
    // 		});
    // 	});
    // }).then(() => {
    // 	// Indicate that the page was loaded
    // 	page.setAttribute('data-loaded', 'true');

    // 	return [pdfPage, annotations];
    // });
  })
}

/**
 * Scale the elements of a page.
 *
 * @param {Number} pageNumber The page number to be scaled
 * @param {Object} viewport The viewport of the PDF page (see pdfPage.getViewport(scale, rotate))
 * @param {Object} context The canvas context that the PDF page is rendered to
 * @return {Array} The transform data for rendering the PDF page
 */
function scalePage(pageNumber, viewport, context) {
  const page = document.getElementById(`pageContainer${pageNumber}`)
  const canvas = page.querySelector('.canvasWrapper canvas')
  const svg = page.querySelector('.annotationLayer')
  const wrapper = page.querySelector('.canvasWrapper')
  const textLayer = page.querySelector('.textLayer')
  const outputScale = getOutputScale(context)
  const transform = !outputScale.scaled ? null : [outputScale.sx, 0, 0, outputScale.sy, 0, 0]
  const sfx = approximateFraction(outputScale.sx)
  const sfy = approximateFraction(outputScale.sy)

  // Adjust width/height for scale
  page.style.visibility = ''
  canvas.width = roundToDivide(viewport.width * outputScale.sx, sfx[0])
  canvas.height = roundToDivide(viewport.height * outputScale.sy, sfy[0])
  canvas.style.width = roundToDivide(viewport.width, sfx[1]) + 'px'
  canvas.style.height = roundToDivide(viewport.height, sfx[1]) + 'px'
  svg.setAttribute('width', viewport.width)
  svg.setAttribute('height', viewport.height)
  svg.style.width = `${viewport.width}px`
  svg.style.height = `${viewport.height}px`
  page.style.width = `${viewport.width}px`
  page.style.height = `${viewport.height}px`
  wrapper.style.width = `${viewport.width}px`
  wrapper.style.height = `${viewport.height}px`
  textLayer.style.width = `${viewport.width}px`
  textLayer.style.height = `${viewport.height}px`

  return transform
}

/**
 * The following methods are taken from mozilla/pdf.js and as such fall under
 * the Apache License (http://www.apache.org/licenses/).
 *
 * Original source can be found at mozilla/pdf.js:
 * https://github.com/mozilla/pdf.js/blob/master/web/ui_utils.js
 */

/**
 * Approximates a float number as a fraction using Farey sequence (max order
 * of 8).
 *
 * @param {Number} x Positive float number
 * @return {Array} Estimated fraction: the first array item is a numerator,
 *                 the second one is a denominator.
 */
function approximateFraction(x) {
  // Fast path for int numbers or their inversions.
  if (Math.floor(x) === x) {
    return [x, 1]
  }

  const xinv = 1 / x
  const limit = 8
  if (xinv > limit) {
    return [1, limit]
  } else if (Math.floor(xinv) === xinv) {
    return [1, xinv]
  }

  const x_ = x > 1 ? xinv : x

  // a/b and c/d are neighbours in Farey sequence.
  let a = 0; let b = 1; let c = 1; let d = 1

  // Limit search to order 8.
  while (true) {
    // Generating next term in sequence (order of q).
    const p = a + c; const q = b + d
    if (q > limit) {
      break
    }
    if (x_ <= p / q) {
      c = p; d = q
    } else {
      a = p; b = q
    }
  }

  // Select closest of neighbours to x.
  if (x_ - a / b < c / d - x_) {
    return x_ === x ? [a, b] : [b, a]
  } else {
    return x_ === x ? [c, d] : [d, c]
  }
}

function getOutputScale(ctx) {
  const devicePixelRatio = window.devicePixelRatio || 1
  const backingStoreRatio = ctx.webkitBackingStorePixelRatio ||
		ctx.mozBackingStorePixelRatio ||
		ctx.msBackingStorePixelRatio ||
		ctx.oBackingStorePixelRatio ||
		ctx.backingStorePixelRatio || 1
  const pixelRatio = devicePixelRatio / backingStoreRatio
  return {
    sx: pixelRatio,
    sy: pixelRatio,
    scaled: pixelRatio !== 1
  }
}

function roundToDivide(x, div) {
  const r = x % div
  return r === 0 ? x : Math.round(x - r + div)
}
