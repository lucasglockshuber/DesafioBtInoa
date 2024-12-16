using AutomationEmail.Providers;
using DesafioBtInoa.Interface;
using Google.Apis.Gmail.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioBtInoa.Service
{
    public class ValidacaoService : IValidacaoService
    {
        public async Task ValidaArgumentos(string[] args)
        {
            // Validação dos argumentos
            if (args.Length != 3 ||
                !decimal.TryParse(args[1], out decimal precoVenda) ||
                !decimal.TryParse(args[2], out decimal precoCompra))
            {
                Console.WriteLine("Por favor, verifique se os argumentos estão corretos.");
                Console.ReadLine();
                return;
            }
        }

        public async Task ValidaService(IEmailService emailService, IValidacaoService validacaoService, ICotacaoService cotacaoService)
        {
            if (emailService == null || cotacaoService == null)
            {
                Console.WriteLine("Erro ao configurar serviços. Entre em contato com o suporte.");
                Console.ReadLine();
                return;
            }
        }
    }
}
