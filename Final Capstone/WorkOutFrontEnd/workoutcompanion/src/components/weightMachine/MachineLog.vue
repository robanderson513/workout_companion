<template>
  <div>
    <h1 id="machine-title">{{ machine.name }}</h1>

    <form @submit.prevent="addWorkout">
      <div id="form-boxes">
        <div class="grid">
          <label label-for="input-1">
            Reps:
          </label>
          <b-form-input
            id="input-1"
            type="number"
            v-model="sessionInfo.reps"
            :class="{ error: repError }"
          />

          <errors id="error" v-if="repError" :type="'number'" />
        </div>

        <div class="grid">
          <label label-for="input-1">
            Weight:
          </label>
          <b-form-input
            id="input-2"
            type="number"
            v-model="sessionInfo.weight"
            :class="{ error: wtError }"
          />
          <errors id="error" v-if="wtError" :type="'number'" />
        </div>

        <div class="grid">
          <label label-for="input-3">
            Duration (Minutes):
          </label>
          <b-form-input
            id="input-3"
            type="number"
            v-model="sessionInfo.duration"
            :class="{ error: durError }"
          />
          <errors id="error" v-if="durError" :type="'number'" />
        </div>
      </div>

      <div id="buttons">
        <b-button  variant="outline-secondary" id="go-back" v-on:click="goBack">
          Go Back
        </b-button>
        <b-button  variant="outline-primary" id="add-workout" type="submit">
          Add to Workout
        </b-button>        
      </div>     
    </form>
    <transition name="fade">
          <b-alert show variant="success" v-if="dataSent" class="banner">
            Workout Logged
          </b-alert>  
        </transition> 
     <transition name="fade">
          <b-alert show variant="danger" v-if="dataError" class="banner">
            You're not checked in, please check in first before logging workout
          </b-alert>
      </transition>
  </div>
</template>

<script>
import Errors from "../Errors";
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  props: {
    machine: Object,
    visit: Number
  },
  data() {
    return {
      firstTime: true,
      dataSent: false,
      dataError: false,
      errorSending: false,
      sessionInfo: {
        visitId: null,
        equipmentId: null,
        reps: null,
        weight: null,
        duration: null
      }
    };
  },
  components: {
    Errors
  },
  computed: {
    repError: function() {
      return !this.firstTime && !this.sessionInfo.reps;
    },
    wtError: function() {
      return !this.firstTime && !this.sessionInfo.weight;
    },
    durError: function() {
      return !this.firstTime && !this.sessionInfo.duration;
    }
  },
  methods: {
    addWorkout() {
      this.firstTime = false;
      this.dataSent = false;
      this.dataError = false;
      this.sessionInfo.equipmentId = this.machine.id;
      this.sessionInfo.visitId = this.visit;
      apiSvc.addWorkout(this.sessionInfo)
      .then( ()=> {
        this.dataSent = true;
      })
      .catch( () =>{
        this.dataError= true;
      })
    },
    goBack() {
      this.$router.go(-1);
    }
  }
};
</script>

<style scoped>
#machine-title {
  display: flex;
  justify-content: center;
  color: rgb(61, 56, 56);
  padding: 15px 0px;
  font-family: Impact, Haettenschweiler, "Arial Narrow Bold", sans-serif;
}
.grid {
  display: grid;
  grid-template-columns: 1fr 2fr;
  grid-template-areas:
    "label box"
    " .  error";
}
.form-group {
  display: flex;
  justify-content: center;
}
label {
  display: flex;
  justify-content: flex-end;
  font-size: 30px;
  color: rgb(0, 0, 0);
  padding-right: 10px;
  grid-area: label;
}
input {
  width: 80%;
  grid-area: box;
}
#buttons {
  margin-top: 15px;
  display: flex;
  justify-content: space-evenly;
}
.error {
  background: rgb(251, 201, 201);
}
.error::placeholder {
  color: red;
}
#error {
  grid-area: error;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 1s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
.banner{
  display: flex;
  justify-content: center;
}
</style>
