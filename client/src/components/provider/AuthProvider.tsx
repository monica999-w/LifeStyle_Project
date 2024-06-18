import axios from "axios";
import {jwtDecode} from "jwt-decode";
import React, { createContext, useContext, useEffect, useMemo, useState, ReactNode } from "react";

interface AuthContextType {
  token: string | null;
  setToken: (token: string | null) => void;
  email: string | null;
  role: string | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [token, setToken_] = useState<string | null>(localStorage.getItem("token"));
  const [email, setEmail] = useState<string | null>(localStorage.getItem("email"));
  const [role, setRole] = useState<string | null>(localStorage.getItem("role"));

  const setToken = (newToken: string | null) => {
    setToken_(newToken);
    if (newToken) {
      axios.defaults.headers.common["Authorization"] = "Bearer " + newToken;
      localStorage.setItem("token", newToken);
      const decodedToken: { email: string; role: string } = jwtDecode(newToken);
      setEmail(decodedToken.email);
      setRole(decodedToken.role);
      localStorage.setItem('email', decodedToken.email);
      localStorage.setItem('role', decodedToken.role);
    } else {
      delete axios.defaults.headers.common["Authorization"];
      localStorage.removeItem("token");
      localStorage.removeItem("email");
      localStorage.removeItem("role");
      setEmail(null);
      setRole(null);
    }
  };

  const isTokenExpired = () => {
    if (!token) return true;
    try {
      const decodedToken: { exp: number } = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      if (decodedToken.exp < currentTime) {
        setToken(null);
        return true;
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      setToken(null);
      return true;
    }
    return false;
  };

  useEffect(() => {
    const interval = setInterval(() => {
      isTokenExpired();
    }, 120000);

    return () => clearInterval(interval);
  }, [token]);

  const contextValue = useMemo(() => ({ token, setToken, email, role }), [token, email, role]);

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

export default AuthProvider;