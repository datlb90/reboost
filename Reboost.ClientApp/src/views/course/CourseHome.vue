<!-- CourseHomeComponent.vue -->
<template>
  <div>
    <h1>Welcome to {{ courseTitle }}</h1>
    <!-- Course overview content -->
  </div>
</template>

  <script>
  export default {
    computed: {
      courseTitle() {
        return this.$route.params.courseName.replace(/-/g, ' ')
      }
    }
  }
  </script>
