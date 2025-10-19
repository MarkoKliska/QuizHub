import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Question } from '../../models/Question';
import { Answer, QuizAttempt } from '../../models/QuizAttempt';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import quizAttemptService from '../../services/quizAttemptService';
import questionService from '../../services/questionService';
import quizService from '../../services/quizService';

const Quiz: React.FC = () => {
  const { quizId } = useParams<{ quizId: string }>();
  const navigate = useNavigate();
  const [attempt, setAttempt] = useState<QuizAttempt | null>(null);
  const [questions, setQuestions] = useState<Question[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [answers, setAnswers] = useState<Answer[]>([]);
  const [timeLeft, setTimeLeft] = useState<number | null>(null);
  const [error, setError] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const timerRef = useRef<NodeJS.Timeout | null>(null);
  const hasSubmittedRef = useRef(false);

  useEffect(() => {
    const startQuiz = async () => {
      if (!quizId) {
        setError('Quiz ID is missing');
        return;
      }
      try {
        console.log(`Starting quiz with ID: ${quizId}`);
        const attemptData = await quizAttemptService.startQuizAttempt(quizId);
        console.log('Attempt data:', attemptData);
        setAttempt(attemptData);

        const questionsData = await questionService.getQuestionsByQuizId(quizId);
        console.log('Questions data:', questionsData);
        
        if (!questionsData || questionsData.length === 0) {
          setError('This quiz has no questions yet. Please try another quiz.');
          return;
        }
        
        setQuestions(questionsData);
        setAnswers(questionsData.map((q: Question) => ({ 
          questionId: q.id, 
          selectedOptionIds: [], 
          textAnswer: '' 
        })));

        const quizzes = await quizService.getQuizzes({});
        const quiz = quizzes.find((q: any) => q.id === quizId);
        if (!quiz) {
          setError('Quiz not found');
          return;
        }
        const timeLimitSeconds = quiz.timeLimit * 60;
        setTimeLeft(timeLimitSeconds);
      } catch (err: any) {
        console.error('Error starting quiz:', err.response?.data || err.message);
        setError(err.response?.data?.error || err.message || 'Failed to start quiz');
      }
    };
    startQuiz();

    return () => {
      if (timerRef.current) {
        clearInterval(timerRef.current);
      }
    };
  }, [quizId]);

  useEffect(() => {
    if (timeLeft === null || timeLeft <= 0 || isSubmitting) return;

    timerRef.current = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev === null || prev <= 0) {
          if (timerRef.current) {
            clearInterval(timerRef.current);
          }
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => {
      if (timerRef.current) {
        clearInterval(timerRef.current);
      }
    };
  }, [timeLeft, isSubmitting]);

  useEffect(() => {
    if (timeLeft === 0 && !hasSubmittedRef.current && !isSubmitting) {
      console.log('Time expired - auto submitting quiz');
      submitQuiz();
    }
  }, [timeLeft]);

  const handleAnswerChange = (questionId: string, selectedOptionIds: string[] | string, textAnswer?: string) => {
    setAnswers((prev) =>
      prev.map((ans) =>
        ans.questionId === questionId
          ? {
              questionId,
              selectedOptionIds: Array.isArray(selectedOptionIds) ? selectedOptionIds : [selectedOptionIds],
              textAnswer: textAnswer || undefined,
            }
          : ans
      )
    );
  };

  const submitQuiz = async () => {
    if (!attempt || isSubmitting || hasSubmittedRef.current) {
      console.log('Submit blocked:', { attempt: !!attempt, isSubmitting, hasSubmitted: hasSubmittedRef.current });
      return;
    }

    hasSubmittedRef.current = true;
    setIsSubmitting(true);

    if (timerRef.current) {
      clearInterval(timerRef.current);
    }

    try {
      const cleanedAnswers = answers.map((ans) => {
        const question = questions.find(q => q.id === ans.questionId);
        if (!question) return ans;

        if (question.type === 'FillInBlank') {
          return {
            questionId: ans.questionId,
            textAnswer: ans.textAnswer || undefined,
            selectedOptionIds: undefined
          };
        } 
        else if (question.type === 'TrueFalse') {
          if (question.options && question.options.length > 0) {
            return {
              questionId: ans.questionId,
              selectedOptionIds: ans.selectedOptionIds?.length ? ans.selectedOptionIds : undefined,
              textAnswer: undefined
            };
          } else {
            return {
              questionId: ans.questionId,
              textAnswer: ans.textAnswer || undefined,
              selectedOptionIds: undefined
            };
          }
        } 
        else {
          return {
            questionId: ans.questionId,
            selectedOptionIds: ans.selectedOptionIds?.length ? ans.selectedOptionIds : undefined,
            textAnswer: undefined
          };
        }
      });

      console.log('Submitting quiz with answers:', cleanedAnswers);
      const response = await quizAttemptService.submitQuizAttempt(attempt.id, cleanedAnswers);
      console.log('Submission response:', response);
      
      navigate(`/results/${attempt.id}`);
    } catch (err: any) {
      console.error('Error submitting quiz:', err.response?.data || err.message);
      setError(err.response?.data?.error || 'Failed to submit quiz');
      setIsSubmitting(false);
      hasSubmittedRef.current = false;
    }
  };

  const formatTime = (seconds: number) => {
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}:${secs < 10 ? '0' : ''}${secs}`;
  };

  const getTimerColor = () => {
    if (timeLeft === null) return 'text-accent';
    if (timeLeft <= 60) return 'text-red-600 font-bold animate-pulse';
    if (timeLeft <= 300) return 'text-orange-500 font-semibold';
    return 'text-accent';
  };

  if (!attempt || !questions.length) {
    return (
      <div className="bg-primary min-h-screen flex items-center justify-center">
        <div className="text-center text-accent text-xl">
          {error || 'Loading quiz...'}
        </div>
      </div>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];
  const progress = ((currentQuestionIndex + 1) / questions.length) * 100;

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold text-accent mb-4 text-center">{attempt.quizName}</h2>
        
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <div className={`mb-6 text-center text-2xl font-semibold ${getTimerColor()}`}>
          Time Left: {timeLeft !== null ? formatTime(timeLeft) : 'Loading...'}
          {timeLeft !== null && timeLeft <= 60 && (
            <div className="text-sm mt-1">Hurry up! Time is running out!</div>
          )}
        </div>

        <div className="mb-6">
          <div className="flex justify-between text-sm text-accent mb-2">
            <span>Question {currentQuestionIndex + 1} of {questions.length}</span>
            <span>{progress.toFixed(0)}% Complete</span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-2">
            <div 
              className="bg-secondary h-2 rounded-full transition-all duration-300"
              style={{ width: `${progress}%` }}
            />
          </div>
        </div>

        <div className="bg-white p-8 rounded-lg shadow-lg">
          <div className="mb-6">
            <div className="flex justify-between items-center mb-4">
              <span className="text-sm font-semibold text-secondary uppercase">
                {currentQuestion.type.replace(/([A-Z])/g, ' $1').trim()}
              </span>
              <span className="text-sm font-semibold text-accent">
                {currentQuestion.points} {currentQuestion.points === 1 ? 'point' : 'points'}
              </span>
            </div>
            <h3 className="text-xl font-semibold text-accent">{currentQuestion.text}</h3>
          </div>

          {currentQuestion.type === 'SingleChoice' ? (
            <div className="space-y-3">
              {currentQuestion.options.map((option) => (
                <label 
                  key={option.id} 
                  className="flex items-center space-x-3 p-4 border-2 rounded-lg cursor-pointer hover:bg-gray-50 transition-colors"
                  style={{
                    borderColor: answers[currentQuestionIndex].selectedOptionIds?.includes(option.id) 
                      ? '#4CAF50' 
                      : '#E0E0E0'
                  }}
                >
                  <input
                    type="radio"
                    name={`question-${currentQuestion.id}`}
                    value={option.id}
                    checked={answers[currentQuestionIndex].selectedOptionIds?.includes(option.id) || false}
                    onChange={() => handleAnswerChange(currentQuestion.id, option.id)}
                    className="w-5 h-5 text-secondary focus:ring-secondary"
                  />
                  <span className="text-accent flex-1">{option.text}</span>
                </label>
              ))}
            </div>
          ) : currentQuestion.type === 'TrueFalse' ? (
            <div className="space-y-3">
              {currentQuestion.options && currentQuestion.options.length > 0 ? (
                currentQuestion.options.map((option) => (
                  <label 
                    key={option.id} 
                    className="flex items-center space-x-3 p-4 border-2 rounded-lg cursor-pointer hover:bg-gray-50 transition-colors"
                    style={{
                      borderColor: answers[currentQuestionIndex].selectedOptionIds?.includes(option.id) 
                        ? '#4CAF50' 
                        : '#E0E0E0'
                    }}
                  >
                    <input
                      type="radio"
                      name={`question-${currentQuestion.id}`}
                      value={option.id}
                      checked={answers[currentQuestionIndex].selectedOptionIds?.includes(option.id) || false}
                      onChange={() => handleAnswerChange(currentQuestion.id, option.id)}
                      className="w-5 h-5 text-secondary focus:ring-secondary"
                    />
                    <span className="text-accent flex-1">{option.text}</span>
                  </label>
                ))
              ) : (
                <div>
                  <p className="text-sm text-gray-600 mb-3">Enter "True" or "False":</p>
                  <Input
                    type="text"
                    value={answers[currentQuestionIndex].textAnswer || ''}
                    onChange={(e) => handleAnswerChange(currentQuestion.id, [], e.target.value)}
                    placeholder="Type True or False..."
                    className="w-full text-lg p-4"
                  />
                </div>
              )}
            </div>
          ) : currentQuestion.type === 'MultipleChoice' ? (
            <div className="space-y-3">
              <p className="text-sm text-gray-600 mb-3">Select all that apply:</p>
              {currentQuestion.options.map((option) => (
                <label 
                  key={option.id} 
                  className="flex items-center space-x-3 p-4 border-2 rounded-lg cursor-pointer hover:bg-gray-50 transition-colors"
                  style={{
                    borderColor: answers[currentQuestionIndex].selectedOptionIds?.includes(option.id) 
                      ? '#4CAF50' 
                      : '#E0E0E0'
                  }}
                >
                  <input
                    type="checkbox"
                    value={option.id}
                    checked={answers[currentQuestionIndex].selectedOptionIds?.includes(option.id) || false}
                    onChange={(e) => {
                      const selected = answers[currentQuestionIndex].selectedOptionIds || [];
                      const newSelected = e.target.checked
                        ? [...selected, option.id]
                        : selected.filter((id) => id !== option.id);
                      handleAnswerChange(currentQuestion.id, newSelected);
                    }}
                    className="w-5 h-5 text-secondary focus:ring-secondary"
                  />
                  <span className="text-accent flex-1">{option.text}</span>
                </label>
              ))}
            </div>
          ) : (
            <div>
              <p className="text-sm text-gray-600 mb-3">Enter your answer:</p>
              <Input
                type="text"
                value={answers[currentQuestionIndex].textAnswer || ''}
                onChange={(e) => handleAnswerChange(currentQuestion.id, [], e.target.value)}
                placeholder="Type your answer here..."
                className="w-full text-lg p-4"
              />
            </div>
          )}

          <div className="flex justify-between mt-8 pt-6 border-t">
            <Button
              onClick={() => setCurrentQuestionIndex((prev) => Math.max(0, prev - 1))}
              className={currentQuestionIndex === 0 ? 'opacity-50 cursor-not-allowed' : ''}
              disabled={currentQuestionIndex === 0}
            >
              ← Previous
            </Button>
            <div className="flex space-x-3">
              {currentQuestionIndex < questions.length - 1 ? (
                <Button onClick={() => setCurrentQuestionIndex((prev) => prev + 1)}>
                  Next →
                </Button>
              ) : null}
              <Button 
                onClick={submitQuiz} 
                disabled={isSubmitting}
                className="bg-green-600 hover:bg-green-700"
              >
                {isSubmitting ? 'Submitting...' : '✓ Finish Quiz'}
              </Button>
            </div>
          </div>
        </div>

        <div className="mt-6 flex justify-center flex-wrap gap-2">
          {questions.map((q, index) => {
            const answer = answers[index];
            const hasAnswer = (answer.selectedOptionIds && answer.selectedOptionIds.length > 0) || 
                             (answer.textAnswer && answer.textAnswer.trim().length > 0);
            
            return (
              <button
                key={index}
                onClick={() => setCurrentQuestionIndex(index)}
                className={`w-10 h-10 rounded-full flex items-center justify-center font-semibold transition-all ${
                  index === currentQuestionIndex
                    ? 'bg-secondary text-white ring-4 ring-secondary ring-opacity-50'
                    : hasAnswer
                    ? 'bg-green-500 text-white'
                    : 'bg-gray-300 text-gray-600'
                }`}
                title={`Question ${index + 1}${hasAnswer ? ' (Answered)' : ''}`}
              >
                {index + 1}
              </button>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default Quiz;