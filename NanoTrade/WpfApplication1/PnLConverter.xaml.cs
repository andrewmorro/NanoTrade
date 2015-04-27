using Microsoft.Win32;
using PnLConverter.service;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using PnLConverter.model;
using System.ComponentModel;
using PnLConverter.Properties;
using System.Collections.Specialized;
using PnLConverter.component;

namespace PnLConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected string templateName = "";
        public MainWindow()
        {
            //base.InitializeComponent();
            this.viewModel = new PnLConverterViewModel();
            this.DataContext = this.viewModel;
            this.viewModel.selectedAccount = NLAccount.XIAOCHAJI;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                this.viewModel.fileName = openFileDialog.FileName;
                IRawRecordExtractor extractor = new HaitongRawRecordExtractor();
                this.viewModel.tradePairList = extractor.extractRawRecord(this.viewModel.fileName);
                this.loadTickerSetting(this.viewModel.selectedAccount);
            }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.loadTickerSetting(this.viewModel.selectedAccount);
        }

        private void btnCreatePerfSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.viewModel.progress = 0;
                this.saveTickerSetting(this.viewModel.selectedAccount);
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = this.viewModel.selectedAccount + DateTime.Today.ToString("yyyy-MM-dd") + txtTraderName.Text; // Default file name
                dlg.DefaultExt = ".xls"; // Default file extension
                dlg.Filter = "Spreadsheets (.xls)|*.xls"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                this.viewModel.progress = 30;
                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string outputFile = dlg.FileName;
                    PerfSheetWriter writer = new PerfSheetWriter(TemplateManager.getTemplate(this.viewModel.selectedAccount));
                    writer.writePerf(this.viewModel.tradePairList, outputFile);
                    this.viewModel.progress = 100;
                    this.viewModel.status = "生成完毕";
                    //MessageBox.Show("Done!");
                }


            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadTickerSetting(NLAccount account)
        {
            StringCollection selectedTickers = (StringCollection)Settings.Default["selectedTickers_" + account];
            if (null != selectedTickers && null!=this.viewModel.tradePairList)
            {
                foreach (TradePair pair in this.viewModel.tradePairList)
                {
                    if (selectedTickers.Contains(pair.ticker))
                    {
                        pair.selected = true;
                    }
                    else
                    {
                        pair.selected = false;
                    }
                }
            }
        }

        private void saveTickerSetting(NLAccount account)
        {
            if (null != this.viewModel.tradePairList)
            {

                StringCollection selectedTickers = new StringCollection();
                foreach (TradePair pair in this.viewModel.tradePairList)
                {
                    if (pair.selected)
                    {
                        selectedTickers.Add(pair.ticker);
                    }
                }
                Settings.Default["selectedTickers_"+account] = selectedTickers;
            }
        }

        public PnLConverterViewModel viewModel { get; set; }

        private void settingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingPopup setting = new SettingPopup();
            setting.ShowDialog();
        }


    }

    public class PnLConverterViewModel : INotifyPropertyChanged
    {
        private string _fileName = "";
        private List<TradePair> _tradePairList = null;
        private NLAccount _selectedAccount = NLAccount.UNKNOWN;
        private int _progress = 0;
        private string _status = "";

        public PnLConverterViewModel() { }

        public string status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
                OnPropertyChanged("status");
            }
        }


        public int progress
        {
            get
            {
                return this._progress;
            }
            set
            {
                this._progress = value;
                OnPropertyChanged("progress");
            }
        }

        public string fileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
                OnPropertyChanged("fileName");
            }
        }

        public List<TradePair> tradePairList
        {
            get
            {
                return this._tradePairList;
            }
            set
            {
                this._tradePairList = value;
                OnPropertyChanged("tradePairList");
            }
        }

        public NLAccount selectedAccount
        {
            get
            {
                return this._selectedAccount;
            }
            set
            {
                this._selectedAccount = value;
                OnPropertyChanged("selectedAccount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
    }
}
