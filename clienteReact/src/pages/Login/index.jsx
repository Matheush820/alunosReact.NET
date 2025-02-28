import React, { useState } from 'react';
import './style.css';
import api from '../../services/api';
import { useNavigate } from 'react-router-dom';

import logoImage from '../../assets/login.png';

function Index() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const navigate = useNavigate();

  async function login(event) {
    event.preventDefault();
    setLoading(true);

    if (password !== confirmPassword) {
      alert("As senhas não coincidem!");
      setLoading(false);
      return;
    }

    const data = { 
      email, 
      password,
      confirmPassword,
    };

    console.log("Dados enviados para a API:", data);

    try {
      const response = await api.post('https://localhost:7135/api/Account/LoginUser', data, {
        headers: { 
          "Content-Type": "application/json"
        }
      });

      console.log("Resposta da API:", response.data);

      localStorage.setItem('email', email);
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('expiration', response.data.expiration);

      navigate('/alunos');
    } catch (error) {
      if (error.response) {
        console.error("Erro ao fazer login:", error.response.data);
        alert("Erro: " + JSON.stringify(error.response.data, null, 2));
      } else {
        console.error("Erro desconhecido:", error);
        alert("Erro desconhecido: " + error.message);
      }
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className='login-container'>
      <section className='form'>
        <img src={logoImage} alt='login' id='im1' />
        <form onSubmit={login}>
          <h1>Cadastro de alunos</h1>
          <input 
            type='email' 
            placeholder='Email' 
            value={email} 
            onChange={e => setEmail(e.target.value)} 
            required 
          />
          <div className="password-container">
            <input 
              type={showPassword ? 'text' : 'password'} 
              placeholder='Senha' 
              value={password} 
              onChange={e => setPassword(e.target.value)} 
              required 
            />
            <button 
              type="button" 
              className="eye-icon" 
              onClick={() => setShowPassword(!showPassword)}
            >
              👁️
            </button>
          </div>
          <div className="password-container">
            <input 
              type={showConfirmPassword ? 'text' : 'password'} 
              placeholder='Confirmar Senha' 
              value={confirmPassword} 
              onChange={e => setConfirmPassword(e.target.value)} 
              required 
            />
            <button 
              type="button" 
              className="eye-icon" 
              onClick={() => setShowConfirmPassword(!showConfirmPassword)}
            >
              👁️
            </button>
          </div>
          <button className='button' type='submit' disabled={loading}>
            {loading ? 'Entrando...' : 'Login'}
          </button>
        </form>
      </section>
    </div>
  );
}

export default Index;
