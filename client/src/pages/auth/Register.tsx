import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../components/provider/AuthProvider';
import { useNotification } from '../../components/provider/NotificationContext';
import axios from 'axios';
import './Auth.css';
import { environment } from '../../environments/environment';
import routes from '../../components/route/routes';

const Register: React.FC = () => {
  const [email, setEmail] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { setToken } = useAuth();
  const { notify } = useNotification();

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const apiUrl = `${environment.apiUrl}Auth/register`;
    console.log("API URL: ", apiUrl);
    try {
      const response = await axios.post(apiUrl, {
        email,
        username,
        password,
        confirmPassword,
        phoneNumber,
      });
      setToken(response.data.token);
      notify('Registration successful', 'success');
      navigate(routes.profile);
    } catch (error) {
      setError('Registration failed. Please check your details.');
      notify('Registration failed. Please check your details.', 'error');
    }
  };

  return (
    <div className="container">
      <div className="signup">
        <h2>Sign up</h2>
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
            type="text"
            name="username"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
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
          <input
            type="password"
            name="confirmPassword"
            placeholder="Confirm Password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
          <input
            type="tel"
            name="phoneNumber"
            placeholder="Phone Number"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
          />
          {error && <p className="error">{error}</p>}
          <a href="/login" className="signin-link">Already have an account? Sign In</a>
          <button type="submit" className="signin-btn">REGISTER</button>
        </form>
      </div>
      <div className="signin">
        <h2>Welcome to LifeStyle</h2>
      </div>
    </div>
  );
};

export default Register;
