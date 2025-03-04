using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Dtos
{
        public class RegistroViewDto
    {
        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [DisplayName("Nome Completo")]
        public string NomeCompleto { get; set; }
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação não correspondem.")]
        public string ConfirmarSenha { get; set; }
        
        [Phone(ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }
    }
}