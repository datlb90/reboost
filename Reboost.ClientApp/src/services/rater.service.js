import http from '@/utils/axios'

const raterService = {
    getAll(){
        return http.get('/rater').then(rs => rs.data);
    },
    getById(id){
        return http.get(`/rater/${id}`).then(rs=> rs.data);
    },
    insert(data){
        console.log(data)
        let formData = new FormData();
        for(let key in data){
            if(key == "iDCardPhotos"){
                for(let f of data[key]){
                    formData.append(key, f);
                }
            }
            else{
                formData.set(key, data[key]);
            }
            //let _key = key.substring(0, 1).toUpperCase() + key.substring(1, key.length);
            //console.log("KEYS", _key);
            
        }
        return http.post('/rater/create', formData, { headers: {
            'Content-Type': 'multipart/form-data'
          } }).then(rs => rs.data);
    },
    update(data){
        data.iELTSCertificatePhotos = [];
        data.iDCardPhotos = [];
        let formData = new FormData();
        for(let key in data){
            formData.set(key, data[key]);
        }
        return http.post('/rater/update', formData, { headers: {
            'Content-Type': 'multipart/form-data'
          } }).then(rs => rs.data);
    }
}


export default raterService;