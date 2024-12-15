using DesafioBtInoa.Model;
using System.Threading.Tasks;
using static Program;

namespace AutomationEmail.Providers
{
    public interface IEmailService
    {
        Task<bool> EnviaEmailCompraVendaAtivo(DetalheCotacaoAtivo detalheCotacaoAtivo);
        Task<bool> ConfiguracaoApiGmail();
    }
}