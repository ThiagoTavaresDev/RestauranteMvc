using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Dtos
{
    public class CategoriaViewDto
    {
    public int CategoriaId { get; set; }
    
    [Required(ErrorMessage = "Nome da categoria é obrigatório")]
    [DisplayName("Nome da Categoria")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "Descrição é obrigatório")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }
}

}