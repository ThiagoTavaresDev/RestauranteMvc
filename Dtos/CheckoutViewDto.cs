using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RestauranteMvc.Models;

namespace RestauranteMvc.Dtos
{
    public class CheckoutViewDto
    {
        public List<ItemCarrinho> Itens { get; set; }
        
        public List<Endereco> Enderecos { get; set; }
        
        [Display(Name = "Endereço de Entrega")]
        public int EnderecoId { get; set; }
        
        public EnderecoViewDto NovoEndereco { get; set; }
        
        [Display(Name = "Forma de Pagamento")]
        [Required(ErrorMessage = "Selecione uma forma de pagamento")]
        public string FormaPagamento { get; set; }
        
        [Display(Name = "Observações")]
        public string Observacoes { get; set; }
        
        public decimal ValorTotal => Itens?.Sum(i => i.SubTotal) ?? 0;
    }
}
