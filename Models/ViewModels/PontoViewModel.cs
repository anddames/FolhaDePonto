using System.ComponentModel.DataAnnotations;

namespace FolhaDePonto.Models.ViewModels
{
    public class PontoViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name = "Hora de Entrada")]
        public DateTime HoraEntrada { get; set; }

        [Required]
        [Display(Name = "Hora de Saída para Almoço")]
        public DateTime HoraAlmoco { get; set; }

        [Required]
        [Display(Name = "Hora de Retorno do Almoço")]
        public DateTime HoraRetornoAlmoco { get; set; }

        [Required]
        [Display(Name = "Hora de Saída")]
        public DateTime HoraSaida { get; set; }

        [StringLength(200)]
        public string Observacao { get; set; }

        [Required]
        [Display(Name = "Total de Horas Trabalhadas")]
        public double TotalHorasTrabalhadas { get; set; }

        [Required]
        [Display(Name = "Horas Extras")]
        public double HorasExtras { get; set; }
    }

}
