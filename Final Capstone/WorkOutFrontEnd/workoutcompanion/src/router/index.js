import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Register from "../views/Register.vue";
import Profile from "../views/Profile.vue";
import Dashboard from "../views/Dashboard.vue";
import MachineSelection from "../views/MachineSelection.vue";
import Machine from "../views/Machine.vue";
import Classes from "../views/Classes.vue";
import AddMachine from "../views/AddMachine.vue";
import RemoveMachine from "../views/RemoveMachine.vue";
import FindUser from "../views/FindUser.vue";
import UserInfo from "../views/UserInfo.vue";
import auth from "../Services/auth";

Vue.use(VueRouter);

const routes = [
  {
    path: "/register",
    name: "register",
    component: Register,
    meta: {
      title: "Register"
    }
  },
  {
    path: "/",
    name: "home",
    component: Home,
    alias: "/home",
    meta: {
      title: "Home"
    }
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue"),
    meta: {
      title: "About"
    }
  },
  {
    path: "/userProfile",
    name: "profile",
    component: Profile,
    meta: {
      title: "Profile"
    }
  },
  {
    path: "/dashboard",
    name: "dashboard",
    component: Dashboard,
    meta: {
      title: "Dashboard"
    }
  },
  {
    path: "/machines",
    name: "machines",
    component: MachineSelection,
    meta: {
      title: "MachineSelection"
    }
  },
  {
    path: "/machine/:machine",
    name: "machine",
    component: Machine
  },
  {
    path: "/classes",
    name: "classes",
    component: Classes
  },
  {
    path: "/addmachine",
    name: "addMachine",
    component: AddMachine
  },
  {
    path: "/removemachine",
    name: "removeMachine",
    component: RemoveMachine
  },
  {
    path: "/finduser",
    name: "findUser",
    component: FindUser
  },
  {
    path: "/user/:username",
    name: "user",
    component: UserInfo
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ["/home", "/register"];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = auth.getUser();

  if (authRequired && !loggedIn) {
    return next("/home");
  }
  next();
});

export default router;
