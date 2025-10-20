import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import adminService from '../../services/adminService';
import { CreateQuestionDto, QuestionOption } from 'models/Admin';

const AdminQuestionForm: React.FC = () => {
  const { quizId, questionId } = useParams<{ quizId: string; questionId?: string }>();
  const isEditMode = !!questionId;
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const [quizName, setQuizName] = useState<string>('');

  const [formData, setFormData] = useState<CreateQuestionDto>({
    quizId: quizId || '',
    text: '',
    type: 'SingleChoice',
    points: 1,
    correctAnswer: '',
    options: [],
  });

  const questionTypes = [
    { value: 'SingleChoice', label: 'Single Choice' },
    { value: 'MultipleChoice', label: 'Multiple Choice' },
    { value: 'TrueFalse', label: 'True/False' },
    { value: 'FillInBlank', label: 'Fill in the Blank' },
  ];

  useEffect(() => {
    if (quizId) {
      fetchQuizInfo();
      if (isEditMode && questionId) {
        fetchQuestion(questionId);
      } else {
        // Initialize options for new question
        initializeOptions('SingleChoice');
      }
    }
  }, [quizId, questionId]);

  const fetchQuizInfo = async () => {
    if (!quizId) return;
    try {
      const quiz = await adminService.getQuiz(quizId);
      setQuizName(quiz.name);
    } catch (err: any) {
      console.error('Failed to load quiz info:', err);
    }
  };

  const fetchQuestion = async (id: string) => {
    try {
      const question = await adminService.getQuestion(id);
      setFormData({
        quizId: question.quizId,
        text: question.text,
        type: question.type,
        points: question.points,
        correctAnswer: question.correctAnswer || '',
        options: question.options || [],
      });
    } catch (err: any) {
      setError('Failed to load question');
    }
  };

  const initializeOptions = (type: string) => {
    if (type === 'SingleChoice') {
      setFormData(prev => ({
        ...prev,
        options: [
          { text: '', isCorrect: false },
          { text: '', isCorrect: false },
        ],
        correctAnswer: '',
      }));
    } else if (type === 'MultipleChoice') {
      setFormData(prev => ({
        ...prev,
        options: [
          { text: '', isCorrect: false },
          { text: '', isCorrect: false },
          { text: '', isCorrect: false },
          { text: '', isCorrect: false },
        ],
        correctAnswer: '',
      }));
    } else if (type === 'TrueFalse') {
      setFormData(prev => ({
        ...prev,
        options: [
          { text: 'True', isCorrect: false },
          { text: 'False', isCorrect: false },
        ],
        correctAnswer: '',
      }));
    } else {
      setFormData(prev => ({
        ...prev,
        options: [],
        correctAnswer: '',
      }));
    }
  };

  const handleTypeChange = (newType: string) => {
    setFormData(prev => ({ ...prev, type: newType }));
    // Uvek inicijalizuj opcije kada se menja tip, čak i u edit mode
    initializeOptions(newType);
  };

  const handleAddOption = () => {
    setFormData(prev => ({
      ...prev,
      options: [...prev.options, { text: '', isCorrect: false }],
    }));
  };

  const handleRemoveOption = (index: number) => {
    const minOptions = formData.type === 'MultipleChoice' ? 4 : 2;
    if (formData.options.length <= minOptions) {
      setError(`Minimum ${minOptions} options required for ${formData.type === 'MultipleChoice' ? 'Multiple Choice' : 'this question type'}`);
      return;
    }
    
    setFormData(prev => ({
      ...prev,
      options: prev.options.filter((_, i) => i !== index),
    }));
    setError('');
  };

  const handleOptionTextChange = (index: number, text: string) => {
    setFormData(prev => ({
      ...prev,
      options: prev.options.map((opt, i) =>
        i === index ? { ...opt, text } : opt
      ),
    }));
  };

  const handleOptionCorrectChange = (index: number) => {
    if (formData.type === 'SingleChoice' || formData.type === 'TrueFalse') {
      // For single choice and true/false, only one option can be correct
      setFormData(prev => ({
        ...prev,
        options: prev.options.map((opt, i) =>
          i === index ? { ...opt, isCorrect: true } : { ...opt, isCorrect: false }
        ),
      }));
    } else {
      // For multiple choice, toggle the checkbox
      setFormData(prev => ({
        ...prev,
        options: prev.options.map((opt, i) =>
          i === index ? { ...opt, isCorrect: !opt.isCorrect } : opt
        ),
      }));
    }
  };

  const validateForm = (): boolean => {
    if (!formData.text.trim()) {
      setError('Question text is required');
      return false;
    }

    if (formData.points < 1) {
      setError('Points must be at least 1');
      return false;
    }

    if (formData.type === 'FillInBlank') {
      if (!formData.correctAnswer?.trim()) {
        setError('Correct answer is required for Fill in the Blank questions');
        return false;
      }
    } else if (formData.type === 'TrueFalse') {
      // Za TrueFalse proveravamo da li je neka opcija selektovana
      if (!formData.options.some(opt => opt.isCorrect)) {
        setError('Please select True or False as the correct answer');
        return false;
      }
    } else if (formData.type === 'SingleChoice' || formData.type === 'MultipleChoice') {
      const minOptions = formData.type === 'MultipleChoice' ? 4 : 2;
      if (formData.options.length < minOptions) {
        setError(`At least ${minOptions} options are required for ${formData.type === 'MultipleChoice' ? 'Multiple Choice' : 'this question type'}`);
        return false;
      }

      if (formData.options.some(opt => !opt.text.trim())) {
        setError('All options must have text');
        return false;
      }

      if (!formData.options.some(opt => opt.isCorrect)) {
        setError('At least one option must be marked as correct');
        return false;
      }
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    setLoading(true);
    setError('');

    try {
      const submitData: CreateQuestionDto = {
        ...formData,
        quizId: quizId || '',
      };

      // Za FillInBlank koristimo correctAnswer i prazan options niz
      if (formData.type === 'FillInBlank') {
        submitData.options = [];
        submitData.correctAnswer = formData.correctAnswer;
      } 
      // Za TrueFalse koristimo correctAnswer (True ili False kao string)
      else if (formData.type === 'TrueFalse') {
        submitData.options = [];
        // Uzmi tekst opcije koja je označena kao tačna (True ili False)
        const correctOption = formData.options.find(opt => opt.isCorrect);
        submitData.correctAnswer = correctOption?.text || '';
      }
      // Za SingleChoice i MultipleChoice koristimo options
      else {
        submitData.options = formData.options;
        submitData.correctAnswer = undefined;
      }

      console.log('Submitting question data:', submitData);

      if (isEditMode && questionId) {
        await adminService.updateQuestion(questionId, submitData);
      } else {
        await adminService.createQuestion(submitData);
      }
      
      navigate(`/admin/quizzes/${quizId}/questions`);
    } catch (err: any) {
      console.error('Error submitting question:', err);
      setError(err.response?.data?.error || err.message || 'Operation failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-4xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h2 className="text-3xl font-bold text-accent">
              {isEditMode ? 'Edit Question' : 'Create New Question'}
            </h2>
            <p className="text-gray-600 mt-1">Quiz: {quizName}</p>
          </div>
          <Button onClick={() => navigate(`/admin/quizzes/${quizId}/questions`)}>
            Back to Questions
          </Button>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <div className="bg-white p-8 rounded-lg shadow-lg">
          <form onSubmit={handleSubmit}>
            <div className="space-y-6">
              <div>
                <label className="block text-sm font-semibold text-accent mb-2">
                  Question Text *
                </label>
                <textarea
                  value={formData.text}
                  onChange={(e) => setFormData(prev => ({ ...prev, text: e.target.value }))}
                  placeholder="Enter your question here..."
                  required
                  rows={3}
                  className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
                />
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-accent mb-2">
                    Question Type *
                  </label>
                  <select
                    value={formData.type}
                    onChange={(e) => handleTypeChange(e.target.value)}
                    className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
                  >
                    {questionTypes.map((type) => (
                      <option key={type.value} value={type.value}>
                        {type.label}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-semibold text-accent mb-2">
                    Points *
                  </label>
                  <Input
                    type="number"
                    value={formData.points}
                    onChange={(e) => setFormData(prev => ({ ...prev, points: parseInt(e.target.value) || 1 }))}
                    min="1"
                    required
                    className="w-full"
                  />
                </div>
              </div>

              {formData.type === 'FillInBlank' ? (
                <div>
                  <label className="block text-sm font-semibold text-accent mb-2">
                    Correct Answer *
                  </label>
                  <Input
                    type="text"
                    value={formData.correctAnswer}
                    onChange={(e) => setFormData(prev => ({ ...prev, correctAnswer: e.target.value }))}
                    placeholder="Enter the correct answer..."
                    required
                    className="w-full"
                  />
                  <p className="text-xs text-gray-500 mt-1">
                    This is the exact answer that will be checked (case-insensitive)
                  </p>
                </div>
              ) : (
                <div>
                  <div className="flex justify-between items-center mb-3">
                    <label className="block text-sm font-semibold text-accent">
                      Answer Options *
                    </label>
                    {formData.type !== 'TrueFalse' && (
                      <Button
                        type="button"
                        onClick={handleAddOption}
                        className="bg-green-600 hover:bg-green-700 px-3 py-1 text-sm"
                      >
                        + Add Option
                      </Button>
                    )}
                  </div>

                  <div className="space-y-3">
                    {formData.options.map((option, index) => (
                      <div key={index} className="flex gap-3 items-start">
                        <div className="flex items-center pt-3">
                          <input
                            type={formData.type === 'MultipleChoice' ? 'checkbox' : 'radio'}
                            name="correct-answer"
                            checked={option.isCorrect}
                            onChange={() => handleOptionCorrectChange(index)}
                            className="w-5 h-5 text-secondary focus:ring-secondary cursor-pointer"
                          />
                        </div>
                        <div className="flex-1">
                          <Input
                            type="text"
                            value={option.text}
                            onChange={(e) => handleOptionTextChange(index, e.target.value)}
                            placeholder={`Option ${index + 1}`}
                            required
                            className="w-full"
                            disabled={formData.type === 'TrueFalse'}
                          />
                        </div>
                        {formData.type !== 'TrueFalse' && formData.options.length > (formData.type === 'MultipleChoice' ? 4 : 2) && (
                          <Button
                            type="button"
                            onClick={() => handleRemoveOption(index)}
                            className="bg-red-600 hover:bg-red-700 px-3 py-2 mt-0"
                          >
                            ✕
                          </Button>
                        )}
                      </div>
                    ))}
                  </div>

                  <p className="text-xs text-gray-500 mt-2">
                    {formData.type === 'SingleChoice' && 'Select one correct answer'}
                    {formData.type === 'MultipleChoice' && 'Select all correct answers'}
                    {formData.type === 'TrueFalse' && 'Select True or False as the correct answer'}
                  </p>
                </div>
              )}

              <div className="flex gap-3 justify-end pt-4 border-t">
                <Button
                  type="button"
                  onClick={() => navigate(`/admin/quizzes/${quizId}/questions`)}
                  className="bg-gray-500 hover:bg-gray-600"
                >
                  Cancel
                </Button>
                <Button
                  type="submit"
                  disabled={loading}
                  className="bg-green-600 hover:bg-green-700"
                >
                  {loading ? 'Saving...' : isEditMode ? 'Update Question' : 'Create Question'}
                </Button>
              </div>
            </div>
          </form>
        </div>

        <div className="mt-6 bg-blue-50 border border-blue-200 rounded-lg p-4">
          <h3 className="font-semibold text-blue-900 mb-2">💡 Tips:</h3>
          <ul className="text-sm text-blue-800 space-y-1 list-disc list-inside">
            <li>
              <strong>Single Choice:</strong> Only one correct answer allowed (min. 2 options)
            </li>
            <li>
              <strong>Multiple Choice:</strong> Multiple correct answers allowed (min. 4 options)
            </li>
            <li>
              <strong>True/False:</strong> Simple binary choice question
            </li>
            <li>
              <strong>Fill in the Blank:</strong> Text-based answer (case-insensitive matching)
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default AdminQuestionForm;