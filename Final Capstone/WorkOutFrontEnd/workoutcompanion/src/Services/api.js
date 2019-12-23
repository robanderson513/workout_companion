import axios from "axios";
import auth from "@/Services/auth";

export class apiService {
  constructor() {}

  async login(data) {
    const url = `https://localhost:44362/loginUser`;
    let res = await axios.post(url, data);
    if (res.status === 401) {
      throw "Your username and/or password is invalid";
    }
    return res.data;
  }

  async register(data) {
    const url = `https://localhost:44362/registerUser`;
    let res = axios.post(url, data);
    return res;
  }

  async userStatus() {
    const url = `https://localhost:44362/getUsernameAndRole`;
    let res = axios.get(url);
    return res;
  }

  async getUserInfo(username) {
    const url = `https://localhost:44362/userProfile/${username}`;
    let res = axios.get(url);
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }

  async checkInUser(username) {
    const url = `https://localhost:44362/checkInUser/${username}`;
    let res = axios.post(url, username, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    return res;
  }

  async checkOutUser(username) {
    const url = `https://localhost:44362/checkOutUser/${username}`;
    let res = axios.put(url, username, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }

  async getUserProfile() {
    const url = `https://localhost:44362/userProfile`;
    let res = axios.get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      return res;
  }

  async getMachineList() {
    const url = `https://localhost:44362/displayAllEquipment`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }

  async machineSummary() {
    const url = `https://localhost:44362/equipmentHistory/summary`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }

  async getMachine(id) {
    const url = `https://localhost:44362/displayEquipment/${id}`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }
  async addMachine(data) {
    const url = `https://localhost:44362/addNewMachine/${data}`;
    let res = axios.post(url, data, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }

  async removeMachine(id) {
    const url = `https://localhost:44362/removeMachine/${id}`;
    let res = axios.delete(url, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }
  async addWorkout(data) {
    const url = `https://localhost:44362/logMachineUse`;
    let res = axios.post(url, data, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }
  async searchUser(data) {
    const url = `https://localhost:44362/userProfile/${data}`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }
  async getClassList() {
    const url = `https://localhost:44362/allClasses`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }

  async getUserMetrics(username) {
    const url = `https://localhost:44362/userMetrics/${username}`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }

  async getUserNRole() {
    const url = `https://localhost:44362/getUsernameAndRole`;
    return axios
      .get(url, {
        headers: {
          Authorization: "Bearer " + auth.getToken()
        }
      })
      .then(response => response.data);
  }

  async addClass(data) {
    const url = `https://localhost:44362/createClass/${data}`;
    let res = axios.post(url, data, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }

  async deleteClass(id) {
    const url = `https://localhost:44362/removeClass/${id}`;
    let res = axios.delete(url, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    });
    if (res.status === 400) {
      throw res.data.message;
    }
    return res.data;
  }

  async updateUser(username, id) {
    const url = `https://localhost:44362/editUserRole/${username}/${id}`;
    let res = axios.put(url, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    })
      return res;
  }

  async updateProfile(data) {
    const url = `https://localhost:44362/updateProfile/`;
    let res = axios.put(url, data, {
      headers: {
        Authorization: "Bearer " + auth.getToken()
      }
    })
      return res;
  }
}
