using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tag_Explorer
{
    public partial class GENCOM_Popup : Form
    {
        public List<TeTag> SortedTags { get; set; }

        public GENCOM_Popup()
        {
            InitializeComponent();
        }

        public GENCOM_Popup(List<TeTag> tags)
        {
            InitializeComponent();
            SortedTags = new List<TeTag>();
            EqtData.Columns.Add("Nom","Nom");
            EqtData.Columns.Add("Type", "Type");
            EqtData.Columns.Add("Adresse", "Adresse");
            EqtData.Columns.Add("Nom Equipement", "Nom Equipement");
            EqtData.Rows.Clear();
            foreach(TeTag tag in tags.Where(x => x.GENCOMREADY == true && x.IsParent == true))
            {
                string[] row = { tag.Name, "", "", tag.Name };
                
                EqtData.Rows.Add(row);
                

                foreach (TeTag child in tag.Childs) 
                {
                    string[] childRow = { child.Name, child.Type, child.COMAddress, tag.Name };

                    EqtData.Rows.Add(childRow);
                }

                
            }
            
            
            foreach (DataGridViewRow row in EqtData.Rows)
            {
                if(row.Cells["Nom"].Value == row.Cells["Nom Equipement"].Value)
                {
                    row.DefaultCellStyle.BackColor = Color.LawnGreen;
                }
            }
                
                // Créé les parents
                foreach(DataGridViewRow row  in EqtData.Rows)
                {
                    TeTag temptag = new TeTag();
                    if(row.Cells["Nom Equipement"].Value != null)
                {
                    if (row.Cells["Nom"].Value == row.Cells["Nom Equipement"].Value)
                    {
                        temptag.Name = row.Cells["Nom"].Value.ToString();
                        temptag.IsParent = true;

                    }

                    if (SortedTags.FirstOrDefault(x => x.Name == row.Cells["Nom Equipement"].Value.ToString()) != null)
                    {

                    }
                    else
                    {
                        temptag.Name = row.Cells["Nom Equipement"].Value.ToString();
                        temptag.IsParent = true;
                    }

                    SortedTags.Add(temptag);
                }
                    

                }

                List<TeTag> childs = new List<TeTag>();
                //Créé les enfants  
                foreach(DataGridViewRow row in EqtData.Rows)
                {
                if (row.Cells["Nom Equipement"].Value != null)
                {
                    TeTag childTag = new TeTag();
                    if (row.Cells["Nom"].Value != row.Cells["Nom Equipement"].Value)
                    {
                        childTag.Name = row.Cells["Nom"].Value.ToString();
                        childTag.Type = row.Cells["Type"].Value.ToString();
                        childTag.COMAddress = row.Cells["Adresse"].Value.ToString();
                        childTag.Parent = row.Cells["Nom Equipement"].Value.ToString();
                        childTag.HasParent = true;

                    }
                    childs.Add(childTag);
                }
                    
                }

                foreach(TeTag parent in SortedTags)
                {
                    foreach(TeTag child in childs.Where(x => x.Parent == parent.Name))
                    {
                        parent.Childs.Add(child);
                    }
                }

                EqtData.ReadOnly = true;

                foreach(TeTag tag in SortedTags)
            {
                tag.CheckTag();
            }
                MessageBox.Show("Veuillez vérifier SVP", "Question", MessageBoxButtons.OK, MessageBoxIcon.Question);
                
            }

        private void oKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Oui.Checked)
                GENCOM.ADDR = true;
            DialogResult = DialogResult.OK;
        }
    }
}



