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
        public List<TradePair> extractRawRecord(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Dictionary<String, TradePair> dict = new Dictionary<string, TradePair>();

            Console.WriteLine("abc");
            using (StreamReader r = new StreamReader(file, Encoding.GetEncoding("gb2312")))
            {
                // skip the first header line
                string line = r.ReadLine();
                while ((line = r.ReadLine()) != null)
                {
                    String[] tokens = line.Split('\t');
                    if (tokens[10].Equals("融券卖出"))
                    {
                        Console.WriteLine("Sell");
                    }
                }
            }

            return dict.Values.ToList();
        }
    }
}
