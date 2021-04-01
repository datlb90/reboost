import PDFJSAnnotate from '@/pdfjs/PDFJSAnnotate'
PDFJSAnnotate.setStoreAdapter(new PDFJSAnnotate.LocalStoreAdapter())
import {
  getMetadata,
  scaleDown
} from '@/pdfjs/UI/utils'
import appendChild from '@/pdfjs/render/appendChild'

export function highlightText(type, node) {
  const that = node
  if (!that.svg) {
    return
  }
  const boundingRect = that.boundingRect

  let color = 'FFFF00'
  if (type == 'strikeout') { color = localStorage.getItem('colorChosen') || 'FF0000' }
  if (type == 'hightlight') { color = localStorage.getItem('colorChosen') || 'FFFF00' }

  // Initialize the annotation
  const annotation = {
    type,
    color,
    rectangles: [...that.rects].map((r) => {
      let offset = 0
      if (type == 'strikeout') { offset = r.height / 2 }
      return scaleDown(that.svg, {
        y: r.top + offset - boundingRect.top,
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
    annotation.rectangles = that.removeMinHeigthRects(annotation.rectangles)
  }

  const { documentId, pageNumber } = getMetadata(that.svg)

  annotation.color = that.colorChosen
  annotation.top = parseInt(that.target.style.top.substring(0, that.target.style.top.length - 2))
  annotation.pageNum = pageNumber
  annotation.pageHeight = that.svg.getAttribute('height')
  that.annotation = annotation
  // Add the annotation
  PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
    .then((annotation) => {
      appendChild(that.svg, annotation)
      that.hideTextToolBar()
      that.hideTextToolGroup()
      that.hideRectToolBar()
      that.hideColorPickerTool()
      annotation.documentId = that.documentId
      if (type != 'comment-highlight') {
        var obj = {
          DocumentId: that.documentId,
          ReviewId: that.reviewId,
          Type: type,
          PageNum: annotation.pageNum,
          Top: annotation.top,
          Color: annotation.color,
          Uuid: annotation.uuid,
          Data: JSON.stringify(annotation)
        }
        that.$store.dispatch('review/addReviewAnnotation', obj).then(async rs => {
          if (that.$store.getters['review/getAddedAnnotation']) {
            annotation.id = that.$store.getters['review/getAddedAnnotation']['id']

            await PDFJSAnnotate.getStoreAdapter().editAnnotation(that.documentId, annotation.uuid, annotation)

            that.addToUndoList(annotation, 'add')
          }
        })
      }
    })
}
