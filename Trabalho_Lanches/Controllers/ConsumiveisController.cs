using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KF_Lanches.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Trabalho_Lanches.Controllers
{
    public class ConsumiveisController : Controller
    {
        private readonly Contexto _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConsumiveisController(Contexto context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Consumiveis
        public async Task<IActionResult> Index()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var logadoID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //coleta id do cliente logado.
                int itemCount = _context.itensPedidos.Where(i => i.cliID == logadoID && i.virouPedido == false).Count();
                ViewBag.CartItemCount = itemCount;
            }

            var contexto = _context.consumiveis.Include(c => c.categoria);
            return View(await contexto.ToListAsync());
        }

        // GET: Consumiveis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.consumiveis == null)
            {
                return NotFound();
            }

            var consumivel = await _context.consumiveis
                .Include(c => c.categoria)
                .FirstOrDefaultAsync(m => m.id == id);
            if (consumivel == null)
            {
                return NotFound();
            }

            return View(consumivel);
        }
    }
}