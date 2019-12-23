<template>
  <div>
    <h2 id="progress-message">{{ progressMessage }}</h2>

    <div class="test">
        <div class="info_tile">
            <font-awesome-icon :icon="['fas', 'stopwatch']" size="5x" class="icon"/> 
            <div class="info_content">
                <h4> Average gym time:</h4>
                <h3 class="duration">{{minutesToHours(userInfo.avgDuration)}}</h3>
            </div>   
        </div>
    
      <div class="info_tile">
        <font-awesome-icon :icon="['fas', 'dumbbell']" size="5x" class="icon" />
        <div class="info_content">
          <h4>Top 5 Machines:</h4>
          <ul id="top_5_list">
            <li v-for="workouts in userInfo.topFiveWorkouts" :key="workouts.id">
              {{ workouts.name }}
            </li>
          </ul>
        </div>
      </div>
      <div class="info_tile">
        <font-awesome-icon :icon="['fas', 'running']" size="5x" class="icon" />
        <div class="info_content">
          <h4>Reminders:</h4>
          <p>Have you drank enough water today?</p>
        </div>
    </div>
</div>
<!-- bottom overview infomation -->
    <div v-if="fullDisplay" id="full-display">
      <h2>Workout Overview</h2>
      <div class="overview-container">
        <div
          id="overview"          
          v-for="workouts in userInfo.weeklyWorkouts"          
          v-show="workouts.sumOfDuration != 0 "
          :key="workouts.id"
          @click="$set(workouts, 'selected', !workouts.selected)"
        >
          <div class="overview-info">
            {{ formatDate(workouts.date) }} |
            {{ minutesToHours(workouts.sumOfDuration) }}
            <font-awesome-icon
              class="carot"
              v-if="carotIcon"
              :icon="['fas', 'angle-down']"
              size="1x"
            />
            <font-awesome-icon
              class="carot"
              v-else
              :icon="['fas', 'angle-up']"
              size="1x"
            />
          </div>
          <div class="full-info" v-show="workouts.selected">
            <div v-for="workout in workouts.workouts" :key="workout.id" >
              <h5 class="stat-header">{{ workout.name }}</h5>
              <div class="stat-container">
                <div class="stats right-vr">
                  <p class="stat-type">Duration:</p>
                  <p class="stat">{{ workout.duration }} min</p>
                </div>
                <div class="stats right-vr">
                  <p class="stat-type">Reps:</p>
                  <p>{{ workout.reps }} reps</p>
                </div>
                <div class="stats">
                  <p class="stat-type">Weight:</p>
                  <p>{{ workout.weight }} lbs</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { apiService } from "@/Services/api.js";
const apiSvc = new apiService();
export default {
  props: {
    fullDisplay: Boolean,
    username: String
  },
  computed: {
    progressMessage: function() {
      let message = "";
      if (this.userInfo.avgDuration > 30) {
        message = "Wow good job, keep it up!";
      } else if (this.userInfo.avgDuration > 20) {
        message = "Pretty good but there's room for improvement";
      } else {
        message = "Let's try a little bit harder";
      }
      return message;
    },
    lastWorkout: function() {
      const lastIndex = this.userInfo.weeklyWorkouts.length - 1;
      return this.userInfo.weeklyWorkouts[lastIndex];
    }
  },
  data() {
    return {
      carotIcon: true,
      avgTime: false,
      topFive: false,
      lastWrkout: false,
      userInfo: {} //end user info
    };
  },
  created() {
    apiSvc.getUserMetrics(this.username).then(data => {
      this.userInfo = data;
    });
  },
  methods: {
    formatDate(dateTime) {
      let dt = new Date(dateTime);
      return dt.toDateString();
    },
    minutesToHours(n) {
      let endHours = "";
      let endMin = "";
      let num = n;
      let hours = num / 60;
      let rhours = Math.floor(hours);
      let minutes = (hours - rhours) * 60;
      let rminutes = Math.round(minutes);
      if (rhours > 1) {
        endHours = rhours + " hours";
      } else {
        endHours = rhours + " hour";
      }
      if (minutes > 0) {
        endMin = rminutes + " minutes";
      } else {
        endMin = rminutes + " minute";
      }
      return `${endHours} \n ${endMin}`;
    }
  }
};
</script>

<style scoped>
.test {
  display: flex;
}
.info_tile {
  display: flex;
  width: 33%;
  color: white;
  background: linear-gradient(90deg,rgba(53, 91, 239, 1) 0%,rgba(54, 216, 248, 1) 100%);
  border-radius: 10px;
  padding: 10px;
  margin: 20px;
}
.info_tile h4 {
  font-weight: bold;
}
.icon {
  color: white;
  height: 100%;
  vertical-align: middle;
  margin-left: 10px;
}
.info_content {
  height: 100%;
  margin: auto;
}
#top_5_list li {
  list-style: none;
}
.duration {
  height: 100%;
  vertical-align: middle;
}
/*overview of past workouts styling*/
.full-info{
    width: 100%;
    padding: 10px; 
}
.overview-container {
  display: flex;
  flex-wrap: wrap;
}
#full-display{
    padding: 0px 10px 40px 10px;
}
.stat-container{
    display: flex;
}
.stat-container p {
  margin: 0;
}
.stat-header {
  font-weight: bold;
}
.stats{
    width: 33%;
    padding: 5px;
    margin: auto;
}
.stat-type {
  font-size: 13px;
}
.right-vr{
    border-right: 2px solid rgba(53,91,239,1);
}
.overview-info{
    display: flex;
    justify-content: center;
    color: black;
    font-size: 20px;
    width: 100%;
    padding: 15px 0px;
    margin-right: 5px;
    border: 2px solid rgba(53,91,239,1);
    border-radius: 5px;
    box-shadow: px 2px 3px black; 
}
.overview-info:hover{
     background: rgb(221, 240, 255);
     cursor: pointer;
}
/*mobile size */
@media screen and (max-width: 600px) {
    .test{
        display: block;
    }
    .info_tile{
        width: 90%;
        margin: auto;
        margin-bottom:20px;
    }
    .info_content{
      padding-left: 10px;
    }
    #overview{
        width: 100%;
    }
    .overview-info{
        font-size: 20px;
        width: 100%!important;
        padding: 15px 0px;
        margin: 5px;
        box-shadow: 2px 2px 5px black; 
    }
    #progress-message{
      display: none;
    }
}
</style>
