using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestauranteMvc.Dtos
{
    public class LoginViewDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        
        [DisplayName("Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}
