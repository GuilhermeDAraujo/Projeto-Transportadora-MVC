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


        public NotaFiscalController(INotaFiscalService notaFiscalServices)
        {
            _notaFiscalServices = notaFiscalServices;
        }

        public IActionResult Menu()
        {
            return View();
        }

        public async Task<IActionResult> CreateNotaFiscal()
        {
            await CarregarViewBag();
            await CarregarViewBagCaminhao();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNotaFiscal(NotaFiscal notaFiscal)
        {
            if (!ModelState.IsValid)
            {
                await CarregarViewBag();
                await CarregarViewBagCaminhao();
                return View(notaFiscal);
            }

            await _notaFiscalServices.CreateNotaFiscalAsync(notaFiscal);
            return RedirectToAction(nameof(CreateNotaFiscal));
        }

        public async Task<IActionResult> UpdateNotaFiscal(int id)
        {
            var notaFiscalBanco = await _notaFiscalServices.BuscarNotaFiscalPorId(id);
            if (notaFiscalBanco == null)
            {
                ModelState.AddModelError("", "Nota Fiscal não existe");
                return RedirectToAction(nameof(CreateNotaFiscal));
            }
            return View(notaFiscalBanco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNotaFiscal(NotaFiscal notaFiscal)
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
                return View(notaFiscal);
            }

            await _notaFiscalServices.UpdateNotaFiscalAsync(notaFiscal);
            return RedirectToAction(nameof(CreateNotaFiscal));
        }

        public async Task<IActionResult> DeleteNotaFiscal(int id)
        {
            var notaFiscalBanco = await _notaFiscalServices.BuscarNotaFiscalPorId(id);
            if(notaFiscalBanco == null)
            {
                ModelState.AddModelError("", "Nota Fiscal não encotrada");
                return RedirectToAction(nameof(CreateNotaFiscal));
            }

            return View(notaFiscalBanco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarDelete(int id)
        {
            var notaFiscalBanco =  await _notaFiscalServices.BuscarNotaFiscalPorId(id);
            if(notaFiscalBanco == null)
            {
                ModelState.AddModelError("", "Nota Fiscal não encontrada");
                return RedirectToAction(nameof(CreateNotaFiscal));
            }
            await _notaFiscalServices.DeleteNotaFiscalAsync(notaFiscalBanco);
            return RedirectToAction(nameof(CreateNotaFiscal));
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