using System.ComponentModel.DataAnnotations;

namespace AlunoApi.ViewModel;

public class LoginModel
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(20, ErrorMessage = "A senha deve ter no mínimo 6 caracteres", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
