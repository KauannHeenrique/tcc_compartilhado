﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace condominio_API.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]  // stringlength conta quantos carcteres tem na variavel para validar ela (maximo, minimo)
        public required string Nome { get; set; }


        [Required]
        [StringLength(14, MinimumLength = 8)]
        public required string Documento { get; set; }


        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 6)]
        public required string Senha { get; set; }


        [Required]
        public nivelAcessoEnum NivelAcesso { get; set; }


        [Required]
        [StringLength(15)]
        public string? Telefone { get; set; }


        [Required]
        public int? ApartamentoId { get; set; }  // apartamento vai ser opcional pelos acessos de funcionario e adm (nao moradores)

        [ForeignKey("ApartamentoId")]
        public Apartamento? Apartamento { get; set; }


        [Required]
        [StringLength(50)]
        public required string CodigoRFID { get; set; }


        [Required]
        public bool Status { get; set; }

    }

    public enum nivelAcessoEnum
    {
        Admin = 1,
        Sindico = 2,
        Funcionario = 3,
        Morador = 4
    }
}