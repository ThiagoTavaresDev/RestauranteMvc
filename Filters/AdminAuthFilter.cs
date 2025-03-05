using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestauranteMvc.Filters
{
    public class AdminAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tipoUsuario = context.HttpContext.Session.GetString("TipoUsuario");
            
            if (string.IsNullOrEmpty(tipoUsuario) || tipoUsuario != "Admin")
            {
                // Redireciona para login se não estiver autenticado
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
