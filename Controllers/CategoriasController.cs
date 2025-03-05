using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestauranteMvc.Data;
using RestauranteMvc.Dtos;
using RestauranteMvc.Filters;
using RestauranteMvc.Models;

namespace RestauranteMvc.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(AdminAuthFilter))]
    public class CategoriasController : Controller
    {
        private readonly RestauranteContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        public CategoriasController(RestauranteContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [Route("")]
        public IActionResult Index()
        {
            var categorias = _context.Categorias.Include(c => c.Pratos).ToList();
            return View(categorias);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewDto model)
        {
            if (ModelState.IsValid){
                var categoria = new Categoria
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao
                };

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();


                TempData["Sucesso"] = "Categoria cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));  
            }
                return View(model);
            }
        
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var categoria = _context.Categorias.Find(id);
            
            if (categoria == null)
            {
                return NotFound();
            }
            
            var model = new CategoriaViewDto
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
            };

            return View(model);
        }
        
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoriaViewDto model)
        {
            if (ModelState.IsValid)
            {
                var categoria = await _context.Categorias.FindAsync(model.CategoriaId);
                
                if (categoria == null)
                {
                    return NotFound();
                }
                    categoria.Nome = model.Nome;
                    categoria.Descricao = model.Descricao;

                    _context.Update(categoria);
                    await _context.SaveChangesAsync();

                    TempData["Sucesso"] = "Categoria atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var categoria = _context.Categorias.Include(p => p.Pratos).FirstOrDefault(p => p.CategoriaId == id);
            
            if (categoria == null)
            {
                return NotFound();
            }
            
            return View(categoria);
        }
        
        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            
            if (categoria == null)
            {
                return NotFound();
            }
            
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            
            TempData["Sucesso"] = "Categoria excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        }
}
    
