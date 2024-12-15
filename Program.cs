using AutomationEmail.Providers;
using DesafioBtInoa.Interface;
using DesafioBtInoa.Model;
using DesafioBtInoa.Service;
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
        ServiceProvider serviceProvider = ConfigureServices();

        IEmailService emailService = serviceProvider.GetService<IEmailService>();
        ICotacao cotacaoService = serviceProvider.GetService<ICotacao>();

        //Validação serviços
        if (emailService == null || cotacaoService == null)
        {
            Console.WriteLine("Erro ao configurar serviços.");
            return;
        }

        // Validação dos argumentos
        if (args.Length != 3 ||
            !decimal.TryParse(args[1], out decimal precoVenda) ||
            !decimal.TryParse(args[2], out decimal precoCompra))
        {
            Console.WriteLine("Por favor, verifique se os argumentos estão corretos.");
            return;
        }

        DetalheCotacaoAtivo detalheCotacaoAtivo = new DetalheCotacaoAtivo()
        {
            nomeAtivo = args[0],
            precoVenda = decimal.Parse(args[1]),
            precoCompra = decimal.Parse(args[2])
        };

        while (true)
        {
            // Obter a cotação atual
            detalheCotacaoAtivo.precoAtual = await cotacaoService.ObtemCotacao(detalheCotacaoAtivo.nomeAtivo);

            await emailService.EnviaEmailCompraVendaAtivo(detalheCotacaoAtivo);

            // Intervalo de monitoramento (2 minuto)
            await Task.Delay(TimeSpan.FromMinutes(2));

        }
    }

    private static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<ICotacao, CotacaoService>()
            .BuildServiceProvider();
    }
}