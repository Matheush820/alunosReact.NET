using AlunoApi.Services;
using AlunoApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlunoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthenticate _authenticateService;

    public AccountController(IConfiguration configuration, IAuthenticate authenticateService)
    {
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));

        _authenticateService = authenticateService
            ?? throw new ArgumentNullException(nameof(authenticateService));
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "As senhas não conferem");
            return BadRequest(ModelState);
        }

        var result = await _authenticateService.RegisterUser(model.Email, model.Password);

        if (result)
        {
            return Ok($"Usuário {model.Email} criado com sucesso");
        }
        else
        {
            ModelState.AddModelError("CreateUser", "Registro inválido");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("LoginUser")]
    public async Task<ActionResult<UserToken>> Login([FromBody] RegisterModel model)
    {
        var result = await _authenticateService.Authenticate(model.Email, model.Password);

        if (result)
        {
            return GenerateToken(model);
        }
        else
        {
            ModelState.AddModelError("LoginUser", "Login inválido");
            return BadRequest(ModelState);
        }
    }

    private ActionResult<UserToken> GenerateToken(RegisterModel userInfo)
    {
        var claims = new[]
        {
            new Claim("email", userInfo.Email),
            new Claim("meuToken", "token do matheus"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(20);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return new UserToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
