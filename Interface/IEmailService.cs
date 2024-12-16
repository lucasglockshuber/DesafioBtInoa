using DesafioBtInoa.Model;
using System.Threading.Tasks;

namespace AutomationEmail.Providers
{
    public interface IEmailService
    {
        Task EnviaEmailCompraVendaAtivo(DetalheCotacaoAtivoModel detalheCotacaoAtivo);
        Task ConfiguracaoApiGmail();
    }
}