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
            this.viewModel.selectedAccount = NLAccount.HAITONG;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                this.viewModel.fileName = openFileDialog.FileName;
                IRawRecordExtractor extractor = new HaitongRawRecordExtractor();
                this.viewModel.tradePairList = extractor.extractRawRecord(this.viewModel.fileName);
            }
        }

        private void btnCreatePerfSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".xls"; // Default file extension
                dlg.Filter = "Spreadsheets (.xls)|*.xls"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string outputFile = dlg.FileName;
                    PerfSheetWriter writer = new PerfSheetWriter(TemplateManager.getTemplate(this.viewModel.selectedAccount));
                    writer.writePerf(this.viewModel.tradePairList, outputFile);
                    MessageBox.Show("Done!");
                }


            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public PnLConverterViewModel viewModel { get; set; }


    }

    public class PnLConverterViewModel : INotifyPropertyChanged
    {
        private string _fileName = "";
        private List<TradePair> _tradePairList = null;
        private NLAccount _selectedAccount = NLAccount.UNKNOWN;

        public PnLConverterViewModel() { }

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
