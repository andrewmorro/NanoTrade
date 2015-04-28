using PnLConverter.usercontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PnLConverter.component
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingPopup : Window
    {
        public SettingPopup()
        {
            InitializeComponent();
            this.stockBox_HAITONG.SettingName = "selectedTickers_HAITONG";
            this.stockBox_XIAOCHAJI.SettingName = "selectedTickers_XIAOCHAJI";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.stockBox_HAITONG.saveStockListToSetting();
            this.stockBox_XIAOCHAJI.saveStockListToSetting();
        }
    }
}
