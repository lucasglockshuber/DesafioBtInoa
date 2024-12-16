using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using System.IO;
using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;
using AutomationEmail.Providers;
using DesafioBtInoa.Model;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DesafioBtInoa.Service
{
    public class EmailService : IEmailService
    {
        public readonly IConfiguration _configuration;
        static string[] Scopes = { GmailService.Scope.GmailSend, GmailService.Scope.GmailReadonly };
        private GmailService _serviceGmail;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviaEmailCompraVendaAtivo(DetalheCotacaoAtivoModel detalheCotacaoAtivo)
        {
            //Configurações para usar Serviço Gmail
            await ConfiguracaoApiGmail();

            //Lê arquivo de configuração JSON
            EmailModel emailModel = new EmailModel();
            emailModel.assunto = $"Alerta Cotação Ativo: {detalheCotacaoAtivo.nomeAtivo}";

            if (detalheCotacaoAtivo.precoAtual > detalheCotacaoAtivo.precoVenda)
                emailModel.corpo = $"A cotação do ativo {detalheCotacaoAtivo.nomeAtivo} subiu para {detalheCotacaoAtivo.precoAtual}. Considere vender!";
            else if (detalheCotacaoAtivo.precoAtual < detalheCotacaoAtivo.precoCompra)
                emailModel.corpo = $"A cotação do ativo {detalheCotacaoAtivo.nomeAtivo} caiu para {detalheCotacaoAtivo.precoAtual}. Considere comprar!";

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Lucas Duarte Glockshuber", _configuration.GetSection("email:origem").Value.ToString()));
            emailMessage.To.Add(new MailboxAddress("", _configuration.GetSection("email:destinatario").Value.ToString()));
            emailMessage.Subject = emailModel.assunto;

            var bodyBuilder = new BodyBuilder() { TextBody = emailModel.corpo };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            var memoryStream = new MemoryStream();
            emailMessage.WriteTo(memoryStream);

            var rawMessage = Convert.ToBase64String(memoryStream.ToArray())
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            var message = new Message { Raw = rawMessage };

            _serviceGmail.Users.Messages.Send(message, "me").Execute();
        }

        public async Task ConfiguracaoApiGmail()
        {
            var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore("token", true)).Result;

            if (credential.Token.IsExpired(credential.Flow.Clock))
                credential.RefreshTokenAsync(CancellationToken.None);

            _serviceGmail = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Gmail API .NET"
            });
        }
    }
}