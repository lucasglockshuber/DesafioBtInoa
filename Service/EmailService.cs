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
using static Program;
using DesafioBtInoa.Model;
using System.Text.Json;

namespace DesafioBtInoa.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailModel _emailConfig;

        static string[] Scopes = { GmailService.Scope.GmailSend, GmailService.Scope.GmailReadonly };
        private GmailService _serivce;


        public async Task EnviaEmailCompraVendaAtivo(DetalheCotacaoAtivo detalheCotacaoAtivo)
        {
            //Configurações para usar Serviço Gmail
            await ConfiguracaoApiGmail();

            //Lê arquivo de configuração JSON
            EmailModel emailModel = JsonSerializer.Deserialize<EmailModel>(File.ReadAllText("config.json"));
            emailModel.assunto = $"Alerta Cotação Ativo: {detalheCotacaoAtivo.nomeAtivo}";

            if (detalheCotacaoAtivo.precoAtual > detalheCotacaoAtivo.precoVenda)
                emailModel.corpo = $"A cotação do ativo {detalheCotacaoAtivo.nomeAtivo} subiu para {detalheCotacaoAtivo.precoAtual}. Considere vender!";
            else if (detalheCotacaoAtivo.precoAtual < detalheCotacaoAtivo.precoCompra)
                emailModel.corpo = $"A cotação do ativo {detalheCotacaoAtivo.nomeAtivo} caiu para {detalheCotacaoAtivo.precoAtual}. Considere comprar!";

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Lucas Duarte Glockshuber", "lucas.glockshuber@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", emailModel.destinatario));
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

            _serivce.Users.Messages.Send(message, "me").Execute();
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

            _serivce = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Gmail API .NET"
            });
        }
    }
}