using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace condominio_API.Models
{
    public class QRCodeTemp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int MoradorId { get; set; }

        [ForeignKey("MoradorId")]
        public Usuario? Morador { get; set; }

        [Required]
        public int VisitanteId { get; set; }

        [ForeignKey("VisitanteId")]
        public Visitante? Visitante { get; set; }

        [Required]
        public bool TipoQRCode { get; set; } //se for true = 24h, se for false = unico 

        [Required]
        public DateTime DataCriacao { get; set; }

        [Required]
        public DateTime DataValidade { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
