using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Models;
using Projeto_Transportadora_MVC.Services;

namespace Projeto_Transportadora_MVC.Controllers
{
    public class NotaFiscalController : Controller
    {
        private readonly INotaFiscalService _notaFiscalServices;
        private readonly TransportadoraContext _context;

        public NotaFiscalController(INotaFiscalService notaFiscalServices, TransportadoraContext context)
        {
            _notaFiscalServices = notaFiscalServices;
            _context = context;
        }

        public IActionResult Menu()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            await CarregarViewBag();
            await CarregarViewBagCaminhao();
            return View("~/Views/NotaFiscal/EntradaManual/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotaFiscal notaFiscal)
        {
            if (!ModelState.IsValid)
            {
                await CarregarViewBag();
                await CarregarViewBagCaminhao();
                return View("~/Views/NotaFiscal/EntradaManual/Create.cshtml", notaFiscal);
            }

            await _notaFiscalServices.CreateNotaFiscalAsync(notaFiscal);
            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Update(int id)
        {
            var notaFiscalBanco = await _notaFiscalServices.BuscarNotaFiscalPorId(id);
            if (notaFiscalBanco == null)
            {
                ModelState.AddModelError("", "Nota Fiscal não existe");
                return RedirectToAction(nameof(Create));
            }
            return View("~/Views/NotaFiscal/EntradaManual/Update.cshtml", notaFiscalBanco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(NotaFiscal notaFiscal)
        {
            if (!ModelState.IsValid)
            {
                await CarregarViewBagCaminhao();
                return View(notaFiscal);
            }

            if (await _notaFiscalServices.NotaFiscalJaExisteAsync(notaFiscal.Id, notaFiscal.NumeroNotaFiscal))
            {
                ModelState.AddModelError("", "Nota Fiscal já existe");
                await CarregarViewBagCaminhao();
                return View("~/Views/NotaFiscal/EntradaManual/Update.cshtml", notaFiscal);
            }

            await _notaFiscalServices.UpdateNotaFiscalAsync(notaFiscal);
            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var notaFiscalBanco = await _notaFiscalServices.BuscarNotaFiscalPorId(id);
            if(notaFiscalBanco == null)
            {
                ModelState.AddModelError("", "Nota Fiscal não encotrada");
                return RedirectToAction(nameof(Create));
            }

            return View("~/Views/NotaFiscal/EntradaManual/Delete.cshtml", notaFiscalBanco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarDelete(int id)
        {
            var notaFiscalBanco =  await _notaFiscalServices.BuscarNotaFiscalPorId(id);
            if(notaFiscalBanco == null)
            {
                ModelState.AddModelError("", "Nota Fiscal não encontrada");
                return RedirectToAction(nameof(Create));
            }
            await _notaFiscalServices.DeleteNotaFiscalAsync(notaFiscalBanco);
            return RedirectToAction(nameof(Create));
        }


        public IActionResult ImportarNotas()
        {
            return View("~/Views/NotaFiscal/ImportarExcel/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportarNotas(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Selecione um arquivo válido");
                return View("~/Views/NotaFiscal/ImportarExcel/Create.cshtml");
            }
            try
            {
                using (var stream = excelFile.OpenReadStream())
                {
                    var nostasFiscais = _notaFiscalServices.ImportarNotasFiscais(stream);

                    _context.NotasFiscais.AddRange(nostasFiscais);
                    _context.SaveChanges();
                }
                ViewBag.Message = "Notas fiscais importadas com sucesso!";
                return View("~/Views/NotaFiscal/ImportarExcel/Create.cshtml");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao importar: {ex.Message}");
                return View("~/Views/NotaFiscal/ImportarExcel/Create.cshtml");
            }
        }


        public async Task CarregarViewBagCaminhao()
        {
            ViewBag.Caminhao = new SelectList(await _notaFiscalServices.ListarTodosCaminhoesAsync(), "id", "Placa");
        }

        public async Task CarregarViewBag()
        {
            ViewBag.NotasFiscais = await _notaFiscalServices.ObjerNotasFiscaisDeHojeAsync();
        }
    }
}