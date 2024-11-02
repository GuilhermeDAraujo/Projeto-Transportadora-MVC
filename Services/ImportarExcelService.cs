using OfficeOpenXml;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Services
{
    public class IImportarExcelService
    {
        private readonly TransportadoraContext _context;

        public IImportarExcelService(TransportadoraContext context)
        {
            _context = context;
        }

        
        public List<NotaFiscal> ImportarNotasFiscais(Stream arquivoExcelStream)
        {
            List<NotaFiscal> notasFiscais = new List<NotaFiscal>();

            using (var package = new ExcelPackage(arquivoExcelStream))
            {
                var planilha = package.Workbook.Worksheets[0];

                for(int linha = 2; linha <= planilha.Dimension.End.Row; linha++)
                {
                    NotaFiscal notaFiscal = new NotaFiscal
                    {
                        NumeroNotaFiscal = int.Parse(planilha.Cells[linha, 1].Text),
                        NomeCliente = planilha.Cells[linha, 2].Text,
                        EnderecoFaturado = planilha.Cells[linha,3].Text,
                        DataDoFaturamento = DateTime.Parse(planilha.Cells[linha,4].Text),
                        DataDaEntrada = DateTime.Parse(planilha.Cells[linha,5].Text),
                        NumeroDaCarga = int.Parse(planilha.Cells[linha,6].Text)
                    };

                    notasFiscais.Add(notaFiscal);
                }   
            }
            return notasFiscais;
        }
    }
}