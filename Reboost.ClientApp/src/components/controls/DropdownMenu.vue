<template>
  <div class="c-dropdown-menu">
    <div class="c-dropdown-menu--title" @click="toggleMenu()">
      <span>{{ tittle }}</span>
      <i class="el-icon-arrow-down el-icon--right" />
    </div>
    <div ref="menu" class="c-dropdown-menu--menu">
      <ul class="c-dropdown-menu--menu--inner">
        <li v-for="(item, index) in data" :key="index" class="c-dropdown-menu--menu-item">
          <input :id="generateId(item.text)" v-model="item.checked" type="checkbox">
          <label :for="generateId(item.text)">{{ item.text }}</label>
        </li>
      </ul>
      <div class="c-dropdown-menu--footer">
        <a style="margin-right: 10px" @click="confirm()">Confirm</a>
        <a @click="reset()">Reset</a>
      </div>
    </div>

  </div>
</template>
<script>
export default {
  model: {
    prop: 'data',
    event: ['confirm,', 'reset']
  },
  props: { data: { type: Array, default: () => [] }, tittle: { type: String, default: 'Unknown' }},
  mounted() {
    document.addEventListener('click', (e) => {
      let exist = false
      for (const el of e.path) {
        if (el.classList && el.className == 'c-dropdown-menu') {
          exist = true
          break
        }
      }
      if (!exist) {
        this.toggleMenu('hide')
      }
    })
  },
  methods: {
    generateId(text) {
      let hash = 0
      if (text.length == 0) return hash
      for (let i = 0; i < text.length; i++) {
        const char = text.charCodeAt(i)
        hash = (hash << 5) - hash + char
        hash = hash & hash // Convert to 32bit integer
      }

      return hash
      // return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      //   var r = Math.random() * 16 | 0; var v = c == 'x' ? r : (r & 0x3 | 0x8)
      //   return v.toString(16)
      // })
    },
    toggleMenu(visibility) {
      document.getElementsByClassName('c-dropdown-menu--menu').forEach(element => {
        element.classList.remove('show')
      })

      if (visibility == 'show') {
        this.$refs.menu.classList.add('show')
      } else if (visibility == 'hide') {
        this.$refs.menu.classList.remove('show')
      } else {
        if (this.$refs.menu.classList.contains('show')) {
          this.$refs.menu.classList.remove('show')
        } else {
          this.$refs.menu.classList.add('show')
        }
      }
    },
    confirm() {
      this.$emit('confirm', this.data)
      this.toggleMenu('hide')
    },
    reset() {
      for (const item of this.data) {
        item.checked = false
      }
      this.$emit('reset')
    }
  }
}
</script>
<style scoped>
.c-dropdown-menu{
      display: inherit;
    position: relative;
}
.c-dropdown-menu--title{
  cursor: pointer;
  user-select: none;
}
.c-dropdown-menu--footer{
    text-align: right;
    padding: 5px 20px;
    border-top: 1px solid #dedcdc;
    display: inline-block;
    white-space: nowrap;
    font-weight: bold;
    width: 100%;
}
.c-dropdown-menu--footer a{
  cursor: pointer;
  transition: none;
}
.c-dropdown-menu--footer a:hover{
  color: #409EFF !important;
}

.c-dropdown-menu--menu{
    display: none;
    position: absolute;
    margin: 5px 0px 0px 0px;
    /* padding: 0; */
    top: 100%;
    right: 0px;
    -webkit-box-shadow: 0 2px 12px 0 rgba(0,0,0,.1);
    box-shadow: 0 2px 12px 0 rgba(0,0,0,.1);
    /* padding: 5px 0px; */
    background-color: white;
}
.c-dropdown-menu--menu.show{
  display: block;
}
.c-dropdown-menu--menu--inner{
    margin: 0;
    padding: 10px 0px;
}
.c-dropdown-menu--menu-item{
    display: inherit;
    margin: 0;
    padding: 3px 20px 3px 20px;
    white-space: nowrap;
    display: flex;
    align-items: center;
}
.c-dropdown-menu--menu-item label{
    margin-bottom: 0px !important;
    font-size: inherit !important;
    font-weight: inherit !important;
    user-select: none;
    margin-left: 10px;
    cursor: pointer;
}
.c-dropdown-menu--menu-item input[type=checkbox]{
  cursor: pointer;
}
.c-dropdown-menu--menu-item input[type=checkbox]:checked + label {
  color: #409EFF;
}
.c-dropdown-menu--menu-item:hover{

}
</style>
