using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Models
{
    public class Endereco
{
    [Key]
    public int EnderecoId { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
    [Required(ErrorMessage = "Logradouro é obrigatório")]
    public string Logradouro { get; set; }
    
    [Required(ErrorMessage = "Número é obrigatório")]
    public string Numero { get; set; }
    
    public string Complemento { get; set; }
    
    [Required(ErrorMessage = "Bairro é obrigatório")]
    public string Bairro { get; set; }
    
    [Required(ErrorMessage = "Cidade é obrigatória")]
    public string Cidade { get; set; }
    
    [Required(ErrorMessage = "Estado é obrigatório")]
    public string Estado { get; set; }
    
    [Required(ErrorMessage = "CEP é obrigatório")]
    [DisplayName("CEP")]
    public string CEP { get; set; }
    
    [DisplayName("Referência")]
    public string PontoReferencia { get; set; }
}

}