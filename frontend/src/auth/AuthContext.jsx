import { createContext, useContext, useState, useMemo, useEffect } from 'react';
import { authService, registerLogout } from '../services/api';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const login = async (credentials) => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await authService.login(credentials);
      // data should contain { token, ...userInfo } based on docs
      if (data.token) {
        localStorage.setItem('token', data.token);
        // Backend returns user info in data, construct user object
        const userData = {
          userId: data.userId,
          username: data.email, // Using email as username based on Login.jsx usage
          email: data.email,
          fullName: data.fullName,
          role: data.role
        };
        localStorage.setItem('user', JSON.stringify(userData));
        setUser(userData);
        return true;
      }
    } catch (err) {
      console.error('Login failed', err);
      setError(err.response?.data?.message || 'Login failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const register = async (userData) => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await authService.register(userData);
      if (data.token) {
        localStorage.setItem('token', data.token);
        const userInfo = {
          userId: data.userId,
          username: data.email,
          email: data.email,
          fullName: data.fullName,
          role: data.role
        };
        localStorage.setItem('user', JSON.stringify(userInfo));
        setUser(userInfo);
        return true;
      }
    } catch (err) {
      console.error('Registration failed', err);
      setError(err.response?.data?.message || 'Registration failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
  };

  const updateUser = (updates) => {
    if (user) {
      const updatedUser = { ...user, ...updates };
      localStorage.setItem('user', JSON.stringify(updatedUser));
      setUser(updatedUser);
    }
  };

  // Register the logout function with the API interceptor
  useEffect(() => {
    registerLogout(logout);
  }, []);

  const value = useMemo(() => ({ user, login, register, logout, updateUser, loading, error }), [user, loading, error]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export const useAuth = () => useContext(AuthContext);
