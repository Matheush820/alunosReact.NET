import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'; 
import Login from './pages/Login'; 
import Alunos from './pages/Alunos';
import NovoAluno from './pages/NovoAluno'

import './App.css';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />  
        <Route path="/alunos" element={<Alunos />} />
        
        <Route path="/aluno/:tipo/:alunoId" element={<NovoAluno />} />
      </Routes>
    </Router>
  );
}

export default App;
