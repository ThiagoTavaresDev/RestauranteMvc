using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Models
{
   public class Prato
{
    [Key]
    public int PratoId { get; set; }
    
    [Required(ErrorMessage = "Nome do prato é obrigatório")]
    [DisplayName("Nome")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [DisplayName("Preço")]
    [DataType(DataType.Currency)]
    public decimal Preco { get; set; }
    
    [DisplayName("Imagem")]
    public string CaminhoImagem { get; set; }
    
    [Required(ErrorMessage = "Categoria é obrigatória")]
    public int CategoriaId { get; set; }
    
    public virtual Categoria Categoria { get; set; }
}

}