<template>
  <div>
    <machine-list @machineNumber="routeMachine" />
  </div>
</template>

<script>
import MachineList from "../components/weightMachine/MachineList";

export default {
  props:{
  },
  data() {
    return {
      user: {}
    };
  },
  components: {
    MachineList
  },
  methods: {
    routeMachine(data) {
      this.$router.push({ name: "machine", params: { machine: data } });
    }
  }
};
</script>

<style scoped></style>
