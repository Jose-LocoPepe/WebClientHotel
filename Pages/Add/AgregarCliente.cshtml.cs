using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Grpc.Net.Client;
using WebClientHotel;
using WebClientHotel.Models;

namespace WebClientHotel.Pages.Add
{
    public class AgregarClienteModel : PageModel
    {
        public async Task OnPostAsync(string rut, string nombre, string apellido)
        {
            try
            {
                var grpcServerUrl = Environment.GetEnvironmentVariable("GRPC_SERVER_URL");
                if (string.IsNullOrEmpty(grpcServerUrl))
                {
                    throw new InvalidOperationException("GRPC_SERVER_URL is not set in .env file");
                }

                // add the user to the database
                Cliente cliente = new();
                cliente.Rut = rut;
                cliente.Nombre = nombre;
                cliente.Apellido = apellido;

                ClientesContext context = new();
                context.Clientes.Add(cliente);
                await context.SaveChangesAsync();

                // Set success message
                TempData["SuccessMessage"] = "Cliente added successfully!";

                // TODO: The port number must match the port of the ServidorgRPC.
                using var channel = GrpcChannel.ForAddress(grpcServerUrl);
                var client = new Greeter.GreeterClient(channel);
                var request = new AgregarRequest { Rut = rut, Nombre = nombre, Apellido = apellido };
                var reply = await client.AgregarClienteAsync(request);

                Console.WriteLine(reply.Message);
                Response.Redirect("/Index");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while adding the client: {ex.Message}");
                throw;
            }
        }
    }
}
