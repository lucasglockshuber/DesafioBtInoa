using DesafioBtInoa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioBtInoa.Interface
{
    public interface ICotacaoService
    {
        Task<decimal> ObtemCotacao(string nomeAtivo);
    }
}
