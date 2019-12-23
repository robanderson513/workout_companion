<template>
  <div class="content">
    <h2>Remove Machine</h2>
    <div v-show="!deleteMachine" >
      <machine-list @machineNumber="getNumber" />
    </div>
    <div v-show="deleteMachine" class="delete_message">
      <h2> Do you wish to delete this machine? </h2>
      <div class="btn_container">
      <b-button  variant="outline-secondary" @click="clearData"> No </b-button>
      <b-button class="btn"  variant="outline-primary" @click="removeMachine"> Yes </b-button>
      
      </div>
    </div>
  </div>
</template>

<script>
import MachineList from "../components/weightMachine/MachineList";

import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      machineId: null,
      deleteMachine: false
    }
  },
  components: {
    MachineList
  },
  methods: {
    getNumber(id) {
      this.machineId = id;
      this.deleteMachine = true;
    },
    removeMachine(){
      apiSvc.removeMachine(this.machineId);
      this.$router.push({ name: "machines" });
    },
    clearData(){
      this.machineId= null;
      this.deleteMachine = false;
    }
  }
};
</script>

<style>
.darkenPage {
  background: black;
}
.delete_message{
  border: 1px solid lightgray;
  border-radius: 5px;
  width: 80%;
  margin: auto;
  padding: 10px;
}
.btn{
  width: 47%;
  margin-right: 10px;
}
/*mobile size */
@media screen and (max-width: 600px) {
.btn{
  width: 100%;
  margin-top: 10px;
}

}
</style>
