<template>
  <div>
      <transition name="fade">
          <b-alert show variant="danger" v-if="updateError">
            Unable to update user
          </b-alert>
          <b-alert show variant="success" v-if="addedUser">
            User updated
          </b-alert>
        </transition>
    <div class="button-container">        
      <b-button @click="goBack" variant="outline-secondary"> Go back </b-button>
        <div id="update">
          <b-form-select v-model="updateRole">
            <option :value="null"> Update User </option>
            <option :value="1"> Gym User </option>
            <option :value="2"> Employee </option>
            <option :value="3"> Admin </option>
          </b-form-select>
          <b-button @click="updateUser" variant="outline-primary"> Update User </b-button>
        </div>
    </div>

    <user-data :fullDisplay="true" :username="username" />
  </div>
</template>

<script>
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

import UserData from '../components/UserData'
export default {
  data() {
    return {
      username: null,
      updateRole: null,
      addedUser: false,
      updateError: false
    };
  },
  components: {
    UserData
  },
    methods:{
        goBack(){
            this.$router.go(-1);
        },
        updateUser(e){
            this.addedUser = false;
            this.updateError = false;
            if(!this.updateRole){
                e.preventDefault();
            }else{
                apiSvc.updateUser(this.username, this.updateRole)
                .then((response)=>{
                   if(response.status == 200){
                       this.addedUser = true;
                   } else{
                       this.updateError= true;
                   }
                })
            }
        },
        checkoutUser(){
            console.log('meow')
        }
    },
    created() {
        this.username = this.$route.params.username;
    }
  
};
</script>

<style scoped>
.button-container {
   display: flex;
   justify-content: space-between;
   height: 60px;
   padding: 0px 20px;
}
.button-container > button{
    width:  15%;
}
#update {
    width: 30%;
    padding: 10px;
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
