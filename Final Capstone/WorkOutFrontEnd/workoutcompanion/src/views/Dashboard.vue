<template>
  <div>
    <h2>Welcome back {{userData.firstName}}, let's go! </h2>
      <div v-if="userData.roleId > 1">
        <equipment-usage class="bar_graph"/> 
     </div>
    <div v-if="userData.roleId >= 1">
      <user-data :fullDisplay="true" :username="userData.username" />
    </div>
    <div class="calendar_container">
    <div >
        <h2>Upcoming Classes</h2>
         <calendar :attr="attr" class="calendar"/>
    </div>
    </div>
</div>
</template>

<script>
import UserData from './../components/UserData'
import EquipmentUsage from './../components/EquipmentUsage'
import Calendar from '@/components/classes/Calendar.vue';

import { apiService } from "@/Services/api";
const apiSvc = new apiService();

export default {
  data() {
    return {
      attr: [],
      userData: {}
    }
  },
  computed:{

  },
  components: {
    UserData,
    EquipmentUsage,
    Calendar
  },
  created(){
        this.getUser();
        apiSvc.getClassList()
        .then(classData => {
            //pass data to the calendar
            let today ={
                key: 'today',
                dot: true,
                dates: [new Date()]
            }
            this.attr.push(today)
            for(let i =0; i < classData.length; i++){
                let object = {};
                object.highlight = classData[i].color;
                object.popover = {label: classData[i].name + ' ' + this.timeOfClass(classData[i].date)};
                object.dates = {start: classData[i].date, weekdays: [this.dayByNumber(classData[i].date) + 1]};
                this.attr.push(object)
            }
        })
        .catch (error => {
            this.error = error.message;
        })
        
  },
  methods: {
    timeOfClass(dateTime){
           let time=  dateTime.slice(-11);
           let hourAndMinute = time.split(':')
           return hourAndMinute[0]+":"+hourAndMinute[1]
        },
    dayByNumber(dateTime){
            let dt = new Date(dateTime)
            return dt.getDay();
        },
        getUser(){
          apiSvc.getUserProfile()
          .then( (response)=>{
           this.userData = response.data;
           this.$emit('send-user', this.userData)
          })
        }
  }    
}
</script>

<style scoped>
.calendar_container{
  width: 100%;
}
.calendar{
  width: 100%;
}
.calendar h4 {
  margin: auto;
}
#equipment-chart {
  display: flex;
  justify-content: center;
}
/*mobile size */
@media screen and (max-width: 600px) {
  .calendar {
    padding-bottom: 150px;
  }
  .bar_graph{
    display: none;
  }
}
</style>
