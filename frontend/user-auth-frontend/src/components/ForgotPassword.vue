<template>
    <div class="password-container">
      <h2>Forgot Password</h2>
      <form @submit.prevent="requestPasswordReset">
        <div class="form-group">
          <input type="email" id="email" v-model="email" placeholder="Enter your email" required />
        </div>
        <button type="submit">Send Reset Code</button>
      </form>
      <div v-if="message" class="message">
      <p>{{ message }}</p>
      <router-link v-if="resetLinkSent" to="/reset-password" class="btn">
        Go to Reset Password
      </router-link>
    </div>
    </div>
  </template>
  
  <script>
  import authService from "../services/authService";
  
  export default {
    data() {
      return {
        email: "",
        message: "",
        resetLinkSent: false,
      };
    },
    methods: {
      async requestPasswordReset() {
        try {
          await authService.forgotPassword(this.email);
          this.message = "A reset code has been sent to your email.";
          this.resetLinkSent = true;
        } catch (error) {
          this.message = "An error occurred. Please try again.";
          this.resetLinkSent = false;
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
  