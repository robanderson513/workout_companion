<template>
  <div>
    <h2>Lets add a machine</h2>
    <b-form class="input-container" @submit.prevent="addMachine">
      <b-form-group label="Machine Name: " class="form-data">
        <b-form-input
          v-model="machine.name"
          :class="{ error: mnError }"
          placeholder="Enter machine name"
        />
        <errors :type="'machineName'" v-if="mnError" />
      </b-form-group>
      <b-form-group label="Description:" class="form-data">
        <textarea
          id="description"
          v-model="machine.description"
          :class="{ error: dscError }"
          placeholder="Enter description"
        />
        <errors :type="'description'" v-if="dscError" />
      </b-form-group>

      <b-form-group label="Upload picture:" class="form-data">
        <b-form-input
          v-model="machine.imageURL"
          :class="{ error: imgError }"
          placeholder="Copy URL here"
        />
        <errors :type="'file'" v-if="imgError" />
      </b-form-group>

      <b-form-group label="Upload video URL:" class="form-data">
        <b-form-input
          v-model="machine.videoURL"
          :class="{ error: vdError }"
          placeholder="Copy URL here"
        />
        <errors :type="'file'" v-if="vdError" />
      </b-form-group>

      <b-form-group label="Machine Number:" class="form-data">
        <b-form-input
          type="number"
          v-model="machine.number"
          :class="{ error: numError }"
          placeholder="Add machine's identifying number"
        />
        <errors :type="'number'" v-if="numError" />
      </b-form-group>

      <b-form-group label="Category:" class="form-data">
        <b-form-select
          v-model="machine.category"
          placeholder="Add machine's identifying number"
          required=""
        >
        <option :value="'Cardio'"> Cardio </option>
        <option :value="'Resistance'"> Resistance </option>
        </b-form-select>
      </b-form-group>

      <div id="buttons">
        <b-button variant="outline-secondary" class="go_back_btn btns" v-on:click="goBack"
          >Go Back</b-button
        >
        <b-button variant="outline-primary" type="submit"> Add Machine </b-button> 
      </div>
    </b-form>
  </div>
</template>

<script>
import Errors from "../components/Errors";

import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      firstTime: true,
      machine: {
        name: null,
        description: null,
        imageURL: null,
        videoURL: null,
        number: null,
        category: "cardio"
      }
    };
  },
  computed: {
    mnError: function() {
      return !this.firstTime && !this.machine.name;
    },
    dscError: function() {
      return !this.firstTime && !this.machine.description;
    },
    imgError: function() {
      return !this.firstTime && !this.machine.imageURL;
    },
    vdError: function() {
      return !this.firstTime && !this.machine.videoURL;
    },
    numError: function() {
      return !this.firstTime && !this.machine.number;
    }
  },
  components: {
    Errors
  },
  methods: {
    addMachine() {
      this.firstTime = false;
      if (!this.mnError && !this.dscError && !this.imgError && !this.vdError) {
        apiSvc.addMachine(this.machine);
        this.$router.push({ name: "machines" });
      }
    },
    goBack() {
      this.$router.go(-1);
    }
  }
};
</script>

<style scoped>
.error {
  background: rgb(251, 201, 201) !important;
}
.error::placeholder {
  color: red !important;
}
#description {
  width: 100%;
  border-radius: 5px;
  border: 1px solid lightgray;
}
.input-container {
  display: flex;
  align-items: center;
  flex-direction: column;
}
.form-data {
  width: 60%;
}
#buttons {
  width: 60%;
  display: flex;
  justify-content: space-around;
}
</style>
