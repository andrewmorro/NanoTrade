using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PnLConverter.model;
using Microsoft.Office.Interop.Excel;
using System.Windows; 

namespace PnLConverter.service
{
    class BaseRecordExtractor:IRawRecordExtractor
    {
        public List<TradePair> extractRawRecord(string filePath)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            MessageBox.Show((String)(xlWorkSheet.Cells[2,2] as Range).Value);

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);


            List<TradePair> result = new List<TradePair>();
            return result;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        } 
    }
}
