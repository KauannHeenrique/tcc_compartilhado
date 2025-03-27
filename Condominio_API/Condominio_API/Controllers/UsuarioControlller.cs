using condominio_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public static List<Usuario> usuarios = new List<Usuario>(); 

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> GetUsuario(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado." });
            }

            return Ok(usuario);
        }

        [HttpPost]
        public ActionResult<Usuario> PostUsuario(Usuario novoUsuario)
        {
            int novoId = usuarios.Count > 0 ? usuarios.Max(u => u.UsuarioId) + 1 : 1;
            novoUsuario.UsuarioId = novoId;

            if ((novoUsuario.NivelAcesso == "sindico" || novoUsuario.NivelAcesso == "morador") && novoUsuario.ApartamentoId == 0)
            {
                return BadRequest(new { mensagem = "Síndicos e moradores devem ter um ApartamentoId válido." });
            }

            usuarios.Add(novoUsuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.UsuarioId }, novoUsuario);
        }

        [HttpPut("{id}")]
        public IActionResult PutUsuario(int id, Usuario usuarioAtualizado)
        {
            var usuarioExistente = usuarios.FirstOrDefault(u => u.UsuarioId == id);
            if (usuarioExistente == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado." });
            }

            usuarioExistente.Nome = usuarioAtualizado.Nome;
            usuarioExistente.Documento = usuarioAtualizado.Documento;
            usuarioExistente.Email = usuarioAtualizado.Email;
            usuarioExistente.Senha = usuarioAtualizado.Senha;
            usuarioExistente.NivelAcesso = usuarioAtualizado.NivelAcesso;
            usuarioExistente.Telefone = usuarioAtualizado.Telefone;
            usuarioExistente.ApartamentoId = usuarioAtualizado.ApartamentoId;
            usuarioExistente.CodigoRFID = usuarioAtualizado.CodigoRFID;
            usuarioExistente.Status = usuarioAtualizado.Status;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado." });
            }

            usuarios.Remove(usuario);
            return Ok(new { mensagem = "Usuário removido com sucesso." });
        }
    }
}
