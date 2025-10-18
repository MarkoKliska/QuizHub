import axios from 'axios';
import { LoginRequestDto, LoginResponseDto } from '../models/LoginDto';
import { RegisterUserRequestDto, RegisterUserResponseDto } from '../models/RegisterDto';

const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7034';

const authService = {
  register: async (data: RegisterUserRequestDto): Promise<RegisterUserResponseDto> => {
    const response = await axios.post(`${API_URL}/api/auth/register`, data);
    return response.data;
  },

  login: async (data: LoginRequestDto): Promise<LoginResponseDto> => {
    const response = await axios.post(`${API_URL}/api/auth/login`, data);
    return response.data;
  },
};

export default authService;