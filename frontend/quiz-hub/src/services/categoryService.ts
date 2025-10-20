import axiosInstance from '../utils/axiosConfig';

const categoryService = {
  getCategories: async () => {
    const response = await axiosInstance.get('/quiz/categories');
    return response.data;
  },
};

export default categoryService;