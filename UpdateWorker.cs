using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Velopack;

namespace UpdateWorkerService
{
    public class UpdateWorker : BackgroundService
    {
        //private readonly string updatePath = @"C:\Users\Gut\source\repos\UpdateWorkerService\updates"; // Ajuste conforme necessário
        private readonly string logFilePath = @"C:\Users\Gut\source\repos\UpdateWorkerSerivce\logs\log.txt"; // Caminho do log

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Verificar por atualizações a cada 10 sec
                await CheckForUpdates();

                // Aguardar 1o segundos antes de verificar novamente
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        // Método para verificar e aplicar atualizações
        private async Task CheckForUpdates()
        {
            try
            {
                LogEvent("Verificando atualizações...");

                var updateManager = new UpdateManager(@"C:\Users\Gut\source\repos\UpdateWorkerService\updates");
                var newVersion = await updateManager.CheckForUpdatesAsync();

                if (newVersion == null)
                {
                    LogEvent("Nenhuma atualização disponível.");

                    return;
                }

                LogEvent($"Nova versão disponível: {newVersion}");
                LogEvent($"Baixando.. {newVersion}");
                await updateManager.DownloadUpdatesAsync(newVersion);
                LogEvent("Atualização baixada com sucesso.");

                updateManager.ApplyUpdatesAndRestart(newVersion);
                LogEvent("Atualização aplicada. Reiniciando...");
            }
            catch (Exception ex)
            {
                LogEvent($"Erro ao verificar ou aplicar a atualização: {ex.Message}");
            }
        }

        // Método para registrar eventos no log
        private void LogEvent(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now}: {message}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                // Como última alternativa, se o log falhar
                Console.Error.WriteLine($"Erro ao gravar no log: {ex.Message}");
            }
        }
    }
}
