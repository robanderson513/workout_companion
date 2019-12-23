<template>
  <div id="main">
    <div>
      <b-navbar class="navbar" type="dark" variant="dark">
        <router-link :to="{ name: 'machines' }" class="add_span">
          <font-awesome-icon :icon="['fas', 'plus']" size="2x" />
        </router-link>
        <check-in-and-out class="check" :data="userData" @visit-id="sendVisit" @check-out="checkOut" :checkedIn="checkIn"/>
        <router-link :to="{ name: 'classes' }" class="calendar">
          <font-awesome-icon :icon="['far', 'calendar']" size="2x" />
        </router-link>
        <div class="line_span" @click="openNav()">
          <font-awesome-icon :icon="['fas', 'bars']" size="2x" />
        </div>
      </b-navbar>
    </div>
    <div id="sideNav" class="sidenav">
      <a href="javascript:void(0)" class="closebtn" @click="closeNav()">&times;</a>
      <b-navbar-brand class="logo_text" :to="{ name: 'dashboard' }">WorkoutCompanion</b-navbar-brand>
      <b-nav-item :to="{ name: 'about' }" @click="closeNav()">About</b-nav-item>
      <b-nav-item :to="{ name: 'dashboard' }" @click="closeNav()">Dashboard</b-nav-item>
      <b-nav-item :to="{ name: 'profile' }" @click="closeNav()">Profile</b-nav-item>

      <b-nav-item :to="{ name: 'home' }" @click="closeNav()"  @click.prevent="logout">Logout</b-nav-item>
    </div>
  </div>
</template>

<script>
import CheckInAndOut from "@/components/navigation/CheckInAndOut.vue";
import auth from "@/Services/auth.js";

export default {
  props:{
    checkIn: Boolean,
    userData: Object
  },
  data() {
    return {
      userNav: true,
      carotIcon: true
    };
  },
  components: {
    CheckInAndOut
  },
  methods: {
    logout() {
      auth.destroyToken();
      this.$router.push("/home");
    },
    openNav() {
      document.getElementById("sideNav").style.width = "300px";
    },
    closeNav() {
      document.getElementById("sideNav").style.width = "0";
    },
    sendVisit(id){
      this.$emit("visit-id", id)
    },
    checkOut(){
      this.$emit('check-out')
    }
  }
};
</script>

<style scoped>
.navbar {
  position: fixed;
  overflow: hidden;
  bottom: 0;
  left: 0px;
  width: 100%;
  min-height: 80px;
  z-index: 10;
}
li {
  list-style: none;
}
.logo_text {
  color: white;
}
/* icons styling */
.line_span {
  margin: auto;
  color: #868e96;
}
.add_span {
  width: 25%;
  text-align: center;
  cursor: pointer;
  color: #868e96;
}
.check {
  width: 25%;
}
.calendar {
  width: 25%;
  color: #868e96;
  text-align: center;
}
.carot {
  margin-left: 90px;
}
/* sidenav styling */
.sidenav {
  height: 100%;
  width: 0;
  position: fixed;
  z-index: 5;
  top: 0px;
  left: 0px;
  overflow-x: hidden;
  transition: 0.5s;
  padding-top: 60px;
  background-color: #232629;
}
.sidenav a {
  padding: 8px 8px 8px 32px;
  text-decoration: none;
  font-size: 25px;
  color: #818181;
  display: block;
  transition: 0.3s;
}
.sidenav a:hover {
  color: #f1f1f1;
}
.sidenav .closebtn {
  position: absolute;
  top: 10px;
  right: 25px;
  font-size: 36px;
  margin-left: 50px;
}
</style>
