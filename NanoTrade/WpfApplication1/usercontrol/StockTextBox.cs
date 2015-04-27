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

        public void saveStockListToSetting()
        {
            StringCollection selectedTickers = new StringCollection();
            string[] tickers = this.Text.Split('\n');
            selectedTickers.AddRange(tickers);
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
                this.Text = list.Aggregate((current, next) => current + ", " + next);
            }
        }
    }


}
