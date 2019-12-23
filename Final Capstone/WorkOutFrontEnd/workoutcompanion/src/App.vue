<template>
  <div id="app">
  <!--Desktop navigation-->
  <b-navbar v-if="loggedIn" class="desktop_nav" toggleable="lg" type="dark" variant="dark">
      <b-navbar-brand :to="{name: 'dashboard'}">WorkoutCompanion</b-navbar-brand>
      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <b-nav-item :to="{name:'about'}">About</b-nav-item>
           <b-nav-item :to="{name:'dashboard'}">Dashboard</b-nav-item>
          <b-nav-item :to="{name:'machines'}">Machines</b-nav-item>
          <b-nav-item :to="{name:'classes'}">Classes</b-nav-item>
        <!-- <b-nav-item :to="{name:'user', params:{username: thisUser}}"> My Workouts </b-nav-item> -->
        </b-navbar-nav>
        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <div id="right_nav"  >           
            <user-navigation :userData="user"/>
          </div>
        </b-navbar-nav>
      </b-collapse>
  </b-navbar>
  <!--Added navigation for employees/admin -->
  <b-navbar v-if="user.roleId > 1 && loggedIn" class="desktop_nav" >
      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
      <b-navbar-nav>
        <b-nav-item><check-in-desktop @visit-id="getVisit" @check-out="removeVisit" :userData="user" /></b-nav-item>
        <b-nav-item :to="{ name: 'addMachine' }">Add Machine</b-nav-item>
        <b-nav-item :to="{ name: 'removeMachine' }">Remove Machine</b-nav-item>
        <b-nav-item :to="{ name: 'findUser' }">Find Users</b-nav-item>
      </b-navbar-nav>
    </b-navbar>

  <!--Mobile Navigation -->
  <mobile-navigation class="mobile_nav" :userData="user" @visit-id="getVisit" @check-out="removeVisit" :checkIn="checkedIn"/>

 <router-view class="add_padding" @send-user="setRole" :userData="user" :checkedIn="checkedIn" :visit="visitId" />
  <footer><p class="copyright">&copy; 2019</p></footer>
  </div>
</template>

<script>
import UserNavigation from '@/components/navigation/UserNavigation.vue';
import MobileNavigation from '@/components/navigation/MobileNavigation.vue';
import CheckInDesktop from '@/components/navigation/CheckInDesktop.vue';

export default { 
  data(){
    return{
      user: {},
      checkedIn: false,
      loggedIn: false,
      visitId: null,
    }
  },
   components: {
    UserNavigation,
    MobileNavigation,
    CheckInDesktop
  },
  methods:{
    setRole(data){
      this.user = data;
    },
    getVisit(id){
      this.checkedIn = true;
      this.visitId = id;
    },
    removeVisit(){
      this.checkedIn = false;
      this.visitId = null;
    }
  },
  created(){
      if (this.$route.path === "/" || this.$route.path === "/home" || this.$route.path === "/register" ) {
      this.loggedIn=false;
      } else {
      this.loggedIn=true;
      }
  },
    watch: {
    $route: function() {
        if (this.$route.path === "/" || this.$route.path === "/register" || this.$route.path === "/home") {
          this.loggedIn=false;
          } else  {
          this.loggedIn=true;
        }
      }
    }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  min-height: 100vh;
  position: relative;
  background: rgb(245, 245, 245);
}
.add_padding{
  padding-bottom: 50px;
}
#right_nav{
  display: flex;
}
h2{
  padding-top:15px;
  text-align: center;
}
.container{
  margin:0!important;
  padding: 0!important;
}
footer{
  position: absolute;
  width: 100%;
  bottom: 0;
  height: 40px;
  margin-top: 10px;
  background-color: #343a40;
}
.copyright{
  color: #868e96;
  width: 10%;
  margin: auto;
  padding-top:10px;
}
@media screen and (min-width: 601px) {
 .mobile_nav {
   display: none;
 }
}
/*mobile size */
@media screen and (max-width: 600px) {
 .desktop_nav {
   display: none!important;
 }
 .mobile_nav {
   display: block;
 }
 footer{
   position: absolute;
   bottom: 0;
   height: 100px;
 }
 .add_padding{
  padding-bottom: 100px;
}
}
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

</style>
