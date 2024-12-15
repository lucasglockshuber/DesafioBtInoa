using DesafioBtInoa.Interface;
using DesafioBtInoa.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesafioBtInoa.Service
{
    public class CotacaoService : ICotacao
    {
        public async Task<decimal> ObtemCotacao(string nomeAtivo)
        {

            var jsonString = File.ReadAllText("appSettings.json");
            var appSettings = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonString);

            // URL da API
            string urlApi = $"https://brapi.dev/api/quote/{nomeAtivo}?token={appSettings["appSettings"]["chaveApi"]}";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(urlApi);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro na requisição: {response.StatusCode}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            CotacaoModel cotacaoModel = JsonSerializer.Deserialize<CotacaoModel>(jsonResponse);

            return cotacaoModel.results.FirstOrDefault().regularMarketPrice;

        }
    }
}
