using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioBtInoa.Model
{
    public class CotacaoModel
    {
        public List<CotacaoItensModel> results { get; set; }
        public DateTime requestedAt { get; set; }
        public string took { get; set; }
    }

    public class CotacaoItensModel
    {
        public string currency { get; set; }
        public string shortName { get; set; }
        public string longName { get; set; }
        public decimal regularMarketChange { get; set; }
        public decimal regularMarketChangePercent { get; set; }
        public DateTime regularMarketTime { get; set; }
        public decimal regularMarketPrice { get; set; }
        public decimal regularMarketDayHigh { get; set; }
        public string regularMarketDayRange { get; set; }
        public decimal regularMarketDayLow { get; set; }
        public long regularMarketVolume { get; set; }
        public decimal regularMarketPreviousClose { get; set; }
        public decimal fegularMarketOpen { get; set; }
        public string fiftyTwoWeekRange { get; set; }
        public decimal fiftyTwoWeekLow { get; set; }
        public decimal fiftyTwoWeekHigh { get; set; }
        public string symbol { get; set; }
        public decimal priceEarnings { get; set; }
        public decimal earningsPerShare { get; set; }
        public string logoUrl { get; set; }
    }
}