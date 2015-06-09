using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnLConverter.model
{
    public class TradePair
    {
        public string ticker { get; set; }
        public Trade buyTrade { get; set; }
        public Trade lendSellTrade { get; set; }
        public bool selected { get; set; }

        public TradePair()
        {
            this.buyTrade = new Trade();
            this.lendSellTrade = new Trade();
            this.selected = false;
        }

        public bool paired
        {
            get
            {
                if (null != buyTrade && null != lendSellTrade)
                {
                    return true;
                }
                return false;
            }
        }

        public int position
        {
            get
            {
                return this.buyTrade.shares - this.lendSellTrade.shares;
            }
        }

        public override string ToString()
        {
            return String.Format("Paired: {0} \n{1}\n{2}", paired, buyTrade, lendSellTrade);
        }
    }
}
