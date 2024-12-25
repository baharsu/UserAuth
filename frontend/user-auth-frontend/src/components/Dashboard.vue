<template>
  <div class="dashboard-container">
    <h1>Dashboard</h1>
    <div class="stat-buttons">
      <button @click="fetchRegistrations">Get Registrations</button>
      <button @click="fetchNotRegistered">Users Not Registered Within 1 Day</button>
      <button @click="fetchOnlineUsers">Get Online Users</button>
      <button @click="fetchAverageLoginTime">Get Average Login Time</button>
    </div>

    <div class="stat-results">
      <h3>Results:</h3>
      <div v-if="loading">Loading...</div>
      <div v-if="error" class="error">{{ error }}</div>
      <div v-else>
        <pre>{{ result }}</pre>
      </div>
    </div>
  </div>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      result: null,
      error: null,
      loading: false,
      loginStartTime: "",
      loginEndTime: "",
    };
  },
  
  methods: {
    created() {
    const username = localStorage.getItem("username");
    if (username) {
      authService
        .getUserByToken() 
        .then((response) => {
          this.loginStartTime = response.data.loginStartTime;
          this.loginEndTime = response.data.loginEndTime;
        })
        .catch(() => {
          console.log("Failed to fetch login times.");
        });
    }
  },
    async fetchRegistrations() {
      this.resetState();
      try {
        const startDate = prompt("Enter start date (YYYY-MM-DD):");
        const endDate = prompt("Enter end date (YYYY-MM-DD):");
        this.loading = true;
        const response = await authService.getRegistrations(startDate, endDate);
        this.result = response.data.data;
      } catch (err) {
        this.error = err.response?.data?.message || "An error occurred.";
      } finally {
        this.loading = false;
      }
    },
    async fetchNotRegistered() {
      this.resetState();
      try {
        this.loading = true;
        const response = await authService.getNotRegisteredWithinOneDay();
        this.result = response.data.data;
      } catch (err) {
        this.error = err.response?.data?.message || "An error occurred.";
      } finally {
        this.loading = false;
      }
    },
    async fetchOnlineUsers() {
      this.resetState();
      try {
        this.loading = true;
        const response = await authService.getOnlineUsers();
        this.result = response.data.data;
      } catch (err) {
        this.error = err.response?.data?.message || "An error occurred.";
      } finally {
        this.loading = false;
      }
    },
    async fetchAverageLoginTime() {
      this.resetState();
      try {
        const date = prompt("Enter date (YYYY-MM-DD):");
        this.loading = true;
        const response = await authService.getAverageLoginTime(date);
        this.result = response.data.data + " seconds";
      } catch (err) {
        this.error = err.response?.data?.message || "An error occurred.";
      } finally {
        this.loading = false;
      }
    },
    resetState() {
      this.result = null;
      this.error = null;
      this.loading = false;
    },
  },
};
</script>

<style scoped>
.dashboard-container {
  max-width: 800px;
  margin: 0 auto;
  text-align: center;
}

.stat-buttons {
  margin-bottom: 20px;
}

.stat-buttons button {
  margin: 5px;
  padding: 10px 20px;
  font-size: 16px;
  background-color: #f53bbd;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.stat-buttons button:hover {
  background-color: #0056b3;
}

.stat-results {
  margin-top: 20px;
  text-align: left;
}

.error {
  color: red;
  font-weight: bold;
}
</style>

