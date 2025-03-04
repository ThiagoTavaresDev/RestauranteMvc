using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Models
{
   public class Pedido
{
    [Key]
    public int PedidoId { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
    public virtual Usuario Usuario { get; set; }
    
    [Required]
    public int EnderecoId { get; set; }
    
    public virtual Endereco Endereco { get; set; }
    
    [Required]
    [DisplayName("Data do Pedido")]
    public DateTime DataPedido { get; set; }
    
    [Required]
    [DisplayName("Valor Total")]
    [DataType(DataType.Currency)]
    public decimal ValorTotal { get; set; }
    
    [DisplayName("Status")]
    public StatusPedido Status { get; set; }
    
    public virtual ICollection<ItemPedido> ItensPedido { get; set; }
    
    [DisplayName("Observações")]
    public string Observacoes { get; set; }
}

public class ItemPedido
{
    [Key]
    public int ItemPedidoId { get; set; }
    
    [Required]
    public int PedidoId { get; set; }
    
    [Required]
    public int PratoId { get; set; }
    
    public virtual Prato Prato { get; set; }
    
    [Required]
    [Range(1, 20, ErrorMessage = "Quantidade deve estar entre 1 e 20")]
    public int Quantidade { get; set; }
    
    [Required]
    [DataType(DataType.Currency)]
    public decimal PrecoUnitario { get; set; }
}

public enum StatusPedido
{
    Recebido,
    EmPreparo,
    SaiuParaEntrega,
    Entregue,
    Cancelado
}

}