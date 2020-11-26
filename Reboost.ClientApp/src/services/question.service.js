import http from '@/utils/axios'

const questionService = {
    getAll(){
        return http.get('/Questions').then(rs => rs.data)
    },
    getById(id){
        return http.get(`/questions/getById/${id}`).then(rs => rs.data)
    },
    getCountQuestionByTasks(){
        return http.get('/questions/summary').then(rs => rs.data)
    }
}

export default questionService;