// src/services/quizAttemptService.ts
import axios from 'axios';
import { QuizAttempt, QuizAttemptDetails } from '../models/QuizAttempt';

const API_URL = 'https://localhost:7034/api';

const quizAttemptService = {
  startQuizAttempt: async (quizId: string): Promise<QuizAttempt> => {
    const response = await axios.post(
      `${API_URL}/quiz/attempts`,
      { quizId },
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      }
    );
    return response.data;
  },

  submitQuizAttempt: async (
    quizAttemptId: string, 
    answers: { questionId: string; selectedOptionIds?: string[]; textAnswer?: string }[]
  ): Promise<QuizAttempt> => {
    const response = await axios.post(
      `${API_URL}/quiz/attempts/submit`,
      { quizAttemptId, answers },
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      }
    );
    return response.data;
  },

  getQuizAttemptDetails: async (attemptId: string): Promise<QuizAttemptDetails> => {
    const response = await axios.get(
      `${API_URL}/quiz/attempts/${attemptId}`,
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      }
    );
    return response.data;
  },

  getMyResults: async () => {
    const response = await axios.get(
      `${API_URL}/quiz/attempts/user`,
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      }
    );
    return response.data;
  },
};

export default quizAttemptService;