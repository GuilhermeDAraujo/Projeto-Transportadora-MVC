using Microsoft.AspNetCore.Mvc;
using Projeto_Transportadora_MVC.Services;

namespace Projeto_Transportadora_MVC.Controllers
{
    public class ImportarExcelController : Controller
    {
        private readonly ImportarExcelService _importarExcelService;


        public ImportarExcelController(ImportarExcelService importarExcelService)
        {
            _importarExcelService = importarExcelService;
        }

        public IActionResult CreateImportarExcel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImportarExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Selecione um arquivo válido");
                return View();
            }
            try
            {
                using (var stream = excelFile.OpenReadStream())
                {
                    var notasFiscais = await _importarExcelService.ImportarNotasFiscais(stream); 
                    ViewBag.Message = $"{notasFiscais.Count} Notas fiscais importadas com sucesso!"; 
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao importar: {ex.Message}");
                return View();
            }
        }
    }
}