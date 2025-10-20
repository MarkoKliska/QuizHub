import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import adminService from '../../services/adminService';

interface QuizAttempt {
  id: string;
  userId: string;
  username: string;
  quizId: string;
  quizName: string;
  startTime: string;
  endTime: string | null;
  score: number | null;
  percentage: number | null;
}

const AdminResults: React.FC = () => {
  const [attempts, setAttempts] = useState<QuizAttempt[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchAllResults();
  }, []);

  const fetchAllResults = async () => {
    setLoading(true);
    try {
      // Pozivamo endpoint koji vraća sve pokušaje
      const response = await fetch('https://localhost:7034/api/quizadmin/attempts', {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });
      
      if (!response.ok) {
        throw new Error('Failed to fetch results');
      }
      
      const data = await response.json();
      setAttempts(data);
      setError('');
    } catch (err: any) {
      console.error('Error fetching results:', err);
      setError(err.message || 'Failed to load results');
    } finally {
      setLoading(false);
    }
  };

  const formatDuration = (startTime: string, endTime: string | null) => {
    if (!endTime) return 'In progress';
    const start = new Date(startTime).getTime();
    const end = new Date(endTime).getTime();
    const seconds = Math.floor((end - start) / 1000);
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}m ${secs}s`;
  };

  const getGradeColor = (percentage: number | null) => {
    if (percentage === null) return 'text-gray-600';
    if (percentage >= 90) return 'text-green-600';
    if (percentage >= 80) return 'text-blue-600';
    if (percentage >= 70) return 'text-yellow-600';
    if (percentage >= 60) return 'text-orange-600';
    return 'text-red-600';
  };

  if (loading) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="text-accent text-xl">Loading results...</div>
      </div>
    );
  }

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-6xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h2 className="text-3xl font-bold text-accent">All Quiz Results</h2>
            <p className="text-gray-600 mt-1">View results from all users</p>
          </div>
          <Button onClick={() => navigate('/admin')}>
            Back to Dashboard
          </Button>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {attempts.length === 0 ? (
          <div className="bg-white p-12 rounded-lg shadow-lg text-center">
            <div className="text-6xl mb-4">📊</div>
            <h3 className="text-2xl font-bold text-accent mb-2">No Results Yet</h3>
            <p className="text-gray-600">No users have completed any quizzes yet.</p>
          </div>
        ) : (
          <div className="bg-white rounded-lg shadow-lg overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-gray-100 border-b-2 border-gray-200">
                  <tr>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">
                      User
                    </th>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">
                      Quiz
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">
                      Score
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">
                      Percentage
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">
                      Duration
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">
                      Completed At
                    </th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-gray-600 uppercase">
                      Actions
                    </th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {attempts.map((attempt) => (
                    <tr key={attempt.id} className="hover:bg-gray-50">
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="font-semibold text-accent">{attempt.username}</div>
                      </td>
                      <td className="px-6 py-4">
                        <div className="text-sm text-gray-600">{attempt.quizName}</div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-lg font-bold text-accent">
                          {attempt.score ?? 'N/A'}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className={`text-lg font-bold ${getGradeColor(attempt.percentage)}`}>
                          {attempt.percentage !== null ? `${attempt.percentage.toFixed(1)}%` : 'N/A'}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-sm text-gray-600">
                          {formatDuration(attempt.startTime, attempt.endTime)}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        <div className="text-sm text-gray-600">
                          {attempt.endTime
                            ? new Date(attempt.endTime).toLocaleString()
                            : 'In progress'}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center">
                        {attempt.endTime && (
                          <Button
                            onClick={() => navigate(`/results/${attempt.id}`)}
                            className="bg-blue-600 hover:bg-blue-700 px-4 py-2"
                          >
                            View Details
                          </Button>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="bg-gray-50 px-6 py-4 border-t border-gray-200">
              <div className="text-sm text-gray-600 text-center">
                Total attempts: {attempts.length}
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminResults;