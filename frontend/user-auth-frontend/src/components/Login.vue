<template>
  <div class="login-page">
    <h1>Login</h1>
    <form @submit.prevent="login">
      <div>
        <input v-model="username" placeholder="Username" required />
      </div>
      <div>
        <input
          type="password"
          v-model="password"
          placeholder="Password"
          required
        />
      </div>
      <button type="submit">Login</button>
    </form>
    <p>
      Don't have an account?
      <router-link to="/register">Register here</router-link>
    </p>
    <p>
      <a href="/forgot-password">Forgot Password?</a>
    </p>
  </div>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      username: "",
      password: "",
      message: "",
    };
  },
  async created() {
    localStorage.setItem("loginStartTime", new Date().toISOString());
    console.log("Login start time: ", localStorage.getItem("loginStartTime"));
    
    },
  methods: {
    async login() {
      try {
        
        const response = await authService.login(this.username, this.password);
        localStorage.setItem("username", this.username);
        this.$router.push("/verification");
      } catch (error) {
        alert("Login failed: " + error.response.data.message);
      }
    },
  },
};
</script>

<style scoped>
h2 {
  text-align: center;
}

button {
  width: 100%;
  margin-top: 10px;
  background-color: #f53bbd;
}
.login-page {
  width: 100%;
  max-width: 400px;
  padding: 20px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
}

a {
  color: #f53bbd;
  text-decoration: none;
}
</style>
