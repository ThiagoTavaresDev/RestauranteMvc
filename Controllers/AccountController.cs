using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestauranteMvc.Data;
using RestauranteMvc.Dtos;
using RestauranteMvc.Models;

namespace RestauranteMvc.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly RestauranteContext _context;
        
        public AccountController(RestauranteContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewDto model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == model.Email && u.Senha == model.Senha);
                
                if (usuario != null)
                {
                    // Autenticação simplificada (em produção, usar Identity)
                    HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
                    HttpContext.Session.SetString("NomeUsuario", usuario.NomeCompleto);
                    HttpContext.Session.SetString("TipoUsuario", usuario.IsAdmin ? "Admin" : "Cliente");
                    
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Email ou senha inválidos");
            }
            
            return View(model);
        }
        
        [HttpGet]
        [Route("Registro")]
        public IActionResult Registro()
        {
            return View();
        }
        
        [HttpPost]
        [Route("Registro")]
        public IActionResult Registro(RegistroViewDto model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Usuarios.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Este email já está sendo utilizado");
                    return View(model);
                }
                
                var usuario = new Usuario
                {
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    Senha = model.Senha, // Em produção, usar hash
                    Telefone = model.Telefone,
                    IsAdmin = false
                };
                
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                
                // Autenticação após registro
                HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
                HttpContext.Session.SetString("NomeUsuario", usuario.NomeCompleto);
                HttpContext.Session.SetString("TipoUsuario", "Cliente");
                
                return RedirectToAction("Index", "Home");
            }
            
            return View(model);
        }
        
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
