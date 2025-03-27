using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace condominio_API.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }

        [Required]
        public required string Nome { get; set; }

        [Required]
        public required string Documento { get; set; }

        [Required]
        public required string Email { get; set; }  

        [Required]
        public required string Senha { get; set; }   
        [Required]
        public required string NivelAcesso { get; set; } // pode ser morador, funcionario ou sindico 

        [Required]
        public string? Telefone { get; set; }

        [Required]
        public int ApartamentoId { get; set; }  

        [ForeignKey("ApartamentoId")]
        public Apartamento? Apartamento { get; set; }

        [Required]
        public required string CodigoRFID { get; set; } 

        [Required]
        public bool Status { get; set; }
    }
}
