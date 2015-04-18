using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnLConverter.model
{
    enum TradeType
    {
        BUY,SELL,LVRGBUY,LENDSELL
    }

    class Trade
    {
        private int shares;
        private float price;
        private TradeType type;
    }
}
