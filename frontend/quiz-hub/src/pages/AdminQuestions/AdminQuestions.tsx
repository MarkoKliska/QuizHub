import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Button from '../../components/common/Button';
import adminService from '../../services/adminService';
import { Question } from 'models/Admin';

const AdminQuestions: React.FC = () => {
  const { quizId } = useParams<{ quizId: string }>();
  const [questions, setQuestions] = useState<Question[]>([]);
  const [quizName, setQuizName] = useState<string>('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();

  useEffect(() => {
    if (quizId) {
      fetchQuestions();
      fetchQuizInfo();
    }
  }, [quizId]);

  const fetchQuizInfo = async () => {
    if (!quizId) return;
    try {
      const quiz = await adminService.getQuiz(quizId);
      setQuizName(quiz.name);
    } catch (err: any) {
      console.error('Failed to load quiz info:', err);
    }
  };

  const fetchQuestions = async () => {
    if (!quizId) return;
    setLoading(true);
    try {
      const data = await adminService.getQuestionsByQuizId(quizId);
      setQuestions(data);
      setError('');
    } catch (err: any) {
      setError(err.response?.data?.error || 'Failed to load questions');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this question?')) return;

    try {
      await adminService.deleteQuestion(id);
      fetchQuestions();
    } catch (err: any) {
      setError(err.response?.data?.error || 'Failed to delete question');
    }
  };

  const getQuestionTypeLabel = (type: string) => {
    return type.replace(/([A-Z])/g, ' $1').trim();
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
          <div>
            <h2 className="text-3xl font-bold text-accent">Manage Questions</h2>
            <p className="text-gray-600 mt-1">Quiz: {quizName}</p>
          </div>
          <div className="flex gap-3">
            <Button
              onClick={() => navigate(`/admin/quizzes/${quizId}/questions/create`)}
              className="bg-green-600 hover:bg-green-700"
            >
              + New Question
            </Button>
            <Button onClick={() => navigate('/admin/quizzes')}>
              Back to Quizzes
            </Button>
          </div>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {questions.length === 0 ? (
          <div className="bg-white p-12 rounded-lg shadow-lg text-center">
            <div className="text-6xl mb-4">❓</div>
            <h3 className="text-2xl font-bold text-accent mb-2">No Questions Yet</h3>
            <p className="text-gray-600 mb-6">Add questions to this quiz to get started!</p>
            <Button
              onClick={() => navigate(`/admin/quizzes/${quizId}/questions/create`)}
              className="bg-green-600 hover:bg-green-700"
            >
              Create First Question
            </Button>
          </div>
        ) : (
          <div className="space-y-4">
            {questions.map((question, index) => (
              <div key={question.id} className="bg-white p-6 rounded-lg shadow-lg">
                <div className="flex justify-between items-start mb-4">
                  <div className="flex-1">
                    <div className="flex items-center gap-3 mb-2">
                      <span className="bg-secondary text-white px-3 py-1 rounded-full text-sm font-semibold">
                        Q{index + 1}
                      </span>
                      <span className="bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm font-semibold">
                        {getQuestionTypeLabel(question.type)}
                      </span>
                      <span className="bg-green-100 text-green-800 px-3 py-1 rounded-full text-sm font-semibold">
                        {question.points} {question.points === 1 ? 'point' : 'points'}
                      </span>
                    </div>
                    <h3 className="text-lg font-semibold text-accent">{question.text}</h3>
                  </div>
                  <div className="flex gap-2">
                    <Button
                      onClick={() => navigate(`/admin/quizzes/${quizId}/questions/edit/${question.id}`)}
                      className="bg-blue-600 hover:bg-blue-700 px-4 py-2"
                    >
                      Edit
                    </Button>
                    <Button
                      onClick={() => handleDelete(question.id)}
                      className="bg-red-600 hover:bg-red-700 px-4 py-2"
                    >
                      Delete
                    </Button>
                  </div>
                </div>

                {question.options && question.options.length > 0 ? (
                  <div className="mt-4 space-y-2">
                    <p className="text-sm font-semibold text-gray-600 mb-2">Options:</p>
                    {question.options.map((option, optIndex) => (
                      <div
                        key={optIndex}
                        className={`p-3 rounded-lg border-2 ${
                          option.isCorrect
                            ? 'bg-green-50 border-green-500'
                            : 'bg-gray-50 border-gray-200'
                        }`}
                      >
                        <div className="flex items-center justify-between">
                          <span className="text-accent">{option.text}</span>
                          {option.isCorrect && (
                            <span className="text-green-600 font-bold">✓ Correct</span>
                          )}
                        </div>
                      </div>
                    ))}
                  </div>
                ) : question.correctAnswer ? (
                  <div className="mt-4">
                    <p className="text-sm font-semibold text-gray-600 mb-2">Correct Answer:</p>
                    <div className="p-3 rounded-lg bg-green-50 border-2 border-green-500">
                      <span className="text-accent font-semibold">{question.correctAnswer}</span>
                    </div>
                  </div>
                ) : null}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminQuestions;