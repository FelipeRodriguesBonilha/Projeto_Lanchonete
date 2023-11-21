//using KF_Lanches.Data;
using KF_Lanches.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace KF_Lanches.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly Contexto _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarrinhoController(Contexto context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var logadoID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //coleta id do cliente logado.

            int itemCount = _context.itensPedidos.Where(i => i.cliID == logadoID && i.virouPedido == false).Count(); //conta quantos itens tem no carrinho
            ViewBag.CartItemCount = itemCount; // joga pra view

            var contexto = _context.itensPedidos.Include(i => i.consumivel).Where(i => i.cliID == logadoID && i.virouPedido == false); // mostrar carrinho
            return View(await contexto.ToListAsync());
        }

        [Authorize]
        public IActionResult AdicionarAoCarrinho(int id)
        {
            var logadoID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //coleta id do cliente logado.

            var consumivel = _context.consumiveis.FirstOrDefault(x => x.id == id); //busca o item que vai ser adicionado ao carrinho.

            if (consumivel != null)
            {
                var itens = _context.itensPedidos.FirstOrDefault(x => x.consumivel.id == consumivel.id && x.cliID == logadoID && x.virouPedido == false); //busca os itens do carrinho do usuário.

                if (itens == null) //se não tiver o item no carrinho do usuário logado, adiciona.
                {
                    itens = new ItemPedido()
                    {
                        consumivel = consumivel,
                        quantidade = 1,
                        cliID = logadoID,
                        virouPedido = false
                    };
                    _context.itensPedidos.Add(itens);
                }
                else //se tiver o item no carrinho do usuário logado apenas incrementa 1 na quantidade.
                {
                    itens.quantidade++;
                }
                _context.SaveChanges(); //salva as alterações no Banco de Dados
            }
            return RedirectToAction("Index", "Carrinho");
        }

        public IActionResult RemoverDoCarrinho(int id)
        {
            var logadoID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //coleta id cliente logado

            var itemParaRemover = _context.itensPedidos.FirstOrDefault(x => x.id == id); // busca item do carrinho a partir da linha que foi apertado o botão de remover

            if (itemParaRemover != null) // se a itemParaRemover conter algum item, faz o que está abaixo
            {
                if (itemParaRemover.quantidade > 1) // se a quantidade for maior que 1 diminui a quantidade do item
                {
                    itemParaRemover.quantidade--;
                }
                else // senão remove o item
                {
                    _context.itensPedidos.Remove(itemParaRemover);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Carrinho");
        }

        public IActionResult LimparCarrinho()
        {
            var logadoID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; // coleta id do cliente logado

            var itensNoCarrinho = _context.itensPedidos.Where(x => x.cliID == logadoID && x.virouPedido == false); //lista de tudo que tem no carrinho do cliente logado
            _context.itensPedidos.RemoveRange(itensNoCarrinho); //remove tudo que está contido na lista itensNoCarrinho
            _context.SaveChanges(); // Salva as alterações no banco

            return RedirectToAction("Index", "Carrinho"); //Recarrega a view
        }
    }
}
