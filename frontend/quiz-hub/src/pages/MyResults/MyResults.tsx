// src/pages/MyResults/MyResults.tsx
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import quizAttemptService from '../../services/quizAttemptService';

interface MyQuizAttempt {
  quizAttemptId: string;
  quizId: string;
  quizName: string;
  completedAt: string;
  score: number;
  percentage: number;
  durationSeconds: number;
}

const MyResults: React.FC = () => {
  const [attempts, setAttempts] = useState<MyQuizAttempt[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchResults = async () => {
      try {
        const data = await quizAttemptService.getMyResults();
        console.log('My results:', data);
        setAttempts(data);
      } catch (err: any) {
        console.error('Error fetching results:', err);
        setError(err.response?.data?.error || 'Failed to load results');
      } finally {
        setLoading(false);
      }
    };

    fetchResults();
  }, []);

  const formatDuration = (seconds: number) => {
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}m ${secs}s`;
  };

  const getGradeColor = (percentage: number) => {
    if (percentage >= 90) return 'text-green-600';
    if (percentage >= 80) return 'text-blue-600';
    if (percentage >= 70) return 'text-yellow-600';
    if (percentage >= 60) return 'text-orange-600';
    return 'text-red-600';
  };

  const getGradeBg = (percentage: number) => {
    if (percentage >= 90) return 'bg-green-50 border-green-200';
    if (percentage >= 80) return 'bg-blue-50 border-blue-200';
    if (percentage >= 70) return 'bg-yellow-50 border-yellow-200';
    if (percentage >= 60) return 'bg-orange-50 border-orange-200';
    return 'bg-red-50 border-red-200';
  };

  if (loading) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="text-accent text-xl">Loading your results...</div>
      </div>
    );
  }

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-6xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-3xl font-bold text-accent">My Quiz Results</h2>
          <Button onClick={() => navigate('/')}>
            Back to Quizzes
          </Button>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {attempts.length === 0 ? (
          <div className="bg-white p-12 rounded-lg shadow-lg text-center">
            <div className="text-6xl mb-4">📝</div>
            <h3 className="text-2xl font-bold text-accent mb-2">No Quiz Attempts Yet</h3>
            <p className="text-gray-600 mb-6">
              Start taking quizzes to see your results here!
            </p>
            <Button onClick={() => navigate('/')}>
              Browse Quizzes
            </Button>
          </div>
        ) : (
          <>
            {/* Summary Stats */}
            <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
              <div className="bg-white p-6 rounded-lg shadow-md text-center">
                <div className="text-4xl font-bold text-accent">{attempts.length}</div>
                <div className="text-gray-600 mt-2">Total Attempts</div>
              </div>
              <div className="bg-white p-6 rounded-lg shadow-md text-center">
                <div className="text-4xl font-bold text-green-600">
                  {attempts.filter(a => a.percentage >= 70).length}
                </div>
                <div className="text-gray-600 mt-2">Passed (≥70%)</div>
              </div>
              <div className="bg-white p-6 rounded-lg shadow-md text-center">
                <div className="text-4xl font-bold text-blue-600">
                  {(attempts.reduce((sum, a) => sum + a.percentage, 0) / attempts.length).toFixed(1)}%
                </div>
                <div className="text-gray-600 mt-2">Average Score</div>
              </div>
              <div className="bg-white p-6 rounded-lg shadow-md text-center">
                <div className="text-4xl font-bold text-purple-600">
                  {Math.max(...attempts.map(a => a.percentage)).toFixed(1)}%
                </div>
                <div className="text-gray-600 mt-2">Best Score</div>
              </div>
            </div>

            {/* Attempts List */}
            <div className="space-y-4">
              {attempts.map((attempt) => (
                <div
                  key={attempt.quizAttemptId}
                  className={`bg-white p-6 rounded-lg shadow-lg border-l-4 ${getGradeBg(attempt.percentage)}`}
                >
                  <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                    <div className="flex-1">
                      <h3 className="text-xl font-bold text-accent mb-2">
                        {attempt.quizName}
                      </h3>
                      <div className="flex flex-wrap gap-4 text-sm text-gray-600">
                        <span>
                          📅 {new Date(attempt.completedAt).toLocaleDateString()} at{' '}
                          {new Date(attempt.completedAt).toLocaleTimeString()}
                        </span>
                        <span>⏱️ Duration: {formatDuration(attempt.durationSeconds)}</span>
                      </div>
                    </div>

                    <div className="flex items-center gap-6">
                      <div className="text-center">
                        <div className={`text-3xl font-bold ${getGradeColor(attempt.percentage)}`}>
                          {attempt.percentage.toFixed(1)}%
                        </div>
                        <div className="text-sm text-gray-600">
                          {attempt.score} points
                        </div>
                      </div>

                      <Button
                        onClick={() => navigate(`/results/${attempt.quizAttemptId}`)}
                        className="whitespace-nowrap"
                      >
                        View Details
                      </Button>
                    </div>
                  </div>

                  {/* Progress Bar */}
                  <div className="mt-4">
                    <div className="w-full bg-gray-200 rounded-full h-2">
                      <div
                        className={`h-2 rounded-full transition-all ${
                          attempt.percentage >= 90
                            ? 'bg-green-500'
                            : attempt.percentage >= 70
                            ? 'bg-blue-500'
                            : attempt.percentage >= 50
                            ? 'bg-yellow-500'
                            : 'bg-red-500'
                        }`}
                        style={{ width: `${attempt.percentage}%` }}
                      />
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default MyResults;