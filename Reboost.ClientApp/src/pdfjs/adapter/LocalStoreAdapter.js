/* eslint-disable no-unused-vars */
import uuid from '../utils/uuid';
import StoreAdapter from './StoreAdapter';

// StoreAdapter for working with localStorage
// This is ideal for testing, examples, and prototyping
export default class LocalStoreAdapter extends StoreAdapter {
	constructor() {
		super({
			getAnnotations(documentId, pageNumber) {
				return new Promise((resolve, reject) => {
					let annotations = getAnnotations(documentId).filter((i) => {
						return i.page === pageNumber && i.class === 'Annotation';
					});

					resolve({
						documentId,
						pageNumber,
						annotations
					});
				});
			},

			getAnnotation(documentId, annotationId) {
				return Promise.resolve(getAnnotations(documentId)[findAnnotation(documentId, annotationId)]);
			},

			addAnnotation(documentId, pageNumber, annotation) {
				return new Promise((resolve, reject) => {
					annotation.class = 'Annotation';
					annotation.uuid = uuid();
					annotation.page = pageNumber;

					let annotations = getAnnotations(documentId);
					annotations.push(annotation);
					updateAnnotations(documentId, annotations);

					resolve(annotation);
				});
			},

			editAnnotation(documentId, annotationId, annotation) {
				return new Promise((resolve, reject) => {
					let annotations = getAnnotations(documentId);
					annotations[findAnnotation(documentId, annotationId)] = annotation;
					updateAnnotations(documentId, annotations);

					resolve(annotation);
				});
			},

			deleteAnnotation(documentId, annotationId) {
				return new Promise((resolve, reject) => {
					let index = findAnnotation(documentId, annotationId);
					if (index > -1) {
						let annotations = getAnnotations(documentId);
						annotations.splice(index, 1);
						updateAnnotations(documentId, annotations);
					}

					resolve(true);
				});
			},

			getComments(documentId) {
				return new Promise((resolve, reject) => {
					resolve(getAnnotations(documentId).filter((i) => {
						return i.class === 'Comment';
					}));
				});
			},

			addComment(documentId, annotation, content, selectedText, topPos) {
				return new Promise((resolve, reject) => {
					let comment = {
						class: 'Comment',
						uuid: annotation.uuid,
						annotation: annotation,
						content: content,
						text: selectedText,
						topPosition: topPos
					};

					let annotations = getAnnotations(documentId);
					annotations.push(comment);
					updateAnnotations(documentId, annotations);

					resolve(comment);
				});
			},

			updateComments(documentId, comments){
				return new Promise((resolve, reject) => {
					let annotations = getAnnotations(documentId);
					for (let i=0, l=annotations.length; i<l; i++) {
						if(annotations[i].class == 'Comment'){
							let thisComment = comments.filter(item => {return (item.uuid == annotations[i].uuid);});
							if(thisComment && thisComment.length > 0)
								annotations[i] = thisComment[0];
						}
					}
					updateAnnotations(documentId, annotations);
					resolve(true);
				});
        
			},
			deleteComment(documentId, commentId) {
				return new Promise((resolve, reject) => {
					getAnnotations(documentId);
					let index = -1;
					let annotations = getAnnotations(documentId);
					for (let i=0, l=annotations.length; i<l; i++) {
						if (annotations[i].uuid === commentId) {
							index = i;
							break;
						}
					}

					if (index > -1) {
						annotations.splice(index, 1);
						updateAnnotations(documentId, annotations);
					}

					resolve(true);
				});
			}
		});
	}
}

function getAnnotations(documentId) {
	return JSON.parse(localStorage.getItem(`${documentId}/annotations`)) || [];
}

function updateAnnotations(documentId, annotations) {
	localStorage.setItem(`${documentId}/annotations`, JSON.stringify(annotations));
}

function findAnnotation(documentId, annotationId) {
	let index = -1;
	let annotations = getAnnotations(documentId);
	for (let i=0, l=annotations.length; i<l; i++) {
		if (annotations[i].uuid === annotationId) {
			index = i;
			break;
		}
	}
	return index;
}
