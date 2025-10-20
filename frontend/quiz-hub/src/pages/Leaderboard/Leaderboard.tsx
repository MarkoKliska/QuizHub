import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import { LeaderboardResponse } from 'models/Leaderboard';
import quizService from '../../services/quizService';
import { Quiz } from '../../models/Quiz';
import leaderboardService from 'services/leaderboardService';

const Leaderboard: React.FC = () => {
  const [data, setData] = useState<LeaderboardResponse | null>(null);
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [selectedQuizId, setSelectedQuizId] = useState<string>('');
  const [timePeriod, setTimePeriod] = useState<string>('all');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();

  const timePeriods = [
    { value: 'all', label: 'All Time' },
    { value: 'monthly', label: 'This Month' },
    { value: 'weekly', label: 'This Week' },
  ];

  useEffect(() => {
    const fetchQuizzes = async () => {
      try {
        const quizData = await quizService.getQuizzes({});
        setQuizzes(quizData);
      } catch (err) {
        console.error('Failed to load quizzes:', err);
      }
    };
    fetchQuizzes();
  }, []);

  useEffect(() => {
    fetchLeaderboard();
  }, [selectedQuizId, timePeriod]);

  const fetchLeaderboard = async () => {
    setLoading(true);
    try {
      const filters: { quizId?: string; timePeriod?: string } = {};
      if (selectedQuizId) filters.quizId = selectedQuizId;
      if (timePeriod !== 'all') filters.timePeriod = timePeriod;

      const result = await leaderboardService.getLeaderboard(filters);
      setData(result);
      setError('');
    } catch (err: any) {
      console.error('Error fetching leaderboard:', err);
      setError(err.response?.data?.error || 'Failed to load leaderboard');
    } finally {
      setLoading(false);
    }
  };

  const formatDuration = (seconds: number) => {
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}m ${secs}s`;
  };

  const getRankMedal = (rank: number) => {
    if (rank === 1) return '#1';
    if (rank === 2) return '#2';
    if (rank === 3) return '#3';
    return `#${rank}`;
  };

  const getRankColor = (rank: number) => {
    if (rank === 1) return 'text-yellow-500';
    if (rank === 2) return 'text-gray-400';
    if (rank === 3) return 'text-orange-400';
    return 'text-gray-600';
  };

  const getRankBg = (rank: number, isCurrentUser: boolean) => {
    if (isCurrentUser) return 'bg-blue-50 border-blue-400 border-2';
    if (rank === 1) return 'bg-yellow-50 border-yellow-200';
    if (rank === 2) return 'bg-gray-50 border-gray-200';
    if (rank === 3) return 'bg-orange-50 border-orange-200';
    return 'bg-white border-gray-200';
  };

  if (loading) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="text-accent text-xl">Loading leaderboard...</div>
      </div>
    );
  }

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-6xl mx-auto">
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-6 gap-4">
          <div>
            <h2 className="text-3xl font-bold text-accent"> Leaderboard</h2>
            <p className="text-gray-600 mt-1">Top quiz performers</p>
          </div>
          <Button onClick={() => navigate('/')}>Back to Quizzes</Button>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <div className="bg-white p-6 rounded-lg shadow-lg mb-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-semibold text-accent mb-2">
                Filter by Quiz
              </label>
              <select
                value={selectedQuizId}
                onChange={(e) => setSelectedQuizId(e.target.value)}
                className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
              >
                <option value="">All Quizzes</option>
                {quizzes.map((quiz) => (
                  <option key={quiz.id} value={quiz.id}>
                    {quiz.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-semibold text-accent mb-2">
                Time Period
              </label>
              <select
                value={timePeriod}
                onChange={(e) => setTimePeriod(e.target.value)}
                className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
              >
                {timePeriods.map((period) => (
                  <option key={period.value} value={period.value}>
                    {period.label}
                  </option>
                ))}
              </select>
            </div>
          </div>
        </div>

        {data?.currentUserEntry && (
          <div className="bg-blue-50 border-2 border-blue-400 p-6 rounded-lg shadow-lg mb-6">
            <h3 className="text-xl font-bold text-accent mb-3">Your Position</h3>
            <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
              <div className="flex items-center gap-4">
                <div className={`text-4xl font-bold ${getRankColor(data.currentUserRank!)}`}>
                  {getRankMedal(data.currentUserRank!)}
                </div>
                <div>
                  <div className="font-semibold text-lg text-accent">
                    {data.currentUserEntry.username}
                  </div>
                  <div className="text-sm text-gray-600">
                    {data.currentUserEntry.quizName}
                  </div>
                </div>
              </div>
              <div className="flex gap-6">
                <div className="text-center">
                  <div className="text-2xl font-bold text-accent">
                    {data.currentUserEntry.score}
                  </div>
                  <div className="text-xs text-gray-600">Points</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-green-600">
                    {data.currentUserEntry.percentage.toFixed(1)}%
                  </div>
                  <div className="text-xs text-gray-600">Score</div>
                </div>
              </div>
            </div>
          </div>
        )}

        {!data || data.entries.length === 0 ? (
          <div className="bg-white p-12 rounded-lg shadow-lg text-center">
            <div className="text-6xl mb-4"></div>
            <h3 className="text-2xl font-bold text-accent mb-2">No Results Yet</h3>
            <p className="text-gray-600 mb-6">
              Be the first to appear on the leaderboard!
            </p>
            <Button onClick={() => navigate('/')}>Start Taking Quizzes</Button>
          </div>
        ) : (
          <div className="bg-white rounded-lg shadow-lg overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-gray-100 border-b-2 border-gray-200">
                  <tr>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Rank
                    </th>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Player
                    </th>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Quiz
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Score
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Percentage
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Time
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase tracking-wider">
                      Date
                    </th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {data.entries.map((entry) => (
                    <tr
                      key={`${entry.userId}-${entry.quizId}-${entry.rank}`}
                      className={`${getRankBg(entry.rank, entry.isCurrentUser)} hover:bg-gray-50 transition-colors`}
                    >
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className={`text-2xl font-bold ${getRankColor(entry.rank)}`}>
                          {getRankMedal(entry.rank)}
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="flex items-center">
                          <div className="font-semibold text-accent">
                            {entry.username}
                            {entry.isCurrentUser && (
                              <span className="ml-2 text-xs bg-blue-500 text-white px-2 py-1 rounded">
                                You
                              </span>
                            )}
                          </div>
                        </div>
                      </td>
                      <td className="px-6 py-4">
                        <div className="text-sm text-gray-600">{entry.quizName}</div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-lg font-bold text-accent">{entry.score}</div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-lg font-bold text-green-600">
                          {entry.percentage.toFixed(1)}%
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-sm text-gray-600">
                          {formatDuration(entry.durationSeconds)}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-sm text-gray-600">
                          {new Date(entry.completedAt).toLocaleDateString()}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="bg-gray-50 px-6 py-4 border-t border-gray-200">
              <div className="text-sm text-gray-600 text-center">
                Showing top {data.entries.length} of {data.totalEntries} entries
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Leaderboard;