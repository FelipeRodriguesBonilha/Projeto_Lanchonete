using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KF_Lanches.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Trabalho_Lanches.Controllers
{
    public class PedidosController : Controller
    {
        private readonly Contexto _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PedidosController(Contexto context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Pedidos
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            //var grpData =
            //    from linha in await _context.pedidos.ToListAsync()
            //    group linha by new { linha.DataPedido, linha.cpf }
            //    into grupo
            //    orderby grupo.Key.DataPedido
            //    select new PedidoGrpData
            //    {
            //        data = linha.DataPedido;
            //        cpf = linha.cpf;
            //        endereco = linha.endereco;
            //        telefone = linha.telefone;
            //    };
            return View(await _context.pedidos.ToListAsync());
        }

        // GET: Pedidos/Details/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.itensPedidos == null)
            {
                return NotFound();
            }

            var pedido = _context.itensPedidos.Include(i => i.consumivel).Where(i => i.pedID == id).ToList(); // retorna todos os itensPedidos com pedID igual ao id passado pelo foreach botão

            if (pedido == null)
            {
                return NotFound();
            }

            ViewBag.ID = id;

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("id,clienteID,DataPedido,nome,cpf,endereco,telefone")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                var logadoID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; // Coleta ID do cliente logado.

                _context.Add(pedido);
                await _context.SaveChangesAsync(); // Salva o pedido para obter o ID gerado pelo banco de dados

                var busca = _context.itensPedidos.Where(x => x.cliID == logadoID && x.virouPedido == false); // Busca os itens do pedido do cliente logado

                foreach (var i in busca)
                {
                    i.pedID = pedido.id; // Define o atributo pedidoID de todos os itens para o ID do pedido criado
                    i.virouPedido = true;
                }

                await _context.SaveChangesAsync(); // Salva as alterações nos itens com os IDs do pedido atualizados

                var contexto = _context.itensPedidos.Include(i => i.consumivel).Where(i => i.pedID == pedido.id && i.virouPedido == true); // Lista os itens do pedido do usuário logado para mostrar na view de pedido realizado
                return View("~/Views/PedidoRealizado/Index.cshtml", await contexto.ToListAsync());
            }
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.pedidos
                .FirstOrDefaultAsync(m => m.id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [Authorize(Roles = "ADMIN")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.pedidos == null)
            {
                return Problem("Entity set 'Contexto.pedidos'  is null.");
            }
            var pedido = await _context.pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.pedidos.Remove(pedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
          return _context.pedidos.Any(e => e.id == id);
        }
    }
}
