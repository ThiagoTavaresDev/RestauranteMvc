using System.Collections.Generic;
using RestauranteMvc.Models;

namespace RestauranteMvc.ViewModels
{
    public class HomeViewModel
    {
        public List<Categoria> Categorias { get; set; }
        public List<Prato> DestaquesPratos { get; set; }
    }
}
