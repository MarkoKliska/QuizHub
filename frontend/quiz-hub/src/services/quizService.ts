import axios from 'axios';

const API_URL = 'https://localhost:7034/api';

const quizService = {
  getQuizzes: async (filters: { categoryId?: string; difficulty?: string; search?: string }) => {
    const response = await axios.get(`${API_URL}/quiz`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      params: filters,
    });
    return response.data;
  },
};

export default quizService;