using System.ComponentModel.DataAnnotations;

namespace Projeto_Transportadora_MVC.Models
{
    public class Fechamento
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe a data de envio do fechamento da Nota Fiscal")]
        [DataType(DataType.Date)]
        public DateTime DataDoFechamento { get; set; }

        public int NotaFiscalId { get; set; }
        public NotaFiscal NotaFiscal { get; set; }
    }
}