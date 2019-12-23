<template>
  <div>
    <h2>Login</h2>
    <div class="form_container">
      <div class="center_form">
        <transition name="fade">
          <b-alert show variant="danger" v-if="error">
            Incorrect Username or Password
          </b-alert>
        </transition>
        <b-form @submit.prevent="login">
          <b-form-group label="Your Email or Username:" label-for="input-1">
            <b-form-input
              id="input-1"
              v-model="Login.emailOrUsername"
              required
              placeholder="Enter email or username"
            >
            </b-form-input>
          </b-form-group>

        <b-form-group label="Your Password:" label-for="input-2">
          <b-form-input
            id="input-2"
            v-model="Login.Password"
            required
            type="password"
            placeholder="Enter password"
          >
          </b-form-input>
        </b-form-group>
        <div class="login_form">
          <b-button type="submit" class="btn" variant="outline-primary" v-on:click="login">Submit</b-button>
          <router-link id="register_link" :to="{ name: 'register' }"
            >Register</router-link
          >
        </div>
      </b-form>
      </div>
    </div>
  </div>
</template>

<script>
import auth from "@/Services/auth";
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      error: false,
      Login: {
        emailOrUsername: "",
        Password: ""
      },
      NameError: false,
      PasswordError: false,
      IsTrue: false
    };
  },
  methods: {
    checkStatus(event) {
      const username = this.Login.Username;
      const password = this.Login.Password;
      if (username === "") {
        this.NameError = true;
      } else {
        this.NameError = false;
      }
      if (password === "") {
        this.PasswordError = true;
      } else {
        this.PasswordError = false;
      }
      if (username && password) {
        this.IsTrue = true;
        //Send data to API to verify data passes
        //Will check if exists
      } else {
        event.preventDefault();
      }
    },
    async login() {
      this.error = "";
      try {
        let token = await apiSvc.login(this.Login);
        auth.saveToken(token);
        this.goDashboard();
      } catch (error) {
        this.error = true;
      }
    },
    goDashboard() {
      this.$router.push("/dashboard");
    }
  }
};
</script>

<style scoped>
.error {
  color: red;
  font-weight: bold;
}
.form_container {
  width: 100%;
}
.center_form {
  width: 30%;
  margin: auto;
}
#register_link {
  padding-left: 10px;
}
.login_form {
  text-align: center;
}
.btn {
  width: 50%;
  background-color:none!important;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 1s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
/*mobile size */
@media screen and (max-width: 600px) {
  .form_container {
    width: 100%;
  }
  .center_form {
    width: 75%;
    margin: auto;
  }
}
</style>
