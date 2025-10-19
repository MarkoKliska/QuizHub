import { LeaderboardResponse } from 'models/Leaderboard';
import axiosInstance from '../utils/axiosConfig';


const leaderboardService = {
  getLeaderboard: async (filters: {
    quizId?: string;
    timePeriod?: string;
  }): Promise<LeaderboardResponse> => {
    const response = await axiosInstance.get('/quiz/leaderboard', { params: filters });
    return response.data;
  },
};

export default leaderboardService;