<template>
  <div class="password-container">
    <h2>Reset Password</h2>
    <form @submit.prevent="resetPassword">
      <div class="form-group">
        <input
          id="verificationCode"
          v-model="verificationCode"
          placeholder="Enter your verification code"
          required
        />
      </div>
      <div class="form-group">
        <input
          type="password"
          id="newPassword"
          v-model="newPassword"
          placeholder="Enter your new password"
          required
        />
      </div>
      <div class="form-group">
        <input
          type="password"
          id="confirmPassword"
          v-model="confirmPassword"
          placeholder="Confirm your new password"
          required
        />
      </div>
      <button type="submit">Reset Password</button>
    </form>
    <p v-if="message">{{ message }}</p>
  </div>
</template>

<script>
import authService from "../services/authService";

export default {
  data() {
    return {
      verificationCode: "", 
      newPassword: "",
      confirmPassword: "",
      message: "",
    };
  },
  methods: {
    async resetPassword() {
      if (this.newPassword !== this.confirmPassword) {
        this.message = "Passwords do not match.";
        return;
      }
      if (!this.verificationCode) {
        this.message = "Please enter the verification code.";
        return;
      }
      try {
        await authService.resetPassword(this.verificationCode, this.newPassword);
        this.message = "Password successfully reset!";
        this.$router.push("/login"); 
      } catch (error) {
        this.message = "An error occurred. Please try again.";
      }
    },
  },
};
</script>

<style scoped>
.password-container {
  max-width: 400px;
  margin: 0 auto;
  text-align: center;
}
.form-group {
  margin-bottom: 15px;
}
input {
  width: 100%;
  padding: 8px;
  margin-top: 5px;
  margin-bottom: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
button {
  padding: 10px 20px;
  background-color: #f53bbd;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
button:hover {
  background-color: #0056b3;
}
</style>
