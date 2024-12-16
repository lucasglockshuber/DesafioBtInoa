using AutomationEmail.Providers;
using DesafioBtInoa.Interface;
using DesafioBtInoa.Model;
using DesafioBtInoa.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Iniciando aplicação");
        Console.WriteLine("Validando serviços e parâmetros");

        ServiceProvider serviceProvider = ConfigureServices();

        IEmailService emailService = serviceProvider.GetService<IEmailService>();
        ICotacaoService cotacaoService = serviceProvider.GetService<ICotacaoService>();
        IValidacaoService validacaoService = serviceProvider.GetService<IValidacaoService>();

        await validacaoService.ValidaArgumentos(args);
        await validacaoService.ValidaService(emailService, validacaoService, cotacaoService);

        DetalheCotacaoAtivoModel detalheCotacaoAtivo = new DetalheCotacaoAtivoModel()
        {
            nomeAtivo = args[0],
            precoVenda = decimal.Parse(args[1]),
            precoCompra = decimal.Parse(args[2])
        };

        while (true)
        {
            Console.WriteLine("Obtendo cotação");
            detalheCotacaoAtivo.precoAtual = await cotacaoService.ObtemCotacao(detalheCotacaoAtivo.nomeAtivo);

            Console.WriteLine("Verificando necessidade envio de email");
            await emailService.EnviaEmailCompraVendaAtivo(detalheCotacaoAtivo);

            Console.WriteLine("Intervalo de monitoramento de 2 minutos.");
            await Task.Delay(TimeSpan.FromMinutes(2));

        }
    }

    private static ServiceProvider ConfigureServices()
    {
        // Carrega o arquivo appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddScoped<IEmailService, EmailService>();  // Registra o EmailService
            serviceCollection.AddScoped<ICotacaoService, CotacaoService>();  // Registra o CotacaoService
            serviceCollection.AddScoped<IValidacaoService, ValidacaoService>();  // Registra o ValidacaoService
         return serviceCollection.BuildServiceProvider();


    }
}