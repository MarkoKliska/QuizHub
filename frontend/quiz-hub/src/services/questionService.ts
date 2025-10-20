import axiosInstance from '../utils/axiosConfig';

const questionService = {
  getQuestionsByQuizId: async (quizId: string) => {
    const response = await axiosInstance.get(`/quiz/quizzes/${quizId}/questions`);
    return response.data;
  },
};

export default questionService;