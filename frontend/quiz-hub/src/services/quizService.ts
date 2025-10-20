import axiosInstance from '../utils/axiosConfig';

const quizService = {
  getQuizzes: async (filters: { categoryId?: string; difficulty?: string; search?: string }) => {
    const response = await axiosInstance.get('/quiz', { params: filters });
    return response.data;
  },
};

export default quizService;