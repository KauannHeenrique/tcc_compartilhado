using condominio_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitanteController : ControllerBase
    {
        public static List<Visitante> visitantes = new List<Visitante>();

        [HttpGet]
        public ActionResult<IEnumerable<Visitante>> GetVisitantes()
        {
            return Ok(visitantes);
        }

        [HttpGet("{id}")]
        public ActionResult<Visitante> GetVisitante(int id)
        {
            var visitante = visitantes.FirstOrDefault(v => v.VisitanteId == id);
            if (visitante == null)
            {
                return NotFound(new { mensagem = "Visitante não encontrado." });
            }
            return Ok(visitante);
        }

        [HttpPost]
        public ActionResult<Visitante> PostVisitante(Visitante novoVisitante)
        {
            if (string.IsNullOrEmpty(novoVisitante.Nome) || string.IsNullOrEmpty(novoVisitante.Documento) || string.IsNullOrEmpty(novoVisitante.Telefone))
            {
                return BadRequest(new { mensagem = "Nome, Documento e Telefone são obrigatórios." });
            }

            var visitanteExistente = visitantes.FirstOrDefault(v => v.Documento == novoVisitante.Documento);
            if (visitanteExistente != null)
            {
                return BadRequest(new { mensagem = "Já existe um visitante cadastrado com este documento." });
            }

            int novoId = visitantes.Count > 0 ? visitantes.Max(v => v.VisitanteId) + 1 : 1;
            novoVisitante.VisitanteId = novoId;

            visitantes.Add(novoVisitante);

            return CreatedAtAction(nameof(GetVisitante), new { id = novoVisitante.VisitanteId }, novoVisitante);
        }

        [HttpPut("{id}")]
        public IActionResult PutVisitante(int id, Visitante visitanteAtualizado)
        {
            var visitanteExistente = visitantes.FirstOrDefault(v => v.VisitanteId == id);
            if (visitanteExistente == null)
            {
                return NotFound(new { mensagem = "Visitante não encontrado." });
            }

            if (string.IsNullOrEmpty(visitanteAtualizado.Nome) || string.IsNullOrEmpty(visitanteAtualizado.Documento) || string.IsNullOrEmpty(visitanteAtualizado.Telefone))
            {
                return BadRequest(new { mensagem = "Nome, Documento e Telefone são obrigatórios." });
            }

            var outroVisitante = visitantes.FirstOrDefault(v => v.Documento == visitanteAtualizado.Documento && v.VisitanteId != id);
            if (outroVisitante != null)
            {
                return BadRequest(new { mensagem = "Outro visitante já está cadastrado com este documento." });
            }

            visitanteExistente.Nome = visitanteAtualizado.Nome;
            visitanteExistente.Documento = visitanteAtualizado.Documento;
            visitanteExistente.Telefone = visitanteAtualizado.Telefone;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVisitante(int id)
        {
            var visitante = visitantes.FirstOrDefault(v => v.VisitanteId == id);
            if (visitante == null)
            {
                return NotFound(new { mensagem = "Visitante não encontrado." });
            }

            visitantes.Remove(visitante);
            return Ok(new { mensagem = "Visitante removido com sucesso." });
        }
    }
}