import { Category, CreateCategoryDto, CreateQuestionDto, CreateQuizDto, Question, Quiz } from 'models/Admin';
import axiosInstance from '../utils/axiosConfig';


const adminService = {
  createCategory: async (data: CreateCategoryDto): Promise<Category> => {
    const response = await axiosInstance.post('/quizadmin/categories', data);
    return response.data;
  },

  getCategories: async (): Promise<Category[]> => {
    const response = await axiosInstance.get('/quizadmin/categories');
    return response.data;
  },

  getCategory: async (id: string): Promise<Category> => {
    const response = await axiosInstance.get(`/quizadmin/categories/${id}`);
    return response.data;
  },

  updateCategory: async (id: string, data: CreateCategoryDto): Promise<Category> => {
    const response = await axiosInstance.put(`/quizadmin/categories/${id}`, data);
    return response.data;
  },

  deleteCategory: async (id: string): Promise<void> => {
    await axiosInstance.delete(`/quizadmin/categories/${id}`);
  },

  // Quiz Operations
  createQuiz: async (data: CreateQuizDto): Promise<Quiz> => {
    const response = await axiosInstance.post('/quizadmin', data);
    return response.data;
  },

  getQuizzes: async (): Promise<Quiz[]> => {
    const response = await axiosInstance.get('/quizadmin');
    return response.data;
  },

  getQuiz: async (id: string): Promise<Quiz> => {
    const response = await axiosInstance.get(`/quizadmin/${id}`);
    return response.data;
  },

  updateQuiz: async (id: string, data: CreateQuizDto): Promise<Quiz> => {
    const response = await axiosInstance.put(`/quizadmin/${id}`, data);
    return response.data;
  },

  deleteQuiz: async (id: string): Promise<void> => {
    await axiosInstance.delete(`/quizadmin/${id}`);
  },

  createQuestion: async (data: CreateQuestionDto): Promise<Question> => {
    const response = await axiosInstance.post('/quizadmin/questions', data);
    return response.data;
  },

  getQuestionsByQuizId: async (quizId: string): Promise<Question[]> => {
    const response = await axiosInstance.get(`/quizadmin/quizzes/${quizId}/questions`);
    return response.data;
  },

  getQuestion: async (id: string): Promise<Question> => {
    const response = await axiosInstance.get(`/quizadmin/questions/${id}`);
    return response.data;
  },

  updateQuestion: async (id: string, data: CreateQuestionDto): Promise<Question> => {
    const response = await axiosInstance.put(`/quizadmin/questions/${id}`, data);
    return response.data;
  },

  deleteQuestion: async (id: string): Promise<void> => {
    await axiosInstance.delete(`/quizadmin/questions/${id}`);
  },

  getAllQuizAttempts: async (): Promise<any[]> => {
    const response = await axiosInstance.get('/quizadmin/attempts');
    return response.data;
  },
};

export default adminService;