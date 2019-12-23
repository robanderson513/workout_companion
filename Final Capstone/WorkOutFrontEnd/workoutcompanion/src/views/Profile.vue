<template>
  <div>
    <h2>Profile</h2>
    <!--view profile-->
    <b-container v-if="editFields">
      <b-row>
        <b-col cols="3">
          <img id="user_img" :src="user.photoURL" alt="user_image" />
        </b-col>
        <b-col cols="9">
          <h3>{{ user.firstName }} {{ user.lastName }}</h3>
          <p>{{ user.username }}</p>
          <p>{{ user.email }}</p>
          <p>{{ user.workoutGoals }}</p>
          <p>{{ user.weeklyExercise }}</p>
          <p>{{ user.experienceLevel }}</p>
        </b-col>
      </b-row>
      <b-row>
        <b-col class="center_btn">
          <b-button v-on:click="editProfile">Edit Profile</b-button>
        </b-col>
      </b-row>
    </b-container>

    <!--edit profile-->
    <div class="form_container" v-else>
      <b-form @submit.prevent="updateProfile">
        <div class="col_3">
          <img id="user_img" :src="user.photoURL" alt="user_image" />
        </div>
        
        <div class="col_9">
           <h3 id="username"> {{user.username}} </h3>
          <b-form-group label="First Name:" label-for="input-1">
            <b-form-input
              id="input-1"
              v-model="user.firstName"
              required
              size="sm"
              :value="user.firstName"
            >
            </b-form-input>
          </b-form-group>

          <b-form-group label="Last Name:" label-for="input-2">
            <b-form-input
              id="input-2"
              v-model="user.lastName"
              required
              size="sm"
              :value="user.lastName"
            >
            </b-form-input>
          </b-form-group>

          <b-form-group label="Email:" label-for="input-3">
            <b-form-input
              id="input-3"
              v-model="user.email"
              type="email"
              required
              size="sm"
              :value="user.email"
            >
            </b-form-input>
          </b-form-group>

         

          <b-form-group label="Image URL:" label-for="input-6">
            <b-form-input
              id="input-6"
              v-model="user.photoURL"             
              size="sm"
              :value="user.photoURL"
            >
            </b-form-input>
          </b-form-group>

           <b-form-group label="Primary Goal: " label-for="input-3">
          <b-form-select
            id="input-3"
            v-model="user.Goals"
            :options="this.Goals"
            size="sm"
          >
          </b-form-select>
        </b-form-group>

          <b-button v-on:click="cancelEdit">Cancel</b-button>
          <b-button type="submit" @click="updateProfile">Update</b-button>
        </div>
      </b-form>
    </div>
    <!--end of form_container-->
  </div>
</template>

<script>
import { apiService } from "@/Services/api.js";
const apiSvc = new apiService();

export default {
  data() {
    return {
      user: {},
      editFields: true,
      Goals: [
        { text: "Select One", value: null },
        "Gain Muscle",
        "Lose Weight",
        "Improve Endurance",
        "Improve Flexibility"
      ],
    };
  },
  created() {  
      apiSvc
        .getUserProfile()
        .then(response => {
          this.user = response.data;
        })
        .catch(error => {
          window.console.log(error);
        });
  },
  methods: {
    cancelEdit() {
      this.editFields = true;
    },
    editProfile() {
      this.editFields = false;
    },
    viewEditedProfile() {
      this.editFields = true;
      this.$router.go("/userProfile");
    },
    updateProfile() {
      apiSvc.updateProfile(this.user)
      this.viewEditedProfile()
    }
  }
};
</script>

<style scoped>
.center_btn {
  text-align: center;
}
#user_img {
  width: 100%;
  border-radius: 100%;
}
.form_container {
  width: 100%;
}
.col_3 {
  width: 25%;
  float: left;
}
.col_9 {
  padding-left: 40px;
  width: 50%;
  float: left;
}
#username{
  display: flex;
  justify-content: center;
  color:rgb(156, 156, 156);
}
/*#input-3{
  padding:0;
  margin: 0!important;
}*/
</style>
