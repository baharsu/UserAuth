import axios from "axios";

const API_URL = "https://localhost:7104/api";

export default {
  login(username, password) {
    return axios.post(`${API_URL}/Authentication/Login`, {
      username,
      password,
    });
  },

  register(data) {
    return axios.post(`${API_URL}/Authentication/Register`, data);
  },

  verifyCode(username, verificationCode, loginStartTime) {
    return axios.post(`${API_URL}/Authentication/Verify`, {
      username,
      verificationCode,
      loginStartTime
    });
  },

  getUserInfo() {
    return axios.get(`${API_URL}/User/GetUserByToken`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },

  forgotPassword(email) {
    return axios.post(`${API_URL}/Authentication/ForgotPassword`, null,{
      params: { email },
    });
  },

  resetPassword(code, newPassword) {
    return axios.put(`${API_URL}/Authentication/ResetPassword`, null,{
      params: { code, newPassword },
    });
  },

  getRegistrations(startDate, endDate) {
    return axios.get(`${API_URL}/Statistics/GetRegistrations`, {
      params: { startDate, endDate },
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },
  
  getNotRegisteredWithinOneDay() {
    return axios.get(`${API_URL}/Statistics/NotRegisteredWithinOneDay`,{
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },
  
  getOnlineUsers() {
    return axios.get(`${API_URL}/Statistics/GetOnlineUsers`,{
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },
  
  getAverageLoginTime(day) {
    return axios.get(`${API_URL}/Statistics/GetAverageLoginTime`, {
      params: { day },
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },

  logOut(username) {
    return axios.post(`${API_URL}/Authentication/LogOut`, null, { 
      params: { username },
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
     });
  },

  getUserRole() {
    return axios.get(`${API_URL}/User/GetUserRole`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
  },
  
};

