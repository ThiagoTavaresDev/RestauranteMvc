using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteMvc.Data;
using RestauranteMvc.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json; // Adicione esta referência

namespace RestauranteMvc.Controllers
{
    public class EstatisticasController : Controller
    {
        private readonly RestauranteContext _context;

        public EstatisticasController(RestauranteContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Verificar se existem dados para exibir
                var existemReservas = await _context.Reservas.AnyAsync();
                var existemPedidos = await _context.Pedidos.AnyAsync();

                if (!existemReservas && !existemPedidos)
                {
                    ViewBag.MensagemErro = "Não existem dados suficientes para gerar estatísticas.";
                    return View();
                }

                // 1. Dados para o gráfico de reservas por dia
                var reservasPorDia = await _context.Reservas
                    .GroupBy(r => r.Data.Date)
                    .Select(g => new { Data = g.Key, Quantidade = g.Count() })
                    .OrderBy(r => r.Data)
                    .Take(30)
                    .ToListAsync();

                // 2. Média de pessoas por reserva por dia
                var mediaPessoasPorDia = await _context.Reservas
                    .GroupBy(r => r.Data.Date)
                    .Select(g => new { Data = g.Key, Media = g.Average(r => r.NumeroPessoas) })
                    .OrderBy(r => r.Data)
                    .Take(30)
                    .ToListAsync();

                // 3. Quantidade de pedidos por categoria
                var pedidosPorCategoria = await _context.ItensPedido
                    .Include(i => i.Prato)
                    .ThenInclude(p => p.Categoria)
                    .GroupBy(i => i.Prato.Categoria.Nome)
                    .Select(g => new { Categoria = g.Key, Quantidade = g.Count() })
                    .OrderByDescending(pc => pc.Quantidade)
                    .ToListAsync();

                // 4. Pratos mais pedidos
                var pratosMaisPedidos = await _context.ItensPedido
                    .Include(i => i.Prato)
                    .GroupBy(i => i.Prato.Nome)
                    .Select(g => new { Prato = g.Key, Quantidade = g.Sum(i => i.Quantidade) })
                    .OrderByDescending(p => p.Quantidade)
                    .Take(10)
                    .ToListAsync();

                // Preparar dados para JSON usando System.Text.Json.JsonSerializer
                ViewBag.DatasReservas = JsonSerializer.Serialize(reservasPorDia.Select(r => r.Data.ToString("dd/MM")).ToList());
                ViewBag.QuantidadesReservas = JsonSerializer.Serialize(reservasPorDia.Select(r => r.Quantidade).ToList());
                
                ViewBag.DatasConvidados = JsonSerializer.Serialize(mediaPessoasPorDia.Select(r => r.Data.ToString("dd/MM")).ToList());
                ViewBag.MediasConvidados = JsonSerializer.Serialize(mediaPessoasPorDia.Select(r => r.Media).ToList());
                
                ViewBag.NomesCategorias = JsonSerializer.Serialize(pedidosPorCategoria.Select(r => r.Categoria).ToList());
                ViewBag.QuantidadesPorCategoria = JsonSerializer.Serialize(pedidosPorCategoria.Select(r => r.Quantidade).ToList());
                
                ViewBag.NomesPratos = JsonSerializer.Serialize(pratosMaisPedidos.Select(r => r.Prato).ToList());
                ViewBag.QuantidadesPratos = JsonSerializer.Serialize(pratosMaisPedidos.Select(r => r.Quantidade).ToList());

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = $"Erro ao gerar estatísticas: {ex.Message}";
                return View();
            }
        }
    }
}
