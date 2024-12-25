<template>
  <div id="app">
    <header>
      <h1>User Authentication App</h1>
    <Navbar v-if="isLoggedIn" />

    </header>

    <main>
      <router-view />
    </main>
    

  </div>
</template>

<script>
import Navbar from "./components/Navbar.vue";

export default {
  name: "App",
  components: { Navbar },
  computed: {
    isLoggedIn() {
      return !!localStorage.getItem("token"); 
    },
  },
  methods: {
    handleTabClose() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      if (userInfo) {
        authService.logOut(userInfo.username).catch(() => {
          console.log("Failed to log out before tab closed.");
        });
      }
    },
  },
  
  created() {
    window.addEventListener("beforeunload", this.handleTabClose);
  },
  destroyed() {
    window.removeEventListener("beforeunload", this.handleTabClose);
  },
};
</script>

<style>
body {
  font-family: Arial, sans-serif;
  margin: 0;
  padding: 0;
  background-color: #f5f5f5;
  color: #333;
}

header {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  z-index: 100;
  background-color: #ec8ee600;
  color: rgb(240, 57, 161);
  padding: 1rem 0;
  text-align: center;
}

main {
  padding: 6rem 2rem;  
}



#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
</style>
