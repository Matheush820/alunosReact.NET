import React, { useState, useEffect } from "react";
import { Link, useParams, useNavigate } from "react-router-dom";
import './style.css';
import { FiCornerDownLeft, FiUserPlus } from "react-icons/fi";
import api from "../../services/api";

function NovoAluno() {
    const { tipo, alunoId } = useParams(); // Obtendo tipo e alunoId da URL
    const navigate = useNavigate();
    
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [idade, setIdade] = useState('');

    useEffect(() => {
        if (tipo === 'editar' && alunoId !== '0') {  // Se for edição, busca os dados do aluno
            const fetchAluno = async () => {
                try {
                    const response = await api.get(`https://localhost:7135/api/Alunos/${alunoId}`);
                    const aluno = response.data;
                    setNome(aluno.nome);
                    setEmail(aluno.email);
                    setIdade(aluno.idade);
                } catch (error) {
                    console.error('Erro ao buscar dados do aluno:', error);
                }
            };

            fetchAluno();
        }
    }, [tipo, alunoId]); // Atualiza quando tipo ou alunoId mudar

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("Dados enviados:", { nome, email, idade });

        try {
            const token = localStorage.getItem('token');
            const url = tipo === 'novo' 
                ? 'https://localhost:7135/api/Alunos'  // Adicionar novo aluno
                : `https://localhost:7135/api/Alunos/${alunoId}`; // Atualizar aluno

            const method = tipo === 'novo' ? 'POST' : 'PUT';
            const response = await api({
                method,
                url,
                data: { nome, email, idade },
                headers: { Authorization: `Bearer ${token}` }
            });

            console.log('Aluno salvo/atualizado com sucesso!', response);
            navigate('/alunos');
        } catch (error) {
            console.error('Erro ao salvar/atualizar aluno', error);
        }
    };

    return (
        <div className="novo-aluno-container">
            <div className="content-alunonovo">
                <section className="form">
                    <FiUserPlus size={105} color="#17202a" />
                    <h1>{tipo === 'novo' ? 'Incluir Novo Aluno' : 'Atualizar Aluno'}</h1>
                    <Link className="back-link" to="/alunos">
                        <FiCornerDownLeft size={25} color="#17202a" /> Retornar
                    </Link>
                </section>
                <form onSubmit={handleSubmit}>
                    <input 
                        placeholder="Nome" 
                        value={nome} 
                        onChange={(e) => setNome(e.target.value)}
                    />
                    <input 
                        placeholder="Email" 
                        value={email} 
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <input 
                        placeholder="Idade" 
                        type="number" 
                        value={idade} 
                        onChange={(e) => setIdade(e.target.value)}
                    />
                    <button className="button" type="submit">
                        {tipo === 'novo' ? 'Incluir Novo Aluno' : 'Atualizar Aluno'}
                    </button>
                </form>
            </div>
        </div>
    );
}

export default NovoAluno;
