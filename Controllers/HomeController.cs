using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteMvc.Data;
using RestauranteMvc.Models;
using RestauranteMvc.ViewModels;
using System.Linq;

namespace RestauranteMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestauranteContext _context;
        
        public HomeController(RestauranteContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var categorias = _context.Categorias.Include(c => c.Pratos).ToList();
            
            // Obter pratos em destaque (por exemplo, os 6 mais recentes)
            var destaquesPratos = _context.Pratos
                .Include(p => p.Categoria)
                .OrderByDescending(p => p.PratoId)
                .Take(6)
                .ToList();
            
            var viewModel = new HomeViewModel
            {
                Categorias = categorias,
                DestaquesPratos = destaquesPratos
            };
            
            return View(viewModel);
        }
        
        public IActionResult Cardapio()
        {
            var categorias = _context.Categorias.Include(c => c.Pratos).ToList();
            return View(categorias);
        }
        
        public IActionResult Sobre()
        {
            return View();
        }
        
        public IActionResult Contato()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
