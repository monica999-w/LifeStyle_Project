import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../components/provider/AuthProvider';
import { useNotification } from '../../components/provider/NotificationContext';
import axios from 'axios';
import './Auth.css';
import { environment } from '../../environments/environment';
import routes from '../../components/route/routes';

const Login: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { setToken } = useAuth();
  const { notify } = useNotification();

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const apiUrl = `${environment.apiUrl}Auth/login`;
    console.log("API URL: ", apiUrl);
    try {
      const response = await axios.post(apiUrl, { email, password });
      setToken(response.data.token);
      notify('Login successful', 'success');
      navigate(routes.profile);
    } catch (error) {
      setError('Login failed. Please check your credentials.');
      notify('Login failed. Please check your credentials.', 'error');
    }
  };

  return (
    <div className="container">
      <div className="signup">
        <h2>Sign in</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          {error && <p className="error">{error}</p>}
          <a href="/register" className="signin-link">Don't have an account? Register</a>
          <button type="submit" className="signin-btn">SIGN IN</button>
        </form>
      </div>
      <div className="signin">
        <h2>Welcome to LifeStyle</h2>
      </div>
    </div>
  );
};

export default Login;

