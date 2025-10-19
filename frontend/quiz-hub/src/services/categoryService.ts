// src/services/authService.ts
import axios from 'axios';

const API_URL = 'https://localhost:7034/api';

const categoryService = {
  getCategories: async () => {
    const response = await axios.get(`${API_URL}/quiz/categories`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return response.data;
  },
};

export default categoryService;