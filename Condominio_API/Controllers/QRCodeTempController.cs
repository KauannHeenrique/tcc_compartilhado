/* using condominio_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QRCoder;
using System.Drawing;
using System.IO;

namespace condominio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeTempController : ControllerBase
    {
        private static List<QRCodeTemp> qrCodes = new List<QRCodeTemp>();

        [HttpGet]
        public ActionResult<IEnumerable<QRCodeTemp>> GetQRCodes()
        {
            return Ok(qrCodes);
        }

        [HttpGet("{id}")]
        public ActionResult<QRCodeTemp> GetQRCode(int id)
        {
            var qrCode = qrCodes.FirstOrDefault(q => q.Id == id);
            if (qrCode == null)
            {
                return NotFound(new { mensagem = "QR Code não encontrado." });
            }
            return Ok(qrCode);
        }

        [HttpPost]
        public ActionResult<object> PostQRCode(QRCodeTemp novoQRCode)
        {
            var morador = UsuarioController.usuarios.FirstOrDefault(u => u.UsuarioId == novoQRCode.MoradorId);
            if (morador == null)
            {
                return BadRequest(new { mensagem = "Morador não encontrado." });
            }

            var visitante = VisitanteController.visitantes.FirstOrDefault(v => v.VisitanteId == novoQRCode.VisitanteId);
            if (visitante == null)
            {
                return BadRequest(new { mensagem = "Visitante não encontrado." });
            }

            var qrCodeExistente = qrCodes.FirstOrDefault(q => q.MoradorId == novoQRCode.MoradorId);
            if (qrCodeExistente != null)
            {
                string qrCodeDataExistente = $"QRCodeId:{qrCodeExistente.Id},MoradorId:{qrCodeExistente.MoradorId},VisitanteId:{qrCodeExistente.VisitanteId},Validade:{qrCodeExistente.DataValidade}";
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCode = qrGenerator.CreateQrCode(qrCodeDataExistente, QRCodeGenerator.ECCLevel.Q);
                    using (var qrCodeImage = new PngByteQRCode(qrCode))
                    {
                        byte[] qrImageBytes = qrCodeImage.GetGraphic(20);
                        string base64Image = Convert.ToBase64String(qrImageBytes);
                        return Ok(new
                        {
                            qrCode = qrCodeExistente,
                            qrCodeImage = $"data:image/png;base64,{base64Image}"
                        });
                    }
                }
            }

            int novoId = qrCodes.Count > 0 ? qrCodes.Max(q => q.Id) + 1 : 1;
            novoQRCode.Id = novoId;
            novoQRCode.DataCriacao = DateTime.Now;
            novoQRCode.DataValidade = novoQRCode.TipoQRCode ? DateTime.Now.AddHours(24) : DateTime.Now.AddYears(100);
            novoQRCode.Status = true;

            qrCodes.Add(novoQRCode);

            string qrCodeData = $"QRCodeId:{novoQRCode.Id},MoradorId:{novoQRCode.MoradorId},VisitanteId:{novoQRCode.VisitanteId},Validade:{novoQRCode.DataValidade}";
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCode = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
                using (var qrCodeImage = new PngByteQRCode(qrCode))
                {
                    byte[] qrImageBytes = qrCodeImage.GetGraphic(20);
                    string base64Image = Convert.ToBase64String(qrImageBytes);
                    return Ok(new
                    {
                        qrCode = novoQRCode,
                        qrCodeImage = $"data:image/png;base64,{base64Image}"
                    });
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQRCode(int id)
        {
            var qrCode = qrCodes.FirstOrDefault(q => q.Id == id);
            if (qrCode == null)
            {
                return NotFound(new { mensagem = "QR Code não encontrado." });
            }

            qrCodes.Remove(qrCode);
            return Ok(new { mensagem = "QR Code removido com sucesso." });
        }
    }
}*/