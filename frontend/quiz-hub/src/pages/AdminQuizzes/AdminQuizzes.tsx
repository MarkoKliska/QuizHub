import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import adminService from '../../services/adminService';
import { Quiz } from 'models/Admin';

const AdminQuizzes: React.FC = () => {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchQuizzes();
  }, []);

  const fetchQuizzes = async () => {
    setLoading(true);
    try {
      const data = await adminService.getQuizzes();
      setQuizzes(data);
      setError('');
    } catch (err: any) {
      setError(err.response?.data?.error || 'Failed to load quizzes');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this quiz?')) return;

    try {
      await adminService.deleteQuiz(id);
      fetchQuizzes();
    } catch (err: any) {
      setError(err.response?.data?.error || 'Failed to delete quiz');
    }
  };

  if (loading) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="text-accent text-xl">Loading...</div>
      </div>
    );
  }

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-6xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-3xl font-bold text-accent">Manage Quizzes</h2>
          <div className="flex gap-3">
            <Button
              onClick={() => navigate('/admin/quizzes/create')}
              className="bg-green-600 hover:bg-green-700"
            >
              + New Quiz
            </Button>
            <Button onClick={() => navigate('/admin')}>
              Back to Dashboard
            </Button>
          </div>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {quizzes.length === 0 ? (
          <div className="bg-white p-12 rounded-lg shadow-lg text-center">
            <div className="text-6xl mb-4">📝</div>
            <h3 className="text-2xl font-bold text-accent mb-2">No Quizzes Yet</h3>
            <p className="text-gray-600 mb-6">Create your first quiz to get started!</p>
            <Button
              onClick={() => navigate('/admin/quizzes/create')}
              className="bg-green-600 hover:bg-green-700"
            >
              Create Quiz
            </Button>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {quizzes.map((quiz) => (
              <div key={quiz.id} className="bg-white p-6 rounded-lg shadow-lg">
                <div className="mb-4">
                  <h3 className="text-xl font-bold text-accent mb-2">{quiz.name}</h3>
                  <p className="text-gray-600 text-sm mb-3">{quiz.description}</p>
                  <div className="space-y-1 text-sm text-gray-700">
                    <p><strong>Category:</strong> {quiz.categoryName}</p>
                    <p><strong>Difficulty:</strong> {quiz.difficulty}</p>
                    <p><strong>Time Limit:</strong> {quiz.timeLimit} minutes</p>
                    <p className="text-xs text-gray-500 mt-2">
                      Created: {new Date(quiz.createdAt).toLocaleDateString()}
                    </p>
                  </div>
                </div>

                <div className="flex flex-col gap-2">
                  <Button
                    onClick={() => navigate(`/admin/quizzes/${quiz.id}/questions`)}
                    className="w-full bg-purple-600 hover:bg-purple-700"
                  >
                    Manage Questions
                  </Button>
                  <div className="flex gap-2">
                    <Button
                      onClick={() => navigate(`/admin/quizzes/edit/${quiz.id}`)}
                      className="flex-1 bg-blue-600 hover:bg-blue-700"
                    >
                      Edit
                    </Button>
                    <Button
                      onClick={() => handleDelete(quiz.id)}
                      className="flex-1 bg-red-600 hover:bg-red-700"
                    >
                      Delete
                    </Button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminQuizzes;