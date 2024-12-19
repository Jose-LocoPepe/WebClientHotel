using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Grpc.Net.Client;
using WebClientHotel;
using DotNetEnv;

namespace WebClientHotel.Pages.Add
{
    public class AgregarClienteModel : PageModel
    {
        public async Task OnPostAsync(string rut, string nombre, string apellido)
        {
            try
            {
                DotNetEnv.Env.Load();
                var grpcServerUrl = Environment.GetEnvironmentVariable("GRPC_SERVER_URL");
                if (string.IsNullOrEmpty(grpcServerUrl))
                {
                    throw new InvalidOperationException("GRPC_SERVER_URL is not set in .env file");
                }

                // TODO: add the user to the database


                // TODO: The port number must match the port of the ServidorgRPC.
                using var channel = GrpcChannel.ForAddress(grpcServerUrl);
                var client = new Greeter.GreeterClient(channel);
                var reply = await client.AgregarClienteAsync(
                                new AgregarRequest { Rut = rut, Nombre = nombre, Apellido = apellido });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while adding the client: {ex.Message}");
                throw;
            }
        }
    }
}
