using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClientHotel.Models;

namespace WebClientHotel.Pages.List
{
    public class ListarClientesModel : PageModel
    {
        public List<Cliente> Clientes { get; set; }
        public void OnGet()
        {
            ClientesContext context = new();
            Clientes = context.Clientes.ToList();
        }
    }
}
