using condominio_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoEntradaVisitanteController : ControllerBase
    {
        private static List<AcessoEntradaVisitante> acessos = new List<AcessoEntradaVisitante>();

        [HttpGet]
        public ActionResult<IEnumerable<AcessoEntradaVisitante>> GetAcessos()
        {
            return Ok(acessos);
        }

        [HttpGet("{id}")]
        public ActionResult<AcessoEntradaVisitante> GetAcesso(int id)
        {
            var acesso = acessos.FirstOrDefault(a => a.Id == id);
            if (acesso == null)
            {
                return NotFound(new { mensagem = "Acesso não encontrado." });
            }
            return Ok(acesso);
        }

        [HttpPost]
        public ActionResult<AcessoEntradaVisitante> PostAcesso(AcessoEntradaVisitante novoAcesso)
        {
            var visitante = VisitanteController.visitantes.FirstOrDefault(v => v.VisitanteId == novoAcesso.VisitanteId);
            if (visitante == null)
            {
                return BadRequest(new { mensagem = "Visitante não encontrado." });
            }

            var usuario = UsuarioController.usuarios.FirstOrDefault(u => u.UsuarioId == novoAcesso.UsuarioId);
            if (usuario == null)
            {
                return BadRequest(new { mensagem = "Usuário não encontrado." });
            }

            int novoId = acessos.Count > 0 ? acessos.Max(a => a.Id) + 1 : 1;
            novoAcesso.Id = novoId;
            novoAcesso.DataHoraEntrada = DateTime.Now;

            acessos.Add(novoAcesso);

            return CreatedAtAction(nameof(GetAcesso), new { id = novoAcesso.Id }, novoAcesso);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAcesso(int id)
        {
            var acesso = acessos.FirstOrDefault(a => a.Id == id);
            if (acesso == null)
            {
                return NotFound(new { mensagem = "Acesso não encontrado." });
            }

            acessos.Remove(acesso);
            return Ok(new { mensagem = "Acesso removido com sucesso." });
        }
    }
}