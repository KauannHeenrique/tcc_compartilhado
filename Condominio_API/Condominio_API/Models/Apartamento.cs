using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace condominio_API.Models
{
    public class Apartamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Bloco { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public required string Proprietario { get; set; }
    }
}
