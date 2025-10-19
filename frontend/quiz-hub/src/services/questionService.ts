import axios from 'axios';

const API_URL = 'https://localhost:7034/api';

const questionService = {
  getQuestionsByQuizId: async (quizId: string) => {
    const response = await axios.get(`${API_URL}/quiz/quizzes/${quizId}/questions`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return response.data;
  },
};

export default questionService;