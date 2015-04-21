using Microsoft.Office.Interop.Excel;
using PnLConverter.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PnLConverter.service
{
    class PerfSheetWriter
    {

        private static readonly string SHANGHAI_PREFIX = "60";
        private static readonly string SHENZHEN_PREFIX = "00";
        private static readonly string CHUANGYE_PREFIX = "30";

        private Dictionary<Market, SheetIndex> index = new Dictionary<Market, SheetIndex> { { Market.SHANGHAI, new SheetIndex(Market.SHANGHAI) }, { Market.SHENZHEN, new SheetIndex(Market.SHENZHEN) } };

        public PerfSheetWriter(string templatePath)
        {
            this.templatePath = templatePath;
        }

        public void writePerf(List<TradePair> tradePairList, string path)
        {
            //path = @"C:\Users\迪\Desktop\output.xls";
            Application app = null;
            Workbook workbook = null;
            Worksheet sheet = null;
            try
            {
                //Copy file
                File.Copy(templatePath, path, true);

                //open book
                app = new Microsoft.Office.Interop.Excel.Application();
                workbook = app.Workbooks.Open(path, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                sheet = (Worksheet)workbook.Worksheets.get_Item(1);

                Range range = sheet.get_Range("B4", "Z27");
                object[,] cache = this.writeTradeListToCache(tradePairList);
                range.Value = cache;
                workbook.Save();
            }
            catch (IOException e)
            {
                throw e;
            }
            finally
            {
                if (sheet != null)
                    Marshal.ReleaseComObject(sheet);
                if (workbook != null)
                {
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                }
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
            }
        }

        private object[,] writeTradeListToCache(List<TradePair> tradePairList)
        {
            object[,] data = new object[24, 25];
            foreach (TradePair pair in tradePairList)
            {
                if (pair.paired)
                {
                    Market market = getMarketByTicker(pair);
                    if (!index[market].hasSpace)
                    {
                        continue;
                    }
                    //Fill in valid shares into performance sheet. Valid shares are the amount that pairs buy and sell shares
                    int validShares = Convert.ToInt32(Math.Min(pair.buyTrade.shares, pair.lendSellTrade.shares));
                    data[index[market].currentRow, index[market].currentCol] = validShares;
                    data[index[market].currentRow, index[market].currentCol + 1] = Math.Round(pair.buyTrade.price,3);
                    data[index[market].currentRow, index[market].currentCol + 2] = Convert.ToSingle(pair.ticker);
                    data[index[market].currentRow, index[market].currentCol + 3] = validShares;
                    data[index[market].currentRow, index[market].currentCol + 4] = Math.Round(pair.lendSellTrade.price,3);
                    Console.WriteLine("Write {2} to grid {0}{1}", index[market].currentCol, index[market].currentRow, pair.ticker);
                    index[market].moveToNext();
                }
            }
            return data;
        }

        private Market getMarketByTicker(TradePair pair)
        {
            if (pair.ticker.StartsWith(SHANGHAI_PREFIX))
            {
                return Market.SHANGHAI;
            }
            else if (pair.ticker.StartsWith(SHENZHEN_PREFIX)||pair.ticker.StartsWith(CHUANGYE_PREFIX))
            {
                return Market.SHENZHEN;
            }
            else
            {
                return Market.UNKNOWN;
            }
        }

        public string templatePath { get; set; }




        private class SheetIndex
        {
            private static readonly int MAX_ROW = 24;
            private static readonly Dictionary<Market, int> MAX_COL = new Dictionary<Market, int> { { Market.SHANGHAI, 3 }, { Market.SHENZHEN, 2 } };

            private int _currentRow = 0;
            private int _currentCol = 0;
            private Market market = Market.SHANGHAI;

            public SheetIndex(Market market)
            {
                this.market = market;
                this.hasSpace = true;
                this._currentCol = 0;
                this._currentRow = 0;
            }

            public int currentRow
            {
                get
                {
                    return this._currentRow;
                }
            }
            public int currentCol
            {
                get
                {
                    if (market == Market.SHANGHAI)
                    {
                        return this._currentCol * 5;
                    }
                    else
                    {
                        return (this._currentCol + 3) * 5;
                    }
                }
            }

            public bool hasSpace { get; set; }

            public void moveToNext()
            {
                //Move to next row
                if (++_currentRow >= MAX_ROW)
                {
                    //If exceeds row bound, reset to 0 and move to next column
                    _currentRow = 0;
                    if (++_currentCol >= MAX_COL[this.market])
                    {
                        Console.WriteLine("There's not enough space for performance record.");
                        this.hasSpace = false;
                    }
                }
            }
        }
    }
}
