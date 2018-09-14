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
    public partial class Popup_auto_assign : Form
    {
        public DataGridView data;
        public bool address_bits_API = true;
        public bool address_bits_SUP = true;
        List<Mot> plageAPI;

        public Popup_auto_assign(DataGridView _data)
        {
            InitializeComponent();
            data = _data;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {

            //Création des Offset dans tags parents
            foreach (TeTag tag in Engine.TeTags.Where(x => x.IsParent))
            {
                var OffDecay = 0;
                var BitDecay = 0;


                foreach (TeTag child in tag.Childs)
                {

                    if (child.BitsTaken > 1)
                    {
                        //Si un mot a été entammé par des bitrs
                        if (BitDecay > 0)
                        {
                            BitDecay = 0;
                            OffDecay++;
                        }

                        // Real pas posé sur impair
                        if (child.Type.ToUpper() == "REAL")
                        {
                            if (OffDecay % 2 != 0)
                                OffDecay++;
                        }
                        child.SUPAddress = "[{COMVICE}]{Offset + " + OffDecay + "}";
                    }
                    else
                    {
                        child.SUPAddress = "[{COMVICE}]{Offset + " + OffDecay + "}." + BitDecay;
                        BitDecay++;
                    }

                    if (BitDecay >= 16)
                    {
                        BitDecay = 0;
                        OffDecay++;
                    }
                    OffDecay += (int)(child.BitsTaken / 16);

                    foreach (DataGridViewRow row in data.Rows)
                    {
                        try
                        {
                            // Contains emmerdant , à remplacer
                            if (row.Cells["Name_Field"].Value.ToString().Contains(child.Name))
                            {
                                row.Cells["Sup_Address_Field"].Value = child.SUPAddress;
                                var titi = 1;
                                row.Cells["Offset"].Value = "1";
                            }
                        }
                        catch (Exception)
                        {

                        }

                    }

                    child.APIAdressed = true;
                    child.SupAdressed = true;
                }

            }

            //Variables pour la supervision
            #region SUP
            if (SupMMax.Value > SupMMin.Value)
            {
                var BitDecay = SupMMin.Value;
                foreach (TeTag tag in Engine.TeTags.Where(x => x.Type.ToUpper() == "BOOL"))
                {
                    if (BitDecay <= SupMMax.Value)
                    {
                        if (!tag.SupAdressed)
                        {
                            tag.SUPAddress = "%M" + BitDecay;
                            BitDecay++;
                            foreach (DataGridViewRow row in data.Rows)
                            {
                                try
                                {
                                    if (row.Cells["Name_Field"].Value.ToString().Contains(tag.Name))
                                    {
                                        row.Cells["Sup_Address_Field"].Value = tag.SUPAddress;
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                }

                            }
                            tag.SupAdressed = true;

                        }
                    }
                    else
                    {

                        break;
                    }


                }



            }

            if (SupMWMax.Value > SupMWMin.Value)
            {
                var OffDecay = SupMWMin.Value;
                var BitDecay = 0;
                foreach (TeTag tag in Engine.TeTags.Where(x => x.Type.ToUpper() == "BOOL"))
                {
                    if (OffDecay <= SupMWMax.Value)
                    {
                        if (!tag.SupAdressed)
                        {
                            tag.SUPAddress = "%MW" + OffDecay + "." + BitDecay;
                            BitDecay++;
                            foreach (DataGridViewRow row in data.Rows)
                            {
                                try
                                {
                                    if (row.Cells["Name_Field"].Value.ToString().Contains(tag.Name))
                                    {
                                        row.Cells["Sup_Address_Field"].Value = tag.SUPAddress;
                                    }
                                }
                                catch (Exception)
                                {

                                }

                            }
                            if (BitDecay >= 16)
                            {
                                BitDecay = 0;
                                OffDecay++;
                            }

                            tag.SupAdressed = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Plage de valeurs trop étroite , certaine variables ne seront pas adressés.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }


                }
                if (OffDecay <= SupMWMax.Value)
                {
                    foreach (TeTag tag in Engine.TeTags.Where(x => x.Type.ToUpper() != "BOOL" ))
                    {
                        if (OffDecay <= SupMWMax.Value)
                        {
                            if (!tag.SupAdressed)
                            {
                                //Si un mot a été entammé par des bitrs
                                if (BitDecay > 0)
                                {
                                    BitDecay = 0;
                                    OffDecay++;
                                }
                                // Real pas posé sur impair
                                if (tag.Type.ToUpper() == "REAL" || !Engine.Types.Contains(tag.Type.ToUpper()))
                                {
                                    if (OffDecay % 2 != 0)
                                        OffDecay++;
                                }
                                tag.SUPAddress = "%MW" + OffDecay;
                                OffDecay++;
                                OffDecay += (int)(tag.BitsTaken / 16);
                                foreach (DataGridViewRow row in data.Rows)
                                {
                                    try
                                    {
                                        if (row.Cells["Name_Field"].Value.ToString().Contains(tag.Name))
                                        {
                                            row.Cells["Sup_Address_Field"].Value = tag.SUPAddress;
                                            if (!Engine.Types.Contains(tag.Type.ToUpper()))
                                            {
                                                row.Cells["Offset"].Value = tag.SUPAddress.Replace("%MW","");
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }

                                }


                                tag.SupAdressed = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Plage de valeurs trop étroite , certaine variables ne seront pas adressés.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }


                    }
                }



            }
            #endregion


            // Si des %M sont dispo on adresse directement tout les booléen
            //Dans API
            #region API
            if (ApiMMax.Value > ApiMMin.Value)
            {
                var BitDecay = ApiMMin.Value;
                foreach (TeTag tag in Engine.TeTags.Where(x => x.Type.ToUpper() == "BOOL"))
                {
                    if (BitDecay <= ApiMMax.Value)
                    {
                        if (!tag.APIAdressed)
                        {
                            tag.APIAddress = "%M" + BitDecay;
                            BitDecay++;
                            foreach (DataGridViewRow row in data.Rows)
                            {
                                try
                                {
                                    if (row.Cells["Name_Field"].Value.ToString().Contains(tag.Name))
                                    {
                                        row.Cells["API_Addr_Field"].Value = tag.APIAddress;
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                }

                            }
                            tag.APIAdressed = true;

                        }
                    }
                    else
                    {

                        break;
                    }


                }



            }

            if (ApiMWMax.Value > ApiMWMin.Value)
            {
                var OffDecay = ApiMWMin.Value;
                var BitDecay = 0;
                foreach (TeTag tag in Engine.TeTags.Where(x => x.Type.ToUpper() == "BOOL"))
                {
                    if (OffDecay <= ApiMWMax.Value)
                    {
                        if (!tag.APIAdressed)
                        {
                            tag.APIAddress = "%MW" + OffDecay + "." + BitDecay;
                            BitDecay++;
                            foreach (DataGridViewRow row in data.Rows)
                            {
                                try
                                {
                                    if (row.Cells["Name_Field"].Value.ToString().Contains(tag.Name))
                                    {
                                        row.Cells["API_Addr_Field"].Value = tag.APIAddress;
                                    }
                                }
                                catch (Exception)
                                {

                                }

                            }
                            if (BitDecay >= 16)
                            {
                                BitDecay = 0;
                                OffDecay++;
                            }

                            tag.APIAdressed = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Plage de valeurs trop étroite , certaine variables ne seront pas adressés.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }


                }
                if (OffDecay <= ApiMWMax.Value)
                {
                    foreach (TeTag tag in Engine.TeTags.Where(x => x.Type.ToUpper() != "BOOL"))
                    {
                        if (OffDecay <= ApiMWMax.Value)
                        {
                            if (!tag.APIAdressed)
                            {
                                //Si un mot a été entammé par des bitrs
                                if (BitDecay > 0)
                                {
                                    BitDecay = 0;
                                    OffDecay++;
                                }
                                // Real pas posé sur impair
                                if (tag.Type.ToUpper() == "REAL")
                                {
                                    if (OffDecay % 2 != 0)
                                        OffDecay++;
                                }
                                tag.APIAddress = "%MW" + OffDecay;
                                OffDecay += (int)(tag.BitsTaken / 16);
                                foreach (DataGridViewRow row in data.Rows)
                                {
                                    try
                                    {
                                        if (row.Cells["Name_Field"].Value.ToString().Contains(tag.Name))
                                        {
                                            row.Cells["API_Addr_Field"].Value = tag.APIAddress;
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }

                                }


                                tag.APIAdressed = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Plage de valeurs trop étroite , certaine variables ne seront pas adressés.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }


                    }
                }
                if (OffDecay <= ApiMWMax.Value)
                {
                }


            }
            #endregion

            var toto = 1;



        }
    }
}
