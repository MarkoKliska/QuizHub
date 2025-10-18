import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../../contexts/AuthContext';

const Navbar: React.FC = () => {
  const authContext = useContext(AuthContext);

  return (
    <nav className="bg-accent text-white p-4 flex justify-between items-center">
      <Link to="/" className="text-xl font-semibold">
        QuizHub
      </Link>
      <div className="space-x-4">
        {authContext?.isAuthenticated ? (
          <>
            <button
              onClick={authContext.logout}
              className="bg-accent text-white px-4 py-2 rounded hover:bg-tertiary"
            >
              Logout
            </button>
          </>
        ) : (
          <>
            <Link to="/login" className="hover:text-secondary">
              Login
            </Link>
            <Link to="/register" className="hover:text-secondary">
              Register
            </Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;