using KF_Lanches.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Trabalho_Lanches.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ConsumiveisAdminController : Controller
    {
        private readonly Contexto _context;

        public ConsumiveisAdminController(Contexto context)
        {
            _context = context;
        }

        // GET: Consumiveis
        public async Task<IActionResult> Index()
        {
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

        // GET: Consumiveis/Create
        public IActionResult Create()
        {
            ViewData["categoriaID"] = new SelectList(_context.categorias, "id", "descricao");
            return View();
        }

        // POST: Consumiveis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,categoriaID,descricao,composicao,valor")] Consumivel consumivel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consumivel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoriaID"] = new SelectList(_context.categorias, "id", "descricao", consumivel.categoriaID);
            return View(consumivel);
        }

        // GET: Consumiveis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.consumiveis == null)
            {
                return NotFound();
            }

            var consumivel = await _context.consumiveis.FindAsync(id);
            if (consumivel == null)
            {
                return NotFound();
            }
            ViewData["categoriaID"] = new SelectList(_context.categorias, "id", "descricao", consumivel.categoriaID);
            return View(consumivel);
        }

        // POST: Consumiveis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,categoriaID,descricao,composicao,valor")] Consumivel consumivel)
        {
            if (id != consumivel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consumivel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsumivelExists(consumivel.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoriaID"] = new SelectList(_context.categorias, "id", "descricao", consumivel.categoriaID);
            return View(consumivel);
        }

        // GET: Consumiveis/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Consumiveis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.consumiveis == null)
            {
                return Problem("Entity set 'Contexto.consumiveis'  is null.");
            }
            var consumivel = await _context.consumiveis.FindAsync(id);
            if (consumivel != null)
            {
                _context.consumiveis.Remove(consumivel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsumivelExists(int id)
        {
          return _context.consumiveis.Any(e => e.id == id);
        }
    }
}
