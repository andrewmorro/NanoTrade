using PnLConverter.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnLConverter.service
{
    interface IRawRecordExtractor
    {
        List<TradePair> extractRawRecord(String filePath);
    }
}
