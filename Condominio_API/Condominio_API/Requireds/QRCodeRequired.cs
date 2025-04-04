using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace condominio_API.Models
{
    public class QRCodeRequired
    {
        [Required]
        public int MoradorId { get; set; }

        [Required]
        public int VisitanteId { get; set; }

        [Required]
        public bool TipoQRCode { get; set; }
    }
}