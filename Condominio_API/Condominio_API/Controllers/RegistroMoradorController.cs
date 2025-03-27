/*using condominio_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoEntradaUsuarioController : ControllerBase
    {
        private static List<Usuario> registros = new List<Usuario>();

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetRegistros()
        {
            return Ok(registros);
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> GetRegistro(int id)
        {
            var registro = registros.FirstOrDefault(r => r.UsuarioId == id);
            if (registro == null)
            {
                return NotFound(new { mensagem = "Registro não encontrado." });
            }
            return Ok(registro);
        }

        [HttpPost]
        public ActionResult<Usuario> PostRegistro(int usuarioId)
        {
            var usuario = UsuarioController.usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);
            if (usuario == null)
            {
                return BadRequest(new { mensagem = "Usuário não encontrado." });
            }

            var registro = new Usuario
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Documento = usuario.Documento,
                Email = usuario.Email,
                Senha = usuario.Senha,
                NivelAcesso = usuario.NivelAcesso,
                Telefone = usuario.Telefone,
                ApartamentoId = usuario.ApartamentoId,
                CodigoRFID = usuario.CodigoRFID,
                Status = usuario.Status
            };

            registros.Add(registro);

            return CreatedAtAction(nameof(GetRegistro), new { id = registro.UsuarioId }, registro);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRegistro(int id)
        {
            var registro = registros.FirstOrDefault(r => r.UsuarioId == id);
            if (registro == null)
            {
                return NotFound(new { mensagem = "Registro não encontrado." });
            }

            registros.Remove(registro);
            return Ok(new { mensagem = "Registro removido com sucesso." });
        }
    }
}*/