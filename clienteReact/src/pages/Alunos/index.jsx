import React, { useState, useEffect } from 'react'; 
import { Link, useNavigate } from 'react-router-dom';  
import { FiUserX, FiEdit } from 'react-icons/fi';
import './styles.css';
import logoCadastro from '../../assets/cadastro.png';
import api from '../../services/api';

function Alunos() {
  const [alunos, setAlunos] = useState([]);
  const [search, setSearch] = useState('');  // Estado para pesquisa
  const navigate = useNavigate();

  const email = localStorage.getItem('email');
  const token = localStorage.getItem('token');

  useEffect(() => {
    if (!token) {
      console.error('Token não encontrado no localStorage.');
      return;
    }

    const fetchAlunos = async () => {
      try {
        const response = await api.get('https://localhost:7135/api/Alunos', {
          headers: { Authorization: `Bearer ${token}` },
        });
        setAlunos(response.data);
      } catch (error) {
        console.error('Erro ao buscar alunos:', error);
      }
    };

    fetchAlunos();
  }, [token]);

  const handleDeleteAluno = async (id) => {
    if (!token) return alert('Token inválido, faça login novamente.');

    const confirmacao = window.confirm('Tem certeza que deseja remover este aluno?');
    if (!confirmacao) return;

    try {
      await api.delete(`https://localhost:7135/api/Alunos/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });

      setAlunos(alunos.filter(aluno => aluno.id !== id));
    } catch (error) {
      console.error('Erro ao excluir aluno:', error);
      alert('Erro ao excluir aluno.');
    }
  }

  const handleEditAluno = (id) => {
    navigate(`/aluno/editar/${id}`);
  }

  // Filtrando os alunos pela pesquisa
  const filteredAlunos = alunos.filter(aluno => 
    aluno.nome.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className='aluno-container-alunos'>
      <header className='header-alunos'>
        <img src={logoCadastro} alt='Cadastro' className='logo-alunos' />
        <span className='saudacao-alunos'>Bem-vindo, <strong>{email}</strong></span>
        <Link className='button-alunos' to="/aluno/novo/0">Novo aluno</Link>

        <h1 className='titulo-alunos'>Relação de alunos</h1>

        {/* Campo de pesquisa */}
        <input 
          type="text" 
          placeholder="Pesquisar por nome" 
          value={search}
          onChange={(e) => setSearch(e.target.value)} 
          className="search-input"
        />

        <ul>
          {filteredAlunos.length > 0 ? (
            filteredAlunos.map(aluno => (
              <li key={aluno.id}>
                <b>Nome:</b> {aluno.nome}<br />
                <b>Email:</b> {aluno.email}<br />
                <b>Idade:</b> {aluno.idade}<br />
                
                <button type='button' className='editar-aluno' onClick={() => handleEditAluno(aluno.id)}>
                  <FiEdit size="25" color="#17202a" />
                </button>
                <button type='button' className='remover-aluno' onClick={() => handleDeleteAluno(aluno.id)}>
                  <FiUserX size="25" color='#17202a' />
                </button>
              </li>
            ))
          ) : (
            <p>Nenhum aluno encontrado.</p>
          )}
        </ul>
      </header>
    </div>
  );
}

export default Alunos;
