using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace Tag_Explorer
{
    public static class UserConf
    {
        public static bool AdditionalComment { get; set;}
        public static bool SupAddress { get; set; }
        public static bool IOScanning { get; set; }
        public static bool COMAddress { get; set; }
        public static bool Alarm { get; set; }
        public static bool GridReadyForIgnition { get; set; }
        public static bool Ignition { get; set; }
        public static bool ClampMode { get; set; }
        public static bool ScaleMode { get; set; }
        public static bool RawMin { get; set; }
        public static bool RawMax { get; set; }
        public static bool ScaledMin { get; set; }
        public static bool ScaledMax { get; set; }
        public static bool Parent { get; set; }
        public static bool SIEMENS { get; set; }
        public static bool OffsetL { get; set; }
        public static bool OffsetE { get; set; }
        public static bool GENCOM { get; set; }

        public static void Reset()
        {
            AdditionalComment = false;
            SupAddress = false;
            IOScanning = false;
            COMAddress = false;
            Alarm = false;
            GridReadyForIgnition = false;
            Ignition = false;
            ClampMode = false;
            ScaleMode = false;
            Parent = false;
            OffsetE = false;
            OffsetL = false;
        }

        public static void Update(DataGridView data)
        {

        }

    }
}
