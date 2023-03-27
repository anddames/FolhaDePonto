using System.ComponentModel.DataAnnotations;

namespace FolhaDePonto.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string Senha { get; set; }

        [Required]
        [StringLength(50)]
        public string Cargo { get; set; }

        [Required]
        public double CargaHorariaDiaria { get; set; }

        public ICollection<RegistroDePontoModel> RegistroDePonto { get; set; }
    }
}

