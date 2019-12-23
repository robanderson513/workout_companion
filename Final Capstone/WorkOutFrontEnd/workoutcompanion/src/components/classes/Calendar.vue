<template>
  <div>
    <v-calendar
      class="calendar"
      title-position="left"
      :min-date="new Date()"
      :attributes="attr"
    />
  </div>
</template>

<script>
export default {
  props: {
    attr: Array
  },
  data() {
    return {};
  }
};
</script>

<style scoped>
.calendar {
  z-index: 1;
  width: 50%;
  margin: auto;
}
/*mobile size */
@media screen and (max-width: 600px) {
  .calendar {
    width: 90%;
    margin: auto;
  }
}
/*tablet size */
@media (min-width: 600px) and (max-width: 979px) {
  .calendar {
    width: 70%;
    margin: auto;
  }
}
</style>
