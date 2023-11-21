using KF_Lanches.Models;
using Microsoft.AspNetCore.Mvc;

namespace Trabalho_Lanches.Controllers
{
    public class PesquisaController : Controller
    {
        private readonly Contexto _context;
        public PesquisaController(Contexto context)
        {
            _context = context;
        }

        public IActionResult Buscar(string filtro)
        {
            List<Consumivel> lista = new List<Consumivel>();

            if(filtro == null)
            {
                lista = _context.consumiveis
                                .OrderBy(c => c.descricao)
                                .OrderBy(c => c.valor)
                                .ToList();
            }
            else
            {
                lista = _context.consumiveis
                                .Where(c => c.descricao.Contains(filtro))
                                .OrderBy(c => c.descricao)
                                .OrderBy(c => c.valor)
                                .ToList();
            }

            if(lista != null)
            {
                return View("~/Views/Consumiveis/Index.cshtml", lista);
            } else return View();
        }
    }
}
