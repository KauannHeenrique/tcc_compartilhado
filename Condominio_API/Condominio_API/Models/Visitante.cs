using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace condominio_API.Models
{
    public class Visitante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VisitanteId { get; set; }

        [Required]
        public required string Nome { get; set; }

        [Required]
        public required string Documento { get; set; }

        [Required]
        public string? Telefone { get; set; }

    }
}
