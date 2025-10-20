import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import quizAttemptService from '../../services/quizAttemptService';
import { QuizAttemptDetails } from '../../models/QuizAttempt';

const Results: React.FC = () => {
  const { attemptId } = useParams<{ attemptId: string }>();
  const navigate = useNavigate();
  const [details, setDetails] = useState<QuizAttemptDetails | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [showDetails, setShowDetails] = useState(false);

  useEffect(() => {
    const fetchDetails = async () => {
      if (!attemptId) {
        setError('Attempt ID is missing');
        setLoading(false);
        return;
      }

      try {
        const data = await quizAttemptService.getQuizAttemptDetails(attemptId);
        console.log('Quiz attempt details:', data);
        setDetails(data);
      } catch (err: any) {
        console.error('Error fetching details:', err);
        setError(err.response?.data?.error || 'Failed to load results');
      } finally {
        setLoading(false);
      }
    };

    fetchDetails();
  }, [attemptId]);

  if (loading) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="text-accent text-xl">Loading results...</div>
      </div>
    );
  }

  if (error || !details) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="bg-white p-6 rounded-lg shadow-lg max-w-md w-full">
          <h2 className="text-2xl font-bold text-red-600 mb-4 text-center">Error</h2>
          <p className="text-accent text-center mb-4">{error || 'No results available'}</p>
          <Button onClick={() => navigate('/')} className="w-full">
            Back to Quizzes
          </Button>
        </div>
      </div>
    );
  }

  const totalQuestions = details.questions.length;
  const correctAnswers = details.questions.filter(q => q.isCorrect).length;
  const duration = Math.floor((new Date(details.endTime).getTime() - new Date(details.startTime).getTime()) / 1000);
  const minutes = Math.floor(duration / 60);
  const seconds = duration % 60;

  const getGrade = (percentage: number) => {
    if (percentage >= 90) return { grade: 'A', color: 'text-green-600', message: 'Excellent!' };
    if (percentage >= 80) return { grade: 'B', color: 'text-blue-600', message: 'Great job!' };
    if (percentage >= 70) return { grade: 'C', color: 'text-yellow-600', message: 'Good effort!' };
    if (percentage >= 60) return { grade: 'D', color: 'text-orange-600', message: 'Keep practicing!' };
    return { grade: 'F', color: 'text-red-600', message: 'Need improvement' };
  };

  const gradeInfo = getGrade(details.percentage);

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-4xl mx-auto">
        <div className="bg-white p-8 rounded-lg shadow-lg mb-6">
          <div className="text-center">
            <h2 className="text-3xl font-bold text-accent mb-2">{details.quizName}</h2>
            <p className="text-gray-600">Quiz Results</p>
          </div>

          <div className="flex justify-center my-8">
            <div className="relative">
              <svg className="transform -rotate-90 w-48 h-48">
                <circle cx="96" cy="96" r="80" stroke="#E5E7EB" strokeWidth="12" fill="none" />
                <circle
                  cx="96" cy="96" r="80"
                  stroke={details.percentage >= 70 ? '#10B981' : details.percentage >= 50 ? '#F59E0B' : '#EF4444'}
                  strokeWidth="12" fill="none"
                  strokeDasharray={`${2 * Math.PI * 80}`}
                  strokeDashoffset={`${2 * Math.PI * 80 * (1 - details.percentage / 100)}`}
                  strokeLinecap="round"
                  className="transition-all duration-1000 ease-out"
                />
              </svg>
              <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 text-center">
                <div className={`text-5xl font-bold ${gradeInfo.color}`}>
                  {details.percentage.toFixed(1)}%
                </div>
              </div>
            </div>
          </div>

          <p className={`text-center text-xl font-semibold ${gradeInfo.color} mb-6`}>
            {gradeInfo.message}
          </p>

          <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
            <div className="bg-gray-50 p-4 rounded-lg text-center">
              <div className="text-3xl font-bold text-accent">{totalQuestions}</div>
              <div className="text-sm text-gray-600 mt-1">Total Questions</div>
            </div>
            <div className="bg-green-50 p-4 rounded-lg text-center">
              <div className="text-3xl font-bold text-green-600">{correctAnswers}</div>
              <div className="text-sm text-gray-600 mt-1">Correct</div>
            </div>
            <div className="bg-red-50 p-4 rounded-lg text-center">
              <div className="text-3xl font-bold text-red-600">{totalQuestions - correctAnswers}</div>
              <div className="text-sm text-gray-600 mt-1">Incorrect</div>
            </div>
            <div className="bg-blue-50 p-4 rounded-lg text-center">
              <div className="text-3xl font-bold text-blue-600">{details.score}</div>
              <div className="text-sm text-gray-600 mt-1">Points</div>
            </div>
          </div>

          <div className="text-center text-gray-600 mb-6">
            <p>Completed in {minutes}m {seconds}s</p>
            <p className="text-sm">
              {new Date(details.startTime).toLocaleString()} - {new Date(details.endTime).toLocaleString()}
            </p>
          </div>

          <Button onClick={() => setShowDetails(!showDetails)} className="w-full mb-4">
            {showDetails ? '▲ Hide Detailed Review' : '▼ Show Detailed Review'}
          </Button>
        </div>

        {showDetails && (
          <div className="space-y-4 mb-6">
            <h3 className="text-2xl font-bold text-accent mb-4">Question Review</h3>
            {details.questions.map((question, index) => (
              <div 
                key={index} 
                className={`bg-white p-6 rounded-lg shadow-lg border-l-4 ${
                  question.isCorrect ? 'border-green-500' : 'border-red-500'
                }`}
              >
                <div className="flex justify-between items-start mb-3">
                  <h4 className="text-lg font-semibold text-accent flex-1">
                    Question {index + 1}: {question.text}
                  </h4>
                  <span className={`px-3 py-1 rounded-full text-sm font-semibold ${
                    question.isCorrect ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                  }`}>
                    {question.isCorrect ? '✓ Correct' : '✗ Incorrect'}
                  </span>
                </div>

                <div className="text-sm text-gray-600 mb-3">
                  Type: {question.type.replace(/([A-Z])/g, ' $1').trim()}
                </div>

                {question.options && question.options.length > 0 ? (
                  <div className="space-y-2">
                    {question.options.map((option, optIndex) => (
                      <div
                        key={optIndex}
                        className={`p-3 rounded-lg border-2 ${
                          option.isCorrect && option.selected
                            ? 'bg-green-50 border-green-500'
                            : option.isCorrect
                            ? 'bg-green-50 border-green-300'
                            : option.selected
                            ? 'bg-red-50 border-red-500'
                            : 'bg-gray-50 border-gray-200'
                        }`}
                      >
                        <div className="flex items-center justify-between">
                          <span className="text-accent">{option.text}</span>
                          <div className="flex items-center space-x-2">
                            {option.selected && (
                              <span className="text-sm font-semibold text-blue-600">Your answer</span>
                            )}
                            {option.isCorrect && <span className="text-green-600 font-bold">✓</span>}
                            {!option.isCorrect && option.selected && <span className="text-red-600 font-bold">✗</span>}
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="space-y-2">
                    <div className={`p-3 rounded-lg ${question.isCorrect ? 'bg-green-50' : 'bg-red-50'}`}>
                      <p className="text-sm text-gray-600">Your answer:</p>
                      <p className="text-accent font-semibold">
                        {question.userAnswer || '(No answer provided)'}
                      </p>
                    </div>
                    {!question.isCorrect && question.correctAnswer && (
                      <div className="p-3 rounded-lg bg-green-50">
                        <p className="text-sm text-gray-600">Correct answer:</p>
                        <p className="text-green-700 font-semibold">{question.correctAnswer}</p>
                      </div>
                    )}
                  </div>
                )}
              </div>
            ))}
          </div>
        )}

        <div className="flex gap-4 justify-center">
          <Button onClick={() => navigate('/')} className="px-8">
            Back to Quizzes
          </Button>
          <Button onClick={() => navigate('/my-results')} className="px-8 bg-blue-600 hover:bg-blue-700">
            View All Results
          </Button>
        </div>
      </div>
    </div>
  );
};

export default Results;