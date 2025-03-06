using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ReservasController : Controller
    {
        private readonly RestauranteContext _context;
        
        public ReservasController(RestauranteContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            int usuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;
            var reservas = _context.Reservas.Where(r => r.UsuarioId == usuarioId).ToList();
            return View(reservas);
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
        public async Task<IActionResult> Create(ReservaViewDto model)
        {
            if (ModelState.IsValid)
            {
                var reserva = new Reserva
                {
                    UsuarioId = HttpContext.Session.GetInt32("UsuarioId").Value,
                    Data = model.Data,
                    Horario = model.Horario,
                    NumeroPessoas = model.NumeroPessoas,
                    Observacoes = model.Observacoes,
                    Status = StatusReserva.Pendente
                };
                
                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();
                
                TempData["Sucesso"] = "Reserva solicitada com sucesso! Aguarde confirmação.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }
        
        [HttpGet]
        [Route("Cancel/{id}")]
       // [Authorize]
        public IActionResult Cancel(int id)
        {
            var reserva = _context.Reservas.Find(id);
            
            if (reserva == null)
            {
                return NotFound();
            }
            
            if (reserva.UsuarioId != HttpContext.Session.GetInt32("UsuarioId").Value)
            {
                return Forbid();
            }
            
            reserva.Status = StatusReserva.Cancelada;
            _context.SaveChanges();
            
            TempData["Sucesso"] = "Reserva cancelada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        [Route("Admin")]
      //  [Authorize(Roles = "Admin")]
        public IActionResult AdminList()
        {
            var reservas = _context.Reservas.Include(r => r.Usuario).ToList();
            return View(reservas);
        }
        
        [HttpPost]
        [Route("UpdateStatus")]
        [ValidateAntiForgeryToken]
      //  [Authorize(Roles = "Admin")]
        public IActionResult UpdateStatus(int id, StatusReserva status)
        {
            var reserva = _context.Reservas.Find(id);
            
            if (reserva == null)
            {
                return NotFound();
            }
            
            reserva.Status = status;
            _context.SaveChanges();
            
            TempData["Sucesso"] = "Status da reserva atualizado com sucesso!";
            return RedirectToAction(nameof(AdminList));
        }
    }
}
