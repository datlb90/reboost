export const map = (src, des) => {
  const _des = { ...des }
  for (const key in src) {
    _des[key] = src[key]
  }
  return _des
}
