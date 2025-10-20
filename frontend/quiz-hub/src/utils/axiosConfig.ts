import axios from 'axios';

const API_URL = 'https://localhost:7034/api';

export const axiosInstance = axios.create({
  baseURL: API_URL,
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      console.log('Token expired or invalid - redirecting to login');
      
      localStorage.removeItem('token');
      localStorage.removeItem('username');
      
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;