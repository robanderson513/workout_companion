<template>
  <div>
    <span class="check" v-if="!checkedIn" v-b-modal.modal-checkIn>
      <font-awesome-icon :icon="['fas', 'check']" size="2x" />
    </span>
    <span class="check" v-if="checkedIn" v-b-modal.modal-checkOut>
      <font-awesome-icon :icon="['fas', 'check']" size="2x" />
    </span>

    <b-modal id="modal-checkIn" title="Check In">
      <transition name="fade">
          <b-alert show variant="danger" v-if="checkinError" class="banner-formatting">
            You're already logged in
          </b-alert>
        </transition>
        <transition name="fade">
          <b-alert show variant="success" v-if="checkinSuccess" class="banner-formatting">
            You've logged in
          </b-alert>
        </transition>
      <p class="my-4">Are you ready to check in?</p>
      <template v-slot:modal-footer="{ cancel, checkInBtn }">
        <b-button size="sm" @click="cancel">Cancel</b-button>
        <b-button size="sm" @click="checkIn">Check In</b-button>
      </template>
    </b-modal>

    <b-modal id="modal-checkOut" title="Check Out">
        <transition name="fade">
          <b-alert show variant="danger" v-if="checkoutError" class="banner-formatting">
            You're not logged in
          </b-alert>
        </transition>
        <transition name="fade">
          <b-alert show variant="success" v-if="checkoutSuccess" class="banner-formatting">
            You've logged out
          </b-alert>
        </transition>
      <p class="my-4">Are you ready to check out?</p>
      <template v-slot:modal-footer="{ cancel, checkOutBtn }">
        <b-button size="sm" @click.prevent="cancel">Cancel</b-button>
        <b-button size="sm" @click="checkOut">Check Out</b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  props:{
    data: Object,
    checkedIn: Boolean
  },
  data() {
    return {
      checkinError: false,
      checkoutError: false,
      checkinSuccess: false,
      checkoutSuccess: false
    };
  },
  computed:{

  },
  methods: {
    checkIn() {
      this.checkedinError = false;
      this.checkinSuccess = false;
      if(!this.checkedIn){
        apiSvc.checkInUser(this.data.username)
         .then( (response) => {
          this.$emit('visit-id', response.data)
          this.checkinSuccess = true;
          this.checkinError = false;
        })
        .catch( () =>{
          this.checkinError = true;
        })
        }else{
          this.checkinError = true;
          this.checkinSuccess = false;
        }
    },
    checkOut() {
      this.checkedout = false;
      this.checkoutError = false;
      if(this.checkedIn){
        apiSvc.checkOutUser(this.data.username)
        .then( () => {
          this.$emit('check-out')
          this.checkoutSuccess = true;
          this.checkoutError = false;
        }) 
        .catch( () =>{
          this.checkoutError = true;
        })
        } else{
          this.checkoutError = true;
          this.checkoutSuccess = false;
        }
    }
  }
};
</script>

<style scoped>
.check {
  color: #868e96;
  padding-left: 15px;
}
.check:hover {
  color: #ced4da;
}
.banner-formatting{
  margin-bottom: 40px;
}
</style>
