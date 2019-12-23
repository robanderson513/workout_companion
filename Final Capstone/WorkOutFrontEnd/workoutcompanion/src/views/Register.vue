<template>
  <div>
    <h2>Register</h2>
    <transition name="fade">
      <b-alert v-show="error" show variant="danger">
        Username or password already taken
      </b-alert>
    </transition>
    <basic-info
      v-if="startingInfo"
      :info="basicInfo"
      @clear-data="resetForm"
      v-on:intro-register="basicRegistration"
    />

    <goals-info
      v-if="!startingInfo"
      @submit-registration="createUser"
      @goBack="switchInfo"
    />
  </div>
</template>

<script>
import BasicInfo from "@/components/registration/BasicInfo.vue";
import GoalsInfo from "@/components/registration/GoalsInfo.vue";

import { apiService } from "@/Services/api";
import auth from "@/Services/auth";
const apiSvc = new apiService();

export default {
  data() {
    return {
      error: false,
      startingInfo: true,
      basicInfo: {
        FirstName: "",
        LastName: "",
        Email: "",
        UserName: "",
        Password: "",
        ConfirmPassword: ""
      },
      Register: {
        FirstName: "",
        LastName: "",
        Email: "",
        UserName: "",
        Password: "",
        ConfirmPassword: "",
        ExperienceLevel: "",
        WeeklyExercise: "",
        Goals: ""
      }
    };
  },
  components: {
    BasicInfo,
    GoalsInfo
  },
  methods: {
    basicRegistration(data) {
      this.Register.FirstName = data.FirstName;
      this.Register.LastName = data.LastName;
      this.Register.Email = data.Email;
      this.Register.UserName = data.UserName;
      this.Register.Password = data.Password;
      this.saveData();
      this.startingInfo = false;
      this.error = false;
    },
    saveData() {
        (this.basicInfo.FirstName = this.Register.FirstName),
        (this.basicInfo.LastName = this.Register.LastName),
        (this.basicInfo.Email = this.Register.Email),
        (this.basicInfo.UserName = this.Register.UserName),
        (this.basicInfo.Password = this.Register.Password),
        (this.basicInfo.ConfirmPassword = this.Register.ConfirmPassword);
    },
    switchInfo() {
      this.startingInfo = true;
    },
    goDashboard() {
      this.$router.push("/dashboard");
    },
    signup() {
      let token = apiSvc
        .register(this.Register)
        .then(() => {
          auth.saveToken(token);
          this.goDashboard();
        })
        .catch(() => {
          this.error = true;
          this.startingInfo = true;
        });
    },
    createUser(data) {
      this.Register.ExperienceLevel = data.ExperienceLevel;
      this.Register.WeeklyExercise = data.WeeklyExercise;
      this.Register.Goals = data.Goals;

      this.signup();
    },
    resetForm() {
      (this.basicInfo.FirstName = ""),
        (this.basicInfo.LastName = ""),
        (this.basicInfo.Email = ""),
        (this.basicInfo.UserName = ""),
        (this.basicInfo.Password = ""),
        (this.basicInfo.ConfirmPassword = "");
      this.goDashboard();
    }
  }
};
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 1s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
