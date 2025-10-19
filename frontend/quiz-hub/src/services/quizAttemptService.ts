import axiosInstance from '../utils/axiosConfig';
import { QuizAttempt, QuizAttemptDetails } from '../models/QuizAttempt';

const quizAttemptService = {
  startQuizAttempt: async (quizId: string): Promise<QuizAttempt> => {
    const response = await axiosInstance.post('/quiz/attempts', { quizId });
    return response.data;
  },

  submitQuizAttempt: async (
    quizAttemptId: string, 
    answers: { questionId: string; selectedOptionIds?: string[]; textAnswer?: string }[]
  ): Promise<QuizAttempt> => {
    const response = await axiosInstance.post('/quiz/attempts/submit', { quizAttemptId, answers });
    return response.data;
  },

  getQuizAttemptDetails: async (attemptId: string): Promise<QuizAttemptDetails> => {
    const response = await axiosInstance.get(`/quiz/attempts/${attemptId}`);
    return response.data;
  },

  getMyResults: async () => {
    const response = await axiosInstance.get('/quiz/attempts/user');
    return response.data;
  },
};

export default quizAttemptService;