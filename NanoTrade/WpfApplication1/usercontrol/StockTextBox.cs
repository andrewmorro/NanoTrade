using PnLConverter.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PnLConverter.usercontrol
{
    public class StockTextBox : TextBox
    {

        internal static DependencyProperty SettingNameProperty;
        static StockTextBox()
        {
            SettingNameProperty = DependencyProperty.Register("SettingName", typeof(string), typeof(StockTextBox));
        }

        public StockTextBox()
        {
            this.AcceptsReturn = true;
        }

        public void saveStockListToSetting()
        {
            StringCollection selectedTickers = new StringCollection();
            string[] tickers = this.Text.Split('\n');
            List<string> result = new List<string>();
            foreach (string ticker in tickers)
            {
                if (null != ticker && !"".Equals(ticker))
                {
                    result.Add(ticker.Trim());
                }
            }
            selectedTickers.AddRange(result.ToArray());
            Settings.Default[SettingName] = selectedTickers;
        }

        public string SettingName
        {
            get
            {
                return (string)base.GetValue(SettingNameProperty);
            }
            set
            {
                SetValue(SettingNameProperty, value);
                StringCollection selectedTickers = (StringCollection)Settings.Default[value];
                string[] list = new string[selectedTickers.Count];
                selectedTickers.CopyTo(list, 0);
                if (list.Length >= 2)
                {
                    this.Text = list.Aggregate((current, next) => current + "\n" + next);
                }
                else if (list.Length == 1)
                {
                    this.Text = list[0];
                }
            }
        }
    }


}
