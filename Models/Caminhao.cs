using System.ComponentModel.DataAnnotations;

namespace Projeto_Transportadora_MVC.Models
{
    public class Caminhao
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe a placa do Caminhão")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "{0} deve conter {1} caracteres")]
        public string Placa { get; set; }
        public decimal? CustoCombustivel { get; set; }
        public decimal? CustoManutencao { get; set; }

    }
}