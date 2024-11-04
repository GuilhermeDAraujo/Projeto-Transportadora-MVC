using Microsoft.AspNetCore.Mvc;
using Projeto_Transportadora_MVC.Services;

namespace Projeto_Transportadora_MVC.Controllers
{
    public class ImportarExcelController : Controller
    {
        private readonly IImportarExcelService _importarExcelService;


        public ImportarExcelController(IImportarExcelService importarExcelService)
        {
            _importarExcelService = importarExcelService;
        }

        public IActionResult CreateImportarExcel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateImportarExcel(IFormFile excelFile)
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
                    var nostasFiscais = _importarExcelService.ImportarNotasFiscais(stream);
                    ViewBag.Message = $"{nostasFiscais.Count} Notas fiscais importadas com sucesso!";
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