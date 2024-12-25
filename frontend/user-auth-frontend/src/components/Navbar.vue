<template>
  <nav class="navbar">
    <div class="navbar-logo">
      <img src="../img/donut-hamster.jpg" alt="Logo" class="logo" />
    </div>
    <h1>User Authentication App</h1>
    <ul class="navbar-links">
      <li><router-link to="/info">User Info</router-link></li>
      <li v-if="role === 'Admin'">
        <router-link to="/dashboard">Dashboard</router-link>
      </li>
      <li class="logout" @click="logOut">Logout</li>
    </ul>
  </nav>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      role: null,
    };
  },
  async created() {
    const res = await authService.getUserRole();
      this.role = res.data;
  },
  methods: {
    async logOut() {
      const userName = localStorage.getItem("username");
      await authService.logOut(userName).then(() => {
        localStorage.removeItem("token");
        localStorage.removeItem("userInfo");
        this.$router.push("/login");
      });
    },
  },
};
</script>

<style>
.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: linear-gradient(0deg, #ec8ec9 0%, #ffffff 100%);
  color: #fff;
  padding: 10px 20px;
  z-index: 1000;
}

.navbar h1 {
  color: rgb(240, 57, 161);
  margin: 0;
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
}

.navbar-logo .logo {
  height: 50px;
}

.navbar-links {
  list-style: none;
  display: flex;
  gap: 20px;
}

.navbar-links li {
  cursor: pointer;
}

.navbar-links li a {
  text-decoration: none;
  color: rgb(240, 57, 161);
  font-weight: bold;
}

.navbar-links li.logout {
  cursor: pointer;
  font-weight: bold;
  color: rgb(74, 74, 74);
}

.navbar-links li a:hover,
.navbar-links li.logout:hover {
  text-decoration: underline;
}
</style>
