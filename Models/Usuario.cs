using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PaschoalottoDemo.Models
{
    /// <summary>
    /// Os objetos dessa classe serão usados pelo Entiy Framework para salvar os dados no BD. 
    /// </summary>
    public class Usuario
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Primeiro_Nome")]
        [DisplayName("Primeiro Nome")]
        public required string PrimeiroNome { get; set; }

        [Column("Ultimo_Nome")]
        [DisplayName("Último Nome")]
        public required string UltimoNome { get; set; }

        [Column("Email")]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Column("Telefone")]
        [DisplayName("Telefone")]
        public required string Telefone { get; set; }

        [Column("Data_Nascimento")]
        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        public required DateTime DataNascimento { get; set; }

        [Column("Tipo_Documento")]
        [DisplayName("Tipo de Documento")]
        public string? TipoDocumento { get; set; }

        [Column("Numero_Documento")]
        [DisplayName("Número do Documento")]
        public string? NumeroDocumento { get; set; }



    }
}
