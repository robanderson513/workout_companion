<template>
  <div>
    <!-- modal is connected to the <p> -->
    <p v-b-modal.modal-3>Check In/Check Out</p>

    <b-modal id="modal-3" title="Check In/Check Out" :hide-footer="hideFooter">
      <b-tabs justified>
        <!-- tab one -->
        <b-tab class="tabs" title="Check In">
          <b-form class="input-container" @submit.prevent="checkIn">
            <b-form-group label="Username: " class="form-data">
              <b-form-input
                v-model="username"
                placeholder="Enter username"
              />
            </b-form-group>
                <b-alert show variant="danger" v-if="loginError" class="logged_in">
                  User is logged in.
                  </b-alert>
                <b-alert show variant="danger" v-if="loginNoUser" class="logged_in">
                  User not found
                </b-alert>
                 <b-alert show variant="success" v-if="inSuccess" class="logged_in">
                  User logged in
                </b-alert>
            <div id="buttons">
              <b-button variant="outline-primary" type="submit"> Check User In </b-button>
            </div>
          </b-form>
        </b-tab>
        <!-- tab two -->
        <b-tab class="tabs" title="Check Out">
          <b-form class="input-container" @submit.prevent="checkOut">
            <b-form-group label="Username: " class="form-data">
              <b-form-input
                v-model="username"
                placeholder="Enter username"
              />
            </b-form-group>
                <b-alert show variant="danger" v-if="logoutError" class="logged_in">
                  User is not logged in.
                  </b-alert>
                <b-alert show variant="danger" v-if="logoutNoUser" class="logged_in">
                  User not found
                </b-alert>
                 <b-alert show variant="success" v-if="outSuccess" class="logged_in">
                  User logged out
                </b-alert>
            <div id="buttons">
              <b-button variant="outline-primary" type="submit"> Check User Out </b-button>
            </div>
          </b-form>
        </b-tab>
      </b-tabs>
    </b-modal>
  </div>
</template>

<script>
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      loginError: false,
      loginNoUser: false,
      inSuccess: false,    
      logoutError: false, 
      logoutNoUser: false,
      outSuccess: false,
      hideFooter: true,
      firstTime: true,
      checkedInUsers: [],
      username: null
    };
  },
  computed: {
    userError: function() {
      return !this.firstTime && !this.user.userName;
    }
  },
  methods: {
    checkIn() {
      this.loginError = false;
      this.loginNoUser = false;
      this.inSuccess = false;
      if (this.checkedInUsers.includes(this.username)){
        this.loginError = true;
      }else {
        apiSvc.checkInUser(this.username)
        .catch( () =>{
        this.loginNoUser = true;
        })
        this.checkedInUsers.push(this.username)
        this.inSuccess = true;
      }
    },
    checkOut() {
      this.logoutError = false;
      this.logoutNoUser = false;
       this.outSuccess = false;
      if(this.checkedInUsers.includes(this.username)){
        apiSvc.checkOutUser(this.username)
        .catch( () => {
          this.logoutNoUser = true;
        })
        const index = this.checkedInUsers.indexOf(this.username)
        this.checkedInUsers.splice(index, 1)
        this.outSuccess = true;
      } else{
        this.logoutError = true;
      }
    }
  }
}
</script>

<style scoped>
.tabs {
  min-height: 150px;
}
.input-container {
  padding-top: 10px;
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
