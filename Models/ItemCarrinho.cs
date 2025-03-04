using System;

namespace RestauranteMvc.Models
{
    public class ItemCarrinho
    {
        public int PratoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string Imagem { get; set; }
        
        public decimal SubTotal => Preco * Quantidade;
    }
}
