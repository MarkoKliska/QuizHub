import { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../contexts/AuthContext';
import { Quiz } from '../../models/Quiz';
import { Category } from '../../models/Category';
import Input from '../../components/common/Input';
import Button from '../../components/common/Button';
import quizService from 'services/quizService';
import categoryService from 'services/categoryService';

const Home: React.FC = () => {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [categoryId, setCategoryId] = useState<string>('');
  const [difficulty, setDifficulty] = useState<string>('');
  const [search, setSearch] = useState<string>('');
  const [error, setError] = useState<string>('');
  const navigate = useNavigate();
  const authContext = useContext(AuthContext);
  
  if (!authContext) {
    throw new Error('Home must be used within AuthProvider');
  }
  
  const { user } = authContext;

  const difficulties = ['All', 'Easy', 'Medium', 'Hard'];

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await categoryService.getCategories();
        setCategories([{ id: '', name: 'All Categories' }, ...data]);
      } catch (err) {
        setError('Failed to load categories');
      }
    };
    fetchCategories();
  }, []);

  const fetchQuizzes = async () => {
    try {
      const filters: { categoryId?: string; difficulty?: string; search?: string } = {};
      if (categoryId) filters.categoryId = categoryId;
      if (difficulty && difficulty !== 'All') filters.difficulty = difficulty;
      if (search) filters.search = search;

      console.log('Fetching quizzes with filters:', filters);
      const data = await quizService.getQuizzes(filters);
      console.log('Received quizzes:', data);
      setQuizzes(data);
      setError('');
    } catch (err: any) {
      console.error('Quiz fetch error:', err);
      setError('Failed to load quizzes');
    }
  };

  useEffect(() => {
    fetchQuizzes();
  }, [categoryId, difficulty, search]);

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold text-accent">Available Quizzes</h2>
        <div className="flex gap-3">
          {user?.role === 'Admin' && (
            <Button 
              onClick={() => navigate('/admin')}
              className="bg-red-600 hover:bg-red-700"
            >
              Admin Panel
            </Button>
          )}
          <Button 
            onClick={() => navigate('/leaderboard')}
            className="bg-yellow-600 hover:bg-yellow-700"
          >
            Leaderboard
          </Button>
          {user?.role !== 'Admin' && (
            <Button 
              onClick={() => navigate('/my-results')}
              className="bg-purple-600 hover:bg-purple-700"
            >
              My Results
            </Button>
          )}
        </div>
      </div>

      {error && <p className="text-red-500 mb-4 text-center">{error}</p>}

      <div className="mb-6 flex flex-col sm:flex-row gap-4 justify-center">
        <select
          value={categoryId}
          onChange={(e) => setCategoryId(e.target.value)}
          className="p-2 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
        >
          {categories.map((cat) => (
            <option key={cat.id} value={cat.id}>
              {cat.name}
            </option>
          ))}
        </select>
        <select
          value={difficulty}
          onChange={(e) => setDifficulty(e.target.value)}
          className="p-2 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary"
        >
          {difficulties.map((diff) => (
            <option key={diff} value={diff}>
              {diff}
            </option>
          ))}
        </select>
        <Input
          type="text"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          placeholder="Search quizzes..."
          className="w-full sm:w-64"
        />
      </div>

      {quizzes.length === 0 ? (
        <p className="text-accent text-center">No quizzes found.</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {quizzes.map((quiz) => (
            <div key={quiz.id} className="bg-white p-4 rounded-lg shadow-md">
              <h3 className="text-xl font-semibold text-accent">{quiz.name}</h3>
              <p className="text-accent mt-2">{quiz.description}</p>
              <p className="text-accent mt-2">Category: {quiz.categoryName}</p>
              <p className="text-accent">Difficulty: {quiz.difficulty}</p>
              <p className="text-accent">Time Limit: {quiz.timeLimit} minutes</p>
              <p className="text-accent">Questions: {quiz.numberOfQuestions}</p>
              {user?.role !== 'Admin' && (
                <Button className="mt-4 w-full" onClick={() => navigate(`/quiz/${quiz.id}`)}>
                  Start Quiz
                </Button>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Home;