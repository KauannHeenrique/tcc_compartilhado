using System.ComponentModel.DataAnnotations;

namespace condominio_API.Request
{
    public class EntradaVisitanteRequest
    {
        [Required]
        public byte[] QRCodeTemp { get; set; } 
    }
}