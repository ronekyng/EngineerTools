using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace EngineerTools
{
    public class SettingFormController
    {

        static   SettingFormController()
        {     

        }

        public static string projectRootFolder { get; internal set; }
        public static string MdbFile { get; internal set; }
        public static string ProgramInitialed { get; internal set; }
        public static string SharePointRootFolder { get; internal set; }

        public static bool SaveSettings()
        {
            global::EngineerTools.Properties.Settings.Default.Save();
            return true;
        }

    }
}
