export const getFileExtension = (fileName) => {
  return (/[.]/.exec(fileName)) ? /[^.]+$/.exec(fileName)[0] : undefined
}
export const stof = (base64, fileName) => {
  var blob = new Blob([window.atob(base64.split(',')[1])], { type: 'image/png', encoding: 'utf-8' })
  return new File([blob], fileName, { type: 'image/png' })
}
export const base64ToArrayBuffer = (dataurl, fileName) => {
  const arr = dataurl.split(',')
  const mime = arr[0].match(/:(.*?);/)[1]
  const base64 = arr[1]

  console.log(mime)

  const binaryString = window.atob(base64)
  const binaryLen = binaryString.length
  const bytes = new Uint8Array(binaryLen)

  for (let i = 0; i < binaryLen; i++) {
    const ascii = binaryString.charCodeAt(i)
    bytes[i] = ascii
  }
  return new File([bytes], fileName, { type: mime })
}
export const urltoFile = (url, filename, mimeType) => {
  return (fetch(url)
    .then(function (res) { return res.arrayBuffer() })
    .then(function (buf) { return new File([buf], filename, { type: mimeType }) })
  )
}
export const dataURLtoFile = (dataurl, filename) => {
  var arr = dataurl.split(',')
  var mime = arr[0].match(/:(.*?);/)[1]
  var bstr = atob(arr[1])
  var n = bstr.length
  var u8arr = new Uint8Array(n)

  while (n--) {
    u8arr[n] = bstr.charCodeAt(n)
  }

  return new File([u8arr], filename, { type: mime })
}
export const b64toBlob = (b64Data, contentType, sliceSize) => {
  contentType = contentType || ''
  sliceSize = sliceSize || 512

  var byteCharacters = atob(b64Data)
  var byteArrays = []

  for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
    var slice = byteCharacters.slice(offset, offset + sliceSize)

    var byteNumbers = new Array(slice.length)
    for (var i = 0; i < slice.length; i++) {
      byteNumbers[i] = slice.charCodeAt(i)
    }

    var byteArray = new Uint8Array(byteNumbers)

    byteArrays.push(byteArray)
  }

  var blob = new Blob(byteArrays, { type: contentType })
  return blob
}

export const getBase64FromFile = (file) => {
  console.log(file)
  var reader = new FileReader()
  reader.readAsDataURL(file)
  reader.onload = function () {
    console.log(reader.result)
    return reader.result
  }
  reader.onerror = function (error) {
    console.log('Error: ', error)
  }
}
