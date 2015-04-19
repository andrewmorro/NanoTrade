using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnLConverter.model
{
    enum TradeType
    {
        BUY, SELL, LVRGBUY, LENDSELL, UNKNOWN
    }

    class Trade
    {
        private string _ticker = "";
        private int _shares = 0;
        private double _price = 0;
        private TradeType _type = TradeType.UNKNOWN;

        public Trade()
        {

        }

        public Trade(string ticker, int shares, double price, TradeType type)
        {
            this._ticker = ticker;
            this._shares = shares;
            this._price = price;
            this._type = type;
        }

        public string ticker
        {
            get
            {
                return this._ticker;
            }
            set
            {
                this._ticker = value;
            }
        }

        public int shares
        {
            get
            {
                return this._shares;
            }
            set
            {
                this._shares = value;
            }
        }

        public double price
        {
            get
            {
                return this._price;
            }
            set
            {
                this._price = value;
            }
        }

        public TradeType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public override string ToString() {
            return String.Format("{0} {1} shares {2} at {3}", type, shares, ticker, price);
        }

    }
}
