import { JSX, useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from '../contexts/AuthContext';

const GuestRoute: React.FC<{ children: JSX.Element }> = ({ children }) => {
  const authContext = useContext(AuthContext);
  if (authContext?.isAuthenticated) {
    return <Navigate to="/" replace />;
  }
  return children;
};

export default GuestRoute;