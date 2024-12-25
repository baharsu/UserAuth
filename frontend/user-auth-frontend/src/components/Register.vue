<template>
  <div class="register-page">
    <h1>Register</h1>
    <form @submit.prevent="register">
      <input v-model="name" placeholder="Name" required />
      <input v-model="surname" placeholder="Surname" required />
      <input v-model="username" placeholder="Username" required />
      <input v-model="email" placeholder="Email" required />
      <input
        type="password"
        v-model="password"
        placeholder="Password"
        required
      />
      <button type="submit">Register</button>
    </form>
  </div>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      name: "",
      surname: "",
      username: "",
      email: "",
      password: "",
    };
  },
  methods: {
    async register() {
      try {
        await authService.register({
          name: this.name,
          surname: this.surname,
          username: this.username,
          email: this.email,
          password: this.password,
        });
        localStorage.setItem("username", this.username);
        this.$router.push("/verification");
      } catch (error) {
        alert("Registration failed: " + error.response.data.message);
      }
    },
  },
};
</script>

<style scoped>
.register-page {
  max-width: 400px;
  margin: auto;
  text-align: center;
  width: 100%;
  padding: 20px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
