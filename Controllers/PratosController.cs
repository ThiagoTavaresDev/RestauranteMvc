using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestauranteMvc.Data;
using RestauranteMvc.Dtos;
using RestauranteMvc.Models;

namespace RestauranteMvc.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
public class PratosController : Controller
{
    private readonly RestauranteContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public PratosController(RestauranteContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }
    
    public IActionResult Index()
    {
        var pratos = _context.Pratos.Include(p => p.Categoria).ToList();
        return View(pratos);
    }
    
    public IActionResult Create()
    {
        ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nome");
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PratoViewDto model)
    {
        if (ModelState.IsValid)
        {
            string caminhoImagem = null;
            
            if (model.Imagem != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "pratos");
                string nomeUnico = Guid.NewGuid().ToString() + "_" + model.Imagem.FileName;
                string filePath = Path.Combine(uploadsFolder, nomeUnico);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Imagem.CopyToAsync(fileStream);
                }
                
                caminhoImagem = "/images/pratos/" + nomeUnico;
            }
            
            var prato = new Prato
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                Preco = model.Preco,
                CategoriaId = model.CategoriaId,
                CaminhoImagem = caminhoImagem
            };
            
            _context.Pratos.Add(prato);
            await _context.SaveChangesAsync();
            
            TempData["Sucesso"] = "Prato cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nome", model.CategoriaId);
        return View(model);
    }
    
    public IActionResult Edit(int id)
    {
        var prato = _context.Pratos.Find(id);
        
        if (prato == null)
        {
            return NotFound();
        }
        
        var model = new PratoViewDto
        {
            PratoId = prato.PratoId,
            Nome = prato.Nome,
            Descricao = prato.Descricao,
            Preco = prato.Preco,
            CategoriaId = prato.CategoriaId,
            CaminhoImagemAtual = prato.CaminhoImagem
        };
        
        ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nome", prato.CategoriaId);
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PratoViewDto model)
    {
        if (ModelState.IsValid)
        {
            var prato = await _context.Pratos.FindAsync(model.PratoId);
            
            if (prato == null)
            {
                return NotFound();
            }
            
            prato.Nome = model.Nome;
            prato.Descricao = model.Descricao;
            prato.Preco = model.Preco;
            prato.CategoriaId = model.CategoriaId;
            
            if (model.Imagem != null)
            {
                // Se tem imagem anterior, exclui
                if (!string.IsNullOrEmpty(prato.CaminhoImagem))
                {
                    var imagemAntiga = Path.Combine(_hostEnvironment.WebRootPath, prato.CaminhoImagem.TrimStart('/'));
                    if (System.IO.File.Exists(imagemAntiga))
                    {
                        System.IO.File.Delete(imagemAntiga);
                    }
                }
                
                // Upload da nova imagem
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "pratos");
                string nomeUnico = Guid.NewGuid().ToString() + "_" + model.Imagem.FileName;
                string filePath = Path.Combine(uploadsFolder, nomeUnico);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Imagem.CopyToAsync(fileStream);
                }
                
                prato.CaminhoImagem = "/images/pratos/" + nomeUnico;
            }
            
            _context.Update(prato);
            await _context.SaveChangesAsync();
            
            TempData["Sucesso"] = "Prato atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nome", model.CategoriaId);
        return View(model);
    }
    
    public IActionResult Delete(int id)
    {
        var prato = _context.Pratos.Include(p => p.Categoria).FirstOrDefault(p => p.PratoId == id);
        
        if (prato == null)
        {
            return NotFound();
        }
        
        return View(prato);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var prato = await _context.Pratos.FindAsync(id);
        
        if (prato == null)
        {
            return NotFound();
        }
        
        // Excluir imagem do servidor
        if (!string.IsNullOrEmpty(prato.CaminhoImagem))
        {
            var imagemPath = Path.Combine(_hostEnvironment.WebRootPath, prato.CaminhoImagem.TrimStart('/'));
            if (System.IO.File.Exists(imagemPath))
            {
                System.IO.File.Delete(imagemPath);
            }
        }
        
        _context.Pratos.Remove(prato);
        await _context.SaveChangesAsync();
        
        TempData["Sucesso"] = "Prato excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}

}