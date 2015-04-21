using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnLConverter.model;

namespace PnLConverter.service
{
    class HaitongRawRecordExtractor : IRawRecordExtractor
    {
        private static readonly String BUY_FLAG = "买";
        private static readonly String LENDSELL_FLAG = "融券卖出";

        public List<TradePair> extractRawRecord(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Dictionary<String, TradePair> dict = new Dictionary<string, TradePair>();

                using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding("gb2312")))
                {
                    // skip the first header line
                    string line = reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] tokens = String.Copy(line).Split('\t');
                        if (tokens.Length <= 1)
                        {
                            continue;
                        }
                        string ticker = tokens[1];
                        TradePair pair = null;
                        if (!dict.ContainsKey(ticker))
                        {
                            pair = new TradePair();
                            pair.ticker = ticker;
                            dict[ticker] = pair;
                        }
                        pair = dict[ticker];

                        Trade trade = new Trade(ticker, Convert.ToInt32(tokens[4]), Convert.ToSingle(tokens[5]), TradeType.UNKNOWN);
                        if (BUY_FLAG.Equals(tokens[3]))
                        {
                            trade.type = TradeType.BUY;
                            pair.buyTrade = trade;
                        }
                        else if (LENDSELL_FLAG.Equals(tokens[10]))
                        {
                            trade.type = TradeType.LENDSELL;
                            pair.lendSellTrade = trade;
                        }
                    }
                    reader.Close();
                }
                file.Close();
                return dict.Values.ToList();
            }
        }
    }
}
