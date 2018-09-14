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
    public partial class PopupModbus : Form
    {

        public DataGridView data;
        public PopupModbus()
        {
            InitializeComponent();
        }

        public PopupModbus(DataGridView _data)
        {
            InitializeComponent();
            data = _data;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex == 3)
            {
                int[] recept = Engine.ReadInputRegisters(textBox1.Text, int.Parse(textBox2.Text), int.Parse(textBox3.Text));
                if(recept != null)
                {
                    data.Columns.Clear();
                    data.Columns.Add("Register", "Register");
                    data.Columns.Add("Value", "Value");

                    int i = int.Parse(textBox2.Text);
                    foreach (int x in recept)
                    {
                        string[] toAdd = { i.ToString(), x.ToString() };
                        data.Rows.Add(toAdd);
                        i++;
                    }
                }
                
            }

            DialogResult = DialogResult.OK;
        }
    }
}
