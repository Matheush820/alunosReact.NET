using AlunoApi.Models;
using AlunoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlunoApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
//[Produces("application/json")]
public class AlunosController : ControllerBase
{
    private readonly IAlunoService _alunoService;

    public AlunosController(IAlunoService alunoService)
    {
        _alunoService = alunoService;
    }

    [HttpGet]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
    {
        try
        {
            var alunos = await _alunoService.GetAlunos();
            return Ok(alunos);
        }
        catch
        {
            //return BadRequest("Request invalido");
            return StatusCode(500, "Erro ao processar a requisição");
        }
    }
    [HttpGet("AlunoPorNome")]
    public async Task<ActionResult<IAsyncEnumerable<Aluno>>>
        GetAlunosByName([FromQuery] string nome)
    {
        try
        {
            var alunos = await _alunoService.GetAlunosByNome(nome);
            if (alunos == null)
                return NotFound($"Nao existe aluno com esse nome {nome}");
            return Ok(alunos);
        }
        catch
        {
            return StatusCode(500, $"Erro ao buscar aluno por nome {nome}");
        }
    }

    [HttpGet("{id:int}", Name = "GetAluno")]
    public async Task<ActionResult<Aluno>> GetAluno(int id)
    {
        try
        {
            var alunos = await _alunoService.GetAluno(id);
            if (alunos == null)
                return NotFound($"Nao existe aluno com esse id {id}");
            return Ok(alunos);
        }
        catch
        {
            return StatusCode(500, $"Nao existe aluno com esse id: {id}");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create(Aluno aluno)
    {
        try
        {
            await _alunoService.CreateAluno(aluno);
            return CreatedAtRoute(nameof(GetAluno), new { id = aluno.Id }, aluno);
        }
        catch
        {
            return StatusCode(500, $"Erro ao criar aluno. Dados enviados: Nome={aluno.Nome}, Email={aluno.Email}, Idade={aluno.Idade}");

        }
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Edit(int id, [FromBody]Aluno aluno)
    {
        try
        {
            if(aluno.Id == id)
            {
               await _alunoService.UpdateAluno(aluno);
                //return NoContent();
                return Ok($"Aluno com id: {id} foi atualizado com sucesso");
            }
            else
            {
                return BadRequest("Dados inconsistentes");
            }
        }
        catch
        {
            return StatusCode(500, $"Erro Atualizar aluno com id: {id}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            
        var aluno = await _alunoService.GetAluno(id);
         if(aluno != null)
           {
             await _alunoService.DeleteAluno(aluno);
             return Ok($"aluno com id:{id} deletado com sucesso");
           }
            else
           {
             return NotFound($"aluno com id: {id} nao encontrado :(");
           }
 
        }
        catch
        {
            return StatusCode(500, $"Erro Atualizar aluno com id: {id}");
        }
    }
}
