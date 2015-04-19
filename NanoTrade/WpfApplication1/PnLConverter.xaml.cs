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
            this.selectedAccount = NLAccount.XIAOCHAJI;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                this.textFilePath.Text = fileName;
            }
        }

        private void btnOpenTemplate_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                templateName = openFileDialog.FileName;
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
                    string filename = dlg.FileName;
                    IRawRecordExtractor extractor = new HaitongRawRecordExtractor();
                    List<TradePair> tradePairList = extractor.extractRawRecord(fileName);
                    PerfSheetWriter writer = new PerfSheetWriter(TemplateManager.getTemplate(selectedAccount));
                    writer.writePerf(tradePairList, filename);
                    MessageBox.Show("Done!");
                }

               
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string fileName { get; set; }

        public NLAccount selectedAccount { get; set; }
    }
}
