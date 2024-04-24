/* eslint-disable no-unused-vars */
import { type } from 'jquery'
import uuid from '../utils/uuid'
import StoreAdapter from './StoreAdapter'

// StoreAdapter for working with localStorage
// This is ideal for testing, examples, and prototyping
export default class LocalStoreAdapter extends StoreAdapter {
  constructor() {
    super({
      getAnnotations(documentId, pageNumber) {
        return new Promise((resolve, reject) => {
          const annotations = getAnnotations(documentId).filter((i) => {
            return i.page === pageNumber && i.class === 'Annotation'
          })
          resolve({
            documentId,
            pageNumber,
            annotations
          })
        })
      },

      getAnnotation(documentId, annotationId) {
        return Promise.resolve(getAnnotations(documentId)[findAnnotation(documentId, annotationId)])
      },

      addAnnotation(documentId, pageNumber, annotation) {
        return new Promise((resolve, reject) => {
          annotation.class = 'Annotation'
          annotation.uuid = uuid()
          annotation.page = pageNumber

          const annotations = getAnnotations(documentId)
          annotations.push(annotation)
          updateAnnotations(documentId, annotations)
          resolve(annotation)
        })
      },

      editAnnotation(documentId, annotationId, annotation) {
        return new Promise(async (resolve, reject) => {
          var annotations = getAnnotations(documentId, annotation.pageNum)
          annotations[findAnnotation(documentId, annotationId)] = annotation
          await updateAnnotations(documentId, annotations)
          resolve(annotation)
        })
      },

      deleteAnnotation(documentId, annotationId) {
        return new Promise((resolve, reject) => {
          const index = findAnnotation(documentId, annotationId)
          const rs = getAnnotations(documentId)[index]
          if (index > -1) {
            const annotations = getAnnotations(documentId)
            annotations.splice(index, 1)
            updateAnnotations(documentId, annotations)
          }
          resolve(rs)
        })
      },

      deleteLastAnnotation(documentId) {
        return new Promise((resolve, reject) => {
          const annotations = getAnnotations(documentId)
          const index = annotations.length - 1
          const rs = getAnnotations(documentId)[index]
          if (index > -1) {
            const annotations = getAnnotations(documentId)
            annotations.splice(index, 1)
            updateAnnotations(documentId, annotations)
          }
          resolve(rs)
        })
      },

      getComments(documentId) {
        return new Promise((resolve, reject) => {
          resolve(getAnnotations(documentId).filter((i) => {
            return i.class === 'Comment'
          }))
        })
      },

      addComment(documentId, annotation, text, type, category, comment, topPos, id) {
        return new Promise((resolve, reject) => {
          const newComment = {
            class: 'Comment',
            uuid: annotation.uuid,
            annotation: annotation,
            comment: comment,
            text: text,
            type: type,
            category: category,
            topPosition: topPos,
            id: id
          }
          const annotations = getAnnotations(documentId)
          annotations.push(newComment)
          updateAnnotations(documentId, annotations)
          resolve(newComment)
        })
      },

      updateComments(documentId, comments) {
        return new Promise((resolve, reject) => {
          const annotations = getAnnotations(documentId)
          for (let i = 0, l = annotations.length; i < l; i++) {
            if (annotations[i].class == 'Comment') {
              const thisComment = comments.filter(item => { return (item.uuid == annotations[i].uuid) })
              if (thisComment && thisComment.length > 0) { annotations[i] = thisComment[0] }
            }
          }
          updateAnnotations(documentId, annotations)
          resolve(true)
        })
      },

      deleteComment(documentId, commentId, status) {
        return new Promise((resolve, reject) => {
          getAnnotations(documentId)
          const index = []
          const annotations = getAnnotations(documentId)
          for (let i = 0, l = annotations.length; i < l; i++) {
            if (annotations[i].uuid === commentId) {
              if (!status && annotations[i].type == 'comment-area') { continue }
              index.push(i)
            }
          }

          var deleteList = []
          if (index.length > 0) {
            index.forEach(id => {
              deleteList.push(annotations[id])
            })
            deleteList.forEach(item => {
              const index = annotations.indexOf(item)
              if (index > -1) {
                annotations.splice(index, 1)
              }
            })
          }
          updateAnnotations(documentId, annotations)
          resolve(deleteList.filter(item => { return (item.class == 'Comment') }))
        })
      },

      clearAnnotations(docId) { localStorage.removeItem(`${docId}/annotations`) },

      loadAnnotations(docId, { annotations, comments }) {
        localStorage.setItem(`${docId}/annotations`, JSON.stringify(annotations))
        comments.forEach(cmt => {
          this.addComment(cmt.documentId, cmt.annotation, cmt.text, cmt.type, cmt.category, cmt.comment, cmt.topPosition, cmt.id)
        })
      }
    })
  }
}

function getAnnotations(documentId) {
  return JSON.parse(localStorage.getItem(`${documentId}/annotations`)) || []
}

function updateAnnotations(documentId, annotations) {
  localStorage.setItem(`${documentId}/annotations`, JSON.stringify(annotations))
}

function findAnnotation(documentId, annotationId) {
  let index = -1
  const annotations = getAnnotations(documentId)
  for (let i = 0, l = annotations.length; i < l; i++) {
    if (annotations[i].uuid === annotationId) {
      index = i
      break
    }
  }
  return index
}
