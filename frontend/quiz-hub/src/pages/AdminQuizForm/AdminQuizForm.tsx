import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import adminService from '../../services/adminService';
import { Category, CreateQuizDto } from 'models/Admin';

const AdminQuizForm: React.FC = () => {
  const { quizId } = useParams<{ quizId: string }>();
  const isEditMode = !!quizId;
  const navigate = useNavigate();

  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  const [formData, setFormData] = useState<CreateQuizDto>({
    name: '',
    description: '',
    timeLimit: 30,
    difficulty: 'Medium',
    categoryId: '',
  });

  const difficulties = ['Easy', 'Medium', 'Hard'];

  useEffect(() => {
    fetchCategories();
    if (isEditMode && quizId) {
      fetchQuiz(quizId);
    }
  }, [quizId]);

  const fetchCategories = async () => {
    try {
      const data = await adminService.getCategories();
      setCategories(data);
      if (data.length > 0 && !formData.categoryId) {
        setFormData(prev => ({ ...prev, categoryId: data[0].id }));
      }
    } catch (err: any) {
      setError('Failed to load categories');
    }
  };

  const fetchQuiz = async (id: string) => {
    try {
      const quiz = await adminService.getQuiz(id);
      setFormData({
        name: quiz.name,
        description: quiz.description,
        timeLimit: quiz.timeLimit,
        difficulty: quiz.difficulty,
        categoryId: quiz.categoryId,
      });
    } catch (err: any) {
      setError('Failed to load quiz');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      if (isEditMode && quizId) {
        await adminService.updateQuiz(quizId, formData);
      } else {
        await adminService.createQuiz(formData);
      }
      navigate('/admin/quizzes');
    } catch (err: any) {
      setError(err.response?.data?.error || 'Operation failed');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'timeLimit' ? parseInt(value) || 0 : value,
    }));
  };

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-3xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-3xl font-bold text-accent">
            {isEditMode ? 'Edit Quiz' : 'Create New Quiz'}
          </h2>
          <Button onClick={() => navigate('/admin/quizzes')}>
            Back to Quizzes
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
                  Quiz Name *
                </label>
                <Input
                  type="text"
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  placeholder="Enter quiz name"
                  required
                  className="w-full"
                />
              </div>

              <div>
                <label className="block text-sm font-semibold text-accent mb-2">
                  Description *
                </label>
                <textarea
                  name="description"
                  value={formData.description}
                  onChange={handleChange}
                  placeholder="Enter quiz description"
                  required
                  rows={4}
                  className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
                />
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-accent mb-2">
                    Category *
                  </label>
                  <select
                    name="categoryId"
                    value={formData.categoryId}
                    onChange={handleChange}
                    required
                    className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
                  >
                    <option value="">Select Category</option>
                    {categories.map((cat) => (
                      <option key={cat.id} value={cat.id}>
                        {cat.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-semibold text-accent mb-2">
                    Difficulty *
                  </label>
                  <select
                    name="difficulty"
                    value={formData.difficulty}
                    onChange={handleChange}
                    required
                    className="w-full p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
                  >
                    {difficulties.map((diff) => (
                      <option key={diff} value={diff}>
                        {diff}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-semibold text-accent mb-2">
                    Time Limit (minutes) *
                  </label>
                  <Input
                    type="number"
                    name="timeLimit"
                    value={formData.timeLimit}
                    onChange={handleChange}
                    min="1"
                    required
                    className="w-full"
                  />
                </div>
              </div>

              <div className="flex gap-3 justify-end pt-4">
                <Button
                  type="button"
                  onClick={() => navigate('/admin/quizzes')}
                  className="bg-gray-500 hover:bg-gray-600"
                >
                  Cancel
                </Button>
                <Button
                  type="submit"
                  disabled={loading}
                  className="bg-green-600 hover:bg-green-700"
                >
                  {loading ? 'Saving...' : isEditMode ? 'Update Quiz' : 'Create Quiz'}
                </Button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AdminQuizForm;