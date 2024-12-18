using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Grpc.Net.Client;
using WebClientHotel;

namespace WebClientHotel.Pages.Add
{
    public class AgregarClienteModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task OnPostAsync(string rut, string nombre, string apellido)
        {
            // TODO: add the user to the database

            // TODO: The port number must match the port of the ServidorgRPC.
            using var channel = GrpcChannel.ForAddress("http://localhost:5165");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.AgregarClienteAsync(
                              new AgregarRequest { Rut = rut, Nombre = nombre, Apellido = apellido });
        }
    }
}
