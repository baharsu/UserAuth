<template>
  <div class="verification-page">
    <h1>Verification</h1>
    <form @submit.prevent="verifyCode">
      <input
        v-model="verificationCode"
        placeholder="Verification Code"
        required
      />
      <button type="submit">Verify</button>
    </form>
  </div>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      verificationCode: "",
    };
  },
  methods: {
    async verifyCode() {
      const username = localStorage.getItem("username");
      const loginStartTime = localStorage.getItem("loginStartTime");      
      try {
        const response = await authService.verifyCode(
          username,
          this.verificationCode,
          loginStartTime
        );
        localStorage.setItem("token", response.data.data);

        this.$router.push("/info");
      } catch (error) {
        alert("Verification failed: " + error.response.data.message);
      }
    },
  },
};
</script>

<style scoped>
.verification-page {
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
