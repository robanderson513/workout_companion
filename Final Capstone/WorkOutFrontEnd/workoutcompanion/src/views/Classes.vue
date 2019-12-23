<template>
  <div>
    <h2>Classes</h2>
    <div class="calendar_container">
      <div class="calendar">
        <calendar :attr="attr" />
      </div>
      <div class="calendar_key">
        <h4>Upcoming Classes:</h4>
        <div class="list_view">
          <div
            :style="{ 'background-color': item.color }"
            v-for="item in classList"
            :key="item.id"
            class="class_item"
          >
            <div class="class_center">
              <h5 class="class_name">{{ item.name }}</h5>
              <hr />
              <div class="class_info">
                <p>{{ dayOfWeek(dayByNumber(item.date)) }}</p>
                <p>{{ timeOfClass(item.date) }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-if="user.roleId > 1" class="employee_btns">
            <b-button class="btn" @click="addBtn"  variant="outline-primary">Add a Class</b-button>
            <b-button class="btn" @click="deleteBtn" variant="outline-secondary">Delete a Class</b-button>
    </div>

    <div class="add_remove_container">
      <div v-if="addClassDiv" class="addClass">
        <h5>Add a Class</h5>
        <b-form @submit.prevent="addClass">
          <div class="form_container">
            <b-form-group
              label="Name of Class:"
              label-for="input-1"
              class="input"
            >
              <b-form-input
                id="input-1"
                v-model="Class.name"
                :class="{ error: nameError }"
                placeholder="Enter name"
              >
              </b-form-input>
              <errors :type="'className'" v-if="nameError" />
            </b-form-group>

            <b-form-group label="Time:" label-for="input-2" class="input">
              <b-form-input
                id="input-2"
                v-model="Class.time"
                :class="{ error: timeError }"
                placeholder="Enter time"
              >
              </b-form-input>
              <errors :type="'classTime'" v-if="timeError" />
            </b-form-group>
          </div>
          <div class="form_container">
            <b-form-group
              label="Start Date: "
              label-for="input-3"
              class="input"
            >
              <v-date-picker
                id="date_picker"
                v-model="Class.startDate"
                placeholder="Enter start date"
              >
              </v-date-picker>
            </b-form-group>

            <b-form-group
              label="Calendar Color: "
              label-for="input-4"
              class="input"
            >
              <b-form-select
                id="color_picker"
                v-model="Class.color"
                :options="Colors"
              >
              </b-form-select>
              <errors :type="'classColor'" v-if="colorError" />
            </b-form-group>
          </div>

          <b-button class="nav_btn" type="submit">Add Class</b-button>
        </b-form>
      </div>

      <div v-if="deleteClassDiv" class="deleteClass">
        <h5>Delete a Class</h5>
        <div class="list_view">
          <div
            :style="{ 'background-color': item.color }"
            v-for="item in classList"
            :key="item.id"
            @click="deleteClass(item.id)"
            class="class_item delete_class_item"
          >
            <div class="class_center">
              <h5 class="class_name">{{ item.name }}</h5>
              <hr />
              <div class="class_info">
                <p>{{ dayOfWeek(dayByNumber(item.date)) }}</p>
                <p>{{ timeOfClass(item.date) }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Calendar from "@/components/classes/Calendar.vue";
import Errors from "../components/Errors";

import { apiService } from "@/Services/api";
const apiSvc = new apiService();
import auth from "@/Services/auth.js";

export default {
  data() {
    return {
      firstTime: true,
      addClassDiv: false,
      deleteClassDiv: false,
      user: {},
      Colors: [
        { text: "Select One", value: null },
        "Gray",
        "Red",
        "Orange",
        "Yellow",
        "Green",
        "Teal",
        "Blue",
        "Indigo",
        "Purple",
        "Pink"
      ],
      classList: [],
      attr: [],
      Class: {
        id: "",
        name: null,
        startDate: "",
        time: null,
        color: null
      },
      sendAddClass: {
        name: "",
        date: "",
        color: ""
      }
    };
  },
  components: {
    Calendar,
    Errors
  },
  created() {
    fetch(`https://localhost:44362/userProfile`, {
      method: "GET",
      headers: {
        // A Header with our authentication token.
        Authorization: "Bearer " + auth.getToken()
      }
    })
      .then(response => response.json())
      .then(json => {
        this.user = json;
      }),
      apiSvc
        .getClassList()
        .then(data => {
          //pass data to list
          this.classList = data;
          //pass data to the calendar
          let today = {
            key: "today",
            dot: true,
            dates: [new Date()]
          };
          this.attr.push(today);
          for (let i = 0; i < data.length; i++) {
            let object = {};
            object.highlight = data[i].color;
            object.popover = {
              label: data[i].name + " " + this.timeOfClass(data[i].date)
            };
            object.dates = {
              start: data[i].date,
              weekdays: [this.dayByNumber(data[i].date) + 1]
            };
            this.attr.push(object);
          }
        })
        .catch(error => {
          this.error = error.message;
        });
  },
  methods: {
    dayOfWeek(number) {
      return [
        "Sun.",
        "Mon.",
        "Tues.",
        "Wed.",
        "Thurs.",
        "Fri.",
        "Sat."
      ][number];
    },
    dayByNumber(dateTime) {
      let dt = new Date(dateTime);
      return dt.getDay();
    },
    timeOfClass(dateTime) {
      let time = dateTime.slice(-8);
      let hourAndMinute = time.split(":");
      return hourAndMinute[0] + ":" + hourAndMinute[1];
    },
    addBtn() {
      if (this.addClassDiv === false) {
        this.addClassDiv = true;
      } else {
        this.addClassDiv = false;
      }
    },
    deleteBtn() {
      if (this.deleteClassDiv === false) {
        this.deleteClassDiv = true;
      } else {
        this.deleteClassDiv = false;
      }
    },
    addClass() {
      this.firstTime = false;
      //populate the new class information
      this.sendAddClass.name = this.Class.name;
      this.sendAddClass.color = this.Class.color;
      //convert the date to proper format YYYY-MM-DDTHH:MM:SS:MS
      let dataDate = this.Class.startDate.toString().split(" ");
      let date = dataDate[1] + " " + dataDate[2] + " " + dataDate[3] + " " + this.Class.time;
      this.sendAddClass.date = date;
      //sends the modified class through api
      apiSvc.addClass(this.sendAddClass);
    },
    deleteClass(id) {
      const confirmation = window.confirm("Are you sure you want to delete?");
      if (confirmation) {
        const classIndex = this.classList.findIndex(i => i.id === id);
        apiSvc.deleteClass(id);
        this.classList.splice(classIndex, 1);
      }
    },
    getMonthFromString(date) {
      return new Date(date).getMonth() + 1;
    }
  },
  computed: {
    nameError: function() {
      return !this.firstTime && !this.Class.name;
    },
    dateError: function() {
      return !this.firstTime && !this.Class.date;
    },
    timeError: function() {
      return !this.firstTime && !this.Class.time;
    },
    colorError: function() {
      return !this.firstTime && !this.Class.color;
    }
  }
};
</script>

<style scoped>
/* calendar styling */
.calendar_container {
  width: 100%;
  display: flex;
}
.calendar {
  width: 50%;
}
.calendar_key {
  width: 50%;
}
/* list item styling */
.list_view {
  display: flex;
  flex-wrap: wrap;
}
.class_item {
  color: white;
  border-radius: 10px;
  width: 25%;
  margin: 5px;
  padding: 5px;
}
.delete_class_item:hover {
  opacity: 0.5;
}
h5 {
  margin: 0;
  font-weight: bold;
}
hr {
  border-color: black;
  margin: 3px 0;
}
p {
  margin: 0;
}
.class_name {
  margin-left: 15px;
}
.class_info {
  display: flex;
  min-width: 90px;
}
.class_info p {
  margin: 0 5px 0 15px;
}
/* employee information styling */
.employee_btns {
  width: 50%;
  padding: 10px;
}
.btn {
  width: 49%;
  margin-right: 5px;
}
.add_remove_container {
  display: flex;
}
.form_container {
  display: flex;
  width: 100%;
}
#color_picker {
  width: 100%;
}
#date_picker {
  width: 100%;
}
.input {
  width: 50%;
  padding-right: 40px;
}
.addClass {
  width: 50%;
  padding: 20px;
}
.deleteClass {
  width: 50%;
  padding: 20px;
}
/*mobile size */
@media screen and (max-width: 600px) {
  .calendar {
    width: 90%;
    margin: auto;
  }
  .calendar_container {
    display: block;
  }
  .calendar_key {
    width: 90%;
    padding: 30px;
  }
  .btn {
    width: 100%;
    margin-bottom: 5px;
  }
  .class_item {
    width: 100%;
  }
  .padding {
    height: 80px;
  }
  .addClass {
    width: 90%;
    margin: auto;
    padding-bottom: 100px;
  }
  .deleteClass {
    width: 90%;
    margin: auto;
    padding-bottom: 100px;
  }
  .class_center {
    width: 80%;
    margin: auto;
  }
  .employee_btns {
    width: 100%;
    padding: 10px;
  }
  .add_remove_container {
    display: block;
  }
  .form_container {
    display: block;
  }
  .input {
    width: 100%;
    padding-right: 0px;
  }
}
/*tablet size */
@media (min-width: 600px) and (max-width: 979px) {
  .calendar {
    width: 70%;
    margin: auto;
  }
  .calendar_container {
    display: block;
  }
  .calendar_key {
    width: 70%;
    padding: 30px;
    margin: auto;
  }
}
</style>
