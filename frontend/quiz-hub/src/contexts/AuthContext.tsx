import React, { createContext, useState, useEffect, ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';
import { User } from '../models/User';
import { getToken, setToken, removeToken } from '../utils/tokenStorage';

interface DecodedToken {
  id: number;
  username?: string;
  userName?: string;
  email: string;
  role: string;
}

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  login: (token: string, userData: User) => void;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

  useEffect(() => {
    const token = getToken();
    if (token) {
      try {
        const decoded: DecodedToken = jwtDecode(token); 
        setUser({
          id: decoded.id,
          username: decoded.username || decoded.userName || '', 
          email: decoded.email,
          role: decoded.role,
        });
        setIsAuthenticated(true);
      } catch (error) {
        console.error('Invalid token:', error);
        removeToken(); 
        setIsAuthenticated(false);
      }
    }
  }, []);

  const login = (token: string, userData: User) => {
    setToken(token);
    setUser(userData);
    setIsAuthenticated(true);
  };

  const logout = () => {
    removeToken();
    setUser(null);
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ user, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};