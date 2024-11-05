using System.ComponentModel.DataAnnotations;

namespace Projeto_Transportadora_MVC.Models
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o número da Nota Fiscal")]
        public int NumeroNotaFiscal { get; set; }

        [Required(ErrorMessage = "Informe o nome do Cliente")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} deve conter entre {2} e {1} caracteres")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "Informe o endereço do Cliente")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} deve conter entre {2} e {1} caracteres")]
        public string EnderecoFaturado { get; set; }

        [Required(ErrorMessage = "Informe a data de faturamento")]
        [DataType(DataType.Date)]
        public DateTime? DataDoFaturamento { get; set; } //Atributo como nulo, permitindo que o modelo trate valores vazios corretamente.

        [Required(ErrorMessage = "Informe o número da carga da Nota Fiscal")]
        [Range(10, 999)]
        public int? NumeroDaCarga { get; set; } //Atributo como nulo, permitindo que o modelo trate valores vazios corretamente.


        public ICollection<AcaoNotaFiscal> Acoes { get; set; } = new List<AcaoNotaFiscal>();
    }
}