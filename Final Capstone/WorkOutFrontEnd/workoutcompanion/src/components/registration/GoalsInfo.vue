<template>
  <div class="form_container">
    <div class="center_form">
      <b-form @submit.prevent="submitRegistration">
        <b-form-group label="Experience Level: " label-for="input-1">
          <b-form-select
            id="input-1"
            v-model="form.ExperienceLevel"
            :options="ExperienceLevels"
            required
            size="sm"
          >
          </b-form-select>
        </b-form-group>

        <b-form-group
          label="How often do you exercise weekly: "
          label-for="input-2"
        >
          <b-form-select
            id="input-2"
            v-model="form.WeeklyExercise"
            :options="WeeklyExercise"
            required
            size="sm"
          >
          </b-form-select>
        </b-form-group>

        <b-form-group label="Primary Goal: " label-for="input-3">
          <b-form-select
            id="input-3"
            v-model="form.Goals"
            :options="Goals"
            required
            size="sm"
          >
          </b-form-select>
        </b-form-group>

        <div class="button_container">
          <b-button class="go_back_btn btns"  variant="outline-secondary" v-on:click="goBack"
            >Go Back</b-button
          >
          <b-button class="btns"  variant="outline-primary" type="submit">Submit</b-button>
        </div>
      </b-form>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      firstAttempt: true,
      form: {
        ExperienceLevel: null,
        WeeklyExercise: null,
        Goals: null
      },
      Goals: [
        { text: "Select One", value: null },
        "Gain Muscle",
        "Lose Weight",
        "Improve Endurance",
        "Improve Flexibility"
      ],
      ExperienceLevels: [
        { text: "Select One", value: null },
        "Beginner",
        "Intermediate",
        "Expert"
      ],
      WeeklyExercise: [
        { text: "Select One", value: null },
        "1-2",
        "3-4",
        "5",
        "Daily"
      ]
    };
  },
  computed: {
    elError: function() {
      let bool = false;
      if (!this.firstAttempt && !this.form.ExperienceLevel) {
        bool = true;
      }
      return bool;
    },
    weError: function() {
      let bool = false;
      if (!this.firstAttempt && !this.form.WeeklyExercise) {
        bool = true;
      }
      return bool;
    },
    gError: function() {
      let bool = false;
      if (!this.firstAttempt && !this.form.Goals) {
        bool = true;
      }
      return bool;
    }
  },
  methods: {
    submitRegistration() {
      this.firstAttempt = false;
      if (!this.elError && !this.weError && !this.gError) {
        this.$emit("submit-registration", this.form);
      }
    },
    goBack() {
      this.$emit("goBack");
    }
  }
};
</script>

<style scoped>
.form_container {
  width: 100%;
}
.center_form {
  width: 30%;
  margin: auto;
}
.go_back_btn {
  margin-right: 10px;
}
.button_container {
  width: 100%;
}
.btns {
  width: 47%;
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
