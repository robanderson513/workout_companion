<template>
  <b-tabs justified>
    <b-tab title="Log Workout">
      <machine-log :machine="Machine" :visit="visit"/>
    </b-tab>
    <b-tab title="Machine Description">
      <machine-info :machine="Machine" />
    </b-tab>
  </b-tabs>
</template>

<script>
import MachineLog from "../components/weightMachine/MachineLog";
import MachineInfo from "../components/weightMachine/MachineInfo";

import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  props:{
    visit: Number
  },
  data() {
    return {
      Machine: {}
    };
  },
  components: {
    MachineLog,
    MachineInfo
  },
  created() {
    const machineId = this.$route.params.machine;
    apiSvc
      .getMachine(machineId)
      .then(data => {
        this.Machine = data;
      })
      .catch(error => {
        this.error = error.message;
      });
  }
};
</script>
<style></style>
