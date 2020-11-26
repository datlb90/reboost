export const getFileExtension = (fileName) => {
  return (/[.]/.exec(fileName)) ? /[^.]+$/.exec(fileName)[0] : undefined
}
export const stof = (base64, fileName) => {
  console.log('STRING TO FILE', base64)
  var blob = new Blob([window.atob(base64.split(',')[1])], { type: 'image/png', encoding: 'utf-8' })
  return new File([blob], fileName, { type: 'image/png' })
}
