using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PnLConverter.model;
using System.Windows.Resources;
using System.Windows;
using Microsoft.Office.Interop.Excel;

namespace PnLConverter.service
{

    class TemplateManager
    {
        private static readonly Dictionary<NLAccount, string> RESOURCES = new Dictionary<NLAccount, string> { { NLAccount.XIAOCHAJI, "templates/小茶几模板.xls" }, {NLAccount.HAITONG, "海通模板.xls"} };


        public static string getTemplate(NLAccount account)
        {
            return RESOURCES[account];
        }
    }
}
