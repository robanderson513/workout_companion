<template>
  <div>
    <div class="weight_container">
      <div
        v-for="machine in MachineList"
        :key="machine.id"
        class="tile"
        @click="machineData(machine.id)">

        <img class="machine_img" :src="machine.imageURL"/>
        <div class="machine_name">
          <h5>{{ machine.name }}</h5>
        </div>
        
      </div>
    </div>
  </div>
</template>

<script>
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      error: "",
      MachineList: []
    };
  },
  created() {
    apiSvc
      .getMachineList()
      .then(data => {
        this.MachineList = data;
      })
      .catch(error => {
        this.error = error.message;
      });
  },
  methods: {
    machineData(id) {
      this.$emit("machineNumber", id);
    }
  }
};
</script>

<style scoped>
.weight_container {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  flex-wrap: wrap;
  margin: 0;
}
.tile {
  background: white;
  font-size: 20px;
  width: 30%;
  height: auto;
  padding: 20px;
  margin: 10px;
  border: none;
  border-radius: 5px;
  box-shadow: 2px 2px 5px black;
}
.machine_name{
  width: 100%;
  height: 50px;
  padding: 10px;
  border-radius: 5px;
}
.tile:hover {
  background: rgb(146, 155, 161);
  font-size: 20px;
  transition: 0.5s;
  cursor: pointer;
  color: white;
}
.machine_img{
  width: 100%;
  border-radius: 5px;
}
/*mobile size */
@media screen and (max-width: 600px) {
  .weight_container {
    margin: 0;
  }
  .tile {
    width: 100%;

  }
}
</style>
