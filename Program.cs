using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using UpdateWorkerService;
using Velopack; // Adicione o namespace Velopack

public class Program
{
    public static async Task Main(string[] args)
    {
        // Inicializa o Velopack para gerenciar atualiza��es
        VelopackApp.Build().Run(); // Garante que o Velopack seja iniciado

        // Cria e executa o Worker Service como um servi�o do Windows
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<UpdateWorker>(); // Seu Worker Service
            })
            .UseWindowsService(); // Registra o servi�o como um servi�o do Windows
}
