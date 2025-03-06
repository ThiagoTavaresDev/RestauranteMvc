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
using RestauranteMvc.Extensions;
using RestauranteMvc.Filters;
using RestauranteMvc.Models;

namespace RestauranteMvc.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(AdminAuthFilter))]
    public class PedidosController : Controller
{
    private readonly RestauranteContext _context;
    
    public PedidosController(RestauranteContext context)
    {
        _context = context;
    }
    [HttpGet]
    [Route("Carrinho")]
   // [Authorize]
    public IActionResult Carrinho()
    {
        return View();
    }
    
    [HttpPost]
    [Route("AdicionarAoCarrinho")]
    public IActionResult AdicionarAoCarrinho(int pratoId, int quantidade)
    {
        var prato = _context.Pratos.Find(pratoId);
        
        if (prato == null)
        {
            return Json(new { success = false, message = "Prato não encontrado" });
        }
        
        // Lógica para adicionar ao carrinho (usando Session)
        var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();
        
        var item = carrinho.FirstOrDefault(i => i.PratoId == pratoId);
        
        if (item != null)
        {
            item.Quantidade += quantidade;
        }
        else
        {
            carrinho.Add(new ItemCarrinho
            {
                PratoId = prato.PratoId,
                Nome = prato.Nome,
                Preco = prato.Preco,
                Quantidade = quantidade,
                Imagem = prato.CaminhoImagem
            });
        }
        
        HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
        
        return Json(new { success = true, message = "Produto adicionado ao carrinho", count = carrinho.Sum(i => i.Quantidade) });
    }
    
    [HttpPost]
    [Route("RemoverDoCarrinho")]
    public IActionResult RemoverDoCarrinho(int pratoId)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho");
        
        if (carrinho != null)
        {
            var item = carrinho.FirstOrDefault(i => i.PratoId == pratoId);
            
            if (item != null)
            {
                carrinho.Remove(item);
                HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
            }
        }
        
        return RedirectToAction(nameof(Carrinho));
    }
    

    [HttpGet]
    [Route("Checkout")]
  //  [Authorize]
    public IActionResult Checkout()
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho");
        
        if (carrinho == null || !carrinho.Any())
        {
            TempData["Erro"] = "Seu carrinho está vazio";
            return RedirectToAction(nameof(Carrinho));
        }
        
        int usuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;
        var enderecos = _context.Enderecos.Where(e => e.UsuarioId == usuarioId).ToList();
        
        var model = new CheckoutViewDto
        {
            Itens = carrinho,
            Enderecos = enderecos,
            NovoEndereco = new EnderecoViewDto()
        };
        
        return View(model);
    }
    
    [HttpPost]
    [Route("FinalizarPedido")]
    [ValidateAntiForgeryToken]
   // [Authorize]
    public async Task<IActionResult> FinalizarPedido(CheckoutViewDto model)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho");
        
        if (carrinho == null || !carrinho.Any())
        {
            TempData["Erro"] = "Seu carrinho está vazio";
            return RedirectToAction(nameof(Carrinho));
        }
        
        int usuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;
        int enderecoId;
        
        // Verifica se é para usar um endereço existente ou cadastrar um novo
        if (model.EnderecoId > 0)
        {
            enderecoId = model.EnderecoId;
        }
        else
        {
            // Cadastra novo endereço
            var endereco = new Endereco
            {
                UsuarioId = usuarioId,
                Logradouro = model.NovoEndereco.Logradouro,
                Numero = model.NovoEndereco.Numero,
                Complemento = model.NovoEndereco.Complemento,
                Bairro = model.NovoEndereco.Bairro,
                Cidade = model.NovoEndereco.Cidade,
                Estado = model.NovoEndereco.Estado,
                CEP = model.NovoEndereco.CEP,
                PontoReferencia = model.NovoEndereco.PontoReferencia
            };
            
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();
            
            enderecoId = endereco.EnderecoId;
        }
        
        // Cria o pedido
        var pedido = new Pedido
        {
            UsuarioId = usuarioId,
            EnderecoId = enderecoId,
            DataPedido = DateTime.Now,
            ValorTotal = carrinho.Sum(i => i.Preco * i.Quantidade),
            Status = StatusPedido.Recebido,
            Observacoes = model.Observacoes,
            ItensPedido = new List<ItemPedido>()
        };
        
        // Adiciona os itens do pedido
        foreach (var item in carrinho)
        {
            pedido.ItensPedido.Add(new ItemPedido
            {
                PratoId = item.PratoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.Preco
            });
        }
        
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        
        // Limpa o carrinho
        HttpContext.Session.Remove("Carrinho");
        
        TempData["Sucesso"] = "Pedido realizado com sucesso! Em breve entraremos em contato.";
        return RedirectToAction("Detalhes", new { id = pedido.PedidoId });
    }
    
    [HttpGet]
    [Route("Detalhes/{id}")]
   // [Authorize]
    public IActionResult Detalhes(int id)
    {
        var pedido = _context.Pedidos
            .Include(p => p.ItensPedido)
            .ThenInclude(i => i.Prato)
            .Include(p => p.Endereco)
            .FirstOrDefault(p => p.PedidoId == id);
        
        if (pedido == null)
        {
            return NotFound();
        }
        
        if (pedido.UsuarioId != HttpContext.Session.GetInt32("UsuarioId").Value && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        
        return View(pedido);
    }
    
    [HttpGet]
    [Route("MeusPedidos")]
   // [Authorize]
    public IActionResult MeusPedidos()
    {
        int usuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;
        var pedidos = _context.Pedidos.Where(p => p.UsuarioId == usuarioId).OrderByDescending(p => p.DataPedido).ToList();
        return View(pedidos);
    }
    
    //[Authorize(Roles = "Admin")]
    public IActionResult GerenciarPedidos()
    {
        var pedidos = _context.Pedidos
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.DataPedido)
            .ToList();
        
        return View(pedidos);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
   // [Authorize(Roles = "Admin")]
    public IActionResult AtualizarStatusPedido(int id, StatusPedido status)
    {
        var pedido = _context.Pedidos.Find(id);
        
        if (pedido == null)
        {
            return NotFound();
        }
        
        pedido.Status = status;
        _context.SaveChanges();
        
        TempData["Sucesso"] = "Status do pedido atualizado com sucesso!";
        return RedirectToAction(nameof(GerenciarPedidos));
    }

    [HttpPost]
    [Route("AtualizarQuantidade")]
    public IActionResult AtualizarQuantidade(int pratoId, string acao)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();
        
        var item = carrinho.FirstOrDefault(i => i.PratoId == pratoId);
        
        if (item != null)
        {
            if (acao == "increase")
            {
                item.Quantidade++;
            }
            else if (acao == "decrease" && item.Quantidade > 1)
            {
                item.Quantidade--;
            }
            
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
        }
        
        return RedirectToAction(nameof(Carrinho));
    }

    [HttpPost]
    [Route("LimparCarrinho")]
    public IActionResult LimparCarrinho()
    {
        HttpContext.Session.Remove("Carrinho");
        return RedirectToAction(nameof(Carrinho));
    }

}

}