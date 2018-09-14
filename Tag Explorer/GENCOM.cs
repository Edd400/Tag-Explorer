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
    public static class GENCOM
    {

        public static List<Equipement> Equipements { get; set; }
        public static bool ADDR { get; set; }

        public static void GenerateEquipements(List<TeTag> _tags)
        {
            Equipements = new List<Equipement>();
            foreach (TeTag tag in _tags.Where(x => x.IsParent == true))
            {
                FBExchangeFile _baseFBex = new FBExchangeFile();
                XmlSerializer xs = new XmlSerializer(typeof(FBExchangeFile));

                using (StreamReader reader = new StreamReader(@"bExchangeFile.xdb"))
                {
                    _baseFBex = xs.Deserialize(reader) as FBExchangeFile;
                }

                    Equipements.Add(new Equipement(tag.Name, _baseFBex.DDTSource.FirstOrDefault(x => x.DDTName == "Eqt_1_var"),
                        _baseFBex.FBSource.FirstOrDefault(x => x.nameOfFBType == "Eqt_1"))
                        );
                foreach (TeTag child in tag.Childs)
                {

                    Equipements.FirstOrDefault(x => x.Name == tag.Name).Variables.Add(new Eqt_Var(child.Name, child.Type, child.COMAddress));
                }



            }

            foreach (Equipement eq in Equipements)
            {
                eq.SortVariables();
                eq.CreateRequests();
                eq.CreateSource();
                eq.CreateFB();

            }
            int toto = 0;
        }

        public static FBExchangeFile GetUnityFile(FBExchangeFile _blankFB)
        {
            FBExchangeFile returnFile = _blankFB;


            foreach (Equipement eq in Equipements)
            {
                returnFile.FBSource.Add(eq.FBSource);
            }
            foreach (Equipement eq in Equipements)
            {
                returnFile.DDTSource.Add(eq.DDTSource);
            }
            return returnFile;
        }

    }

    public class Equipement
    {
        public string Name { get; set; }
        public FBExchangeFileDDTSource DDTSource { get; set; }
        public FBExchangeFileFBSource FBSource { get; set; }



        private List<Eqt_Var> listeM;
        private List<Eqt_Var> listeBoolMW;
        private List<Eqt_Var> listeMW;
        private List<Eqt_Var> assigned;
        private string STSource = "";
        private string CONV = "";
        private string Defaut = "";

        //IMPORTANT///////
        int REQUESTCOUNTER = 0;


        public List<Eqt_Var> Variables { get; set; }

        public List<Request> Requetes { get; set; }

        public Equipement() { }

        public Equipement(string _name, FBExchangeFileDDTSource _baseDDT, FBExchangeFileFBSource _baseFB)
        {
            Name = _name;
            DDTSource = _baseDDT;
            DDTSource.DDTName = Name;
            FBSource = _baseFB;
            Variables = new List<Eqt_Var>();
            Requetes = new List<Request>();
            listeM = new List<Eqt_Var>();
            listeMW = new List<Eqt_Var>();
            listeBoolMW = new List<Eqt_Var>();
            assigned = new List<Eqt_Var>();
        }

        public void SortVariables()
        {
            foreach (Eqt_Var var in Variables.Where(x => x.Type.ToUpper().Contains("BOOL")))
            {
                if (var.Adresse.ToUpper().Contains("%MW"))
                {
                    listeBoolMW.Add(var);
                }
                else if (var.Adresse.ToUpper().Contains("%M"))
                {
                    listeM.Add(var);
                }
            }
            foreach (Eqt_Var var in Variables.Where(x => !x.Type.ToUpper().Contains("BOOL")))
            {
                if (var.Adresse.ToUpper().Contains("%MW"))
                {
                    listeMW.Add(var);
                }

            }

        }

        public void CreateRequests()
        {



            List<Eqt_Var> TempList = new List<Eqt_Var>();

            foreach (Eqt_Var var in listeM)
            {
                if (!assigned.Contains(var))
                {
                    TempList.Add(var);
                    assigned.Add(var);
                    foreach (Eqt_Var var2 in listeM)
                    {
                        if (var2 != var & !assigned.Contains(var2))
                        {
                            if (Math.Abs(var2.AddrInt - var.AddrInt) < 10)
                            {
                                TempList.Add(var2);
                                assigned.Add(var2);
                            }
                        }
                    }

                    Requetes.Add(new Request(TempList, "%M", REQUESTCOUNTER));
                    REQUESTCOUNTER++;
                    TempList = new List<Eqt_Var>();
                }





            }
            TempList = new List<Eqt_Var>();
            foreach (Eqt_Var var in listeMW)
            {
                if (!assigned.Contains(var))
                {
                    TempList.Add(var);
                    assigned.Add(var);
                    foreach (Eqt_Var var2 in listeMW)
                    {
                        if (var2 != var & !assigned.Contains(var2))
                        {
                            if (Math.Abs(var2.AddrInt - var.AddrInt) < 10)
                            {
                                TempList.Add(var2);
                                assigned.Add(var2);
                            }
                        }
                    }

                    Requetes.Add(new Request(TempList, "%MW", REQUESTCOUNTER));
                    REQUESTCOUNTER++;
                    TempList = new List<Eqt_Var>();
                }
            }
            TempList = new List<Eqt_Var>();
            foreach (Eqt_Var var in listeBoolMW)
            {
                if (!assigned.Contains(var))
                {
                    TempList.Add(var);
                    assigned.Add(var);
                    foreach (Eqt_Var var2 in listeBoolMW)
                    {
                        if (var2 != var & !assigned.Contains(var2))
                        {
                            if (Math.Abs(var2.AddrInt - var.AddrInt) < 10)
                            {
                                TempList.Add(var2);
                                assigned.Add(var2);
                            }
                        }
                    }

                    Requetes.Add(new Request(TempList, "%MW", REQUESTCOUNTER));
                    REQUESTCOUNTER++;
                    TempList = new List<Eqt_Var>();
                }
            }
        }

        public void CreateSource()
        {


            int BitWords = 0;
            byte bitCOunt = 0;
            foreach (Eqt_Var var in listeM)
            {
                if (var.Type.ToUpper() == "BOOL")
                {
                    if (bitCOunt == 0)
                    {
                        DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("BitsWord_" + BitWords, "WORD"));
                        DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables(var.Name, "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", bitCOunt)));
                        bitCOunt++;
                        BitWords++;
                    }
                    else
                    {
                        DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables(var.Name, "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", bitCOunt)));
                        bitCOunt++;
                        if (bitCOunt >= 16)
                            bitCOunt = 0;
                    }

                }
            }
            bitCOunt = 0;
            foreach (Eqt_Var var in listeBoolMW)
            {
                if (var.Type.ToUpper() == "BOOL")
                {
                    if (bitCOunt == 0)
                    {
                        DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("BitsWord_" + BitWords, "WORD"));
                        DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables(var.Name, "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", bitCOunt)));
                        bitCOunt++;
                        BitWords++;
                    }
                    else
                    {
                        DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables(var.Name, "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", bitCOunt)));
                        bitCOunt++;
                        if (bitCOunt >= 16)
                            bitCOunt = 0;
                    }

                }
            }
            foreach (Eqt_Var var in listeMW)
            {
                DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables(var.Name, var.Type.ToUpper()));
            }
            DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("Gest_Word", "WORD"));
            DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("Def_Com", "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", 0)));
            DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("Act_Com", "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", 1)));    
            DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("Fin_Com", "BOOL", new FBExchangeFileDDTSourceVariablesAttribute("ExtractBit", 2)));
            DDTSource.structure.Add(new FBExchangeFileDDTSourceVariables("NB_Ech", "DINT"));
        }

        public void CreateFB()
        {

            FBSource.nameOfFBType = "GENCOM_" + Name;
            FBSource.outputParameters.FirstOrDefault(x => x.name == "Eqt_var").typeName = Name;
            FBSource.publicLocalVariables.FirstOrDefault(x => x.name == "Requetes").typeName = "ARRAY[0.." + (REQUESTCOUNTER -1) + "] OF Request";
            FBSource.publicLocalVariables.FirstOrDefault(x => x.name == "Tabs_Gest").typeName = "ARRAY[0.." + (REQUESTCOUNTER -1) + "] OF Tab_Gest_Com";

            //TimeOut réglable
            FBSource.publicLocalVariables.Add(new FBExchangeFileFBSourcePublicLocalVariablesVariables("TimeOut", "INT"));

            #region Ajout test token

            STSource = STSource + "If(Index = Token and not Def_Com) then \n\n";

            #endregion
            #region Ajout des requetes
            foreach (Request req in Requetes)
            {


                foreach (string s in req.Output_Code)
                {
                    STSource = STSource + s + "\n";
                }
                foreach (string s in req.Output_Conv)
                {
                    CONV = CONV + s + "\n";
                }



                FBSource.publicLocalVariables.Add(new FBExchangeFileFBSourcePublicLocalVariablesVariables("Tab_Recep_" + req.ID, "ARRAY[0.." + req.Length + "] OF INT"));

            }
            FBSource.publicLocalVariables.FirstOrDefault(x => x.name == "Requetes").typeName = "ARRAY[0.." + (REQUESTCOUNTER - 1) + "] OF Request";
            #endregion
            STSource = STSource + "End_If; \n\n";

            /////Contrôle d'activité et de fin d'échange
            STSource = STSource + "(*Contrôle fin d'échange*)\n";
            STSource = STSource + "Fin_Com :=  Com_Act";
            for (int i = 0; i < REQUESTCOUNTER; i++)
            {

                STSource = STSource + " and not  ";
                STSource = STSource + "Requetes[" + i + "].Act";
            }
            STSource = STSource + "; \n\n";
            //Contrôle echange en cours
            STSource = STSource + "Com_Act := ";
            for (int i = 0; i < REQUESTCOUNTER; i++)
            {
                if (i != 0)
                    STSource = STSource + " or ";
                STSource = STSource + "Requetes[" + i + "].Act";
            }
            STSource = STSource + "; \n\n";
            /////
            STSource = STSource + "If (Fin_Com or Def_Com) then InnerToken := 0; Token := Token + 1; end_if; \n\n";

            #region Section defaut
            Defaut = Defaut + "If(Acquit) then \n";
            Defaut = Defaut + "     InnerToken := 0;\n";
            Defaut = Defaut + "     Def_Com := 0;\n";
            Defaut = Defaut + "End_If;\n\n";

            Defaut = Defaut + "If(";
            for (int i = 0; i < REQUESTCOUNTER; i++)
            {
                if (i != 0)
                    Defaut = Defaut + " or ";
                Defaut = Defaut + "Requetes[" + i + "].Def_Com";
            }
            Defaut = Defaut + ")";
            Defaut = Defaut + " then Nb_Err_En_Cours := Nb_Err_En_Cours + 1; else Nb_Err_En_Cours := 0; end_if;\n";
            Defaut = Defaut + "If(Nb_Err_En_Cours > 5) then Def_Com := 1; end_if;\n\n";
            Defaut = Defaut + "TON_DEF (IN := Def_Com(*BOOL*),\n";
            Defaut = Defaut + "         PT := T#120s(*TIME*),\n";
            Defaut = Defaut + "         Q => Fin_Tempo_Defaut(*BOOL*),\n";
            Defaut = Defaut + "         ET => Temps_Avant_Nouvel_Essai(*TIME*));\n";


            #endregion

            //Ajout aux sources
            FBSource.FBProgram.FirstOrDefault(x => x.name == "Read").STSource = STSource;
            FBSource.FBProgram.FirstOrDefault(x => x.name == "CONV").STSource = CONV;
            FBSource.FBProgram.FirstOrDefault(x => x.name == "Defaut").STSource = Defaut;



        }

    }

    public class Eqt_Var
    {
        #region Propriétés

        public string Name { get; set; }
        public string Type { get; set; }
        public string Adresse { get; set; }

        public int AddrInt { get; set; }

        #endregion

        public Eqt_Var() { }

        public Eqt_Var(string _name, string _type, string _adresse)
        {

            Name = _name;
            Type = _type;
            Adresse = _adresse;
            var t1 = Adresse;
            try
            {
                t1 = Adresse.Substring(0, Adresse.LastIndexOf("."));
            }
            catch (Exception)
            {


            }

            var t2 = t1.Replace("%MW", "");
            var t3 = t2.Replace("%M", "");
            AddrInt = int.Parse(t3);

        }
    }

    public class Req_Var
    {
        #region Propriétés
        public int ID { get; set; }
        public int Index { get; set; }
        public int Bit { get; set; }

        #endregion

        public Req_Var() { }

        public Req_Var(int _ID, int _index, int _bit = 666)
        {
            ID = _ID;
            Index = _index;
            Bit = _bit;

        }
    }

    public class Request
    {
        private List<Req_Var> LinkedVars;
        private List<GENMot> Tab_Recept;
        public int Length { get; set; }
        public int ID { get; set; }
        public string Type { get; set; }
        public List<string> Output_Code { get; set; }
        public List<string> Output_Conv { get; set; }

        public Request() { }

        public Request(List<Eqt_Var> EqtVars, string _type, int _id)
        {
            Output_Code = new List<string>();
            Output_Conv = new List<string>();
            Type = _type;
            ID = _id;
            int min = 0;
            int max = 0;
            string addr = "";
            LinkedVars = new List<Req_Var>();
            #region Determine begin et length + tab_recept
            foreach (Eqt_Var var in EqtVars)
            {
                int val = 0;
                if (var.Adresse.Contains("."))
                {
                    addr = var.Adresse.Substring(0, var.Adresse.LastIndexOf("."));
                    addr = addr.Replace("%MW", "");
                    val = int.Parse(addr);
                    #region  test min
                    if (min == 0)
                    {
                        min = val;
                    }
                    else
                    {
                        if (val < min)
                            min = val;
                    }
                    #endregion

                    #region  test max
                    if (max == 0)
                    {
                        max = val;
                    }
                    else
                    {
                        if (val > max)
                            max = val;
                    }
                    #endregion
                }
                else
                {

                    val = var.AddrInt;
                    if (var.Type.ToUpper() == "DINT")
                    {
                        #region  test min
                        if (min == 0)
                        {
                            min = val;
                        }
                        else
                        {
                            if (val < min)
                                min = val;
                        }
                        #endregion

                        #region  test max
                        if (max == 0)
                        {
                            max = val + 1;
                        }
                        else
                        {
                            if (val + 1 > max)
                                max = val + 1;
                        }
                        #endregion
                    }
                    else if (var.Type.ToUpper() == "REAL")
                    {
                        #region  test min
                        if (min == 0)
                        {
                            min = val;
                        }
                        else
                        {
                            if (val < min)
                                min = val;
                        }
                        #endregion

                        #region  test max
                        if (max == 0)
                        {
                            max = val + 1;
                        }
                        else
                        {
                            if (val + 1 > max)
                                max = val + 1;
                        }
                        #endregion
                    }
                    else
                    {
                        #region  test min
                        if (min == 0)
                        {
                            min = val;
                        }
                        else
                        {
                            if (val < min)
                                min = val;
                        }
                        #endregion

                        #region  test max
                        if (max == 0)
                        {
                            max = val;
                        }
                        else
                        {
                            if (val > max)
                                max = val;
                        }
                        #endregion
                    }

                }




            }
            Length = max - min + 1;

            Tab_Recept = new List<GENMot>();
            if (Type.ToUpper() == "%MW")
            {
                Output_Code.Add("(*Requête n°" + ID + " , Lecture de " + Length + " Mots à partir de %MW" + min + " *)");
                for (int i = min; i <= max; i++)
                {
                    Tab_Recept.Add(new GENMot(i, "%MW"));
                }
            }
            if (Type.ToUpper() == "%M")
            {
                Output_Code.Add("(*Requête n°" + ID + " , Lecture de " + Length + " Bits à partir de %M" + min + " *)");
                for (int i = min; i <= max; i += 16)
                {
                    Tab_Recept.Add(new GENMot(i, "%M"));
                }
            }

            //AJoute le parmétrage du timeout
            Output_Code.Add("");
            Output_Code.Add("Tabs_Gest[" + ID + "][2] := TimeOut;");

            Output_Code.Add("");

            //Pour les requêtes après la première , on vérifie que celle d'avant est terminée
            if (ID == 0)
                Output_Code.Add("If(InnerToken = " + ID + " and not Requetes["+ID+"].Act) then");
            else
                Output_Code.Add("If(InnerToken = " + ID + " and not Requetes[" + ID + "].Act and Requetes[" + (ID - 1) + "].End) then");

            Output_Code.Add("");
            if (GENCOM.ADDR)
            {

                Output_Code.Add("   READ_VAR(ADR:= ADDR(Adresse_Eqt)(*ANY_ARRAY_INT *),");
                Output_Code.Add("       OBJ:= '" + Type.ToUpper() + "'(*STRING *),");
                Output_Code.Add("       NUM:= " + min + "(*DINT *),");
                Output_Code.Add("       NB:= " + Length + "(*INT *),");
                Output_Code.Add("       GEST:= Tabs_Gest[" + ID + "](*ANY_ARRAY_INT *),");
                Output_Code.Add("       RECP => tab_recep_" + ID + "(*ANY_ARRAY_INT *));");
                Output_Code.Add("   InnerToken := InnerToken +1;");
                Output_Code.Add("");
               
                
            }
            else
            {

                Output_Code.Add("   READ_VAR(ADR:= ADDM(Adresse_Eqt)(*ANY_ARRAY_INT *),");
                Output_Code.Add("       OBJ:= '" + Type.ToUpper() + "'(*STRING *),");
                Output_Code.Add("       NUM:= " + min + "(*DINT *),");
                Output_Code.Add("       NB:= " + Length + "(*INT *),");
                Output_Code.Add("       GEST:= Tabs_Gest[" + ID + "](*ANY_ARRAY_INT *),");
                Output_Code.Add("       RECP => tab_recep_" + ID + "(*ANY_ARRAY_INT *));");
                Output_Code.Add("   InnerToken := InnerToken +1;");
                Output_Code.Add("");
               
            }
            Output_Code.Add("End_if;");
            Output_Code.Add("");
            Output_Code.Add("(*Contrôle de fin de requête*)");
            Output_Code.Add("Requetes[" + ID + "].End := Requetes[" + ID + "].Act and not Tabs_Gest[" + ID + "][0].0;");
            Output_Code.Add("(*Contrôle Requête en cours*)");
            Output_Code.Add("Requetes[" + ID + "].Act := Tabs_Gest[" + ID + "][0].0;");
            Output_Code.Add("(*Fin de la requête n°" + ID + " *)");
            //Defaut com
            Output_Code.Add("If(Tabs_Gest[" + ID + "][1] <> 0) then Requetes[" + ID + "].Def_Com := 1; else Requetes[" + ID + "].Def_Com := 0; end_if;");
            #endregion

            // Section convertion / Recopie
            #region MyRegion
            for (int i = 0; i < Tab_Recept.Count; i++)
            {
                foreach (Eqt_Var var in EqtVars)
                {
                    if (var.Type.ToUpper() == "BOOL")
                    {
                        foreach (GENBit bit in Tab_Recept[i].Bits)
                        {
                            if (var.Adresse == ("%M" + bit.Addr))
                            {
                                Output_Conv.Add("eqt_var." + var.Name + " := tab_recep_" + ID + "[" + i + "]" + "." + bit.number + ";");
                            }
                            else if (var.Adresse == ("%MW" + bit.Addr))
                            {
                                Output_Conv.Add("eqt_var." + var.Name + " := tab_recep_" + ID + "[" + i + "]" + "." + bit.number + ";");
                            }
                        }
                    }
                    else
                    {
                        if (var.Adresse == ("%MW" + Tab_Recept[i].Addr.ToString()))
                        {
                            if (var.Type.ToUpper() == "REAL")
                            {
                                Output_Conv.Add("eqt_var." + var.Name + " := WORD_AS_REAL(LOW := INT_TO_WORD(tab_recep_" + ID + "[" + i + "])(*WORD *),HIGH := INT_TO_WORD(tab_recep_" + ID + "[" + (i + 1) + "])(*WORD *));");
                                i++;
                            }
                            else if (var.Type.ToUpper() == "DINT")
                            {
                                Output_Conv.Add("eqt_var." + var.Name + " := WORD_AS_REAL(LOW:= INT_TO_WORD(tab_recep_" + ID + "[" + i + "])(*WORD *),HIGH := INT_TO_WORD(tab_recep_" + ID + "[" + (i + 1) + "])(*WORD *));");
                                i++;
                            }
                            else
                            {
                                Output_Conv.Add("eqt_var." + var.Name + " := tab_recep_" + ID + "[" + i + "]" + ";");
                            }

                        }
                    }


                }
            }

            #endregion

        }
    }

    public class GENMot
    {
        public int Addr { get; set; }
        public GENBit[] Bits { get; set; }
        public string Type { get; set; }

        public GENMot() { }

        public GENMot(int _addr, string _type)
        {
            Addr = _addr;
            Type = _type;
            Bits = new GENBit[16];
            if (Type.ToUpper() == "%M")
            {
                for (int i = 0; i <= 15; i++)
                {
                    Bits[i] = new GENBit(Addr + i + "", i);
                }
            }
            else if (Type.ToUpper() == "%MW")
                for (int i = 0; i <= 15; i++)
                {
                    Bits[i] = new GENBit(Addr + "." + i, i);
                }
        }
    }

    public class GENBit
    {
        public string Addr { get; set; }
        public int number { get; set; }

        public GENBit() { }

        public GENBit(string _addr, int _n)
        {
            Addr = _addr;
            number = _n;
        }

    }


    #region XML Vars from unity

    // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class FBExchangeFile
    {

        private FBExchangeFileFileHeader fileHeaderField;

        private FBExchangeFileContentHeader contentHeaderField;

        private List<FBExchangeFileFBSource> fBSourceField;

        private List<FBExchangeFileDDTSource> dDTSourceField;

        /// <remarks/>
        public FBExchangeFileFileHeader fileHeader
        {
            get
            {
                return this.fileHeaderField;
            }
            set
            {
                this.fileHeaderField = value;
            }
        }

        /// <remarks/>
        public FBExchangeFileContentHeader contentHeader
        {
            get
            {
                return this.contentHeaderField;
            }
            set
            {
                this.contentHeaderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FBSource")]
        public List<FBExchangeFileFBSource> FBSource
        {
            get
            {
                return this.fBSourceField;
            }
            set
            {
                this.fBSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DDTSource")]
        public List<FBExchangeFileDDTSource> DDTSource
        {
            get
            {
                return this.dDTSourceField;
            }
            set
            {
                this.dDTSourceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFileHeader
    {

        private string companyField;

        private string productField;

        private string dateTimeField;

        private string contentField;

        private byte dTDVersionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string product
        {
            get
            {
                return this.productField;
            }
            set
            {
                this.productField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte DTDVersion
        {
            get
            {
                return this.dTDVersionField;
            }
            set
            {
                this.dTDVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileContentHeader
    {

        private string nameField;

        private string versionField;

        private string dateTimeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSource
    {

        private List<FBExchangeFileFBSourceAttribute> attributeField;

        private List<FBExchangeFileFBSourceVariables> inputParametersField;

        private List<FBExchangeFileDDTSourceVariables> outputParametersField;

        private List<FBExchangeFileFBSourceInOutParametersVariables> inOutParametersField;

        private List<FBExchangeFileFBSourcePublicLocalVariablesVariables> publicLocalVariablesField;

        private List<FBExchangeFileFBSourceVariables1> privateLocalVariablesField;

        private List<FBExchangeFileFBSourceFBProgram> fBProgramField;

        private string nameOfFBTypeField;

        private decimal versionField;

        private string dateTimeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attribute")]
        public List<FBExchangeFileFBSourceAttribute> attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<FBExchangeFileFBSourceVariables> inputParameters
        {
            get
            {
                return this.inputParametersField;
            }
            set
            {
                this.inputParametersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<FBExchangeFileFBSourceInOutParametersVariables> inOutParameters
        {
            get
            {
                return this.inOutParametersField;
            }
            set
            {
                this.inOutParametersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<FBExchangeFileDDTSourceVariables> outputParameters
        {
            get
            {
                return this.outputParametersField;
            }
            set
            {
                this.outputParametersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<FBExchangeFileFBSourcePublicLocalVariablesVariables> publicLocalVariables
        {
            get
            {
                return this.publicLocalVariablesField;
            }
            set
            {
                this.publicLocalVariablesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<FBExchangeFileFBSourceVariables1> privateLocalVariables
        {
            get
            {
                return this.privateLocalVariablesField;
            }
            set
            {
                this.privateLocalVariablesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FBProgram")]
        public List<FBExchangeFileFBSourceFBProgram> FBProgram
        {
            get
            {
                return this.fBProgramField;
            }
            set
            {
                this.fBProgramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nameOfFBType
        {
            get
            {
                return this.nameOfFBTypeField;
            }
            set
            {
                this.nameOfFBTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceAttribute
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceVariables
    {

        private FBExchangeFileFBSourceVariablesAttribute attributeField;

        private string nameField;

        private string typeNameField;

        public FBExchangeFileFBSourceVariables() { }

        public FBExchangeFileFBSourceVariables(string _name, string _type)
        {
            name = _name;
            typeName = _type;
        }

        /// <remarks/>
        public FBExchangeFileFBSourceVariablesAttribute attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeName
        {
            get
            {
                return this.typeNameField;
            }
            set
            {
                this.typeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceVariablesAttribute
    {

        private string nameField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceOutputParameters
    {

        private FBExchangeFileFBSourceOutputParametersVariables variablesField;

        /// <remarks/>
        public FBExchangeFileFBSourceOutputParametersVariables variables
        {
            get
            {
                return this.variablesField;
            }
            set
            {
                this.variablesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceOutputParametersVariables
    {

        private FBExchangeFileFBSourceOutputParametersVariablesAttribute attributeField;

        private string nameField;

        private string typeNameField;

        /// <remarks/>
        public FBExchangeFileFBSourceOutputParametersVariablesAttribute attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeName
        {
            get
            {
                return this.typeNameField;
            }
            set
            {
                this.typeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceInOutParametersVariables
    {

        private FBExchangeFileFBSourceInOutParametersVariablesAttribute attributeField;

        private string nameField;

        private string typeNameField;

        /// <remarks/>
        public FBExchangeFileFBSourceInOutParametersVariablesAttribute attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeName
        {
            get
            {
                return this.typeNameField;
            }
            set
            {
                this.typeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceInOutParametersVariablesAttribute
    {

        private string nameField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceOutputParametersVariablesAttribute
    {

        private string nameField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourcePublicLocalVariables
    {

        private FBExchangeFileFBSourcePublicLocalVariablesVariables variablesField;

        /// <remarks/>
        public FBExchangeFileFBSourcePublicLocalVariablesVariables variables
        {
            get
            {
                return this.variablesField;
            }
            set
            {
                this.variablesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourcePublicLocalVariablesVariables
    {

        private string nameField;

        private string typeNameField;
        public FBExchangeFileFBSourcePublicLocalVariablesVariables() { }
        public FBExchangeFileFBSourcePublicLocalVariablesVariables(string _name, string _type)
        {
            name = _name;
            typeName = _type;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeName
        {
            get
            {
                return this.typeNameField;
            }
            set
            {
                this.typeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceVariables1
    {

        private string nameField;

        private string typeNameField;

        public FBExchangeFileFBSourceVariables1() { }

        public FBExchangeFileFBSourceVariables1(string _name, string _type)
        {
            name = _name;
            typeName = _type;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeName
        {
            get
            {
                return this.typeNameField;
            }
            set
            {
                this.typeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileFBSourceFBProgram
    {

        private string sTSourceField;

        private string nameField;

        /// <remarks/>
        public string STSource
        {
            get
            {
                return this.sTSourceField;
            }
            set
            {
                this.sTSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileDDTSource
    {

        private FBExchangeFileDDTSourceAttribute attributeField;

        private string arrayField;

        private List<FBExchangeFileDDTSourceVariables> structureField;

        private string dDTNameField;

        private decimal versionField;

        private string dateTimeField;

        /// <remarks/>
        public FBExchangeFileDDTSourceAttribute attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        public string array
        {
            get
            {
                return this.arrayField;
            }
            set
            {
                this.arrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<FBExchangeFileDDTSourceVariables> structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DDTName
        {
            get
            {
                return this.dDTNameField;
            }
            set
            {
                this.dDTNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileDDTSourceAttribute
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileDDTSourceVariables
    {

        private FBExchangeFileDDTSourceVariablesAttribute attributeField;

        private string nameField;

        private string typeNameField;

        public FBExchangeFileDDTSourceVariables() { }
        public FBExchangeFileDDTSourceVariables(string _name, string _type, FBExchangeFileDDTSourceVariablesAttribute _attribute = null)
        {
            name = _name;
            typeName = _type;
            attribute = _attribute;
        }
        /// <remarks/>
        public FBExchangeFileDDTSourceVariablesAttribute attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeName
        {
            get
            {
                return this.typeNameField;
            }
            set
            {
                this.typeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FBExchangeFileDDTSourceVariablesAttribute
    {

        private string nameField;

        private byte valueField;

        public FBExchangeFileDDTSourceVariablesAttribute() { }

        public FBExchangeFileDDTSourceVariablesAttribute(string _name, byte _value)
        {
            name = _name;
            value = _value;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


    #endregion

}
