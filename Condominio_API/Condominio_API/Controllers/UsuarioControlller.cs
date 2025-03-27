using condominio_API.Data;
using condominio_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ExibirTodosUsuarios")]

        public async Task<ActionResult<IEnumerable<Apartamento>>> GetApartamento()
        {
            return await _context.Apartamentos.ToListAsync();
        }

        [HttpGet("BuscarUsuarioPor")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario([FromQuery] string? nomeUsuario, [FromQuery] string? documento, [FromQuery]  string? emailUsuario)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomeUsuario))
            {
                query = query.Where(user => user.Nome.Contains(nomeUsuario));
            }

            if (!string.IsNullOrWhiteSpace(documento))
            {
                query = query.Where(user => user.Documento.Contains(documento));
            }

            if (!string.IsNullOrWhiteSpace(emailUsuario))
            {
                query = query.Where(user => user.Email.Contains(emailUsuario));
            }

            var usuarios = await query.ToListAsync();

            if (usuarios.Count == 0)
            {
                return NotFound(new { mensagem = "Nenhum usuário encontrado." });
            }

            return Ok(usuarios);
        }

        [HttpPost("AdicionarUsuario")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario novoUsuario)
        {
            try
            {
                if (novoUsuario == null)
                {
                    return BadRequest(new { mensagem = "Por favor, preencha todos os campos" });
                }

                var locatarioFirst = await _context.Usuarios.FirstOrDefaultAsync(user => user.Documento == novoUsuario.Documento
               || user.Email == novoUsuario.Email);

                if (locatarioFirst != null)
                {
                    return BadRequest(new { mensagem = "Documento ou e-mail já cadastrado. Por favor, tente novamente." });
                }

                if (((int)novoUsuario.NivelAcesso == 2 || (int)novoUsuario.NivelAcesso == 3) && novoUsuario.ApartamentoId <= 0)
                {
                    return BadRequest(new { mensagem = "Síndicos e moradores devem ter um apartamento válido." });
                }

                _context.Usuarios.Add(novoUsuario);
                await _context.SaveChangesAsync();

                var usuarioRetornado = new  // usado pra retornar na tela os dados "required"(?)
                {
                    novoUsuario.UsuarioId,
                    novoUsuario.Nome,
                    novoUsuario.Email,
                    novoUsuario.NivelAcesso,
                    novoUsuario.ApartamentoId
                };

                return Ok(new { mensagem = "Usuário cadastrado com sucesso!", usuarioRetornado });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao cadastrar usuário!", detalhes = ex.Message });
            }
        }

        [HttpPut("AtualizarUsuario/{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id <= 0)
            {
                return BadRequest("Usuário inválido.");
            }

            var usuarioEx = await _context.Usuarios.FindAsync(id);

            if (usuarioEx == null)
            {
                return NotFound();
            }

            usuarioEx.Nome = usuario.Nome;
            usuarioEx.Documento = usuario.Documento;
            usuarioEx.Email = usuario.Email;
            usuarioEx.Senha = usuario.Senha;
            usuarioEx.Telefone = usuario.Telefone;
            usuarioEx.ApartamentoId = usuario.ApartamentoId;
            usuarioEx.CodigoRFID = usuario.CodigoRFID;
            usuarioEx.Status = usuario.Status;

            _context.Usuarios.Update(usuarioEx);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("ExcluirUsuario/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Usuario inválido.");
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
