using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Tag_Explorer
{
    public partial class Form1 : Form
    {

        #region instance variables
        public bool IsProjectOpen = false;
        public List<DataGridViewCell> SavedCells;
        public bool canCloseDropDown = true;
        public bool left_main_dropdown = false;
        public bool mouseHover = false;
        public bool ProjectSaved = false;
        public List<string> Parameters;
        public DataGridView DataGrid;

        #endregion
        public Form1()
        {
            InitializeComponent();
            Time_aff.Text = DateTime.Now.ToLongTimeString();
            
            enregistrerToolStripMenuItem.Enabled = false;
            enregistrerSousToolStripMenuItem.Enabled = false;
            
            projectToolStripMenuItem.Enabled = false;

            SavedCells = new List<DataGridViewCell>();

            treeView1.Visible = false;
            treeView1.Enabled = false;
            splitContainer1.SplitterDistance = 0;
            treeView1.Dock = DockStyle.None;
            
            Parameters = new List<string>();

            #region Initialize stuff
            Save.Enabled = enregistrerToolStripMenuItem.Enabled;
            SaveUnder.Enabled = enregistrerSousToolStripMenuItem.Enabled;
            #endregion

            #region Personnal events
            attributesToolStripMenuItem.DropDown.Closing += new ToolStripDropDownClosingEventHandler(DropDown_Closing);
            ignitionToolStripMenuItem1.DropDown.Closing += new ToolStripDropDownClosingEventHandler(DropDown_Closing);
            minValueToolStripMenuItem.DropDown.Closing += new ToolStripDropDownClosingEventHandler(DropDown_Closing);
            maxValueToolStripMenuItem.DropDown.Closing += new ToolStripDropDownClosingEventHandler(DropDown_Closing);

            #endregion

        }

        //Click sur Importer/Excel/Unity
        private void unityToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
            Engine.ImportFromExcelAsUnity(DataGrid);
            
        }
        //Click sur Importer/Variables/Unity
        private void unityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGrid.DataSource = null;
            DataGrid.Rows.Clear();
            DataGrid.Columns.Clear();
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.DefaultExt = ".xsy";
            OpenFile.CheckFileExists = true;
            
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(VariablesExchangeFile));

                    using (StreamReader reader = new StreamReader(OpenFile.FileName))
                    {
                        Engine.UnityVariableFile = xs.Deserialize(reader) as VariablesExchangeFile;
                    }

                    #region DataGridView Editing
                    DataGrid.ColumnCount = 5;
                    #region Columns creation
                    //"Name" Column
                    DataGrid.Columns[0].Name = "name";

                    //"typeName" Column
                    DataGrid.Columns[1].Name = "typeName";


                    //"topologicalAddress" Column
                    DataGrid.Columns[2].Name = "topologicalAddress";


                    //"Comment" Column
                    DataGrid.Columns[3].Name = "comment";

                    DataGrid.Columns[4].Name = "variableInit";
                    #endregion

                    foreach (VariablesExchangeFileVariables v in Engine.UnityVariableFile.dataBlock)
                    {
                        if (v.variableInit == null)
                        {
                            string[] row = new string[] { v.name, v.typeName, v.topologicalAddress, v.comment };
                            DataGrid.Rows.Add(row);
                        }
                        else
                        {
                            string[] row = new string[] { v.name, v.typeName, v.topologicalAddress, v.comment};
                            DataGrid.Rows.Add(row);
                        }

                    }
                    #endregion
                }
                catch (Exception)
                {
                    MessageBox.Show("Le fichier selectionné ne correspond pas à un fichier de variables Unity", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
                
            }

            
        }

        // CLick sur Exporter/Excel/Unity
        private void unityToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Engine.ExportUnityVariablesToExcel(DataGrid);
        }

        //Click sur Exporter/Variables/Unity
        private void unityToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Engine.ExportToUnity(DataGrid); 
        }

        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked )
                e.Cancel = true;
        }

        private void tagExplorerProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                Form1 newForm = new Form1();
                newForm.IsProjectOpen = true;
                newForm.Show();
                newForm.NewProject();
            }
            else
            {

                tabControl1.TabPages.Add("Data");
                tabControl1.TabPages.Add("Programs");
                tabControl1.TabPages.Add("Network");
                DataGrid = new DataGridView()
                {
                    DataSource = DataGrid,
                    Dock = DockStyle.Fill,
                    BackgroundColor = Color.White,
                    RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
                };
                tabControl1.TabPages[0].Select();
                tabControl1.TabPages[0].Controls.Add(DataGrid);
                tabControl1.TabPages[1].Controls.Add(new RichTextBox());
                IsProjectOpen = true;

                NewProject();
            }
            
        }

        private void attributesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

      
        //Global key events
        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                Copy();
            }

            if(e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                PasteClipboardValue();
            }

            if(e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                foreach (DataGridViewCell cell in DataGrid.SelectedCells)
                {
                    if (cell.GetType() == typeof(DataGridViewCheckBoxCell))
                    {
                        cell.Value = CheckState.Unchecked;
                    }
                    else
                    {
                        cell.Value = string.Empty;
                    }
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                UpdateVariables();
                UpdateTree();
            }
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(DataGrid.ColumnCount > 0)
            {
                int value;
                if (e.KeyCode == Keys.Enter)
                {
                   
                    if (int.TryParse(toolStripTextBox1.Text, out value))
                    {
                        for (int i = 0; i < value; i++)
                        {
                            try
                            {
                                DataGrid.Rows.Add();
                            }
                            catch (Exception)
                            {

                                DataTable table = (DataTable)DataGrid.DataSource;
                                DataRow newRow = table.NewRow();

                                // Add the row to the rows collection.
                                table.Rows.Add(newRow);
                            }
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("La valeur entrée ne correspond pas à un entier, veuillez la modifer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time_aff.Text = DateTime.Now.ToLongTimeString();
        }

        private void PasteClipboardValue()
        {
            if (DataGrid.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a cell to paste", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            DataGridViewCell StartCell = Engine.GetStartCell(DataGrid);

            Dictionary<int, Dictionary<int, string>> cbValue = Engine.ClipBoardValues(Clipboard.GetText());

            int iRowIndex = StartCell.RowIndex;

            foreach (int rowkey in cbValue.Keys)
            {
                int iColIndex = StartCell.ColumnIndex;
                foreach(int cellkey in cbValue[rowkey].Keys)
                {

                    if(iColIndex <= DataGrid.Columns.Count -1 && iRowIndex <= DataGrid.Rows.Count - 1)
                    {
                        DataGridViewCell cell = DataGrid[iColIndex, iRowIndex];
                        cell.Value = cbValue[rowkey][cellkey];
                    }
                    iColIndex++;
                }
                iRowIndex++;
            }
        }

        private void alarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (alarmToolStripMenuItem.CheckState == CheckState.Unchecked)
            {
                alarmToolStripMenuItem.Checked = true;
                alarmToolStripMenuItem.CheckState = CheckState.Checked;
                DataGridViewCheckBoxColumn Temp_Col = new DataGridViewCheckBoxColumn();
                Temp_Col.Name = "Alarm_Field";
                Temp_Col.HeaderText = "Alarm On";
                DataGrid.Columns.Add(Temp_Col);

                DataGridViewColumn Temp_Col2 = new DataGridViewColumn(new DataGridViewTextBoxCell());
                Temp_Col2.Name = "Active_Field";
                Temp_Col2.HeaderText = "Alarme Active Value";
                DataGrid.Columns.Add(Temp_Col2);

                

                DataGrid.Columns.Add("Priority", "Priority");
                DataGrid.Columns.Add("Alarm_Text", "Alarm Text");


            }
            else if (alarmToolStripMenuItem.CheckState == CheckState.Checked)
            {
                alarmToolStripMenuItem.Checked = false;
                alarmToolStripMenuItem.CheckState = CheckState.Unchecked;

                DataGrid.Columns.RemoveAt(Engine.FindIndexByName("Alarm_Field", DataGrid.Columns));
                DataGrid.Columns.RemoveAt(Engine.FindIndexByName("Active_Field", DataGrid.Columns));
                DataGrid.Columns.RemoveAt(DataGrid.Columns["Priority"].Index);
                DataGrid.Columns.RemoveAt(DataGrid.Columns["Alarm_Text"].Index);
            }
        }

        private void Copy()
        {
            if (DataGrid.GetClipboardContent() != null)
            {
                Clipboard.SetDataObject(DataGrid.GetClipboardContent());
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                
                contextMenuStrip1.Show();

            }
        }

        private void DataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                contextMenuStrip1.Show(Cursor.Position);
               
            }
        }
        #region Context Menu Events
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
            foreach(DataGridViewCell cell in DataGrid.SelectedCells)
            {
                if(cell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    cell.Value = CheckState.Unchecked;
                }
                else
                {
                    cell.Value = string.Empty;
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboardValue();
        }


        #endregion

        private void unityVariablesExchangeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void autoAssignVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Popup_auto_assign fen = new Popup_auto_assign(DataGrid);
            if(fen.ShowDialog() == DialogResult.OK)
            {
                DataGrid = fen.data;
            }
        }

        private void inputOutputListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.ExportUnityVariablesToIOList(DataGrid);
        }

        private void enregistrerSousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".teproj";
            SaveFile.Filter = "Tag Explorer Project|*.teproj";
            SaveFile.FileName = "TE_Project1";
            if(SaveFile.ShowDialog() == DialogResult.OK)
            {
                Engine.SaveUnder(SaveFile.FileName, DataGrid);
                Engine.actualProjectPath = SaveFile.FileName;
                enregistrerToolStripMenuItem.Enabled = true;
                ProjectSaved = true;
            }
        }

        private void projetTagExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            #region Open tag explorer project
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.DefaultExt = ".teproj";
            OpenFile.Filter = "Projet Tag Explorer | *.teproj";
            OpenFile.CheckFileExists = true;
            OpenFile.Title = "Open Project";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (IsProjectOpen)
                {
                    Form1 newForm = new Form1();
                    newForm.Show();
                    newForm.OpenProject(OpenFile.FileName, newForm.DataGrid);
                    newForm.ProjectSaved = true;





                }
                else
                {
                    OpenProject(OpenFile.FileName, DataGrid);
                    ProjectSaved = true;
                }

            }
            #endregion

        }

        public void NewProject()
        {
            projectToolStripMenuItem.Enabled = true;
            enregistrerSousToolStripMenuItem.Enabled = true;
            Engine.CreateNewTEProject(DataGrid);
        }

        public bool OpenProject(string dir,DataGridView data)
        {


            if (Engine.OpenTEProject(dir, data, new Form1()))
            {
                IsProjectOpen = true;
                projectToolStripMenuItem.Enabled = true;
                enregistrerToolStripMenuItem.Enabled = true;
                enregistrerSousToolStripMenuItem.Enabled = true;
                CheckUserConf();
                Engine.actualProjectPath = dir;
                return true;
            }
            else
                return false;

            
        }

        public void CheckUserConf()
        {
            #region checkuserconf
            if (UserConf.IOScanning)
                iOScanningVariableToolStripMenuItem.CheckState = CheckState.Checked;
            else
                iOScanningVariableToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.COMAddress)
                cOMAddressToolStripMenuItem.CheckState = CheckState.Checked;
            else
                cOMAddressToolStripMenuItem.CheckState = CheckState.Unchecked;
            //
            if (UserConf.AdditionalComment)
                additionnalCommentToolStripMenuItem.CheckState = CheckState.Checked;
            else
                additionnalCommentToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.Alarm)
                alarmToolStripMenuItem.CheckState = CheckState.Checked;
            else
                alarmToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.SupAddress)
                supervisorAddressToolStripMenuItem.CheckState = CheckState.Checked;
            else
                supervisorAddressToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.ClampMode)
                clampModeToolStripMenuItem.CheckState = CheckState.Checked;
            else
                clampModeToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.ScaleMode)
                scaleModeToolStripMenuItem.CheckState = CheckState.Checked;
            else
                scaleModeToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.RawMin)
                rawToolStripMenuItem.CheckState = CheckState.Checked;
            else
                rawToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.RawMax)
                rawToolStripMenuItem1.CheckState = CheckState.Checked;
            else
                rawToolStripMenuItem1.CheckState = CheckState.Unchecked;
            if (UserConf.ScaledMin)
                scaledToolStripMenuItem.CheckState = CheckState.Checked;
            else
                scaledToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (UserConf.ScaledMax)
                scaledToolStripMenuItem1.CheckState = CheckState.Checked;
            else
                scaledToolStripMenuItem1.CheckState = CheckState.Unchecked;
            if (UserConf.Ignition)
            {
                textField1ToolStripMenuItem.CheckState = CheckState.Checked;
                UserConf.GridReadyForIgnition = true;
            }
            else
                textField1ToolStripMenuItem.CheckState = CheckState.Unchecked;

            if (UserConf.Parent)
                parentToolStripMenuItem.CheckState = CheckState.Checked;
            else
                parentToolStripMenuItem.CheckState = CheckState.Unchecked;

            if (UserConf.SIEMENS)
                sIEMENSToolStripMenuItem.CheckState = CheckState.Checked;
            else
                sIEMENSToolStripMenuItem.CheckState = CheckState.Unchecked;

            #endregion
        }

        private void columnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog chooseColor = new ColorDialog();

            if(chooseColor.ShowDialog() == DialogResult.OK)
            {
                foreach(DataGridViewColumn col in DataGrid.SelectedColumns)
                {
                    col.DefaultCellStyle.BackColor = chooseColor.Color;
                }
            }
        }

        private void rowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog chooseColor = new ColorDialog();

            if (chooseColor.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewRow row in DataGrid.SelectedRows)
                {
                    row.DefaultCellStyle.BackColor = chooseColor.Color;
                    foreach(DataGridViewCell cell in row.Cells)
                    {
                        SavedCells.Add((DataGridViewCell)cell.Clone());
                    }
                }
            }
        }

        private void cellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog chooseColor = new ColorDialog();

            if (chooseColor.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewCell cell in DataGrid.SelectedCells)
                {
                    cell.Style.BackColor = chooseColor.Color;
                    SavedCells.Add((DataGridViewCell)cell.Clone());
                }
            }
        }

        private void DataGrid_Sorted(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in DataGrid.Rows)
            {
                foreach(DataGridViewCell cell in row.Cells)
                {
                    if(SavedCells.FirstOrDefault(x => x.Value == cell.Value) != null)
                        cell.Style.BackColor = SavedCells.FirstOrDefault(x => x.Value == cell.Value).Style.BackColor;
                    
                }
            }
        }

        private void ignitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGrid.DataSource = null;
            DataGrid.Rows.Clear();
            DataGrid.Columns.Clear();
            projectToolStripMenuItem.Enabled = false;
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.DefaultExt = ".xml";
            OpenFile.CheckFileExists = true;

            if(OpenFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Tags));

                    using (StreamReader reader = new StreamReader(OpenFile.FileName))
                    {
                        Engine.IgnitionVariableFile = xs.Deserialize(reader) as Tags;
                    }

                    #region DataGridView Editing
                    DataGrid.ColumnCount = 18;
                    #region Columns creation
                    //"Name" Column
                    DataGrid.Columns[0].Name = "Name";

                    //"typeName" Column
                    DataGrid.Columns[1].Name = "Path";


                    //"topologicalAddress" Column
                    DataGrid.Columns[2].Name = "Type";

                    DataGrid.Columns[3].Name = "DataType";
                    DataGrid.Columns[4].Name = "OPCServer";
                    DataGrid.Columns[5].Name = "OPCItemPath";
                    DataGrid.Columns[6].Name = "ScanClass";
                    DataGrid.Columns[7].Name = "HistoryEnabled";
                    DataGrid.Columns[8].Name = "PrimaryHistoryProvider";
                    DataGrid.Columns[9].Name = "HistoryMaxAgeMode";
                    DataGrid.Columns[10].Name = "Alarm Name";
                    DataGrid.Columns[11].Name = "Alarm SetPoint";
                    DataGrid.Columns[12].Name = "Alarm Notes";
                    DataGrid.Columns[13].Name = "Alarm Display Path";
                    DataGrid.Columns[14].Name = "Alarm Priority";
                    DataGrid.Columns[15].Name = "Alarm Ack Notes Required";
                    DataGrid.Columns[16].Name = "Alarm Ack Mode";



                    #endregion

                    foreach (TagsTag tag in Engine.IgnitionVariableFile.Tag)
                    {
                        DataGrid.Rows.Add();

                        DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[0].Value = tag.name; ;
                        DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[1].Value = tag.path;
                        DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[2].Value = tag.type;
                        if (tag.Property.FirstOrDefault(x => x.name == "DataType") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[3].Value = tag.Property.FirstOrDefault(x => x.name == "DataType").Value;
                        if (tag.Property.FirstOrDefault(x => x.name == "OPCServer") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[4].Value = tag.Property.FirstOrDefault(x => x.name == "OPCServer").Value;
                        if (tag.Property.FirstOrDefault(x => x.name == "OPCItemPath") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[5].Value = tag.Property.FirstOrDefault(x => x.name == "OPCItemPath").Value;
                        if (tag.Property.FirstOrDefault(x => x.name == "ScanClass") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[6].Value = tag.Property.FirstOrDefault(x => x.name == "ScanClass").Value;
                        if (tag.Property.FirstOrDefault(x => x.name == "HistoryEnabled") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[7].Value = tag.Property.FirstOrDefault(x => x.name == "HistoryEnabled").Value;
                        if (tag.Property.FirstOrDefault(x => x.name == "PrimaryHistoryProvider") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[8].Value = tag.Property.FirstOrDefault(x => x.name == "PrimaryHistoryProvider").Value;
                        if (tag.Property.FirstOrDefault(x => x.name == "HistoryMaxAgeMode") != null)
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[9].Value = tag.Property.FirstOrDefault(x => x.name == "HistoryMaxAgeMode").Value;
                        if (tag.Alarms != null)
                        {
                            DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[10].Value = tag.Alarms.Alarm.name;
                            if (tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "setpointA") != null)
                                DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[11].Value = tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "setpointA").Value;
                            if (tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "notes") != null)
                                DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[12].Value = tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "notes").Value;
                            if (tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "displayPath") != null)
                                DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[13].Value = tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "displayPath").Value;
                            if (tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "priority") != null)
                                DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[14].Value = tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "priority").Value;
                            if (tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "ackNotesReqd") != null)
                                DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[15].Value = tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "ackNotesReqd").Value;
                            if (tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "ackMode") != null)
                                DataGrid.Rows[DataGrid.Rows.Count - 1].Cells[16].Value = tag.Alarms.Alarm.Property.FirstOrDefault(x => x.name == "ackMode").Value;
                        }



                    }
                    #endregion
                }
                catch (Exception)
                {
                    MessageBox.Show("Le fichier choisit ne correspond pas à un fichier de variables ignition", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
                
            }
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.ImportFromExcelAsUnity(DataGrid);
        }

        private void excelToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            #region open excel project
            if (IsProjectOpen)
            {
                Form1 newForm = new Form1();
                if (newForm.OpenExcelProject(newForm.DataGrid))
                {

                    newForm.Show();
                }
                else
                {
                    newForm.Dispose();
                }

            }
            else
            {

                OpenExcelProject(DataGrid);
            }
            #endregion



        }
        public bool OpenExcelProject(DataGridView data)
        {
            if (Engine.ImportProjectFromExcel(data, new Form1()))
            {
                CheckUserConf();
                IsProjectOpen = true;
                projectToolStripMenuItem.Enabled = true;
                enregistrerSousToolStripMenuItem.Enabled = true;
                return true;
            }
            else
                return false;
        }

        private void ignitionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                Engine.ExportProjectToIgnition();
            }
            else
            {
                Engine.ExportToIgnition(DataGrid);
            }
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void hierarchieToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if(hierarchieToolStripMenuItem.CheckState == CheckState.Checked)
            {
                treeView1.Visible = true;
                treeView1.Enabled = true;
                treeView1.Dock = DockStyle.Fill;
                splitContainer1.SplitterDistance = 220;
                DataGrid.BringToFront();
                
            }
            else if (hierarchieToolStripMenuItem.CheckState == CheckState.Unchecked)
            {
                treeView1.Visible = false;
                treeView1.Enabled = false;
                splitContainer1.SplitterDistance = 0;
                treeView1.Dock = DockStyle.None;
            }
        }

       

        bool doOnce = false;
        public void UpdateTree()
        {
            
            if (IsProjectOpen)
            {
                projectToolStripMenuItem.Enabled = true;
            }
            
            if (!doOnce)
            {
                treeView1.Nodes.Add("Data", "Data");
                treeView1.Nodes.Add("Programs", "Programs");

                treeView1.Nodes["Data"].Nodes.Add("Types", "Types");
                
                treeView1.Nodes["Data"].Nodes.Add("Variables", "Variables");
                treeView1.Nodes["Data"].Nodes.Add("Equipments", "Equipments");
                doOnce = true;
            }
            treeView1.Nodes["Data"].Nodes["Variables"].Nodes.Clear();
            treeView1.Nodes["Data"].Nodes["Types"].Nodes.Clear();
            treeView1.Nodes["Data"].Nodes["Equipments"].Nodes.Clear();

            foreach (TeTag tag in Engine.TeTags)
            {
                //Variables de base
                if (!string.IsNullOrEmpty(tag.Name) && !tag.GENCOMREADY && !tag.IsParent && !tag.IsEqt)
                {
                    
                    treeView1.Nodes["Data"].Nodes["Variables"].Nodes.Add(tag.Name, tag.Name);
                    if (!string.IsNullOrEmpty(tag.APIAddress))
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes.Add(tag.APIAddress, "API Address : " + tag.APIAddress);
                    if (!string.IsNullOrEmpty(tag.SUPAddress))
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes.Add(tag.SUPAddress, "Supervisor Address : " + tag.SUPAddress);
                    if (!string.IsNullOrEmpty(tag.Type))
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes.Add(tag.Type, "Type : " + tag.Type);
                    if (tag.myAlarm.Exists)
                    {
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes.Add("Alarm", "Alarm");

                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes["Alarm"].Nodes.Add("Alarm's text : " + tag.AlarmText);
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes["Alarm"].Nodes.Add("Alarm's display path : " + tag.DisplayPath);
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes["Alarm"].Nodes.Add("Alarm's set point : " + tag.AlarmSetPoint);
                        int priority;
                        int.TryParse(tag.AlarmPriority, out priority);
                        treeView1.Nodes["Data"].Nodes["Variables"].Nodes[tag.Name].Nodes["Alarm"].Nodes.Add("Alarm's priority : " + ((TeTag.Priorities)priority).ToString());
                        
                    }
                }

                //Variables d'équipement
                if(!string.IsNullOrEmpty(tag.Name) && tag.IsEqt)
                {

                    treeView1.Nodes["Data"].Nodes["Equipments"].Nodes.Add(tag.Name, tag.Name);
                    if (!string.IsNullOrEmpty(tag.APIAddress))
                        treeView1.Nodes["Data"].Nodes["Equipments"].Nodes[tag.Name].Nodes.Add(tag.APIAddress, "API Address : " + tag.APIAddress);
                    if (!string.IsNullOrEmpty(tag.SUPAddress))
                        treeView1.Nodes["Data"].Nodes["Equipments"].Nodes[tag.Name].Nodes.Add(tag.SUPAddress, "Supervisor Address : " + tag.SUPAddress);
                    if (!string.IsNullOrEmpty(tag.Type))
                        treeView1.Nodes["Data"].Nodes["Equipments"].Nodes[tag.Name].Nodes.Add(tag.Type, "Type : " + tag.Type);
                    if(!string.IsNullOrEmpty(tag.COMAddress))
                        treeView1.Nodes["Data"].Nodes["Equipments"].Nodes[tag.Name].Nodes.Add(tag.COMAddress, "COM Address : " + tag.COMAddress);
                }

                //Variables parentes (datatypes)
                if (!string.IsNullOrEmpty(tag.Name) && tag.IsParent)
                {

                    treeView1.Nodes["Data"].Nodes["Types"].Nodes.Add(tag.Name, tag.Name);
                    treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes.Add("Variables", "Variables");
                    foreach(TeTag child in tag.Childs)
                    {
                        if (!string.IsNullOrEmpty(child.Name))
                        {

                            treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes.Add(child.Name, child.Name);
                            if (!string.IsNullOrEmpty(child.APIAddress))
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes.Add(child.APIAddress, "API Address : " + child.APIAddress);
                            if (!string.IsNullOrEmpty(child.SUPAddress))
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes.Add(child.SUPAddress, "Supervisor Address : " + child.SUPAddress);
                            if (!string.IsNullOrEmpty(child.COMAddress))
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes.Add(child.COMAddress, "COM Address : " + child.COMAddress);
                            if (!string.IsNullOrEmpty(child.Type))
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes.Add(child.Type, "Type : " + child.Type);
                            if(child.myAlarm.Exists)
                            {
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes.Add("Alarm", "Alarm");

                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes["Alarm"].Nodes.Add("Alarm's text : " + child.AlarmText);
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes["Alarm"].Nodes.Add("Alarm's display path : " + child.DisplayPath);
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes["Alarm"].Nodes.Add("Alarm's set point : " + child.AlarmSetPoint);
                                int priority;
                                int.TryParse(child.AlarmPriority, out priority);
                                treeView1.Nodes["Data"].Nodes["Types"].Nodes[tag.Name].Nodes["Variables"].Nodes[child.Name].Nodes["Alarm"].Nodes.Add("Alarm's priority : " + ((TeTag.Priorities)priority).ToString());

                            }
                        }

                    }
                    
                }



            }
        }

        public void UpdateVariables()
        {
            if (IsProjectOpen)
            {
                projectToolStripMenuItem.Enabled = true;
            }
            Engine.TeTags = new List<TeTag>();
            Engine.UpdateEngine();
            #region Update parameters
            Parameters = new List<string>();
            foreach (DataGridViewColumn col in DataGrid.Columns)
            {
                if (!Engine.ColumnsNames.Contains(col.HeaderText))
                {
                    Parameters.Add(col.HeaderText);
                }
            }
            #endregion

            foreach (DataGridViewRow row in DataGrid.Rows)
            {
                TeTag temptag = new TeTag();
                foreach(DataGridViewCell cell in row.Cells)
                {
                    if(cell.Value != null)
                    {
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Variable Name")
                        {
                            temptag = new TeTag(cell.Value.ToString());
                        }
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Comment")
                        {
                            temptag.Comment = cell.Value.ToString();
                        }
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Type")
                            temptag.Type = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Additional Comment")
                            temptag.AdditionnalComment = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Supervisor Address")
                        {
                            var s = cell.Value.ToString();
                           
                            temptag.SUPAddress = s;
                            
                            
                            
                        }
                            
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "API Address")
                            temptag.APIAddress = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Alarm On" && cell.Value != null)
                            temptag.AlarmActive = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Alarm Active Value")
                            temptag.AlarmSetPoint = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Priority")
                            temptag.AlarmPriority = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Alarm Text")
                            temptag.AlarmText = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "OPCServer")
                            temptag.IgnitionOPCServer = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Device")
                            temptag.IgnitionDevice = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Alarm Filter")
                            temptag.DisplayPath = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Clamp Mode")
                            temptag.ClampMode = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Scale Mode")
                            temptag.ScaleMode = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Scaled Min")
                            temptag.ScaledMin = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Scaled Max")
                            temptag.ScaledMax = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Raw Min")
                            temptag.RawMin = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Raw Max")
                            temptag.RawMax = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "COM Address")
                            temptag.COMAddress = cell.Value.ToString();
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "Parent")
                        {
                            if (!string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                if (Engine.TeTags.FirstOrDefault(x => cell.Value.ToString().Contains(x.Name)) != null)
                                {
                                    temptag.HasParent = true;
                                }
                                else
                                {
                                    TeTag parent = new TeTag();
                                    parent.Name = cell.Value.ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "");
                                    parent.IsParent = true;
                                    Engine.TeTags.Add(parent);
                                    temptag.HasParent = true;
                                }
                                temptag.Parent = cell.Value.ToString();
                            }
                            
                        }
                        if (DataGrid.Columns[cell.ColumnIndex].HeaderText == "DB")
                        {
                            var s = cell.Value.ToString();

                            temptag.DB = s;



                        }



                        foreach (string param in Parameters)
                        {
                            if(DataGrid.Columns[cell.ColumnIndex].HeaderText == param)
                            {
                                temptag.Parameters.Add(param, cell.Value.ToString());
                            }
                        }
                    }
                    
                }
                temptag.CheckTag();
                if (!string.IsNullOrEmpty(temptag.Name))
                {
                    if (temptag.HasParent)
                    {
                        Engine.TeTags.First(x => x.Name == temptag.Parent.Replace("\n","").Replace("\r","").Replace(" ","")).Childs.Add(temptag);
                    }
                    else
                    {
                        Engine.TeTags.Add(temptag);
                    }
                    
                }
                    
            }

            foreach(TeTag tag in Engine.TeTags.Where(x => x.IsParent))
            {
                tag.CheckTag();
            }
            foreach (TeTag tag in Engine.TeTags)
            {
                tag.CheckTag();
            }

            var toto = 1;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textField1ToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if(textField1ToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("OPCServer"))
                {
                    DataGrid.Columns.Add("OPCServer", "OPCServer");
                    DataGrid.Columns.Add("Device", "Device");

                    UserConf.GridReadyForIgnition = true;
                }
                
            }else
            {
                if (DataGrid.Columns.Contains("OPCServer"))
                {
                    DataGrid.Columns.Remove("OPCServer");
                    DataGrid.Columns.Remove("Device");
                }
            }
        }

        private void alarmToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (alarmToolStripMenuItem.CheckState == CheckState.Unchecked)
            {
                try
                {
                    DataGrid.Columns.RemoveAt(Engine.FindIndexByName("Alarm_Field", DataGrid.Columns));
                    DataGrid.Columns.RemoveAt(Engine.FindIndexByName("Active_Field", DataGrid.Columns));
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Priority"].Index);
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Alarm_Text"].Index);
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Alarm Filter"].Index);
                }
                catch (Exception)
                {

                    
                }
                



            }
            else if (alarmToolStripMenuItem.CheckState == CheckState.Checked)
            {

                if (!DataGrid.Columns.Contains("Alarm_Field"))
                {
                    DataGridViewCheckBoxColumn tempcol = new DataGridViewCheckBoxColumn();
                    tempcol.Name = "Alarm_Field";
                    tempcol.HeaderText = "Alarm On";
                    DataGrid.Columns.Add(tempcol);
                    DataGrid.Columns.Add("Active_Field", "Alarm Active Value");
                    DataGrid.Columns.Add("Priority", "Priority");
                    DataGrid.Columns.Add("Alarm_Text", "Alarm Text");
                    DataGrid.Columns.Add("Alarm Filter", "Alarm Filter");
                }
                

            
            }
        }

        private void ignitionTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.SaveUnder(Engine.actualProjectPath, DataGrid);
        }

        private void hierarchieToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fermerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {

                if (!ProjectSaved)
                {
                    var confirmResult = MessageBox.Show("Le projet actuellement ouvert n'a pas été sauvegardé , voulez vous sauvegarder avant de quitter ?", "Attention", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        #region Prompt enregistrer sous
                        SaveFileDialog SaveFile = new SaveFileDialog();
                        SaveFile.DefaultExt = ".teproj";
                        SaveFile.Filter = "Tag Explorer Project|*.teproj";
                        SaveFile.FileName = "TE_Project1";
                        if (SaveFile.ShowDialog() == DialogResult.OK)
                        {
                            Engine.SaveUnder(SaveFile.FileName, DataGrid);
                            Engine.actualProjectPath = SaveFile.FileName;
                            enregistrerToolStripMenuItem.Enabled = true;
                            ResetProject();
                        }
                        #endregion
                        
                    }
                    else if (confirmResult == DialogResult.No)
                    {
                        ResetProject();
                    }else if(confirmResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    ResetProject();
                }
                

            }
            
            
        }

       

        private void New_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void New_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                Form1 newForm = new Form1();
                newForm.IsProjectOpen = true;
                newForm.Show();
                newForm.NewProject();
            }
            else
            {
                IsProjectOpen = true;

                NewProject();
            }
        }

        private void Save_MouseMove(object sender, MouseEventArgs e)
        {
            Save.Enabled = enregistrerToolStripMenuItem.Enabled;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Engine.SaveUnder(Engine.actualProjectPath, DataGrid);
        }

        private void SaveUnder_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".teproj";
            SaveFile.Filter = "Tag Explorer Project|*.teproj";
            SaveFile.FileName = "TE_Project1";
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                Engine.SaveUnder(SaveFile.FileName, DataGrid);
                Engine.actualProjectPath = SaveFile.FileName;
                enregistrerToolStripMenuItem.Enabled = true;
            }
        }

        private void SaveUnder_MouseMove(object sender, MouseEventArgs e)
        {
            SaveUnder.Enabled = enregistrerSousToolStripMenuItem.Enabled;
        }

        private void Periodic_Tick(object sender, EventArgs e)
        {
            Save.Enabled = enregistrerToolStripMenuItem.Enabled;
            SaveUnder.Enabled = enregistrerSousToolStripMenuItem.Enabled;

           
        }

        private void clampModeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if(clampModeToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Clamp Mode"))
                {
                    DataGrid.Columns.Add("Clamp Mode", "Clamp Mode");
                }
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Clamp Mode"].Index);

                }
                catch (Exception)
                {

                    
                }
            }
        }

        private void autoHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void scaleModeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (scaleModeToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Scale Mode"))
                {
                    DataGrid.Columns.Add("Scale Mode", "Scale Mode");

                }
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Scale Mode"].Index);

                }
                catch (Exception)
                {

                    
                }
            }
        }

        private void rawToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (rawToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Raw Min"))
                    DataGrid.Columns.Add("Raw Min", "Raw Min");
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Raw Min"].Index);
                }
                catch (Exception)
                {

                   
                }
                
            }
        }

        private void scaledToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if(scaledToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Scaled Min"))
                    DataGrid.Columns.Add("Scaled Min", "Scaled Min");
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Scaled Min"].Index);
                }
                catch (Exception)
                {

                    
                }
                
            }
        }

        private void rawToolStripMenuItem1_CheckStateChanged(object sender, EventArgs e)
        {
            if (rawToolStripMenuItem1.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Raw Max"))
                    DataGrid.Columns.Add("Raw Max", "Raw Max");
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Raw Max"].Index);
                }
                catch (Exception)
                {

                   
                }
                
            }
        }

        private void scaledToolStripMenuItem1_CheckStateChanged(object sender, EventArgs e)
        {
            if (scaledToolStripMenuItem1.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Scaled Min"))
                    DataGrid.Columns.Add("Scaled Min", "Scaled Max");
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Scaled Max"].Index);
                }
                catch (Exception)
                {

                    
                }
                
            }
        }

        private void additionnalCommentToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (additionnalCommentToolStripMenuItem.CheckState == CheckState.Checked)
            {

                if (!DataGrid.Columns.Contains("Additional_Comment_Field"))
                    DataGrid.Columns.Add("Additional_Comment_Field", "Additional Comment");
                    

                
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(Engine.FindIndexByName("Additional_Comment_Field", DataGrid.Columns));
                }
                catch (Exception)
                {

                    
                }
                
            }
        }

        private void supervisorAddressToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (supervisorAddressToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Sup_Address_Field"))
                {
                    DataGridViewTextBoxColumn Temp_Col = new DataGridViewTextBoxColumn();
                    Temp_Col.Name = "Sup_Address_Field";
                    Temp_Col.HeaderText = "Supervisor Address";

                    DataGrid.Columns.Insert(Engine.FindIndexByName("API_Addr_Field", DataGrid.Columns), Temp_Col);
                }
                
            }
            else 
            {
                try
                {
                    DataGrid.Columns.RemoveAt(Engine.FindIndexByName("Sup_Address_Field", DataGrid.Columns));
                }
                catch (Exception)
                {

                    
                }

                
            }
        }

        private void iOScanningVariableToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (iOScanningVariableToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("IO_Field"))
                {
                    DataGridViewCheckBoxColumn Temp_Col = new DataGridViewCheckBoxColumn();
                    Temp_Col.Name = "IO_Field";
                    Temp_Col.HeaderText = "IO Scanning";
                    DataGrid.Columns.Add(Temp_Col);
                }
                    


            }
            else
            {

                try
                {
                    DataGrid.Columns.RemoveAt(Engine.FindIndexByName("IO_Field", DataGrid.Columns));

                }
                catch (Exception)
                {

                    
                }
            }
        }

        private void cOMAddressToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (cOMAddressToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("COM_Field"))
                {
                    DataGridViewTextBoxColumn Temp_Col = new DataGridViewTextBoxColumn();
                    Temp_Col.Name = "COM_Field";
                    Temp_Col.HeaderText = "COM Address";
                    DataGrid.Columns.Add(Temp_Col);
                }
                   


            }
            else
            {

                try
                {
                    DataGrid.Columns.RemoveAt(Engine.FindIndexByName("COM_Field", DataGrid.Columns));

                }
                catch (Exception)
                {

                    
                }
            }
        }

        private void ResetProject()
        {
            ProjectSaved = false;
            IsProjectOpen = false;
            enregistrerSousToolStripMenuItem.Enabled = false;
            enregistrerToolStripMenuItem.Enabled = false;
            projectToolStripMenuItem.Enabled = false;
            DataGrid = null;
            tabControl1.TabPages.Clear();
            UserConf.Reset();
            CheckUserConf();

        }

        private void variablesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Engine.CreateWagoVariables();
        }

        private void additionnalCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void recopieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.CreateWagoRecopy();
        }

        private void tagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                Engine.ExportProjectToIgnition();
            }
        }

        private void unityVariablesExchangeFilexmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Projet vide , impossible de créer un fichier d'échange", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Engine.CreateUnityExchangeFile(DataGrid);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            #region Open tag explorer project
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.DefaultExt = ".teproj";
            OpenFile.Filter = "Projet Tag Explorer | *.teproj";
            OpenFile.CheckFileExists = true;
            OpenFile.Title = "Open Project";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (IsProjectOpen)
                {
                    Form1 newForm = new Form1();
                    newForm.Show();
                    newForm.OpenProject(OpenFile.FileName, newForm.DataGrid);
                    newForm.ProjectSaved = true;





                }
                else
                {
                    OpenProject(OpenFile.FileName, DataGrid);
                    ProjectSaved = true;
                }

            }
            #endregion
        }

        private void sTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.CreateUnitySTRecopie();
        }

        private void ladderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.CreateUnityLDRecopie();
        }

        private void parentToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (parentToolStripMenuItem.CheckState == CheckState.Checked)
            {
                if (!DataGrid.Columns.Contains("Parent"))
                {
                    DataGrid.Columns.Add("Parent", "Parent");

                }
            }
            else
            {
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["Parent"].Index);

                }
                catch (Exception)
                {


                }
            }
        }

        private void vérificationDFBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GETEX.InitializeVariables();
            //GETEX.CheckAdresses(DataGrid);
        }

        private void sIEMENSToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (sIEMENSToolStripMenuItem.CheckState == CheckState.Checked)
            {
                Engine.SIEMENS = true;
                if (!DataGrid.Columns.Contains("DB"))
                {
                    DataGrid.Columns.Add("DB", "DB");

                }
            }
            else
            {
                Engine.SIEMENS = false;
                try
                {
                    DataGrid.Columns.RemoveAt(DataGrid.Columns["DB"].Index);

                }
                catch (Exception)
                {


                }
            }
        }

      

        private void offsetParameterNameToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Parameters.Add(toolStripTextBox2.Text);
                DataGrid.Columns.Add(toolStripTextBox2.Text, toolStripTextBox2.Text);
            }



        }

        private void lireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                Form1 newForm = new Form1();
                newForm.IsProjectOpen = false;
                newForm.Show();
                newForm.ReadRegisters();
            }
            else
            {
                ReadRegisters();
            }
        }

        private void ReadRegisters()
        {
            PopupModbus fen = new PopupModbus(DataGrid);
            fen.ShowDialog();
            if(fen.DialogResult == DialogResult.OK)
            {
                DataGrid = fen.data;
            }
        }

        private void gENCOMEquipementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.CreateUnityGENCOMFile();
        }

        private void DefFunction()
        {

            string output = "USE [MES]\n"+
                            "GO\n"+
                            "SET ANSI_NULLS ON\n"+
                            "GO\n"+
                            "SET QUOTED_IDENTIFIER ON\n"+
                            "GO\n";
            List<Table> tables = new List<Table>();
            #region BDD
            bool getNexts = false;
            foreach(DataGridViewRow row in DataGrid.Rows)
            {
                if(!string.IsNullOrEmpty(row.Cells[DataGrid.Columns["table"].Index].Value.ToString())&& !getNexts)
                {
                    tables.Add(new Table(row.Cells[DataGrid.Columns["table"].Index].Value.ToString()));
                    getNexts = true;
                }

                if (!string.IsNullOrEmpty(row.Cells[DataGrid.Columns["Champs"].Index].Value.ToString()) && getNexts)
                {
                    tables[tables.Count - 1].myChamps.Add(new Champ(row.Cells[DataGrid.Columns["Champs"].Index].Value.ToString(),row.Cells[DataGrid.Columns["Type_champs"].Index].Value.ToString()));
                    
                }else
                {
                    getNexts = false;
                }
                
            }

            foreach(Table t in tables)
            {
                output = output +
                
                
                "CREATE TABLE[dbo].[" + t.Nom + "](\n" +
                "   [Id_" + t.Nom + "] [bigint] IDENTITY(1, 1) NOT NULL,\n"

                ;
                bool f = false;
                foreach(Champ c in t.myChamps)
                {
                    if (!f)
                    {
                        f = true;
                    }
                    else
                    {
                        output = output + "    [" + c.Nom + "] [" + c.type + "] NULL,\n";
                    }
                    
                }
                f = false;

                output = output +
                "PRIMARY KEY CLUSTERED\n"+
                "(\n"+
                "[Id_"+t.Nom+"] ASC\n" +
                ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]\n"+
                ") ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]\n"+
                "GO\n"+
                "ALTER TABLE[dbo].[" + t.Nom + "] SET(LOCK_ESCALATION = AUTO)\n" +
                "GO\n";
            }

            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".sql";
            SaveFile.Filter = "Fichier SQL| *.sql";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = "exp.sql";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(SaveFile.OpenFile()))
                {
                    writer.Write(output);
                }



            }
            #endregion

            #region PCVUE
            /*
            List<string> PCVUEDatFile = new List<string>();
            foreach(DataGridViewRow row in DataGrid.Rows)
            {
                string API = row.Cells["API"].Value.ToString();
                string ARMOIRE = row.Cells["ARMOIRE"].Value.ToString();
                string GROUPE = row.Cells["GROUPE"].Value.ToString();
                string TYPE = row.Cells["TYPE2"].Value.ToString();
                string EQT = row.Cells["EQT"].Value.ToString();
                string INFO = row.Cells["INFO"].Value.ToString();
                string PREFIX= row.Cells["PREFIX"].Value.ToString();
                string DESC = row.Cells["Désignation"].Value.ToString();
                string OFFSET = row.Cells["OffsetVue"].Value.ToString();
                string BIT = row.Cells["BitVue"].Value.ToString();
                string TRAME = row.Cells[9].Value.ToString();

                if (!string.IsNullOrEmpty(API))
                {
                    if (PREFIX == "ACM")
                        PCVUEDatFile.Add(PREFIX + ",," + API + "," + ARMOIRE + "," + GROUPE + "," + TYPE + "," + EQT + "," + INFO + ",," + API + " - " + DESC + ",,DALI_MP2,DALI,,,P,E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,,, 1,ETH,DALI_MP2_A," + TRAME.Replace(" ", "") + ",B," + OFFSET + "," + BIT + ", 1,,,,1,1,HI,0,1,1,,0,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,0,,,0,,,,,,,,,,,,,,0,-1,-2,-2,0");
                    if (PREFIX == "ALA")
                        PCVUEDatFile.Add(PREFIX + ",," + API + "," + ARMOIRE + "," + GROUPE + "," + TYPE + "," + EQT + "," + INFO + ",," + API + " - " + DESC + ",,DALI_MP2,DALI,,,P,E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,,, 1,ETH,DALI_MP2_A," + TRAME.Replace(" ", "") + ",B," + OFFSET + "," + BIT + ", 1,,,,1,1,HI,,1,1,,0,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,0,,,0,,,,,,,,,,,,,,0,-1,-1,-1,0");
                    if (PREFIX == "BIT")
                        PCVUEDatFile.Add(PREFIX + ",," + API + "," + ARMOIRE + "," + GROUPE + "," + TYPE + "," + EQT + "," + INFO + ",," + API + " - " + DESC + ",,DALI_MP2,DALI,,,P,E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,,, 1,ETH,DALI_MP2_A," + TRAME.Replace(" ", "") + ",B," + OFFSET + "," + BIT + ", 1,,,,1,1,HI,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,0,,,0,,,,,,,,,,,,,,0,-1,-2,-2,0");
                    if (PREFIX == "REG" || PREFIX == "CTV")
                        PCVUEDatFile.Add(PREFIX + ",," + API + "," + ARMOIRE + "," + GROUPE + "," + TYPE + "," + EQT + "," + INFO + ",," + API + " - " + DESC + ",,DALI_MP2,DALI,,,P,E, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,,, 1,ETH,DALI_MP2_A," + TRAME.Replace(" ", "") + ",U," + OFFSET + "," + BIT + ", 16,,,,,,,,,,,,,,,,,,,,,,,,0,0,1000000,0,0,1000,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,0,0,,,0,,,,,,,,,,,,,,0,-1,-2,-2,0");
                }

            }

            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".dat";
            SaveFile.Filter = "Text File| *.dat";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = "varexp.dat";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(SaveFile.OpenFile()))
                {
                    foreach (string s in PCVUEDatFile)
                    {
                        writer.WriteLine(s);
                    }
                }



            }
            */
            #endregion


            #region CodeSys
            /*
              List<List<string>> ListOfLadderLine = new List<List<string>>();
            List<string> InnerList = new List<string>();

            
            
            for(int C = 1; C <= 5; C++)
            {
                for(int B = 1;B <= 64; B++)
                {
                    string Bs = "";
                    string Bi = "" + (B - 1);
                    if (B < 10) {Bs = "0" + B; } else { Bs = ""+B; }
                    if(B <= 5)
                    {
                        InnerList.Add("_COMMENT");
                        InnerList.Add("'Recopie valeur détection lux'");
                        InnerList.Add("_END_COMMENT");
                        InnerList.Add("_LD_ASSIGN");
                        InnerList.Add("_EMPTY");
                        InnerList.Add("_EXPRESSION");
                        InnerList.Add("_POSITIV");
                        InnerList.Add("");
                        InnerList.Add("");
                        InnerList.Add("ENABLELIST : 1");
                        InnerList.Add("_ASSIGN");
                        InnerList.Add("_OPERATOR");
                        InnerList.Add("_BOX_EXPR: 1");
                        InnerList.Add("_ENABLED");
                        InnerList.Add("_OPERAND");
                        InnerList.Add("_EXPRESSION");
                        InnerList.Add("_POSITIV");
                        InnerList.Add("awLuxLevel_C" + C + "[" + Bi + "]");
                        InnerList.Add("_EXPRESSION");
                        InnerList.Add("_POSITIV");
                        InnerList.Add("REAL_TO_WORD");
                        InnerList.Add("_EXPRESSION");
                        InnerList.Add("_POSITIV");
                        InnerList.Add("_OUTPUTS : 1");
                        InnerList.Add("_OUTPUT");
                        InnerList.Add("_POSITIV");
                        InnerList.Add("_NO_SET");
                        InnerList.Add("VAL_ECL_DET_CT" + C + "_" + B);
                        InnerList.Add("ENABLELIST_END");
                        InnerList.Add("_OUTPUTS : 0");
                        InnerList.Add("_NETWORK");
                        InnerList.Add("");
                    }
                    
                    InnerList.Add("_COMMENT");
                    InnerList.Add("''");
                    InnerList.Add("_END_COMMENT");
                    InnerList.Add("_LD_ASSIGN");
                    InnerList.Add("_LD_CONTACT");
                    InnerList.Add("axLamp_On_C"+C+"["+Bi+"]");
                    InnerList.Add("_EXPRESSION"); 
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("");
                    InnerList.Add("");
                    InnerList.Add("ENABLELIST: 0");
                    InnerList.Add("ENABLELIST_END");
                    InnerList.Add("_OUTPUTS : 1");
                    InnerList.Add("_OUTPUT");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_NO_SET");
                    InnerList.Add("LMP_ON_CT"+C+"_B"+Bs+"");
                    InnerList.Add("_NETWORK");
                    InnerList.Add("");
                    InnerList.Add("_COMMENT");
                    InnerList.Add("''");
                    InnerList.Add("_END_COMMENT");
                    InnerList.Add("_LD_ASSIGN");
                    InnerList.Add("_LD_CONTACT");
                    InnerList.Add("axDefBal_C"+C+"["+Bi+"]");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("");
                    InnerList.Add("");
                    InnerList.Add("ENABLELIST: 0");
                    InnerList.Add("ENABLELIST_END");
                    InnerList.Add("_OUTPUTS : 1");
                    InnerList.Add("_OUTPUT");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_NO_SET");
                    InnerList.Add("BAL_DEF_CT"+C+"_B"+Bs+"");
                    InnerList.Add("_NETWORK");
                    InnerList.Add("");
                    InnerList.Add("_COMMENT");
                    InnerList.Add("''");
                    InnerList.Add("_END_COMMENT");
                    InnerList.Add("_LD_ASSIGN");
                    InnerList.Add("_LD_CONTACT");
                    InnerList.Add("axDefLamp_C"+C+"["+Bi+"]");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("");
                    InnerList.Add("");
                    InnerList.Add("ENABLELIST: 0");
                    InnerList.Add("ENABLELIST_END");
                    InnerList.Add("_OUTPUTS : 1");
                    InnerList.Add("_OUTPUT");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_NO_SET");
                    InnerList.Add("LMP_DEF_CT"+C+"_B"+Bs+"");
                    InnerList.Add("_NETWORK");
                    InnerList.Add("");
                    InnerList.Add("_COMMENT");
                    InnerList.Add("''");
                    InnerList.Add("_END_COMMENT");
                    InnerList.Add("_LD_ASSIGN");
                    InnerList.Add("_LD_CONTACT");
                    InnerList.Add("axPresBAl_C"+C+"["+Bi+"]");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_NEGATIV");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("");
                    InnerList.Add("");
                    InnerList.Add("ENABLELIST: 0");
                    InnerList.Add("ENABLELIST_END");
                    InnerList.Add("_OUTPUTS : 1");
                    InnerList.Add("_OUTPUT");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_NO_SET");
                    InnerList.Add("BAL_ABS_CT"+C+"_B"+Bs+"");
                    InnerList.Add("_NETWORK");
                    InnerList.Add("");
                    InnerList.Add("_COMMENT");
                    InnerList.Add("'Recopie valeur retour balast'");
                    InnerList.Add("_END_COMMENT");
                    InnerList.Add("_LD_ASSIGN");
                    InnerList.Add("_EMPTY");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("");
                    InnerList.Add("");
                    InnerList.Add("ENABLELIST : 1");
                    InnerList.Add("_ASSIGN");
                    InnerList.Add("_OPERATOR");
                    InnerList.Add("_BOX_EXPR: 1");
                    InnerList.Add("_ENABLED");
                    InnerList.Add("_OPERAND");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("arActualValue_C"+C+"["+Bi+"]");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("REAL_TO_INT");
                    InnerList.Add("_EXPRESSION");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_OUTPUTS : 1");
                    InnerList.Add("_OUTPUT");
                    InnerList.Add("_POSITIV");
                    InnerList.Add("_NO_SET");
                    InnerList.Add("LMP_VAL_CT"+C+"_"+B+"");
                    InnerList.Add("ENABLELIST_END");
                    InnerList.Add("_OUTPUTS : 0");
                    InnerList.Add("_NETWORK");
                }
            }

            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = ".txt";
            SaveFile.Filter = "Text File| *.txt";
            SaveFile.FilterIndex = 1;
            SaveFile.FileName = "DEVFUNCTION.txt";
            var dir = SaveFile.FileName;

            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(SaveFile.OpenFile()))
                    {
                        foreach (string s in InnerList)
                        {
                            writer.WriteLine(s);                      
                        }
                    }
                

           
            }
            
             */
            #endregion



        }

        private void exécuterFonctionDevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefFunction();
        }

        private void gENCOMSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Engine.CreateUnityGENCOMSections();
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }

        private void DataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateVariables();
            UpdateTree();
            ProjectSaved = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
