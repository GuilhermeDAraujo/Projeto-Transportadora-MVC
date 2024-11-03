using OfficeOpenXml;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Interface
{
    public interface IImportarExcelService
    {
        List<NotaFiscal> ImportarNotasFiscais(IFormFile arquivoExcel);
        void SalvarNotasFiscais(List<NotaFiscal> notasFiscais);
    }
}