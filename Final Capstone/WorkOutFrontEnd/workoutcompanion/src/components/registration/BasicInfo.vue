<template>
  <div class="form_container">
    <div class="center_form">
      <b-form @submit.prevent="introRegister($event)">
        <b-form-group label="First Name:" label-for="input-1">
          <b-form-input
            id="input-1"
            :class="{ error: fnError }"
            size="sm"
            :value="Register.FirstName"
            v-model="Register.FirstName"
            placeholder="Enter first name"
          >
          </b-form-input>
          <errors :type="'firstName'" v-if="fnError" />
        </b-form-group>

        <b-form-group label="Last Name:" label-for="input-2">
          <b-form-input
            id="input-2"
            :class="{ error: lnError }"
            size="sm"
            :value="Register.LastName"
            v-model="Register.LastName"
            placeholder="Enter last name"
          >
          </b-form-input>
          <errors :type="'lastName'" v-if="lnError" />
        </b-form-group>

        <b-form-group label="Your Email:" label-for="input-3">
          <b-form-input
            id="input-3"
            type="email"
            :class="{ error: emError }"
            size="sm"
            :value="Register.Email"
            v-model="Register.Email"
            placeholder="Enter email"
          >
          </b-form-input>
          <errors :type="'email'" v-if="emError" />
        </b-form-group>

        <b-form-group label="Your Username:" label-for="input-4">
          <b-form-input
            id="input-4"
            :class="{ error: unError }"
            size="sm"
            :value="Register.UserName"
            v-model="Register.UserName"
            placeholder="Enter username"
          >
          </b-form-input>
          <errors :type="'userName'" v-if="unError" />
        </b-form-group>

        <b-form-group label="Your Password:" label-for="input-5">
          <b-form-input
            id="input-5"
            :class="{ error: pwError }"
            type="password"
            size="sm"
            :value="Register.Password"
            v-model="Register.Password"
            placeholder="Enter password"
          >
          </b-form-input>
          <errors :type="'password'" v-if="pwError" />
        </b-form-group>

        <b-form-group label="Confirm Password:" label-for="input-6">
          <b-form-input
            id="input-6"
            :class="{ error: cpError }"
            type="password"
            size="sm"
            :value="Register.ConfirmPassword"
            v-model="Register.ConfirmPassword"
            placeholder="Re-enter password"
          >
          </b-form-input>
          <errors :type="'confirmPassword'" v-if="cpError" />
        </b-form-group>
        <b-button class="nav_btn"  variant="outline-secondary" @click="goBack">Back</b-button>
        <b-button class="nav_btn"  variant="outline-primary" type="submit">Next</b-button>
      </b-form>
    </div>
  </div>
</template>

<script>
import Errors from "../../components/Errors";

export default {
  props: {
    info: Object
  },
  data() {
    return {
      firstAttempt: true,
      Register: {
        FirstName: null,
        LastName: null,
        Email: null,
        UserName: null,
        Password: null,
        ConfirmPassword: null
      }
    };
  },
  components: {
    Errors
  },
  computed: {
    fnError: function() {
      return !this.firstAttempt && !this.Register.FirstName;
    },
    lnError: function() {
      return !this.firstAttempt && !this.Register.LastName;
    },
    emError: function() {
      return !this.firstAttempt && !this.Register.Email;
    },
    unError: function() {
      return !this.firstAttempt && !this.Register.UserName;
    },
    pwError: function() {
      return !this.firstAttempt && !this.Register.Password;
    },
    cpError: function() {
      return (
        (!this.firstAttempt && !this.Register.ConfirmPassword) ||
        (!this.firstAttempt &&
          this.Register.Password != this.Register.ConfirmPassword)
      );
    },
    validationPass: function() {
      return (
        !this.fnError &&
        !this.lnError &&
        !this.emError &&
        !this.unError &&
        !this.pwError &&
        !this.cpError
      );
    }
  },
  methods: {
    introRegister(event) {
      this.firstAttempt = false;
      if (this.validationPass === true) {
        this.$emit("intro-register", this.Register);
      } else {
        event.preventDefault();
      }
    },
    goBack() {
      this.$emit("clear-data");
    }
  },
  created() {
    this.Register.FirstName = this.info.FirstName;
    this.Register.LastName = this.info.LastName;
    this.Register.Email = this.info.Email;
    this.Register.UserName = this.info.UserName;
    this.Register.Password = this.info.Password;
    this.Register.ConfirmPassword = this.info.ConfirmPassword;
  }
};
</script>

<style scoped>
.form_container {
  width: 100%;
  margin-bottom: 100px;
}
.center_form {
  width: 30%;
  margin: auto;
}
.nav_btn {
  margin-right: 5px;
  width: 48%;
}
.error {
  background: rgb(251, 201, 201);
}
.error::placeholder {
  color: red;
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
