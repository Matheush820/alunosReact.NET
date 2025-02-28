using AlunoApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlunoApi.Context;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Aluno> Alunos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Importante para ASP.NET Identity funcionar corretamente!

        modelBuilder.Entity<Aluno>().HasData(
            new Aluno
            {
                Id = 1,
                Nome = "João",
                Email = "matheuszin@gmail.com",
                Idade = 20
            },
            new Aluno
            {
                Id = 2,
                Nome = "Maria",
                Email = "matheuszao@gmail.com",
                Idade = 21
            }
        );
    }
}
