using PnLConverter.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnLConverter.service
{
    class PerfSheetWriter
    {

        private static readonly string SHANGHAI_PREFIX = "60";
        private static readonly string SHENZHEN_PREFIX = "00";

        private Dictionary<Market,SheetIndex> index =  new Dictionary<Market, SheetIndex>{{Market.SHANGHAI,new SheetIndex(Market.SHANGHAI)},{Market.SHENZHEN,new SheetIndex(Market.SHENZHEN)}};

        public PerfSheetWriter()
        {

        }

        public PerfSheetWriter(string templatePath)
            : this()
        {
            this.templatePath = templatePath;
        }

        public void writePerf(List<TradePair> tradePairList, string path)
        {
            foreach(TradePair pair in tradePairList){
                if (pair.paired)
                {
                    this.writeItem(pair);
                }
            }
        }

        private void writeItem(TradePair pair)
        {
            Market market = getMarketByTicker(pair);
            Console.WriteLine("Write {2} to grid {0}{1}", index[market].currentCol, index[market].currentRow, pair.ticker);
            index[market].moveToNext();
        }

        private Market getMarketByTicker(TradePair pair)
        {
            if (pair.ticker.StartsWith(SHANGHAI_PREFIX))
            {
                return Market.SHANGHAI;
            }
            else if (pair.ticker.StartsWith(SHENZHEN_PREFIX))
            {
                return Market.SHENZHEN;
            }
            else
            {
                return Market.UNKNOWN;
            }
        }

        public string templatePath { get; set; }




        private class SheetIndex
        {

            private static readonly int ROW_BEGIN = 4;
            private static readonly int ROW_END = 27;
            private static readonly char[] COL_SHANGHAI = { 'B', 'G', 'L' };
            private static readonly char[] COL_SHENZHEN = { 'Q', 'V' };
            private static readonly Dictionary<Market, char[]> COLUMNS = new Dictionary<Market, char[]> { { Market.SHANGHAI, COL_SHANGHAI }, { Market.SHENZHEN, COL_SHENZHEN } };

            private int _currentRow = ROW_BEGIN;
            private int _currentCol = 0;
            private Market market = Market.SHANGHAI;

            public SheetIndex(Market market)
            {
                this.market = market;
            }

            public int currentRow
            {
                get
                {
                    return this._currentRow;
                }
            }
            public char currentCol
            {
                get
                {
                    return COLUMNS[this.market][this._currentCol];
                }
            }

            public void moveToNext()
            {
                if (++_currentRow > ROW_END)
                {
                    _currentRow = ROW_BEGIN;
                    if (++_currentCol >= COLUMNS[this.market].Length)
                    {
                        throw new Exception("There's not enough space for performance record.");
                    }
                }
            }
        }
    }
}
