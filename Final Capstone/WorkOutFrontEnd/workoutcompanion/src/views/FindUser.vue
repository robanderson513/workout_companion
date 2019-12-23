<template>
  <div>
    <transition name="fade">
      <b-alert show variant="danger" v-show="noUsers"> No user found </b-alert>
    </transition>
    <b-form @submit.prevent="searchUser">
      <!-- <div class="formfield">
                <label label-for="input-1">First Name: </label>       
                <b-form-input
                    id="input-1"
                    v-model="firstName"
                />
          </div>
           <div class="formfield">
                <label label-for="input-2">Last Name:</label>       
                <b-form-input
                    id="input-2"
                    v-model="lastName"
                />
           </div> -->
      <h2>Search By Username</h2>
      <div class="formfield">
        <label label-for="input-3">Username: </label>
        <b-form-input id="input-3" v-model="search" />
      </div>
      <div id="b-container">
        <div id="buttons">
          <b-button  variant="outline-primary" id="b1" type="submit"> Search </b-button>
          <b-button  variant="outline-secondary" id="b2" v-on:click="clearSearch">
            Clear
          </b-button>
        </div>
      </div>
    </b-form>
    <ul id="search-container">
      <router-link
        v-if="displayUser"
        tag="li"
        class="tile"
        :to="{ name: 'user', params: { username: user.username } }"
      >
        <div class="user-summary">
          <div id="user-picture">
            <img :src="user.photoURL" />
          </div>
          <div id="user-info">
            <p>
              Name: {{ user.firstName }} {{ user.lastName }} Username:
              {{ user.username }}
            </p>
            Email: {{ user.email }}
          </div>
        </div>
      </router-link>
    </ul>
  </div>
</template>

<script>
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      noUsers: false,
      displayUser: false,
      search: null,
      user: null
    };
  },
  methods: {
    searchUser() {
      this.user = null;
      this.displayUser = false;
      if (!this.search) {
        this.noUsers = true;
        this.displayUser = false;
      } else {
        apiSvc
          .searchUser(this.search)
          .then(data => {
            if (!data.username) {
              this.noUsers = true;
              this.displayUser = false;
            } else {
              this.displayUser = true;
              this.noUsers = false;
              this.user = data;
            }
          })
          .catch(error => {
            this.error = error.message;
          });
      }
    },
    clearSearch() {
      this.firstName = null;
      this.lastName = null;
      this.search = null;
      this.noUsers = false;
    }
  }
};
</script>

<style scoped>
.formfield {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 10px;
  padding: 15px;
}
.formfield > input {
  width: 60%;
  margin-left: 10px;
}
#b-container {
  display: flex;
  justify-content: center;
}
#buttons {
  width: 60%;
  display: flex;
  justify-content: space-around;
}
button {
  padding: 10px 25px;
}
#search-container {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
}
.tile {
  background: linear-gradient(
    90deg,
    rgba(0, 159, 255, 1) 4%,
    rgba(9, 62, 121, 1) 32%
  );
  color: white;
  font-size: 20px;
  width: 70%;
  height: 120px;
  padding: 20px;
  margin: 10px;
  border: none;
  box-shadow: 2px 2px 5px black;
  list-style: none;
  transition: 0.5s;
}
.tile:hover {
  opacity: 90%;
  transition: 0.5s;
}
.user-summary {
  display: flex;
}
#user-picture {
  width: 10%;
}
#user-picture > img {
  width: 100%;
}
#user-info {
  padding-left: 10px;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 1s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
