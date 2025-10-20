export interface LeaderboardEntry {
  rank: number;
  userId: string;
  username: string;
  quizId: string;
  quizName: string;
  score: number;
  percentage: number;
  completedAt: string;
  durationSeconds: number;
  isCurrentUser: boolean;
}

export interface LeaderboardResponse {
  entries: LeaderboardEntry[];
  currentUserEntry?: LeaderboardEntry;
  currentUserRank?: number;
  totalEntries: number;
}