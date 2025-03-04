using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Models
{
   public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }
    
    [Required(ErrorMessage = "Nome completo é obrigatório")]
    [DisplayName("Nome Completo")]
    public string NomeCompleto { get; set; }
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Senha é obrigatória")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }
    
    [Phone(ErrorMessage = "Telefone inválido")]
    public string Telefone { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public virtual ICollection<Reserva> Reservas { get; set; }
    public virtual ICollection<Pedido> Pedidos { get; set; }
    public virtual ICollection<Endereco> Enderecos { get; set; }
}

}