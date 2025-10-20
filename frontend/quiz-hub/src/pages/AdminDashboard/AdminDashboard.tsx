import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';

const AdminDashboard: React.FC = () => {
  const navigate = useNavigate();

  const menuItems = [
    {
      title: 'Manage Categories',
      description: 'Create, update, and delete quiz categories',
      icon: '📁',
      path: '/admin/categories',
      color: 'bg-blue-500 hover:bg-blue-600'
    },
    {
      title: 'Manage Quizzes',
      description: 'Create, update, and delete quizzes',
      icon: '📝',
      path: '/admin/quizzes',
      color: 'bg-green-500 hover:bg-green-600'
    },
    {
      title: 'View All Results',
      description: 'View results of all users',
      icon: '📊',
      path: '/admin/results',
      color: 'bg-purple-500 hover:bg-purple-600'
    },
  ];

  return (
    <div className="bg-primary min-h-screen p-6">
      <div className="max-w-6xl mx-auto">
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-accent mb-2">Admin Dashboard</h1>
          <p className="text-gray-600">Manage your quiz application</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {menuItems.map((item) => (
            <div
              key={item.path}
              className="bg-white rounded-lg shadow-lg p-6 hover:shadow-xl transition-shadow cursor-pointer"
              onClick={() => navigate(item.path)}
            >
              <div className="text-6xl mb-4">{item.icon}</div>
              <h2 className="text-2xl font-bold text-accent mb-2">{item.title}</h2>
              <p className="text-gray-600 mb-4">{item.description}</p>
              <Button className={`w-full ${item.color}`}>
                Open
              </Button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;