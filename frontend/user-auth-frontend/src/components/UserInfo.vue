<template>
  <div class="user-info-page">
    <h1>User Info</h1>
    <div v-if="userInfo">
      <p><strong>Name:</strong> {{ userInfo.name }}</p>
      <p><strong>Surname:</strong> {{ userInfo.surname }}</p>
      <p><strong>Username:</strong> {{ userInfo.username }}</p>
      <p><strong>Email:</strong> {{ userInfo.email }}</p>
      <div v-if="role === 'Admin'">
        <router-link to="/dashboard">Go to Dashboard</router-link>
      </div>
    </div>
    <div v-else>
      <p>Loading user info...</p>
    </div>
  </div>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      userInfo: null, 
      role: null,
    };
  },
  async created() {
    try {
      const response = await authService.getUserInfo();
      this.userInfo = response.data;
      const res = await authService.getUserRole();
      this.role = res.data;
    } catch (error) {
      console.error("Failed to fetch user info:", error);
      this.userInfo = null; 
    }
  },
  watch: {
    $route(to, from) {
      if (!localStorage.getItem("token")) {
        this.userInfo = null;
        this.role = null;
      }
    },
  },
};
</script>

<style scoped>
.user-info-page {
  max-width: 400px;
  margin: auto;
  text-align: center;
  
}

a {
  color: #f53bbd;
  text-decoration: none;
}

a:hover {
  text-decoration: underline;
}
</style>
