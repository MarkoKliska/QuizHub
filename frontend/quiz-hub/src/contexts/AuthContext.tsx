import React, { createContext, useState, useEffect, ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';
import { User } from '../models/User';
import { getToken, setToken, removeToken } from '../utils/tokenStorage';

interface DecodedToken {
  sub?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
  id?: number;
  username?: string;
  userName?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'?: string;
  email?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'?: string;
  role?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
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
        
        // Ekstraktuj role iz različitih mogućih claim names
        const role = decoded.role || 
                     decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
                     'User';
        
        // Ekstraktuj username iz različitih mogućih claim names
        const username = decoded.username || 
                        decoded.userName || 
                        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
                        '';
        
        // Ekstraktuj email iz različitih mogućih claim names
        const email = decoded.email ||
                     decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ||
                     '';
        
        // Ekstraktuj ID iz različitih mogućih claim names
        const userId = decoded.id || 
                      (decoded.sub ? parseInt(decoded.sub) : undefined) ||
                      (decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] 
                        ? parseInt(decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']) 
                        : 0);
        
        console.log('Decoded token:', { username, email, role, userId }); // Debug log
        
        setUser({
          id: userId,
          username: username,
          email: email,
          role: role,
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