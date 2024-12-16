using AutomationEmail.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioBtInoa.Interface
{
    public interface IValidacaoService
    {
        Task ValidaArgumentos(string[] args);
        Task ValidaService(IEmailService emailService, IValidacaoService validacaoService, ICotacaoService cotacaoService);
    }
}
