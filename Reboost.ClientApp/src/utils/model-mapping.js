export const map = (src, des) =>{
    let _des = {...des};
    for(let key in src){
        _des[key] = src[key];
    }
    return _des;
}