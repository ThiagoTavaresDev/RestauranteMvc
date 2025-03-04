using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RestauranteMvc.Dtos
{
    public class PratoViewDto
    {
        public int PratoId { get; set; }
        
        [Required(ErrorMessage = "Nome do prato é obrigatório")]
        [DisplayName("Nome")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [DisplayName("Descrição")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }
        
        [Required(ErrorMessage = "Preço é obrigatório")]
        [DisplayName("Preço")]
        [DataType(DataType.Currency)]
        [Range(0.01, 9999.99, ErrorMessage = "O preço deve estar entre R$ 0,01 e R$ 9.999,99")]
        public decimal Preco { get; set; }
        
        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Categoria é obrigatória")]
        public int CategoriaId { get; set; }
        
        [DisplayName("Imagem")]
        public IFormFile Imagem { get; set; }
        
        [DisplayName("Imagem Atual")]
        public string CaminhoImagemAtual { get; set; }
    }
}
