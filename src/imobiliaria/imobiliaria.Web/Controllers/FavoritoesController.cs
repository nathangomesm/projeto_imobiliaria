﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using imobiliaria.Web;

namespace imobiliaria.Web.Controllers
{
    public class FavoritoesController : Controller
    {
        private readonly ImobiliariaDbContext _context;

        public FavoritoesController(ImobiliariaDbContext context)
        {
            _context = context;
        }

        // GET: Favoritoes
        public async Task<IActionResult> Index()
        {
            var imobiliariaDbContext = _context.Favoritos.Include(f => f.Cliente).Include(f => f.Imovel);
            return View(await imobiliariaDbContext.ToListAsync());
        }

        // GET: Favoritoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorito = await _context.Favoritos
                .Include(f => f.Cliente)
                .Include(f => f.Imovel)
                .FirstOrDefaultAsync(m => m.FavoritoId == id);
            if (favorito == null)
            {
                return NotFound();
            }

            return View(favorito);
        }

        // GET: Favoritoes/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Cpf");
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "Endereco");
            return View();
        }

        // POST: Favoritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FavoritoId,ClienteId,ImovelId,DataAdicionado")] Favorito favorito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Cpf", favorito.ClienteId);
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "Endereco", favorito.ImovelId);
            return View(favorito);
        }

        // GET: Favoritoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorito = await _context.Favoritos.FindAsync(id);
            if (favorito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Cpf", favorito.ClienteId);
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "Endereco", favorito.ImovelId);
            return View(favorito);
        }

        // POST: Favoritoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FavoritoId,ClienteId,ImovelId,DataAdicionado")] Favorito favorito)
        {
            if (id != favorito.FavoritoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoritoExists(favorito.FavoritoId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Cpf", favorito.ClienteId);
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "Endereco", favorito.ImovelId);
            return View(favorito);
        }

        // GET: Favoritoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorito = await _context.Favoritos
                .Include(f => f.Cliente)
                .Include(f => f.Imovel)
                .FirstOrDefaultAsync(m => m.FavoritoId == id);
            if (favorito == null)
            {
                return NotFound();
            }

            return View(favorito);
        }

        // POST: Favoritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorito = await _context.Favoritos.FindAsync(id);
            if (favorito != null)
            {
                _context.Favoritos.Remove(favorito);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoritoExists(int id)
        {
            return _context.Favoritos.Any(e => e.FavoritoId == id);
        }
    }
}
