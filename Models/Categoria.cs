using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Models
{
   public class Categoria
    {
    [Key]
    public int CategoriaId { get; set; }
    
    [Required(ErrorMessage = "Nome da categoria é obrigatório")]
    [DisplayName("Nome da Categoria")]
    public string Nome { get; set; }
    
    [DisplayName("Descrição")]
    public string Descricao { get; set; }
    
    public virtual ICollection<Prato> Pratos { get; set; }
}

}