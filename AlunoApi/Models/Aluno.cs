using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlunoApi.Models;

[Table("Alunos")]
public class Aluno
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required]
    public int Idade { get; set; }
}
