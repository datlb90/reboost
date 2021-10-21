import Vue from 'vue'
import { MLInstaller, MLCreate, MLanguage } from 'vue-multilanguage'
import langES from './lang/en/en'
import langVN from './lang/vn/vn'

Vue.use(MLInstaller)

export default new MLCreate({
  initial: 'english',
  //   save: process.env.NODE_ENV === 'production',
  languages: [
    new MLanguage('english').create(langES),

    new MLanguage('vietnamese').create(langVN)
  ]
})
