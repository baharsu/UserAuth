import { createRouter, createWebHistory } from "vue-router"; 
import Login from "../components/Login.vue";
import Register from "../components/Register.vue";
import Verification from "../components/Verification.vue";
import UserInfo from "../components/UserInfo.vue";
import Dashboard from "../components/Dashboard.vue";
import ForgotPassword from "@/components/ForgotPassword.vue";
import ResetPassword from "@/components/ResetPassword.vue";

const routes = [
  { path: "/", component: Login },
  { path: "/register", component: Register },
  { path: "/verification", component: Verification },
  { path: "/info", component: UserInfo },
  { path: "/dashboard", component: Dashboard },
  { path: "/forgot-password", component: ForgotPassword },
  { path: "/reset-password", component: ResetPassword },
];

const router = createRouter({
  history: createWebHistory(), 
  routes: [
    { path: "/login", component: Login },
    { path: "/register", component: Register },
    { path: "/verification", component: Verification },
    { path: "/forgot-password", component: ForgotPassword },
    { path: "/reset-password", component: ResetPassword },
    { 
      path: "/dashboard", 
      component: Dashboard, 
      meta: { requiresAuth: true } 
    },
    { 
      path: "/info", 
      component: UserInfo, 
      meta: { requiresAuth: true } 
    },
    { 
      path: "/:pathMatch(.*)*", 
      name: "NotFound", 
      component: Login
    },
  ],
});

router.beforeEach((to, from, next) => {
  const isLoggedIn = !!localStorage.getItem("token"); 

  if (to.matched.some(record => record.meta.requiresAuth) && !isLoggedIn) {
    next("/login"); 
  } else {
    next();
  }
});


export default router;
