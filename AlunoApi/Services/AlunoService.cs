using AlunoApi.Context;
using AlunoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunoApi.Services;

public class AlunoService : IAlunoService
{
    private readonly AppDbContext _context;

    public AlunoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Aluno>> GetAlunos()
    {
        try
        {
            return await _context.Alunos.ToListAsync();
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<Aluno>> GetAlunosByNome(string nome)
    {
        try
        {
            IEnumerable<Aluno> alunos;

            // Se o nome não for nulo ou vazio, filtra os alunos que contêm o nome informado
            if (!string.IsNullOrEmpty(nome))
            {
                alunos = await _context.Alunos.Where(a => a.Nome.Contains(nome)).ToListAsync();
            }
            else
            {
                // Se o nome for nulo ou vazio, retorna todos os alunos
                alunos = await _context.Alunos.ToListAsync();
            }

            return alunos; // Retorna a lista de alunos
        }
        catch
        {
            throw;
        }
    }


    public async Task<Aluno> GetAluno(int id)
    {
       var aluno = await _context.Alunos.FindAsync(id);
       return aluno;
    }
    public async Task CreateAluno(Aluno aluno)
    {
        _context.Alunos.Add(aluno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAluno(Aluno aluno)
    {
       _context.Entry(aluno).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAluno(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        await _context.SaveChangesAsync();
    }

}
