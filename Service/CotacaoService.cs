using DesafioBtInoa.Interface;
using DesafioBtInoa.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesafioBtInoa.Service
{
    public class CotacaoService : ICotacaoService
    {
        public readonly IConfiguration _configuration;

        public CotacaoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<decimal> ObtemCotacao(string nomeAtivo)
        {
            var chaveApi = _configuration.GetSection("chaveApi:chaveApi").Value;
            var urlApiBrapi = _configuration.GetSection("urlApiBrapi:urlApiBrapi").Value;

            string urlApiCompleta = $"{urlApiBrapi}{nomeAtivo}?token={chaveApi}";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(urlApiCompleta);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                Console.ReadLine();
                return 0;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            CotacaoModel cotacaoModel = JsonSerializer.Deserialize<CotacaoModel>(jsonResponse);

            return cotacaoModel.results.FirstOrDefault().regularMarketPrice;

        }
    }
}
