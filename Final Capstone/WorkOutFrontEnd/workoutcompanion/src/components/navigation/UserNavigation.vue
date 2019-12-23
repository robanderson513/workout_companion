<template>
  <div class="nav_container">
    <b-button id="popover-1-bottom" :style="{ 'background-image': `url('` + userData.photoURL +`')` }" class="btn_popover"></b-button>
    <b-popover class="popover" target="popover-1-bottom" triggers="focus" placement="bottom">
        <h6>{{userData.firstName}} {{userData.lastName}}</h6><hr />
        <ul class="link_box">
            <li><router-link class="link" :to="{name: 'profile'}">Profile</router-link></li>
            <li><a id="logoutButton" @click.prevent="logout">Logout</a></li>
        </ul>
    </b-popover>
  </div>
</template>

<script>
import auth from "@/Services/auth.js";

export default {
  props:{
    userData: Object
  },
  data() {
    //need to pass in user information
    return {
    };
  },
  methods: {
    logout() {
      auth.destroyToken();
      this.$router.push("/home");
    }
  }
}
</script>

<style scoped>
.nav_container {
  display: flex;
}
#checkin {
  padding-right: 10px;
  cursor: pointer;
}
.btn_popover {
  width: 40px;
  height: 40px;
  border-radius: 100%;
}
.popover {
  min-width: 250px;
}
hr {
  margin: 0;
  padding: 2px 0;
}
.link_box {
  padding: 0;
}
.link_box li {
  list-style-type: none;
}
.link_box li a {
  text-decoration: none;
  color: black;
}
.link_box li a:hover {
  color: gray;
}
#popover-1-bottom {
  min-height: 50px;
  min-width: 50px;
  background-color: transparent;
  background-position: center center;
  background-repeat: no-repeat;
  background-size: cover;
  border-radius: 50%;
}
#logoutButton {
  cursor: pointer;
}
</style>
