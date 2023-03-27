using System.ComponentModel.DataAnnotations;

namespace FolhaDePonto.Models
{
    public class RegistroDePontoModel
    {
        public int UserId { get; set; }
        public UsuarioModel Usuario { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public DateTime HoraEntrada { get; set; }

        [Required]
        public DateTime HoraAlmoco { get; set; }

        [Required]
        public DateTime HoraRetornoAlmoco { get; set; }

        [Required]
        public DateTime HoraSaida { get; set; }

        [StringLength(200)]
        public string Observacao { get; set; }

        [Required]
        public double TotalHorasTrabalhadas { get; set; }  

        [Required]
        public double HorasExtras { get; set; }
    }

}
