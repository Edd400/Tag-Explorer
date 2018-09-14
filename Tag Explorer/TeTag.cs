using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tag_Explorer
{
    public class TeTag
    {
        public string Name { get; set; }
        public string APIVariableName { get; set; }
        public string APIAddress { get; set; }
        public string SUPAddress { get; set; }
        public string SupervisorVariableName { get; set; }
        public string Comment { get; set; }
        public string AdditionnalComment { get; set; }
        public string Type { get; set; }
        public string IOScanningVariable { get; set; }
        public string COMAddress { get; set; }
        public string AlarmActive { get; set; }
        public string AlarmSetPoint { get; set; }
        public string AlarmPriority { get; set; }
        public string IgnitionOPCServer { get; set; }
        public string IgnitionDevice { get; set; }
        public string AlarmText { get; set; }
        public string DisplayPath { get; set; }
        public bool ReadyForIgnition { get; set; }
        public bool ReadyForUnity { get; set; }
        public string ScaleMode { get; set; }
        public string ClampMode { get; set; }
        public string RawMin { get; set; }
        public string RawMax { get; set; }
        public string ScaledMin { get; set; }
        public string ScaledMax { get; set; }
        public string Parent { get; set; }
        public string DB { get; set; }
        public Dictionary<string, string> Parameters;
        public bool GENCOMREADY { get; set; }
        public bool IsParent { get; set; }
        public bool HasParent { get; set; }
        public bool IsEqt { get; set; }
        public string SectionName { get; set; }
        public int OffsetDecay { get; set; }
        public int BitsTaken { get; set; }

        public bool SupAdressed { get; set; }
        public bool APIAdressed { get; set; }


        public List<TeTag> Childs { get; set; }
        public TeTag Master { get; set; }

        public Alarm myAlarm { get; set; }

        public enum Priorities : int
        {
            High = 3,
            Medium = 2,
            Low = 1

        }

        public TeTag()
        {
            Parameters = new Dictionary<string, string>();
            Childs = new List<TeTag>();
            Type = "";
            myAlarm = new Alarm();
        }

        public TeTag(string name, string comment = "", string type = "", string apiAddress = "", string supAddress = "", string addComment = "", string alarmActive = "", string alarmSetPoint = "",
            string alarmPriority = "", string opcServer = "", string device = "" )
        {
            Name = name;
            APIVariableName = name;
            
            
            APIAddress = apiAddress;
            Comment = comment;
            AdditionnalComment = addComment;
            Type = type;
            AlarmActive = alarmActive;
            AlarmSetPoint = alarmSetPoint;
            AlarmPriority = alarmPriority;
            IgnitionOPCServer = opcServer;
            IgnitionDevice = device;
            SUPAddress = supAddress;
            if (!string.IsNullOrEmpty(SUPAddress))
                SupervisorVariableName = name;
            Parent = "";
            DB = "";
            Parameters = new Dictionary<string, string>();
            Childs = new List<TeTag>();
            myAlarm = new Alarm();

        }
        
        public void CheckTag()
        {
            if (IsParent)
            {
                GENCOMREADY = true;
                ReadyForIgnition = true;
                ReadyForUnity = true;
                var off = 0;
                foreach(TeTag tag in Childs)
                {
                    BitsTaken += tag.BitsTaken;
                    IsParent = true;
                    tag.OffsetDecay = off;
                    tag.Parent = Name;
                    tag.CheckTag();                 
                    if (!tag.GENCOMREADY)
                    {
                        GENCOMREADY = false;
                    }
                    if (!tag.ReadyForIgnition)
                    {
                        ReadyForIgnition = false;
                    }
                    if (!tag.ReadyForUnity)
                    {
                        ReadyForUnity = false;
                    }
                    Parameters = tag.Parameters;
                    off++;
                }
                

            }
            else
            {

                #region Vérif des différentes entrées
                if (!string.IsNullOrEmpty(Parent))
                {
                    Parent = Parent.Replace("\n", "");
                    Parent = Parent.Replace("\r", "");
                    Parent = Parent.Replace(" ", "");
                    Master = Engine.TeTags.First(x => x.Name == Parent);
                    IsParent = true;

                }

                if (!string.IsNullOrEmpty(Comment))
                {
                    Comment = Comment.Replace("\n", "");
                    Comment = Comment.Replace("\r", "");

                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Name = Name.Replace("\n", "");
                    Name = Name.Replace("\r", "");
                    Name = Name.Replace(" ", "");

                }
                if (!string.IsNullOrEmpty(APIAddress))
                {
                    APIAddress = APIAddress.Replace("\n", "");
                    APIAddress = APIAddress.Replace("\r", "");
                    APIAddress = APIAddress.Replace(" ", "");

                    if (APIAddress.Contains("{Section}") || Master.GENCOMREADY)
                    {
                        IsEqt = true;
                        SectionName = APIAddress.Replace("{Section}", "");
                    }

                }
                if (!string.IsNullOrEmpty(COMAddress))
                {
                    COMAddress = COMAddress.Replace("\n", "");
                    COMAddress = COMAddress.Replace("\r", "");
                    COMAddress = COMAddress.Replace(" ", "");

                }



                if (!string.IsNullOrEmpty(SUPAddress))
                {
                    SUPAddress = SUPAddress.Replace("\n", "");
                    SUPAddress = SUPAddress.Replace("\r", "");
                    SUPAddress = SUPAddress.Replace(" ", "");
                    SupervisorVariableName = Name;
                }




                if (!string.IsNullOrEmpty(APIVariableName) && !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(APIAddress))
                {
                    APIAddress = APIAddress.Replace("\n", "");
                    APIAddress = APIAddress.Replace("\r", "");


                    Type = Type.Replace("\n", "");
                    Type = Type.Replace("\r", "");

                    ReadyForUnity = true;
                }

                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Type))
                {


                    Type = Type.Replace("\n", "");
                    Type = Type.Replace("\r", "");
                    if (!string.IsNullOrEmpty(IgnitionOPCServer) & !string.IsNullOrEmpty(IgnitionDevice))
                    {
                        IgnitionDevice = IgnitionDevice.Replace("\r", "");
                        IgnitionDevice = IgnitionDevice.Replace("\n", "");
                        IgnitionOPCServer = IgnitionOPCServer.Replace("\r", "");
                        IgnitionOPCServer = IgnitionOPCServer.Replace("\n", "");
                    }


                    ReadyForIgnition = true;
                }
                #region Gencom
                if (!string.IsNullOrEmpty(Name))
                    if (!string.IsNullOrEmpty(Type))
                        if (!string.IsNullOrEmpty(COMAddress))
                            if (!string.IsNullOrEmpty(Parent))
                                GENCOMREADY = true;
                #endregion

            }

            // Check si le tag est adressé ou pas
            if (!string.IsNullOrEmpty(APIAddress))
                APIAdressed = true;
            if (!string.IsNullOrEmpty(SUPAddress))
                SupAdressed = true;

            #endregion

            #region Creation alarm
            if (!string.IsNullOrEmpty(AlarmActive))
            {
                if(AlarmActive.ToUpper() == "TRUE")
                {
                    myAlarm.Exists = true;
                    myAlarm.AlarmText = AlarmText;
                    var s = 0;
                    int.TryParse(AlarmPriority, out s);
                    myAlarm.AlarmPriority = s;
                    myAlarm.AlarmActiveValue = AlarmSetPoint;
                    myAlarm.AlarmPath = DisplayPath;
                }      
            }
            
            #endregion

            #region Calcul du nombre de bits consommé par la variable
            if (!Engine.Types.Contains(Type.ToUpper()) && !string.IsNullOrEmpty(Type))
            {
                if(Engine.TeTags.FirstOrDefault(x => x.Name == Type) != null)
                {
                    BitsTaken = Engine.TeTags.FirstOrDefault(x => x.Name == Type).BitsTaken;
                }
                
            }
            else
            {
                if (Type.ToUpper() == "BOOL")
                {
                    BitsTaken = 1;
                }
                if (Type.ToUpper() == "INT")
                {
                    BitsTaken = 16;
                }
                if (Type.ToUpper() == "DINT")
                {
                    BitsTaken = 32;
                }
                if (Type.ToUpper() == "REAL")
                {
                    BitsTaken = 32;
                }
                if (Type.ToUpper() == "FLOAT")
                {
                    BitsTaken = 32;
                }
                if (Type.ToUpper() == "DOUBLE")
                {
                    BitsTaken = 32;
                }
                if (Type.ToUpper() == "WORD")
                {
                    BitsTaken = 16;
                }
            }
            #endregion

        }
        public void AddParameter(string name, string value)
        {
            Parameters.Add(name, value);
        }
    }

    public class Alarm
    {
        public string AlarmText { get; set; }
        public int AlarmPriority { get; set; }
        public string AlarmActiveValue { get; set; }
        public string AlarmPath { get; set; }
        public bool Exists { get; set; }
        public Priotity myPriority { get; set; }

        public enum Priotity : int
        {
            VeryHigh = 4,
            High = 3,
            Medium = 2,
            Low = 1,
            Info = 0
        }

        public Alarm()
        {

        }

    }
}
