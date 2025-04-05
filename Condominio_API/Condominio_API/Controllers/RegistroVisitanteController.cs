using condominio_API.Request;
using condominio_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using condominio_API.Models;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoEntradaVisitanteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AcessoEntradaVisitanteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("RegistrarEntradaVisitante")]
        public async Task<ActionResult<AcessoEntradaVisitante>> RegistrarEntradaVisitante([FromBody] EntradaVisitanteRequest entradaVisitanteReq)
        {
            try
            {
                if (entradaVisitanteReq == null || entradaVisitanteReq.QRCodeTemp == null || entradaVisitanteReq.QRCodeTemp.Length == 0)
                {
                    return BadRequest(new { mensagem = "O QR Code é obrigatório!" });
                }

                var visitante = await _context.Visitantes!
                    .Include(v => v.Visitante)
                    .FirstOrDefaultAsync(v => v.QRCodeTemp == entradaVisitanteReq.QRCodeTemp);

                if (visitante == null)
                {
                    return BadRequest(new { mensagem = "QR Code não cadastrado!" });
                }

                var novaEntradaVisitante = new AcessoEntradaVisitante
                {
                    VisitanteId = visitante.visitanteId,
                    DataHoraEntrada = DateTime.Now
                };

                _context.AcessoEntradaVisitantes!.Add(novaEntradaVisitante);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensagem = "Entrada registrada com sucesso!",
                    entrada = new
                    {
                        novaEntradaVisitante.Id,
                        novaEntradaVisitante.VisitanteId,
                        UsuarioNome = visitante.Nome,
                        usuario.ApartamentoId,
                        ApartamentoNumero = usuario.Apartamento?.Numero,
                        bloco = usuario.Apartamento?.Bloco,
                        novaEntrada.DataHoraEntrada
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao registrar entrada!", detalhes = ex.Message });
            }
        }

        [HttpGet("ListarEntradasVisitantes")]
        public async Task<ActionResult> ListarEntradasVisitantes()
        {
            try
            {
                var entradasVisitante = await _context.AcessoEntradaVisitantes!
                    .Include(e => e.Usuario)
                        .ThenInclude(u => u.Apartamento)
                    .OrderByDescending(e => e.DataHoraEntrada)
                    .Select(e => new
                    {
                        e.Id,
                        e.VisitanteId,
                        Nome = e.Visitante!.Nome,
                        e.Usuario!.ApartamentoId,
                        Apartamento = e.Usuario!.Apartamento!.Numero,
                        bloco = e.Usuario!.Apartamento!.Bloco,
                        documento = e.Visitante!.Documento,
                        e.DataHoraEntrada
                    })
                    .ToListAsync();

                return Ok(entradasVisitante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar acessos!", detalhes = ex.Message });
            }
        }

        [HttpGet("FiltrarEntradasVisitanteAdmin")]
        public async Task<ActionResult> FiltrarEntradasVisitanteAdmin([FromQuery] string? documento = null, [FromQuery] int? apartamentoId = null,
            [FromQuery] string? bloco = null, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            try
            {
                if (string.IsNullOrEmpty(documento) && !apartamentoId.HasValue && string.IsNullOrEmpty(bloco) && !dataInicio.HasValue && !dataFim.HasValue)
                {
                    return BadRequest(new { mensagem = "Informe pelo menos um filtro!" });
                }

                var query = _context.AcessoEntradaVisitantes!
                    .Include(e => e.Visitante)
                        .ThenInclude(u => u.Apartamento)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(documento))
                {
                    query = query.Where(e => e.Visitante!.Documento == documento);
                }

                if (apartamentoId.HasValue)
                {
                    query = query.Where(e => e.Usuario!.ApartamentoId == apartamentoId.Value);
                }

                if (dataInicio.HasValue)
                {
                    query = query.Where(e => e.DataHoraEntrada >= dataInicio.Value);
                }

                if (dataFim.HasValue)
                {
                    query = query.Where(e => e.DataHoraEntrada <= dataFim.Value);
                }

                var entradasVisitante = await query
                    .OrderByDescending(e => e.DataHoraEntrada)
                    .Select(e => new
                    {
                        e.Id,
                        e.VisitanteId,
                        Nome = e.VIsitante!.Nome,
                        e.Usuario!.ApartamentoId,
                        Apartamento = e.Usuario!.Apartamento!.Numero,
                        bloco = e.Usuario!.Apartamento!.Bloco,
                        documento = e.visitante!.Documento, 
                        e.DataHoraEntrada
                    })
                    .ToListAsync();

                if (entradasVisitante.Count == 0)
                {
                    return NotFound(new { mensagem = "Nenhuma entrada encontrada!" });
                }

                return Ok(entradasVisitante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao filtrar entradas!", detalhes = ex.Message });
            }
        }

        [HttpGet("FiltrarEntradasVisitante_Usuario")]
        public async Task<ActionResult> FiltrarEntradasVisitante_Usuario([FromQuery] int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    return BadRequest(new { mensagem = "O usuário não é valido!" });
                }

                var usuarioLogado = await _context.Usuarios!.FirstOrDefaultAsync(user => user.UsuarioId == usuarioId);

                if (usuarioLogado == null)
                {
                    return NotFound(new { mensagem = "Usuário não encontrado!" });
                }

                var apartamentoId = usuarioLogado.ApartamentoId;

                var entradas = await _context.AcessoEntradaMoradores!
                    .Include(e => e.Usuario)
                        .ThenInclude(user => user.Apartamento)
                    .Where(e => e.Usuario!.ApartamentoId == apartamentoId)
                    .OrderByDescending(e => e.DataHoraEntrada)
                    .Select(e => new
                    {
                        id = e.Id,
                        idUsuario = e.UsuarioId,
                        nome = e.Usuario!.Nome,
                        idApartamento = e.Usuario!.ApartamentoId,
                        apartamento = e.Usuario!.Apartamento!.Numero,
                        bloco = e.Usuario!.Apartamento!.Bloco,
                        dataEntrada = e.DataHoraEntrada
                    })
                    .ToListAsync();

                if (entradas.Count == 0)
                {
                    return NotFound(new { mensagem = "Nenhuma entrada encontrada para o apartamento deste usuário!" });
                }

                return Ok(entradas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar entradas do apartamento!", detalhes = ex.Message });
            }
        }
    }
}