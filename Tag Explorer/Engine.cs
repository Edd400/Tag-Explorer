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
using Modbus.Core;
using System.Runtime.InteropServices;
using Modbus.Core.Converters;
using EasyModbus;

namespace Tag_Explorer
{
    public static class Engine
    {
        public static VariablesExchangeFile UnityVariableFile { get; set; }
        public static Tags IgnitionVariableFile { get; set; }
        public static string Project_Name { get; set; }
        public static BindingSource bs;
        public static System.Data.DataTable datatable;
        public static List<TeTag> TeTags { get; set; }
        public static string actualProjectPath { get; set; }
        public static bool SIEMENS { get; set; }
        public static List<string> ColumnsNames { get; set; }
        
        //Listing des différent types reconnus par le logiciel , le reste est considéré comme une structure
        public static List<string> Types = new List<string> { "BOOL", "INT", "DINT", "FLOAT", "REAL", "DOUBLE","WORD" };


        public static void UpdateEngine()
        {
            #region Liste des columns
            ColumnsNames = new List<string>();
            ColumnsNames.Add("Variable Name");

            ColumnsNames.Add("Comment");

            ColumnsNames.Add("Type");

            ColumnsNames.Add("API Address");

            ColumnsNames.Add("Additional Comment");

            ColumnsNames.Add("Supervisor Address");

            ColumnsNames.Add("IO Scanning");

            ColumnsNames.Add("COM Address");

            ColumnsNames.Add("Alarm On");

            ColumnsNames.Add("Alarm Active Value");
            ColumnsNames.Add("Alarm Filter");
            ColumnsNames.Add("Alarm Text");
            ColumnsNames.Add("Priority");

            ColumnsNames.Add("Device");

            ColumnsNames.Add("OPCServer");

            ColumnsNames.Add("Clamp Mode");

            ColumnsNames.Add("Scale Mode");

            ColumnsNames.Add("Raw Min");

            ColumnsNames.Add("Raw Max");

            ColumnsNames.Add("Scaled Min");

            ColumnsNames.Add("Scaled Max");

            ColumnsNames.Add("Parent");

            ColumnsNames.Add("DB");

            #endregion
            SIEMENS = UserConf.SIEMENS;
        }
        

        #region Fichier/Importer
        public static void ImportFromExcelAsUnity(DataGridView data)
        {
            data.DataSource = null;
            data.Rows.Clear();
            data.Columns.Clear();
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.DefaultExt = ".xlsx";
            OpenFile.CheckFileExists = true;
            Popup_feuillecs Choix_Feuil = new Popup_feuillecs();
            if (Choix_Feuil.ShowDialog() == DialogResult.OK)
            {
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        String name = Choix_Feuil.textBox1.Text;
                        String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                        OpenFile.FileName +
                                        ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

                        OleDbConnection con = new OleDbConnection(constr);
                        OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
                        con.Open();

                        OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                        System.Data.DataTable data1 = new System.Data.DataTable();
                        sda.Fill(data1);
                        data.DataSource = data1;

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("La feuille renseignée n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
            }


        }
        #endregion

        #region Fichier/Nouveau   
        public static void CreateNewTEProject(DataGridView data)
        {



            Project_Name = "New_TE_Project";

            #region Add required columns

            data.Columns.Add("Name_Field", "Variable Name");
            data.Columns.Add("Comment_Field", "Comment");
            data.Columns.Add("Type_Field", "Type");
            data.Columns.Add("API_Addr_Field", "API Address");

            #endregion
        }
        #endregion

        #region Fichier/Ouvrir
        public static bool ImportProjectFromExcel(DataGridView data_modif, Form1 form)
        {

            UpdateEngine();
            DataGridView data = new DataGridView();
            form.Controls.Add(data);
            #region Excel import
            data.DataSource = null;
            data.Rows.Clear();
            data.Columns.Clear();
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.DefaultExt = ".xlsx";
            OpenFile.CheckFileExists = true;
            Popup_feuillecs Choix_Feuil = new Popup_feuillecs();
            if (Choix_Feuil.ShowDialog() == DialogResult.OK)
            {
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        String name = Choix_Feuil.textBox1.Text;
                        String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                        OpenFile.FileName +
                                        ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

                        OleDbConnection con = new OleDbConnection(constr);
                        OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
                        con.Open();

                        OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                        System.Data.DataTable data1 = new System.Data.DataTable();
                        sda.Fill(data1);
                        data.DataSource = data1;
                        int titi = 1;
                        #region Formattage columns
                        foreach (DataGridViewColumn col in data.Columns)
                        {

                            switch (col.Name)
                            {
                                case "Variable Name":
                                    data_modif.Columns.Add("Name_Field", "Variable Name");
                                    break;
                                case "Comment":
                                    data_modif.Columns.Add("Comment_Field", "Comment");
                                    break;
                                case "Type":
                                    data_modif.Columns.Add("Type_Field", "Type");
                                    break;
                                case "API Address":
                                    data_modif.Columns.Add("API_Addr_Field", "API Address");
                                    break;
                                case "Additional Comment":
                                    data_modif.Columns.Add("Additional_Comment_Field", "Additional Comment");
                                    UserConf.AdditionalComment = true;
                                    break;
                                case "Supervisor Address":
                                    data_modif.Columns.Add("Sup_Address_Field", "Supervisor Address");
                                    UserConf.SupAddress = true;
                                    break;
                                case "IO Scanning":
                                    DataGridViewCheckBoxColumn Temp_Col = new DataGridViewCheckBoxColumn();
                                    Temp_Col.Name = "IO_Field";
                                    Temp_Col.HeaderText = "IO Scanning";
                                    data_modif.Columns.Add(Temp_Col);
                                    UserConf.IOScanning = true;
                                    break;
                                case "COM Address":
                                    data_modif.Columns.Add("COM_Field", "COM Address");
                                    UserConf.COMAddress = true;
                                    break;
                                case "Alarm On":

                                    DataGridViewCheckBoxColumn Temp_Col2 = new DataGridViewCheckBoxColumn();
                                    Temp_Col2.Name = "Alarm_Field";
                                    Temp_Col2.HeaderText = "Alarm On";
                                    data_modif.Columns.Add(Temp_Col2);
                                    data_modif.Columns.Add("Priority", "Priority");
                                    data_modif.Columns.Add("Alarm_Text", "Alarm Text");
                                    data_modif.Columns.Add("Alarm Filter", "Alarm Filter");
                                    UserConf.Alarm = true;
                                    break;
                                case "Alarm Active Value":
                                    data_modif.Columns.Add("Active_Field", "Alarm Active Value");
                                    break;
                                case "Device":
                                    UserConf.Ignition = true;
                                    data_modif.Columns.Add("Device", "Device");
                                    break;
                                case "OPCServer":
                                    data_modif.Columns.Add("OPCServer", "OPCServer");
                                    break;
                                case "Clamp Mode":
                                    data_modif.Columns.Add("Clamp Mode", "Clamp Mode");
                                    UserConf.ClampMode = true;
                                    break;
                                case "Scale Mode":
                                    data_modif.Columns.Add("Scale Mode", "Scale Mode");
                                    UserConf.ScaleMode = true;
                                    break;
                                case "Raw Min":
                                    data_modif.Columns.Add("Raw Min", "Raw Min");
                                    UserConf.RawMin = true;
                                    break;
                                case "Raw Max":
                                    data_modif.Columns.Add("Raw Max", "Raw Max");
                                    UserConf.RawMax = true;
                                    break;
                                case "Scaled Min":
                                    data_modif.Columns.Add("Scaled Min", "Scaled Min");
                                    UserConf.ScaledMin = true;
                                    break;
                                case "Scaled Max":
                                    data_modif.Columns.Add("Scaled Max", "Scaled Max");
                                    UserConf.ScaledMax = true;
                                    break;
                                case "Parent":
                                    data_modif.Columns.Add("Parent", "Parent");
                                    UserConf.Parent = true;
                                    break;
                                case "DB":
                                    data_modif.Columns.Add("DB", "DB");
                                    UserConf.SIEMENS = true;
                                    break;


                            }

                            if (!ColumnsNames.Contains(col.HeaderText))
                            {

                                data_modif.Columns.Add(col.HeaderText, col.HeaderText);
                            }

                        }
                        data_modif.RowCount = data.RowCount;
                        foreach (DataGridViewRow row in data.Rows)
                        {
                            foreach (DataGridViewColumn col in data_modif.Columns)
                            {
                                data_modif.Rows[row.Index].Cells[col.Name].Value = row.Cells[col.HeaderText].Value;

                            }
                        }
                        int toto = 1;
                        #endregion
                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("La feuille renseignée n'existe pas ou ne correspond pas à un projet Tag Explorer ,  cela peut aussi venir d'un problème de drivers", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }


                }
            }
            return false;
            #endregion


        }
        public static bool OpenTEProject(string directory, DataGridView data, Form1 form)
        {

            try
            {
                DataGridView saved_data = new DataGridView();
                form.Controls.Add(saved_data);
                System.Data.DataTable table;
                XmlSerializer xs = new XmlSerializer(typeof(System.Data.DataTable));
                using (StreamReader reader = new StreamReader(directory))
                {
                    table = xs.Deserialize(reader) as System.Data.DataTable;
                }



                data.DataSource = table;
                saved_data.DataSource = table;

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    var col = data.Columns[i];
                    switch (col.Name)
                    {
                        case "Name_Field":
                            data.Columns[i].HeaderText = "Variable Name";
                            break;
                        case "Comment_Field":
                            data.Columns[i].HeaderText = "Comment";
                            break;
                        case "Type_Field":
                            data.Columns[i].HeaderText = "Type";
                            break;
                        case "API_Addr_Field":
                            data.Columns[i].HeaderText = "API Address";
                            break;
                        case "Additional_Comment_Field":
                            data.Columns[i].HeaderText = "Additional Comment";
                            UserConf.AdditionalComment = true;
                            break;
                        case "Sup_Address_Field":
                            data.Columns[i].HeaderText = "Supervisor Address";
                            UserConf.SupAddress = true;
                            break;
                        case "IO_Field":
                            data.Columns.RemoveAt(i);
                            DataGridViewCheckBoxColumn Temp_Col = new DataGridViewCheckBoxColumn();
                            Temp_Col.Name = "IO_Field";
                            Temp_Col.HeaderText = "IO Scanning";
                            data.Columns.Insert(i, Temp_Col);
                            int y = 0;
                            foreach (DataGridViewRow row in data.Rows)
                            {
                                row.Cells["IO_Field"].Value = saved_data.Rows[y].Cells["IO_Field"].Value;
                                y++;
                            }
                            UserConf.IOScanning = true;
                            break;
                        case "COM_Field":
                            data.Columns[i].HeaderText = "COM Address";
                            UserConf.COMAddress = true;
                            break;
                        case "Alarm_Field":
                            data.Columns.RemoveAt(i);
                            DataGridViewCheckBoxColumn Temp_Col2 = new DataGridViewCheckBoxColumn();
                            Temp_Col2.Name = "Alarm_Field";
                            Temp_Col2.HeaderText = "Alarm On";
                            data.Columns.Insert(i, Temp_Col2);
                            data.Columns["Alarm_Text"].HeaderText = "Alarm Text";

                            int u = 0;
                            foreach (DataGridViewRow row in data.Rows)
                            {
                                row.Cells["Alarm_Field"].Value = saved_data.Rows[u].Cells["Alarm_Field"].Value;
                                u++;
                            }
                            UserConf.Alarm = true;
                            break;
                        case "Active_Field":
                            data.Columns[i].HeaderText = "Alarm Active Value";
                            break;
                        case "Device":
                            UserConf.Ignition = true;
                            data.Columns[i].HeaderText = "Device";
                            break;
                        case "OPCServer":
                            data.Columns[i].HeaderText = "OPCServer";
                            break;
                        case "Clamp Mode":
                            UserConf.ClampMode = true;
                            break;
                        case "Scale Mode":
                            UserConf.ScaleMode = true;
                            break;
                        case "Raw Min":
                            UserConf.RawMin = true;
                            break;
                        case "Raw Max":
                            UserConf.RawMax = true;
                            break;
                        case "Scaled Min":
                            UserConf.ScaledMin = true;
                            break;
                        case "Scaled Max":
                            UserConf.ScaledMax = true;
                            break;
                        case "Parent":
                            UserConf.Parent = true;
                            break;
                        case "DB":
                            UserConf.SIEMENS = true;
                            break;
                    }
                }

                return true;
            }
            catch (Exception)
            {

                MessageBox.Show("Le fichier sélectionné est corrompu", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        #endregion

        #region Fichier/Exporter
        public static void ExportUnityVariablesToExcel(DataGridView data)
        {
            copyAlltoClipboard(data);
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            bool toto = true;
        }

        public static void ExportUnityVariablesToIOList(DataGridView _data)
        {
            Form1 form = new Form1();
            DataGridView data = new DataGridView();
            #region Old

            /*
            if (_data.Columns.Contains("Name_Field"))
                data.Columns.Add((DataGridViewColumn)_data.Columns["Name_Field"].Clone());
            if (_data.Columns.Contains("Comment_Field"))
                data.Columns.Add((DataGridViewColumn)_data.Columns["Comment_Field"].Clone());
            if (_data.Columns.Contains("Type_Field"))
                data.Columns.Add((DataGridViewColumn)_data.Columns["Type_Field"].Clone());
            if (_data.Columns.Contains("API_Addr_Field"))
                data.Columns.Add((DataGridViewColumn)_data.Columns["API_Addr_Field"].Clone());
            if (_data.Columns.Contains("Sup_Address_Field"))
                data.Columns.Add((DataGridViewColumn)_data.Columns["Sup_Address_Field"].Clone());


            foreach (DataGridViewRow row in _data.Rows)
            {
                DataGridViewRow dr = new DataGridViewRow();

                dr.Cells.Add((DataGridViewCell)row.Cells["Name_Field"].Clone());
                dr.Cells[0].Value = row.Cells["Name_Field"].Value;
                dr.Cells.Add((DataGridViewCell)row.Cells["Comment_Field"].Clone());
                dr.Cells[1].Value = row.Cells["Comment_Field"].Value;
                dr.Cells.Add((DataGridViewCell)row.Cells["Type_Field"].Clone());
                dr.Cells[2].Value = row.Cells["Type_Field"].Value;
                dr.Cells.Add((DataGridViewCell)row.Cells["API_Addr_Field"].Clone());
                dr.Cells[3].Value = row.Cells["API_Addr_Field"].Value;
                if (_data.Columns.Contains("Sup_Address_Field"))
                {
                    dr.Cells.Add((DataGridViewCell)row.Cells["Sup_Address_Field"].Clone());
                    dr.Cells[4].Value = row.Cells["Sup_Address_Field"].Value;
                }


                data.Rows.Add(dr);
            }
            */
            #endregion

            #region New
            form.Controls.Add(data);

            data.Columns.Add("Mnemonique", "Mnemonique");
            data.Columns.Add("Désignation", "Désignation");
            data.Columns.Add("Type", "Type");
            data.Columns.Add("Adresse API", "Adresse API");
            data.Columns.Add("Adresse pour supervision", "Adresse pour supervisio");

            foreach (TeTag tag in TeTags)
            {
                data.Rows.Add(tag.Name, tag.Comment, tag.Type, tag.APIAddress, tag.SUPAddress);
            }
            #endregion


            copyAlltoClipboard(data);
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            form.Dispose();
        }

        public static void ExportToUnity(DataGridView data)
        {
            Select_Fields_Unity SelectUnity = new Select_Fields_Unity();
            #region init dialog box
            SelectUnity.NameField.Text = "Variable Name";
            SelectUnity.typeField.Text = "Type";
            SelectUnity.addressField.Text = "API Address";
            SelectUnity.commentField.Text = "Comment";
            if (data.Columns.Contains("Sup_Address_Field"))
                SelectUnity.SupervisorField.Text = "Supervisor Address";
            SelectUnity.PrefixField.Text = "S_";
            #endregion
            if (SelectUnity.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog SaveFile = new SaveFileDialog();
                SaveFile.DefaultExt = ".xsy";
                SaveFile.Filter = "Unity variable|*.xsy";
                SaveFile.FileName = "Var1";
                if (SaveFile.ShowDialog() == DialogResult.OK)
                {

                    #region Create base variable
                    XmlSerializer xs = new XmlSerializer(typeof(VariablesExchangeFile));
                    using (StreamReader reader = new StreamReader(@"CreationTemplate.xml"))
                    {
                        UnityVariableFile = xs.Deserialize(reader) as VariablesExchangeFile;

                    }
                    #endregion


                    #region Verify variables
                    foreach (DataGridViewRow row in data.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null)
                            {
                                string tempstring = cell.Value.ToString();
                                cell.Value = tempstring.Replace("\n", "");
                            }

                        }
                    }
                    #endregion
                    #region Create the variables

                    foreach (DataGridViewRow row in data.Rows)
                    {

                        #region variable API
                        string text1;
                        if (row.Cells[FindIndexByHeader(SelectUnity.NameField.Text, data.Columns)].Value != null)
                        {
                            text1 = row.Cells[FindIndexByHeader(SelectUnity.NameField.Text, data.Columns)].Value.ToString();
                        }
                        else
                        {
                            text1 = "";
                        }
                        string text2;
                        string text3;
                        string text4;
                        string text5 = "";

                        if (row.Cells[FindIndexByHeader(SelectUnity.typeField.Text, data.Columns)].Value != null)
                        {
                            text2 = row.Cells[FindIndexByHeader(SelectUnity.typeField.Text, data.Columns)].Value.ToString();
                        }
                        else
                        {
                            text2 = "";
                        }

                        if (row.Cells[FindIndexByHeader(SelectUnity.addressField.Text, data.Columns)].Value != null)
                        {
                            text3 = row.Cells[FindIndexByHeader(SelectUnity.addressField.Text, data.Columns)].Value.ToString();
                        }
                        else
                        {
                            text3 = "";
                        }

                        if (row.Cells[FindIndexByHeader(SelectUnity.commentField.Text, data.Columns)].Value != null)
                        {
                            text4 = row.Cells[FindIndexByHeader(SelectUnity.commentField.Text, data.Columns)].Value.ToString();
                        }
                        else
                        {
                            text4 = "";
                        }
                        /*
                        if (row.Cells[FindIndexByHeader("variableInit", data.Columns)].Value != null)
                        {
                            text5 = row.Cells[FindIndexByHeader("variableInit", data.Columns)].Value.ToString();
                        }
                        else
                        {
                            text5 = "";
                        }
                        */
                        UnityVariableFile.dataBlock.Add(
                            new VariablesExchangeFileVariables(
                                text1,
                                text2,
                                text3,
                                text4,
                                text5
                                ));
                        #endregion

                        if (SelectUnity.SupervisorField.Text != string.Empty)
                        {
                            if (row.Cells[FindIndexByHeader(SelectUnity.SupervisorField.Text, data.Columns)].Value != null &&
                                row.Cells[FindIndexByHeader(SelectUnity.SupervisorField.Text, data.Columns)].Value.ToString() != string.Empty)
                            {
                                #region variable sup

                                if (row.Cells[FindIndexByHeader(SelectUnity.NameField.Text, data.Columns)].Value != null)
                                {
                                    text1 = SelectUnity.PrefixField.Text + row.Cells[FindIndexByHeader(SelectUnity.NameField.Text, data.Columns)].Value.ToString();
                                }
                                else
                                {
                                    text1 = "";
                                }

                                text5 = "";

                                if (row.Cells[FindIndexByHeader(SelectUnity.typeField.Text, data.Columns)].Value != null)
                                {
                                    text2 = row.Cells[FindIndexByHeader(SelectUnity.typeField.Text, data.Columns)].Value.ToString();
                                }
                                else
                                {
                                    text2 = "";
                                }

                                if (row.Cells[FindIndexByHeader(SelectUnity.SupervisorField.Text, data.Columns)].Value != null)
                                {
                                    text3 = row.Cells[FindIndexByHeader(SelectUnity.SupervisorField.Text, data.Columns)].Value.ToString();
                                }
                                else
                                {
                                    text3 = "";
                                }

                                if (row.Cells[FindIndexByHeader(SelectUnity.commentField.Text, data.Columns)].Value != null)
                                {
                                    text4 = row.Cells[FindIndexByHeader(SelectUnity.commentField.Text, data.Columns)].Value.ToString();
                                }
                                else
                                {
                                    text4 = "";
                                }
                                /*
                                if (row.Cells[FindIndexByHeader("variableInit", data.Columns)].Value != null)
                                {
                                    text5 = row.Cells[FindIndexByHeader("variableInit", data.Columns)].Value.ToString();
                                }
                                else
                                {
                                    text5 = "";
                                }
                                */
                                UnityVariableFile.dataBlock.Add(
                                    new VariablesExchangeFileVariables(
                                        text1,
                                        text2,
                                        text3,
                                        text4,
                                        text5
                                        ));
                                #endregion
                            }

                        }
                    }
                    #endregion

                    using (StreamWriter writer = new StreamWriter(SaveFile.FileName))
                    {
                        xs.Serialize(writer, UnityVariableFile);
                    }



                }
            }





        }
        public static void ExportToIgnition(DataGridView data)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = ".xml";
            saveFile.AddExtension = true;
            saveFile.Filter = "Document XML|*.xml";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Tags Taglist = new Tags();
                foreach (DataGridViewRow row in data.Rows)
                {
                    TagsTag untag = new TagsTag();
                    untag.name = row.Cells[data.Columns["Name"].Index].Value.ToString();
                    untag.path = row.Cells[data.Columns["Path"].Index].Value.ToString();
                    untag.type = row.Cells[data.Columns["Type"].Index].Value.ToString();
                    if (row.Cells[data.Columns["DataType"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("DataType", row.Cells[data.Columns["DataType"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["OPCServer"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("OPCServer", row.Cells[data.Columns["OPCServer"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["OPCItemPath"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("OPCItemPath", row.Cells[data.Columns["OPCItemPath"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["ScanClass"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("ScanClass", row.Cells[data.Columns["ScanClass"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["HistoryEnabled"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("HistoryEnabled", row.Cells[data.Columns["HistoryEnabled"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["PrimaryHistoryProvider"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("PrimaryHistoryProvider", row.Cells[data.Columns["PrimaryHistoryProvider"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["HistoryMaxAgeMode"].Index].Value != null)
                        untag.Property.Add(new TagsTagProperty("HistoryMaxAgeMode", row.Cells[data.Columns["HistoryMaxAgeMode"].Index].Value.ToString()));
                    if (row.Cells[data.Columns["Alarm Name"].Index].Value != null)
                    {
                        TagsTagAlarms listAlarms = new TagsTagAlarms();
                        TagsTagAlarmsAlarm alarm = new TagsTagAlarmsAlarm();

                        alarm.name = row.Cells[data.Columns["Alarm Name"].Index].Value.ToString();
                        if (row.Cells[data.Columns["Alarm SetPoint"].Index].Value != null)
                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("setpointA", row.Cells[data.Columns["Alarm SetPoint"].Index].Value.ToString()));
                        if (row.Cells[data.Columns["Alarm Notes"].Index].Value != null)
                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("notes", row.Cells[data.Columns["Alarm Notes"].Index].Value.ToString()));
                        if (row.Cells[data.Columns["Alarm Display Path"].Index].Value != null)
                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("displayPath", row.Cells[data.Columns["Alarm Display Path"].Index].Value.ToString()));
                        if (row.Cells[data.Columns["Alarm Priority"].Index].Value != null)
                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("priority", row.Cells[data.Columns["Alarm Priority"].Index].Value.ToString()));
                        if (row.Cells[data.Columns["Alarm Ack Notes Required"].Index].Value != null)
                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("ackNotesReqd", row.Cells[data.Columns["Alarm Ack Notes Required"].Index].Value.ToString()));
                        if (row.Cells[data.Columns["Alarm Ack Mode"].Index].Value != null)
                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("ackMode", row.Cells[data.Columns["Alarm Ack Mode"].Index].Value.ToString()));

                        listAlarms.Alarm = alarm;
                        untag.Alarms = listAlarms;

                    }

                    Taglist.Tag.Add(untag);
                }

                XmlSerializer xs = new XmlSerializer(typeof(Tags));

                using (StreamWriter writer = new StreamWriter(saveFile.FileName))
                {
                    xs.Serialize(writer, Taglist);
                }
            }
        }
        #endregion

        #region Project/Create
        public static void CreateUnityExchangeFile(DataGridView data)
        {

            #region init dialog box
            List<string> list_tag_erreur = new List<string>();
            bool problem = false;
            #endregion


            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".xsy";
            SaveFile.Filter = "Unity variable|*.xsy";
            SaveFile.FileName = "Var1";
            CreateVariables popup = new CreateVariables();
            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (SaveFile.ShowDialog() == DialogResult.OK)
                {

                    #region Create base variable
                    XmlSerializer xs = new XmlSerializer(typeof(VariablesExchangeFile));
                    using (StreamReader reader = new StreamReader(@"CreationTemplate.xml"))
                    {
                        UnityVariableFile = xs.Deserialize(reader) as VariablesExchangeFile;

                    }
                    #endregion

                    #region Create the variables

                    foreach (TeTag tag in TeTags)
                    {
                        if (string.IsNullOrEmpty(tag.Parent))
                        {
                            if (Types.Contains(tag.Type))
                            {
                                #region variable API
                                if (popup.checkBox1.Checked)
                                {
                                    if (tag.ReadyForUnity)
                                    {
                                        UnityVariableFile.dataBlock.Add(
                                        new VariablesExchangeFileVariables(
                                            popup.textBox1.Text + tag.Name,
                                            tag.Type,
                                            tag.APIAddress,
                                            tag.Comment + " - " + tag.AdditionnalComment,
                                            ""
                                            ));
                                    }
                                    else
                                    {
                                        list_tag_erreur.Add(tag.Name);
                                        problem = true;
                                    }
                                }


                                #endregion
                                #region variables pour la supervision
                                if (popup.checkBox2.Checked)
                                {
                                    if (!string.IsNullOrEmpty(tag.SupervisorVariableName))
                                    {

                                        UnityVariableFile.dataBlock.Add(
                                            new VariablesExchangeFileVariables(
                                                popup.textBox2.Text + tag.Name,
                                                tag.Type,
                                                tag.SUPAddress,
                                                "Recopie - " + tag.Comment,
                                                ""
                                                ));
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                if (!tag.IsEqt)
                                {
                                    UnityVariableFile.dataBlock.Add(
                                                                    new VariablesExchangeFileVariables(
                                                                        tag.Name,
                                                                        tag.Type,
                                                                        tag.APIAddress,
                                                                        tag.Comment + " - " + tag.AdditionnalComment,
                                                                        ""
                                                                        ));
                                }
                                else
                                {
                                    UnityVariableFile.dataBlock.Add(
                                                                    new VariablesExchangeFileVariables(
                                                                        tag.Name,
                                                                        tag.Type,
                                                                        "",
                                                                        tag.Comment + " - " + tag.AdditionnalComment,
                                                                        ""
                                                                        ));
                                }
                                
                            }
                        }
                        else
                        {
                            if (UnityVariableFile.DDTSource.FirstOrDefault(x => x.DDTName == tag.Parent) != null)
                            {
                                UnityVariableFile.DDTSource.First(x => x.DDTName == tag.Parent).structure.Add(new DDTExchangeFileDDTSourceVariables(tag.Name, tag.Type));
                            }
                            else
                            {
                                DDTExchangeFileDDTSource tempSource = new DDTExchangeFileDDTSource(tag.Parent);
                                tempSource.structure.Add(new DDTExchangeFileDDTSourceVariables(tag.Name, tag.Type));
                                UnityVariableFile.DDTSource.Add(tempSource);
                            }

                        }

                    }
                    #endregion

                    using (StreamWriter writer = new StreamWriter(SaveFile.FileName))
                    {
                        xs.Serialize(writer, UnityVariableFile);
                    }
                    if (problem)
                    {
                        MessageBox.Show("Certaine variables n'ont pas pu être exportées", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }



                }
            }


        }
        public static void CreateUnityDDTsFile()
        {
            #region init dialog box
            List<string> list_tag_erreur = new List<string>();
            bool problem = false;
            #endregion


            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".xdd";
            SaveFile.Filter = "Unity ddt|*.xdd";
            SaveFile.FileName = "DDTs1";
            CreateVariables popup = new CreateVariables();
            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (SaveFile.ShowDialog() == DialogResult.OK)
                {
                    DDTExchangeFile ExchangeFile = new DDTExchangeFile();
                    #region Create base variable
                    XmlSerializer xs = new XmlSerializer(typeof(DDTExchangeFile));
                    using (StreamReader reader = new StreamReader(@"DDTsTemplate.xml"))
                    {
                        ExchangeFile = xs.Deserialize(reader) as DDTExchangeFile;

                    }
                    #endregion

                    #region Create the variables

                    foreach (TeTag tag in TeTags)
                    {

                        if (!string.IsNullOrEmpty(tag.Parent))
                        {
                            if (ExchangeFile.DDTSource.FirstOrDefault(x => x.DDTName == tag.Parent) != null)
                            {
                                ExchangeFile.DDTSource.First(x => x.DDTName == tag.Parent).structure.Add(new DDTExchangeFileDDTSourceVariables(tag.Name, tag.Type));
                            }
                        }
                    }
                    #endregion

                    using (StreamWriter writer = new StreamWriter(SaveFile.FileName))
                    {
                        xs.Serialize(writer, UnityVariableFile);
                    }
                    if (problem)
                    {
                        MessageBox.Show("Certaine variables n'ont pas pu être exportées", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }



                }
            }
        }
        public static void ExportProjectToIgnition()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = ".xml";
            saveFile.AddExtension = true;
            saveFile.Filter = "Document XML|*.xml";
            #region Verify variables
            bool problem = false;
            #endregion
            Tags Taglist = new Tags();
            if (saveFile.ShowDialog() == DialogResult.OK)
            {

                foreach (TeTag tag in TeTags)
                {
                    if (tag.ReadyForIgnition)
                    {


                        if (tag.IsParent)
                        {
                            #region Creation parent
                            TagsTag parentTag = new TagsTag();
                            parentTag.name = tag.Name;
                            parentTag.path = "_types_";
                            parentTag.type = "UDT_DEF";
                            parentTag.Parameters = new TagsTagParameters();
                            parentTag.Property.Add(new TagsTagProperty("DataType", "2"));

                            foreach (KeyValuePair<string, string> param in tag.Parameters)
                            {
                                string actualType = "";
                                int testint = 0;
                                if (int.TryParse(param.Value, out testint))
                                {
                                    actualType = "Integer";
                                }
                                else
                                {
                                    actualType = "String";
                                }

                                parentTag.Parameters.Property.Add(new TagsTagParametersProperty(param.Key, actualType, param.Value));
                            }

                            #endregion
                            foreach (TeTag child in tag.Childs)
                            {
                                #region Ajout tag actual
                                string type = "";
                                TagsTag untag = new TagsTag();
                                #region basic stuff
                                untag.name = child.Name;
                                //untag.path = row.Cells[data.Columns["Path"].Index].Value.ToString();
                                untag.path = "";
                                untag.type = "OPC";//row.Cells[data.Columns["Type"].Index].Value.ToString();
                                if (!string.IsNullOrEmpty(child.Type))
                                {
                                    type = child.Type;
                                    if (type.ToUpper() == "BOOL")
                                        untag.Property.Add(new TagsTagProperty("DataType", "6"));
                                    if (type.ToUpper() == "INT" || type.ToUpper() == "WORD")
                                        untag.Property.Add(new TagsTagProperty("DataType", "2"));
                                    if (type.ToUpper() == "DINT")
                                        untag.Property.Add(new TagsTagProperty("DataType", "3"));
                                    if (type.ToUpper() == "BYTE")
                                        untag.Property.Add(new TagsTagProperty("DataType", "0"));
                                    if (type.ToUpper() == "FLOAT" || type.ToUpper() == "REAL")
                                        untag.Property.Add(new TagsTagProperty("DataType", "4"));


                                }
                                if (child.IgnitionOPCServer != null)
                                    untag.Property.Add(new TagsTagProperty("OPCServer", child.IgnitionOPCServer));

                                if (!string.IsNullOrEmpty(child.Comment))
                                {
                                    untag.Property.Add(new TagsTagProperty("Tooltip", child.Comment));
                                }
                                if (child.SUPAddress != null)
                                {
                                    

                                        string addr = child.SUPAddress;

                                        untag.Property.Add(new TagsTagProperty("OPCItemPath", addr));


                                    


                                }
                                #endregion
                                #region history stuff
                                /*
                                if (row.Cells[data.Columns["ScanClass"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("ScanClass", row.Cells[data.Columns["ScanClass"].Index].Value.ToString()));
                                if (row.Cells[data.Columns["HistoryEnabled"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("HistoryEnabled", row.Cells[data.Columns["HistoryEnabled"].Index].Value.ToString()));
                                if (row.Cells[data.Columns["PrimaryHistoryProvider"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("PrimaryHistoryProvider", row.Cells[data.Columns["PrimaryHistoryProvider"].Index].Value.ToString()));
                                if (row.Cells[data.Columns["HistoryMaxAgeMode"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("HistoryMaxAgeMode", row.Cells[data.Columns["HistoryMaxAgeMode"].Index].Value.ToString()));
                                    */
                                #endregion
                                #region alarm
                                if (child.AlarmActive != null)
                                {
                                    if (child.AlarmActive.Equals("True"))
                                    {
                                        TagsTagAlarms listAlarms = new TagsTagAlarms();
                                        TagsTagAlarmsAlarm alarm = new TagsTagAlarmsAlarm();

                                        alarm.name = child.AlarmText;
                                        if (child.AlarmSetPoint != null)
                                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("setpointA", child.AlarmSetPoint));
                                        //if (row.Cells[data.Columns["Alarm Notes"].Index].Value != null)
                                        // alarm.Property.Add(new TagsTagAlarmsAlarmProperty("notes", row.Cells[data.Columns["Alarm Notes"].Index].Value.ToString()));
                                        if (child.DisplayPath != null)
                                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("displayPath", child.DisplayPath));
                                        if (child.AlarmPriority != null)
                                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("priority", child.AlarmPriority));
                                        //if (row.Cells[data.Columns["Alarm Ack Notes Required"].Index].Value != null)
                                        alarm.Property.Add(new TagsTagAlarmsAlarmProperty("ackNotesReqd", "false"));
                                        //if (row.Cells[data.Columns["Alarm Ack Mode"].Index].Value != null)
                                        //alarm.Property.Add(new TagsTagAlarmsAlarmProperty("ackMode", row.Cells[data.Columns["Alarm Ack Mode"].Index].Value.ToString()));

                                        listAlarms.Alarm = alarm;
                                        untag.Alarms = listAlarms;
                                    }


                                }
                                #endregion
                                #region Float gestion
                                if (!string.IsNullOrEmpty(child.ScaleMode))
                                {
                                    untag.Property.Add(new TagsTagProperty("ScaleMode", child.ScaleMode));
                                }
                                if (!string.IsNullOrEmpty(child.ClampMode))
                                {
                                    untag.Property.Add(new TagsTagProperty("ClampMode", child.ClampMode));
                                }
                                if (!string.IsNullOrEmpty(child.RawMin))
                                {
                                    untag.Property.Add(new TagsTagProperty("RawLow", child.RawMin));
                                }
                                if (!string.IsNullOrEmpty(child.RawMax))
                                {
                                    untag.Property.Add(new TagsTagProperty("RawHigh", child.RawMax));
                                }
                                if (!string.IsNullOrEmpty(child.ScaledMin))
                                {
                                    untag.Property.Add(new TagsTagProperty("ScaledLow", child.ScaledMin));
                                }
                                if (!string.IsNullOrEmpty(child.ScaledMax))
                                {
                                    untag.Property.Add(new TagsTagProperty("ScaledHigh", child.ScaledMax));
                                }
                                #endregion
                                
                                parentTag.Tag.Add(untag);
                                

                                #endregion
                            }
                            Taglist.Tag.Add(parentTag);

                        }
                        else
                        {
                            if (!Types.Contains(tag.Type.ToUpper()))
                            {
                                TagsTag untag = new TagsTag();
                                untag.name = tag.Name;
                                untag.path = "";
                                untag.type = "UDT_INST";
                                untag.Parameters = new TagsTagParameters();
                                #region Définition adresse
                                foreach (KeyValuePair<string, string> param in tag.Parameters)
                                {
                                    string actualType = "";
                                    int testint = 0;
                                    if (int.TryParse(param.Value, out testint))
                                    {
                                        actualType = "Integer";
                                    }
                                    else
                                    {
                                        actualType = "String";
                                    }
                                    untag.Parameters.Property.Add(new TagsTagParametersProperty(param.Key, actualType, param.Value));
                                }
                                if (SIEMENS)
                                {
                                    untag.Parameters.Property.Add(new TagsTagParametersProperty("DB", "Integer", tag.DB));
                                }
                                
                                #endregion

                                //untag.Property.Add(new TagsTagProperty("DataType", Taglist.Tag.First(x => x.name == tag.Type).Property.First(y => y.name == "DataType").Value));
                                untag.Property.Add(new TagsTagProperty("UDTParentType", tag.Type));
                                Taglist.Tag.Add(untag);

                            }
                            else
                            {
                                #region  tag creation
                                string type = "";
                                TagsTag untag = new TagsTag();
                                #region basic stuff
                                untag.name = tag.Name;
                                //untag.path = row.Cells[data.Columns["Path"].Index].Value.ToString();
                                untag.path = "";
                                untag.type = "OPC";//row.Cells[data.Columns["Type"].Index].Value.ToString();
                                if (!string.IsNullOrEmpty(tag.Type))
                                {
                                    type = tag.Type;
                                    if (type.ToUpper() == "BOOL")
                                        untag.Property.Add(new TagsTagProperty("DataType", "6"));
                                    if (type.ToUpper() == "INT" || type.ToUpper() == "WORD")
                                        untag.Property.Add(new TagsTagProperty("DataType", "2"));
                                    if (type.ToUpper() == "DINT")
                                        untag.Property.Add(new TagsTagProperty("DataType", "3"));
                                    if (type.ToUpper() == "BYTE")
                                        untag.Property.Add(new TagsTagProperty("DataType", "0"));
                                    if (type.ToUpper() == "FLOAT" || type.ToUpper() == "REAL")
                                        untag.Property.Add(new TagsTagProperty("DataType", "4"));


                                }
                                if (tag.IgnitionOPCServer != null)
                                    untag.Property.Add(new TagsTagProperty("OPCServer", tag.IgnitionOPCServer));

                                if (!string.IsNullOrEmpty(tag.Comment))
                                {
                                    untag.Property.Add(new TagsTagProperty("Tooltip", tag.Comment));
                                }
                                if (tag.SUPAddress != null)
                                {
                                    if (type != "")
                                    {

                                        string addr = tag.SUPAddress;
                                        string API = tag.IgnitionDevice;
                                        if (addr.Contains("%MW"))
                                        {
                                            if (type.ToUpper() == "BOOL")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HR" + addr.Replace("%MW", "")));
                                            if (type.ToUpper() == "INT")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HR" + addr.Replace("%MW", "")));
                                            if (type.ToUpper() == "DINT")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HRI" + addr.Replace("%MW", "")));
                                            if (type.ToUpper() == "FLOAT")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HRF" + addr.Replace("%MW", "")));
                                        }
                                        else if (addr.Contains("%MX"))
                                        {
                                            if (type.ToUpper() == "BOOL")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "C" + (12289 + int.Parse(addr[3].ToString()) * 15 + int.Parse(addr[addr.Length - 1].ToString()))));

                                            if (type.ToUpper() == "INT")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HR" + addr.Replace("%MX", "")));
                                            if (type.ToUpper() == "DINT")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HRI" + addr.Replace("%MX", "")));
                                            if (type.ToUpper() == "FLOAT")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "HRF" + addr.Replace("%MX", "")));
                                        }
                                        else if (addr.Contains("%M") & !addr.Contains("%MW"))
                                        {
                                            if (type.ToUpper() == "BOOL")
                                                untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "C" + addr.Replace("%M", "")));
                                        }
                                        else
                                        {
                                            if (SIEMENS)
                                            {
                                                if (tag.Type.ToUpper().Equals("BOOL"))
                                                {
                                                    untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "DB" + tag.DB + ",X" + addr));
                                                }
                                                if (tag.Type.ToUpper().Equals("INT"))
                                                {
                                                    untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "DB" + tag.DB + ",I" + addr.Replace(".0","")));
                                                }
                                                if (tag.Type.ToUpper().Equals("REAL") || tag.Type.ToUpper().Equals("FLOAT"))
                                                {
                                                    untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "DB" + tag.DB + ",REAL" + addr.Replace(".0","")));
                                                }
                                                if (tag.Type.ToUpper().Equals("BYTE"))
                                                {
                                                    untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "DB" + tag.DB + ",B" + addr.Replace(".0", "")));
                                                }
                                                if (tag.Type.ToUpper().Equals("WORD"))
                                                {
                                                    untag.Property.Add(new TagsTagProperty("OPCItemPath", "[" + API + "]" + "DB" + tag.DB + ",W" + addr.Replace(".0", "")));
                                                }
                                            }

                                        }

                                    }
                                }
                                #endregion
                                #region history stuff
                                /*
                                if (row.Cells[data.Columns["ScanClass"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("ScanClass", row.Cells[data.Columns["ScanClass"].Index].Value.ToString()));
                                if (row.Cells[data.Columns["HistoryEnabled"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("HistoryEnabled", row.Cells[data.Columns["HistoryEnabled"].Index].Value.ToString()));
                                if (row.Cells[data.Columns["PrimaryHistoryProvider"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("PrimaryHistoryProvider", row.Cells[data.Columns["PrimaryHistoryProvider"].Index].Value.ToString()));
                                if (row.Cells[data.Columns["HistoryMaxAgeMode"].Index].Value != null)
                                    untag.Property.Add(new TagsTagProperty("HistoryMaxAgeMode", row.Cells[data.Columns["HistoryMaxAgeMode"].Index].Value.ToString()));
                                    */
                                #endregion
                                #region alarm
                                if (tag.AlarmActive != null)
                                {
                                    if (tag.AlarmActive.Equals("True"))
                                    {
                                        TagsTagAlarms listAlarms = new TagsTagAlarms();
                                        TagsTagAlarmsAlarm alarm = new TagsTagAlarmsAlarm();

                                        alarm.name = tag.AlarmText;
                                        if (tag.AlarmSetPoint != null)
                                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("setpointA", tag.AlarmSetPoint));
                                        //if (row.Cells[data.Columns["Alarm Notes"].Index].Value != null)
                                        // alarm.Property.Add(new TagsTagAlarmsAlarmProperty("notes", row.Cells[data.Columns["Alarm Notes"].Index].Value.ToString()));
                                        if (tag.DisplayPath != null)
                                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("displayPath", tag.DisplayPath));
                                        if (tag.AlarmPriority != null)
                                            alarm.Property.Add(new TagsTagAlarmsAlarmProperty("priority", tag.AlarmPriority));
                                        //if (row.Cells[data.Columns["Alarm Ack Notes Required"].Index].Value != null)
                                        alarm.Property.Add(new TagsTagAlarmsAlarmProperty("ackNotesReqd", "false"));
                                        //if (row.Cells[data.Columns["Alarm Ack Mode"].Index].Value != null)
                                        //alarm.Property.Add(new TagsTagAlarmsAlarmProperty("ackMode", row.Cells[data.Columns["Alarm Ack Mode"].Index].Value.ToString()));

                                        listAlarms.Alarm = alarm;
                                        untag.Alarms = listAlarms;
                                    }


                                }
                                #endregion
                                #region Float gestion
                                if (!string.IsNullOrEmpty(tag.ScaleMode))
                                {
                                    untag.Property.Add(new TagsTagProperty("ScaleMode", tag.ScaleMode));
                                }
                                if (!string.IsNullOrEmpty(tag.ClampMode))
                                {
                                    untag.Property.Add(new TagsTagProperty("ClampMode", tag.ClampMode));
                                }
                                if (!string.IsNullOrEmpty(tag.RawMin))
                                {
                                    untag.Property.Add(new TagsTagProperty("RawLow", tag.RawMin));
                                }
                                if (!string.IsNullOrEmpty(tag.RawMax))
                                {
                                    untag.Property.Add(new TagsTagProperty("RawHigh", tag.RawMax));
                                }
                                if (!string.IsNullOrEmpty(tag.ScaledMin))
                                {
                                    untag.Property.Add(new TagsTagProperty("ScaledLow", tag.ScaledMin));
                                }
                                if (!string.IsNullOrEmpty(tag.ScaledMax))
                                {
                                    untag.Property.Add(new TagsTagProperty("ScaledHigh", tag.ScaledMax));
                                }
                                #endregion
                                Taglist.Tag.Add(untag);
                                #endregion
                            }

                        }

                    }
                    else
                    {
                        problem = true;
                    }
                }


                XmlSerializer xs = new XmlSerializer(typeof(Tags));

                using (StreamWriter writer = new StreamWriter(saveFile.FileName))
                {
                    xs.Serialize(writer, Taglist);
                }
            }





        }
        public static void CreateWagoVariables()
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".txt";
            SaveFile.Filter = "Text File| *.txt";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = "WagoVariables1.txt";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                #region Text stream
                CreateVariables Popup = new CreateVariables();
                if (Popup.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(SaveFile.OpenFile()))
                    {
                        foreach (TeTag tag in TeTags)
                        {
                            if (Popup.checkBox1.Checked)
                                writer.WriteLine(Popup.textBox1.Text + tag.Name + " AT " + tag.APIAddress + ":" + tag.Type + ";(*"+tag.Comment+"*)");
                            if (Popup.checkBox2.Checked)
                                writer.WriteLine(Popup.textBox2.Text + tag.Name + " AT " + tag.SUPAddress + ":" + tag.Type + ";");
                        }
                    }
                }

                #endregion
            }

        }
        public static void CreateWagoRecopy()
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".txt";
            SaveFile.Filter = "Text File| *.txt";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = "WagoRecopy.txt";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                #region Text stream
                CreateVariables Popup = new CreateVariables();
                if (Popup.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(SaveFile.OpenFile()))
                    {
                        foreach (TeTag tag in TeTags)
                        {

                            writer.WriteLine(Popup.textBox2.Text + tag.Name + " := " + Popup.textBox1.Text + tag.Name + ";(*" + tag.Comment + "*)");


                        }
                    }
                }

                #endregion
            }
        }
        public static void CreateUnityGENCOMFile()
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".xdb";
            SaveFile.Filter = "GENCOM File for Unity|*.xdb";
            SaveFile.FileName = "GENCOM1";

            GENCOM_Popup popup = new GENCOM_Popup(TeTags);
            if(popup.ShowDialog() == DialogResult.OK)
            {
                FBExchangeFile baseFBe;
                XmlSerializer xs = new XmlSerializer(typeof(FBExchangeFile));

                using (StreamReader reader = new StreamReader(@"bExchangeFile.xdb"))
                {
                    baseFBe = xs.Deserialize(reader) as FBExchangeFile;
                    GENCOM.GenerateEquipements(popup.SortedTags);
                }

                if(SaveFile.ShowDialog() == DialogResult.OK)
                {
                    FBExchangeFile blankFBE;
                    using (StreamReader reader = new StreamReader(@"blankExchangeFile.xdb"))
                    {
                        blankFBE = xs.Deserialize(reader) as FBExchangeFile;
                        
                    }

                    FBExchangeFile FBeToSerialize;
                    FBeToSerialize =  GENCOM.GetUnityFile(blankFBE);

                    using (StreamWriter writer = new StreamWriter(SaveFile.FileName))
                    {
                        xs.Serialize(writer, FBeToSerialize);

                    }
                }
            }
           
        }

        public static void CreateUnitySTRecopie()
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".xst";
            SaveFile.Filter = "ST Section| *.xst";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = "UnitySTRecopy.xst";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                #region Text stream
                CreateVariables Popup = new CreateVariables();
                Popup.checkBox1.Enabled = false;
                Popup.checkBox2.Enabled = false;
                if (Popup.ShowDialog() == DialogResult.OK)
                {
                    STExchangeFile ExchangeFile = new STExchangeFile();
                    #region Create file
                    XmlSerializer xs = new XmlSerializer(typeof(STExchangeFile));
                    using (StreamReader reader = new StreamReader(@"STTemplate.xst"))
                    {
                        ExchangeFile = xs.Deserialize(reader) as STExchangeFile;

                    }
                    #endregion
                    using (StreamWriter writer = new StreamWriter(SaveFile.OpenFile()))
                    {
                        string toto = "";
                        foreach (TeTag tag in TeTags)
                        {

                            

                            if (tag.ReadyForUnity & !string.IsNullOrEmpty(tag.SUPAddress) &! tag.IsEqt)
                            {

                                #region Creation Variables
                                ExchangeFile.dataBlock.Add(
                                        new VariablesExchangeFileVariables(
                                            Popup.textBox1.Text + tag.Name,
                                            tag.Type,
                                            tag.APIAddress,
                                            tag.Comment + " - " + tag.AdditionnalComment,
                                            ""
                                            ));
                                #endregion

                                #region Creation variables
                                ExchangeFile.dataBlock.Add(
                                            new VariablesExchangeFileVariables(
                                                Popup.textBox2.Text + tag.Name,
                                                tag.Type,
                                                tag.SUPAddress,
                                                "Recopie - " + tag.Comment,
                                                ""
                                                ));
                                #endregion

                                if (tag.AlarmActive.ToUpper() == "TRUE")
                                {
                                    ExchangeFile.program.STSource = ExchangeFile.program.STSource + Popup.textBox2.Text + tag.Name + " := NOT " + Popup.textBox1.Text + tag.Name + ";(*" + tag.Comment + "*)" + "\n";
                                }
                                else
                                {
                                    ExchangeFile.program.STSource = ExchangeFile.program.STSource + Popup.textBox2.Text + tag.Name + " := " + Popup.textBox1.Text + tag.Name + ";(*" + tag.Comment + "*)" + "\n";
                                }
                            }

                        }

                        xs.Serialize(writer, ExchangeFile);
                    }
                }

                #endregion
            }
        }
        public static void CreateUnityGENCOMSections()
        {

            string InitSection = "";
            Dictionary<string, STExchangeFile> Sections = new Dictionary<string, STExchangeFile>();
            XmlSerializer xs = new XmlSerializer(typeof(STExchangeFile));
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".xst";
            SaveFile.Filter = "ST Section| *.xst";
            SaveFile.FilterIndex = 1;
            foreach(TeTag tag in TeTags.Where(x => x.IsEqt))
            {
                if (!Sections.ContainsKey(tag.SectionName))
                {
                    STExchangeFile ExchangeFile = new STExchangeFile();
                    #region Create file
                   
                    using (StreamReader reader = new StreamReader(@"STTemplate.xst"))
                    {
                        ExchangeFile = xs.Deserialize(reader) as STExchangeFile;
                    }
                    #endregion
                    ExchangeFile.program.identProgram.name = tag.SectionName;
                    Sections.Add(tag.SectionName, ExchangeFile);
                }

                    
              
            }

            // Section INIT
            STExchangeFile ExchFile = new STExchangeFile();
            #region Create file

            using (StreamReader reader = new StreamReader(@"STTemplate.xst"))
            {
                ExchFile = xs.Deserialize(reader) as STExchangeFile;
            }
            #endregion
            ExchFile.program.identProgram.name = "INIT_GENCOM";
            Sections.Add("INIT_GENCOM", ExchFile);

            foreach (KeyValuePair<string,STExchangeFile> ExFile in Sections)
            {
                //Programme ST
                string sourceBegin = "";
                string source = "";

               
                SaveFile.FileName = ExFile.Key+".xst";
                var dir = SaveFile.FileName;
                if (SaveFile.ShowDialog() == DialogResult.OK)
                {
                    #region Text stream

                    using (StreamWriter writer = new StreamWriter(SaveFile.OpenFile()))
                    {
                        #region Creation ecquit def
                        ExFile.Value.dataBlock.Add(
                                new VariablesExchangeFileVariables(
                                    "Acq_Def_"+ExFile.Key,
                                    "BOOL",
                                    "",
                                    "Acquit défauts section de communication gencom : "+ ExFile.Key,
                                    ""
                                    ));
                        #endregion

                        #region Creation jeton
                        ExFile.Value.dataBlock.Add(
                                new VariablesExchangeFileVariables(
                                    "Token_"+ ExFile.Key,
                                    "INT",
                                    "",
                                    "Jeton section de communication gencom : " + ExFile.Key,
                                    ""
                                    ));
                        #endregion

                        
                        int index = 0;
                        foreach (TeTag tag in TeTags.Where(x => x.SectionName == ExFile.Key))
                        {

                            
                                #region Creation Variables
                                ExFile.Value.dataBlock.Add(
                                        new VariablesExchangeFileVariables(
                                            tag.Name,
                                            tag.Type,
                                            tag.SUPAddress,
                                            tag.Comment + " - " + tag.AdditionnalComment,
                                            ""
                                            ));
                            ExFile.Value.dataBlock.Add(
                                        new VariablesExchangeFileVariables(
                                            "GENCOM_" + tag.Type + "_" + tag.Name,
                                            "GENCOM_" + tag.Type,
                                            "",
                                            "Instance de " + "GENCOM_" + tag.Type,
                                            ""
                                            ));
                            #endregion


                            source = source + "GENCOM_"+tag.Type+"_"+tag.Name+"(Index:= "+index+"(*INT *),\n";
                            source = source + "     Adresse_Eqt:= '"+tag.COMAddress+ "'(*STRING *),\n";
                            source = source + "     Acquit:= Acq_Def_"+ExFile.Key+ "(*BOOL*),\n";
                            source = source + "     Token:= Token_"+ ExFile.Key+ "(*INT *),\n";
                            source = source + "     Eqt_var => "+tag.Name+ "(* *),\n";
                            source = source + "     Def_Com => " + tag.Name + ".Def_Com(*BOOL *),\n";
                            source = source + "     Com_Act => " + tag.Name + ".Act_Com(*BOOL *),\n";
                            source = source + "     Nb_Ech => " + tag.Name + ".NB_ECH(*DINT *),\n";
                            source = source + "     Fin_Com => " + tag.Name + ".Fin_Com(*BOOL *));\n\n\n";

                            index++;

                            InitSection = InitSection + "GENCOM_" + tag.Type + "_" + tag.Name + ".TimeOut := 100; \n";



                        }


                        if(ExFile.Key != "INIT_GENCOM")
                        {
                            sourceBegin = sourceBegin + "If(Token_" + ExFile.Key + " >= " + (index) + ") then Token_" + ExFile.Key + " := 0; end_if; \n \n";
                            ExFile.Value.program.STSource = sourceBegin + source;
                        }
                        else
                        {
                            ExFile.Value.program.STSource = InitSection;
                        }
                        
                        xs.Serialize(writer, ExFile.Value);
                    }


                    #endregion
                }

            }


            
        }
        public static void CreateUnityLDRecopie()
        {
            LDExchangeFile ExchangeFile = new LDExchangeFile();
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".xld";
            SaveFile.Filter = "Unity Section|*.xld";
            SaveFile.FileName = "Section1";
            CreateVariables popup = new CreateVariables();
            popup.checkBox1.Enabled = false;
            popup.checkBox2.Enabled = false;
            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (SaveFile.ShowDialog() == DialogResult.OK)
                {

                    #region Create file
                    XmlSerializer xs = new XmlSerializer(typeof(LDExchangeFile));
                    using (StreamReader reader = new StreamReader(@"LDTemplate.xld"))
                    {
                        ExchangeFile = xs.Deserialize(reader) as LDExchangeFile;

                    }
                    #endregion

                    #region Fill
                    byte x = 0;
                    byte y = 0;
                    foreach (TeTag tag in TeTags)
                    {

                        #region variable API


                        if (tag.ReadyForUnity &! string.IsNullOrEmpty(tag.SUPAddress) &! tag.IsEqt)
                        {
                            #region Creation Variables
                            ExchangeFile.dataBlock.Add(
                                    new VariablesExchangeFileVariables(
                                        popup.textBox1.Text + tag.Name,
                                        tag.Type,
                                        tag.APIAddress,
                                        tag.Comment + " - " + tag.AdditionnalComment,
                                        ""
                                        ));
                            #endregion

                            #region Creation variables
                            ExchangeFile.dataBlock.Add(
                                        new VariablesExchangeFileVariables(
                                            popup.textBox2.Text + tag.Name,
                                            tag.Type,
                                            tag.SUPAddress,
                                            "Recopie - " + tag.Comment,
                                            ""
                                            ));
                            #endregion
                            if (!string.IsNullOrEmpty(tag.AlarmActive))
                            {
                                if (tag.AlarmActive.ToUpper() == "FALSE")
                                {
                                    ExchangeFile.program.LDSource.networkLD.typeLine.Add(
                                new LDExchangeFileProgramLDSourceNetworkLDTypeLine(
                                    null,
                                    null,
                                    null,
                                    new LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine(1)
                                ));
                                    ExchangeFile.program.LDSource.networkLD.textBox.Add(new LDExchangeFileProgramLDSourceNetworkLDTextBox(11, 1, x, y, new string[] { tag.Comment }));
                                    ExchangeFile.program.LDSource.networkLD.typeLine.Add(
                                        new LDExchangeFileProgramLDSourceNetworkLDTypeLine(
                                            new LDExchangeFileProgramLDSourceNetworkLDTypeLineContact("openContact", popup.textBox2.Text + tag.Name),
                                            new LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink(9),
                                            new LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil("coil", popup.textBox1.Text + tag.Name),
                                            null
                                            ));
                                }
                                else
                                {
                                    ExchangeFile.program.LDSource.networkLD.typeLine.Add(
                                new LDExchangeFileProgramLDSourceNetworkLDTypeLine(
                                    null,
                                    null,
                                    null,
                                    new LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine(1)
                                ));
                                    ExchangeFile.program.LDSource.networkLD.textBox.Add(new LDExchangeFileProgramLDSourceNetworkLDTextBox(11, 1, x, y, new string[] { tag.Comment }));
                                    ExchangeFile.program.LDSource.networkLD.typeLine.Add(
                                        new LDExchangeFileProgramLDSourceNetworkLDTypeLine(
                                            new LDExchangeFileProgramLDSourceNetworkLDTypeLineContact("closedContact", popup.textBox2.Text + tag.Name),
                                            new LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink(9),
                                            new LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil("coil", popup.textBox1.Text + tag.Name),
                                            null
                                            ));
                                }
                            }
                            else
                            {
                                ExchangeFile.program.LDSource.networkLD.typeLine.Add(
                                new LDExchangeFileProgramLDSourceNetworkLDTypeLine(
                                    null,
                                    null,
                                    null,
                                    new LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine(1)
                                ));
                                ExchangeFile.program.LDSource.networkLD.textBox.Add(new LDExchangeFileProgramLDSourceNetworkLDTextBox(11, 1, x, y, new string[] { tag.Comment }));
                                ExchangeFile.program.LDSource.networkLD.typeLine.Add(
                                    new LDExchangeFileProgramLDSourceNetworkLDTypeLine(
                                        new LDExchangeFileProgramLDSourceNetworkLDTypeLineContact("openContact", popup.textBox2.Text + tag.Name),
                                        new LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink(9),
                                        new LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil("coil", popup.textBox1.Text + tag.Name),
                                        null
                                        ));
                            }
                            y += 2;

                        }
                        else
                        {

                        }



                        #endregion

                    }
                    #endregion

                    using (StreamWriter writer = new StreamWriter(SaveFile.FileName))
                    {
                        xs.Serialize(writer, ExchangeFile);
                    }

                }





            }
        }
        #endregion

        #region Outils/Modbus TCP/Lire
        
        public static int[] ReadInputRegisters(string addr,int reg,int l)
        {
            try
            {
                ModbusClient modbusClient = new ModbusClient(addr, 502);    //Ip-Address and Port of Modbus-TCP-Server
                modbusClient.Connect();                                                    //Connect to Server
                int[] readInputRegisters = modbusClient.ReadInputRegisters(reg, l);    //Read 10 Holding Registers from Server, starting with Address 1
                return readInputRegisters;
            }
            catch (Exception)
            {
                
                MessageBox.Show("Error", "Impossible d'atteindre la cible sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
               
            


        }
        public static int[] ReadHoldingRegisters(string addr,int reg,int l)
        {

            ModbusClient modbusClient = new ModbusClient(addr, 502);    //Ip-Address and Port of Modbus-TCP-Server
            modbusClient.Connect();                                                    //Connect to Server
            int[] readInputRegisters = modbusClient.ReadInputRegisters(reg, l);    //Read 10 Holding Registers from Server, starting with Address 1
            return readInputRegisters;



        }
        public static bool[] ReadCoils(string addr,int reg,int l)
        {

            ModbusClient modbusClient = new ModbusClient(addr, 502);    //Ip-Address and Port of Modbus-TCP-Server
            modbusClient.Connect();                                                    //Connect to Server
            Boolean[] readInputRegisters = modbusClient.ReadCoils(reg, l);    //Read 10 Holding Registers from Server, starting with Address 1
            return readInputRegisters;
        }
        
        #endregion

        #region Outils
        public static int FindIndexByName(string name, DataGridViewColumnCollection laList)
        {
            int index = 0;
            for (int i = 0; i < laList.Count; i++)
            {
                if (laList[i].Name == name)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
        public static int FindIndexByHeader(string name, DataGridViewColumnCollection laList)
        {
            int index = 505;
            for (int i = 0; i < laList.Count; i++)
            {
                if (laList[i].HeaderText == name)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
        public static DataGridViewCell GetStartCell(DataGridView data)
        {
            //get the smallest row,column index
            if (data.SelectedCells.Count == 0)
                return null;

            int rowIndex = data.Rows.Count - 1;
            int colIndex = data.Columns.Count - 1;

            foreach (DataGridViewCell cell in data.SelectedCells)
            {
                if (cell.RowIndex < rowIndex)
                    rowIndex = cell.RowIndex;
                if (cell.ColumnIndex < colIndex)
                    colIndex = cell.ColumnIndex;
            }

            return data[colIndex, rowIndex];
        }

        public static Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
        {
            Dictionary<int, Dictionary<int, string>>
            copyValues = new Dictionary<int, Dictionary<int, string>>();

            String[] lines = clipboardValue.Split('\n');

            for (int i = 0; i <= lines.Length - 1; i++)
            {
                copyValues[i] = new Dictionary<int, string>();
                String[] lineContent = lines[i].Split('\t');

                //if an empty cell value copied, then set the dictionary with an empty string
                //else Set value to dictionary
                if (lineContent.Length == 0)
                    copyValues[i][0] = string.Empty;
                else
                {
                    for (int j = 0; j <= lineContent.Length - 1; j++)
                        copyValues[i][j] = lineContent[j];
                }
            }
            return copyValues;
        }

        public static void AutoAdressVariables(DataGridView data, List<Mot> availableAPI, bool _sup, bool address_bits)
        {
            foreach (DataGridViewRow row in data.Rows)
            {

                if (row.Cells[FindIndexByName("Type_Field", data.Columns)].Value.ToString().ToUpper().Contains("BOOL") && address_bits)
                {
                    if (address_bits)
                        AssignBit(availableAPI, row, data, _sup);
                }
                else if (row.Cells[FindIndexByName("Type_Field", data.Columns)].Value.ToString().ToUpper().Contains("INT")
                    & !row.Cells[FindIndexByName("Type_Field", data.Columns)].Value.ToString().ToUpper().Contains("DINT") ||
                    row.Cells[FindIndexByName("Type_Field", data.Columns)].Value.ToString().ToUpper().Contains("WORD"))
                {
                    AssignINT(availableAPI, row, data, _sup);
                }
                else if (row.Cells[FindIndexByName("Type_Field", data.Columns)].Value.ToString().ToUpper().Contains("DINT"))
                {
                    AssignDINT(availableAPI, row, data, _sup);
                }


            }
        }

        public static void AutoAdressBits(List<Bit> Bits, DataGridView data, bool _sup)
        {
            foreach (DataGridViewRow row in data.Rows)
            {
                if (row.Cells[FindIndexByName("Type_Field", data.Columns)].Value.ToString().ToUpper().Contains("BOOL"))
                {
                    foreach (Bit b in Bits)
                    {
                        if (b.Available)
                        {
                            if (_sup)
                            {
                                row.Cells["Sup_Address_Field"].Value = "%M" + b.Number;
                            }
                            else
                            {
                                row.Cells["API_Addr_Field"].Value = "%M" + b.Number;


                            }
                            b.Available = false;
                            break;
                        }
                    }
                }

            }
        }

        public static void AssignBit(List<Mot> _availableAPI, DataGridViewRow _row, DataGridView _data, bool sup)
        {
            foreach (Mot m in _availableAPI)
            {
                m.Check();
                if (m.Available & !m.Assigned)
                {
                    foreach (Bit b in m.Bits)
                    {
                        if (b.Available)
                        {
                            if (sup)
                            {

                                _row.Cells[FindIndexByName("Sup_Address_Field", _data.Columns)].Value = "%MW" + m.Address + "." + b.Number;
                            }
                            else
                            {
                                _row.Cells[FindIndexByName("API_Addr_Field", _data.Columns)].Value = "%MW" + m.Address + "." + b.Number;

                            }
                            b.Available = false;
                            return;

                        }
                    }
                }
            }
        }

        public static void AssignINT(List<Mot> _availableAPI, DataGridViewRow _row, DataGridView _data, bool sup)
        {
            foreach (Mot m in _availableAPI)
            {
                m.Check();
                if (!m.Assigned & !m.BitsUsed)
                {
                    if (sup)
                    {

                        _row.Cells[FindIndexByName("Sup_Address_Field", _data.Columns)].Value = "%MW" + m.Address;
                    }
                    else
                    {
                        _row.Cells[FindIndexByName("API_Addr_Field", _data.Columns)].Value = "%MW" + m.Address;

                    }
                    m.Available = false;
                    m.Assigned = true;
                    return;

                }
            }
        }

        public static void AssignDINT(List<Mot> _availableAPI, DataGridViewRow _row, DataGridView _data, bool sup)
        {

            for (int i = 0; i < _availableAPI.Count; i++)
            {
                _availableAPI[i].Check();
                if (_availableAPI[i].Address % 2 == 0)
                {
                    if (!_availableAPI[i].Assigned & !_availableAPI[i].BitsUsed && !_availableAPI[i + 1].Assigned & !_availableAPI[i + 1].BitsUsed)
                    {

                        if (sup)
                        {

                            _row.Cells[FindIndexByName("Sup_Address_Field", _data.Columns)].Value = "%MW" + _availableAPI[i].Address;
                        }
                        else
                        {
                            _row.Cells[FindIndexByName("API_Addr_Field", _data.Columns)].Value = "%MW" + _availableAPI[i].Address;

                        }
                        _availableAPI[i].Available = false;
                        _availableAPI[i].Assigned = true;
                        _availableAPI[i + 1].Available = false;
                        _availableAPI[i + 1].Assigned = true;

                        return;



                    }
                }
            }
        }

        public static void SaveUnder(string directory, DataGridView data)
        {
            System.Data.DataTable t = new System.Data.DataTable("exportData");

            foreach (DataGridViewColumn col in data.Columns)
            {

                t.Columns.Add(col.Name, typeof(string));
            }

            foreach (DataGridViewRow row in data.Rows)
            {
                DataRow dr = t.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] = row.Cells[i].Value;
                }

                t.Rows.Add(dr);
            }





            XmlSerializer xs = new XmlSerializer(typeof(System.Data.DataTable));
            using (StreamWriter writer = new StreamWriter(directory))
            {
                xs.Serialize(writer, t);
            }
        }

        private static void copyAlltoClipboard(DataGridView dataGridView1)
        {
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        #endregion
    }

    public class Mot
    {

        public bool Available { get; set; }
        public bool Assigned { get; set; }
        public bool BitsUsed { get; set; }
        public int Address { get; set; }
        public Bit[] Bits { get; set; }

        public Mot()
        {
            Available = true;
            Assigned = false;
            Bits = new Bit[16];
            for(int i = 0; i< Bits.Count(); i++)
            {
                Bits[i] = new Bit();
            }
        }

        public Mot(int n)
        {
            Address = n;
            Assigned = false;
            Available = true;

            Bits = new Bit[16];
            for (int i = 0; i < Bits.Count(); i++)
            {
                Bits[i] = new Bit(i);
            }
        }

        public void Check()
        {
            Available = false;
            foreach(Bit b in Bits)
            {
                if(b.Available == true)
                {
                    Available = true;
                }
                else
                {
                    BitsUsed = true;
                }
            }
            if (Assigned)
            {
                Available = false;
            }
        }
    }

    public class Bit
    {
        public bool Available { get; set; }
        public int Number { get; set; }
        public int Address { get; set; }
        public Bit()
        {
            Available = true;
        }

        public Bit(int n)
        {
            Available = true;
            Number = n;
        }
    }

    
    


    public class Log
    {
        #region Ok Pas Ok
        public Dictionary<int, bool> Tab_M_R_AdressesCheck { get; set; }
        public Dictionary<int, bool> Tab_M_L_AdressesCheck { get; set; }
        public Dictionary<int, bool> Tab_M_ECR_AdressesCheck { get; set; }
        public Dictionary<int, bool> Tab_B_L_AdressesCheck { get; set; }
        public Dictionary<int, bool> Tab_B_R_AdressesCheck { get; set; }
        public Dictionary<int, bool> Tab_B_ECR_AdressesCheck { get; set; }

        public Dictionary<int, bool> Tab_M_R_RequestCheck { get; set; }
        public Dictionary<int, bool> Tab_M_L_RequestCheck { get; set; }
        public Dictionary<int, bool> Tab_M_ECR_RequestCheck { get; set; }
        public Dictionary<int, bool> Tab_B_L_RequestCheck { get; set; }
        public Dictionary<int, bool> Tab_B_R_RequestCheck { get; set; }
        public Dictionary<int, bool> Tab_B_ECR_RequestCheck { get; set; }
        #endregion

        #region Correspondances
        public Dictionary<string,string> Tab_M_R_CorrespCheck { get; set; }
        public Dictionary<string,string> Tab_M_L_CorrespCheck { get; set; }
        public Dictionary<string,string> Tab_M_ECR_CorrespCheck { get; set; }
        public Dictionary<string,string> Tab_B_L_CorrespCheck { get; set; }
        public Dictionary<string,string> Tab_B_R_CorrespCheck { get; set; }
        public Dictionary<string,string> Tab_B_ECR_CorrespCheck { get; set; }

        
        #endregion

        List<string> LogFile { get; set; }


        public Log()
        {
            Tab_M_R_AdressesCheck = new Dictionary<int,bool>();
            Tab_M_L_AdressesCheck = new Dictionary<int,bool>();
            Tab_M_ECR_AdressesCheck = new Dictionary<int,bool>();
            Tab_B_L_AdressesCheck = new Dictionary<int,bool>();
            Tab_B_R_AdressesCheck = new Dictionary<int,bool>();
            Tab_B_ECR_AdressesCheck = new Dictionary<int,bool>();

            Tab_M_R_CorrespCheck = new Dictionary<string, string>();
            Tab_M_L_CorrespCheck = new Dictionary<string, string>();
            Tab_M_ECR_CorrespCheck = new Dictionary<string, string>();
            Tab_B_L_CorrespCheck = new Dictionary<string, string>();
            Tab_B_R_CorrespCheck = new Dictionary<string, string>();
            Tab_B_ECR_CorrespCheck = new Dictionary<string, string>();

            Tab_M_R_RequestCheck = new Dictionary<int,bool>();
            Tab_M_L_RequestCheck = new Dictionary<int,bool>();
            Tab_M_ECR_RequestCheck = new Dictionary<int,bool>();
            Tab_B_L_RequestCheck = new Dictionary<int,bool>();
            Tab_B_R_RequestCheck = new Dictionary<int,bool>();
            Tab_B_ECR_RequestCheck = new Dictionary<int,bool>();

            LogFile = new List<string>();
        }

        public bool GenerateLogFile(string SepamName,int NbMr,int NbMl,int NbMe,int NbBR,int NbBl)
        {
            bool Etat = true;
            //Nom du fichier vérifé
            LogFile.Add( "****" + SepamName + "**** \n");
            //Types Vérifié
            #region Types Verif
            LogFile.Add( "\n***Adresses*** \n");
            #region Tab_M_R
            LogFile.Add( "\n**Tab_M_R** \n");
            LogFile.Add("Lecture " + NbMr + " mots rapides");
            int i = 0;
            foreach (KeyValuePair<int,bool> element in Tab_M_R_AdressesCheck)
            {
                LogFile.Add("Mot " + element.Key + " : " + element.Value + ", Valeurs :"+Tab_M_R_CorrespCheck.ElementAt(i) +" \n");
                i++;
                if (!element.Value)
                    Etat = false;
            }
            i = 0;
            #endregion
            #region Tab_M_L
            LogFile.Add("\n**Tab_M_L** \n");
            LogFile.Add("Lecture " + NbMl + " mots lents");
            foreach (KeyValuePair<int, bool> element in Tab_M_L_AdressesCheck)
            {
                LogFile.Add("Mot " + element.Key + " : " + element.Value + ", Valeurs :" + Tab_M_L_CorrespCheck.ElementAt(i)+ " \n");
                i++;
                if (!element.Value)
                    Etat = false;
            }
            i = 0;
            #endregion

            #region Tab_M_ECR
            LogFile.Add("\n**Tab_M_ECR** \n");
            LogFile.Add("Ecriture " + NbMe + " mots ");
            foreach (KeyValuePair<int, bool> element in Tab_M_ECR_AdressesCheck)
            {
                LogFile.Add("Mot " + element.Key + " : " + element.Value + ", Valeurs :" + Tab_M_ECR_CorrespCheck.ElementAt(i)+" \n");
                i++;
                if (!element.Value)
                    Etat = false;
            }
            i = 0;
            #endregion
            #region Tab_B_R
            LogFile.Add("\n**Tab_B_R** \n");
            LogFile.Add("Lecture " + NbBR + " Bits rapides");
            foreach (KeyValuePair<int, bool> element in Tab_B_R_AdressesCheck)
            {
                LogFile.Add("Bit " + element.Key + " : " + element.Value + ", Valeurs :" + Tab_B_R_CorrespCheck.ElementAt(i)+" \n");
                i++;
                if (!element.Value)
                    Etat = false;
            }
            i = 0;
            #endregion

            #region Tab_B_L
            LogFile.Add("\n**Tab_B_L** \n");
            LogFile.Add("Lecture " + NbBl + " Bits lents");
            foreach (KeyValuePair<int, bool> element in Tab_B_L_AdressesCheck)
            {
                LogFile.Add("Bit " + element.Key + " : " + element.Value + ", Valeurs :" + Tab_B_L_CorrespCheck.ElementAt(i)+ " \n");
                i++;
                if (!element.Value)
                    Etat = false;
            }
            i = 0;
            #endregion
            #region Tab_B_ECR
            LogFile.Add("\n**Tab_B_ECR** \n");
            
            foreach (KeyValuePair<int, bool> element in Tab_B_ECR_AdressesCheck)
            {
                LogFile.Add("Bit " + element.Key + " : " + element.Value + ", Valeurs :" + Tab_B_ECR_CorrespCheck.ElementAt(i)+" \n");
                i++;
                if (!element.Value)
                    Etat = false;
            }
            i = 0;
            #endregion

            #endregion

            #region Requêtes vérif
            LogFile.Add("\n***Request*** \n");
            #region Tab_M_R
            LogFile.Add("\n**Tab_M_R** \n");
            foreach (KeyValuePair<int, bool> element in Tab_M_R_RequestCheck)
            {
                LogFile.Add("Mot " + element.Key + " : " + element.Value + " \n");
                if (!element.Value)
                    Etat = false;
            }

            #endregion
            #region Tab_M_L
            LogFile.Add("\n**Tab_M_L** \n");
            foreach (KeyValuePair<int, bool> element in Tab_M_L_RequestCheck)
            {
                LogFile.Add("Mot " + element.Key + " : " + element.Value + " \n");
                if (!element.Value)
                    Etat = false;
            }

            #endregion

            #region Tab_M_ECR
            LogFile.Add("\n**Tab_M_ECR** \n");
            foreach (KeyValuePair<int, bool> element in Tab_M_ECR_RequestCheck)
            {
                LogFile.Add("Mot " + element.Key + " : " + element.Value + " \n");
                if (!element.Value)
                    Etat = false;
            }

            #endregion
            #region Tab_B_R
            LogFile.Add("\n**Tab_B_R** \n");
            foreach (KeyValuePair<int, bool> element in Tab_B_R_RequestCheck)
            {
                LogFile.Add("Bit " + element.Key + " : " + element.Value + " \n");
                if (!element.Value)
                    Etat = false;
            }

            #endregion

            #region Tab_B_L
            LogFile.Add("\n**Tab_B_L** \n");
            foreach (KeyValuePair<int, bool> element in Tab_B_L_RequestCheck)
            {
                LogFile.Add("Bit " + element.Key + " : " + element.Value + " \n");
                if (!element.Value)
                    Etat = false;
            }

            #endregion
            #region Tab_B_ECR
            LogFile.Add("\n**Tab_B_ECR** \n");
            foreach (KeyValuePair<int, bool> element in Tab_B_ECR_RequestCheck)
            {
                LogFile.Add("Bit " + element.Key + " : " + element.Value + " \n");
                if (!element.Value)
                    Etat = false;
            }

            #endregion

            #endregion

            LogFile.Add( "\n ###Status###\n");
            LogFile.Add(Etat.ToString());
            #region text
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".txt";
            SaveFile.Filter = "Text File| *.txt";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = SepamName.Replace(".xdb","")+".txt";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                #region Text stream


                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(SaveFile.OpenFile()))
                {
                    foreach(string s in LogFile)
                    {
                        writer.WriteLine(s);
                    }
               




                }


                #endregion
            }
            #endregion
            return Etat;

            

        }

    }

    public class Table
    {
        public List<Champ> myChamps { get; set; }
        public string Nom { get; set; }

        public Table()
        {
            myChamps = new List<Champ>();
        }

        public Table(string _nom)
        {
            myChamps = new List<Champ>();
            Nom = _nom;
        }
    }

    public class Champ
    {
        public string type { get; set; }
        public string Nom { get; set; }
        public Champ() { }
        public Champ(string _nom, string _type)
        {
            Nom = _nom;
            type = _type;

        }


    }
}
