using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tag_Explorer
{

    #region Variables Export/Import
    // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VariablesExchangeFile
    {

        private VariablesExchangeFileFileHeader fileHeaderField;

        private VariablesExchangeFileContentHeader contentHeaderField;

        private List<VariablesExchangeFileVariables> dataBlockField;
        private List<DDTExchangeFileDDTSource> dDTSourceField;

        public VariablesExchangeFile() { DDTSource = new List<DDTExchangeFileDDTSource>(); }

        /// <remarks/>
        public VariablesExchangeFileFileHeader fileHeader
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
        public VariablesExchangeFileContentHeader contentHeader
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
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<VariablesExchangeFileVariables> dataBlock
        {
            get
            {
                return this.dataBlockField;
            }
            set
            {
                this.dataBlockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DDTSource")]
        public List<DDTExchangeFileDDTSource> DDTSource
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
    public partial class VariablesExchangeFileFileHeader
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
    public partial class VariablesExchangeFileContentHeader
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
    public partial class VariablesExchangeFileVariables
    {

        private string commentField;

        private VariablesExchangeFileVariablesVariableInit variableInitField;

        private VariablesExchangeFileVariablesAttribute attributeField;

        private string nameField;

        private string typeNameField;

        private string topologicalAddressField;

        public VariablesExchangeFileVariables() { }
        public VariablesExchangeFileVariables(string _name, string _type, string topAddr, string _comment, string _init)
        {
            name = _name;
            typeName = _type;
            topologicalAddress = topAddr;
            comment = _comment;
            if (_init != "")
            {
                variableInit = new VariablesExchangeFileVariablesVariableInit(_init);
            }
        }

        /// <remarks/>
        public string comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        public VariablesExchangeFileVariablesVariableInit variableInit
        {
            get
            {
                return this.variableInitField;
            }
            set
            {
                this.variableInitField = value;
            }
        }

        public bool ShouldSerializevariableInit()
        {
            return variableInit != null;
        }

        /// <remarks/>
        public VariablesExchangeFileVariablesAttribute attribute
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string topologicalAddress
        {
            get
            {
                return this.topologicalAddressField;
            }
            set
            {
                this.topologicalAddressField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class VariablesExchangeFileVariablesVariableInit
    {

        private string valueField;
        public VariablesExchangeFileVariablesVariableInit()
        {

        }

        public VariablesExchangeFileVariablesVariableInit(string v)
        {
            value = v;
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
    public partial class VariablesExchangeFileVariablesAttribute
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
    #endregion

    #region Ladder

    // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class LDExchangeFile
    {

        private LDExchangeFileFileHeader fileHeaderField;

        private LDExchangeFileContentHeader contentHeaderField;

        private LDExchangeFileProgram programField;

        private List<VariablesExchangeFileVariables> dataBlockField;

        /// <remarks/>
        public LDExchangeFileFileHeader fileHeader
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
        public LDExchangeFileContentHeader contentHeader
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
        public LDExchangeFileProgram program
        {
            get
            {
                return this.programField;
            }
            set
            {
                this.programField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<VariablesExchangeFileVariables> dataBlock
        {
            get
            {
                return this.dataBlockField;
            }
            set
            {
                this.dataBlockField = value;
            }
        }
    }
    #region pas utile
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileFileHeader
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
    public partial class LDExchangeFileContentHeader
    {

        private string nameField;

        private string versionField;

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
    }
    #endregion


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgram
    {

        private LDExchangeFileProgramIdentProgram identProgramField;

        private LDExchangeFileProgramLDSource lDSourceField;

        /// <remarks/>
        public LDExchangeFileProgramIdentProgram identProgram
        {
            get
            {
                return this.identProgramField;
            }
            set
            {
                this.identProgramField = value;
            }
        }

        /// <remarks/>
        public LDExchangeFileProgramLDSource LDSource
        {
            get
            {
                return this.lDSourceField;
            }
            set
            {
                this.lDSourceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramIdentProgram
    {

        private string nameField;

        private string typeField;

        private string taskField;

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
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string task
        {
            get
            {
                return this.taskField;
            }
            set
            {
                this.taskField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSource
    {

        private LDExchangeFileProgramLDSourceNetworkLD networkLDField;

        private byte nbColumnsField;

        /// <remarks/>
        public LDExchangeFileProgramLDSourceNetworkLD networkLD
        {
            get
            {
                return this.networkLDField;
            }
            set
            {
                this.networkLDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte nbColumns
        {
            get
            {
                return this.nbColumnsField;
            }
            set
            {
                this.nbColumnsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLD
    {

        private List<LDExchangeFileProgramLDSourceNetworkLDTypeLine> typeLineField;

        private List<LDExchangeFileProgramLDSourceNetworkLDTextBox> textBoxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("typeLine")]
        public List<LDExchangeFileProgramLDSourceNetworkLDTypeLine> typeLine
        {
            get
            {
                return this.typeLineField;
            }
            set
            {
                this.typeLineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("textBox")]
        public List<LDExchangeFileProgramLDSourceNetworkLDTextBox> textBox
        {
            get
            {
                return this.textBoxField;
            }
            set
            {
                this.textBoxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTypeLine
    {

        private LDExchangeFileProgramLDSourceNetworkLDTypeLineContact contactField;

        private LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink hLinkField;

        private LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil coilField;

        private LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine emptyLineField;

        public LDExchangeFileProgramLDSourceNetworkLDTypeLine()
        {

        }

        public LDExchangeFileProgramLDSourceNetworkLDTypeLine(LDExchangeFileProgramLDSourceNetworkLDTypeLineContact _contact,
            LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink _h,
            LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil _coil,
            LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine _e)
        {
            contact = _contact;
            HLink = _h;
            coil = _coil;
            emptyLine = _e;
        }
        /// <remarks/>
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineContact contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }

        /// <remarks/>
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink HLink
        {
            get
            {
                return this.hLinkField;
            }
            set
            {
                this.hLinkField = value;
            }
        }

        /// <remarks/>
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil coil
        {
            get
            {
                return this.coilField;
            }
            set
            {
                this.coilField = value;
            }
        }

        /// <remarks/>
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine emptyLine
        {
            get
            {
                return this.emptyLineField;
            }
            set
            {
                this.emptyLineField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTypeLineContact
    {

        private string typeContactField;

        private string contactVariableNameField;
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineContact() { }
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineContact(string type,string vName)
        {
            typeContact = type;
            contactVariableName = vName;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeContact
        {
            get
            {
                return this.typeContactField;
            }
            set
            {
                this.typeContactField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contactVariableName
        {
            get
            {
                return this.contactVariableNameField;
            }
            set
            {
                this.contactVariableNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink
    {

        private byte nbCellsField;
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink() { }
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineHLink(byte nb)
        {
            nbCells = nb;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte nbCells
        {
            get
            {
                return this.nbCellsField;
            }
            set
            {
                this.nbCellsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil
    {

        private string typeCoilField;

        private string coilVariableNameField;
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil() { }
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineCoil(string type,string vName  )
        {
            typeCoil = type;
            coilVariableName = vName;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCoil
        {
            get
            {
                return this.typeCoilField;
            }
            set
            {
                this.typeCoilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string coilVariableName
        {
            get
            {
                return this.coilVariableNameField;
            }
            set
            {
                this.coilVariableNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine
    {

        private byte nbRowsField;
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine() { }
        public LDExchangeFileProgramLDSourceNetworkLDTypeLineEmptyLine(byte nb)
        {
            nbRows = nb;
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte nbRows
        {
            get
            {
                return this.nbRowsField;
            }
            set
            {
                this.nbRowsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTextBox
    {

        private LDExchangeFileProgramLDSourceNetworkLDTextBoxObjPosition objPositionField;

        private string[] textField;

        private byte widthField;

        private byte heightField;

        public LDExchangeFileProgramLDSourceNetworkLDTextBox() { }
        public LDExchangeFileProgramLDSourceNetworkLDTextBox(byte _w,byte _h,byte _x,byte _y,string[] text)
        {
            width = _w;
            height = _h;
            objPosition = new LDExchangeFileProgramLDSourceNetworkLDTextBoxObjPosition(_x, _y);
            Text = text;
        }
        /// <remarks/>
        public LDExchangeFileProgramLDSourceNetworkLDTextBoxObjPosition objPosition
        {
            get
            {
                return this.objPositionField;
            }
            set
            {
                this.objPositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileProgramLDSourceNetworkLDTextBoxObjPosition
    {

        private byte posXField;

        private byte posYField;

        public LDExchangeFileProgramLDSourceNetworkLDTextBoxObjPosition() { }
        public LDExchangeFileProgramLDSourceNetworkLDTextBoxObjPosition(byte _x,byte _y)
        {
            posX = _x;
            posY = _y;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte posX
        {
            get
            {
                return this.posXField;
            }
            set
            {
                this.posXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte posY
        {
            get
            {
                return this.posYField;
            }
            set
            {
                this.posYField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LDExchangeFileVariables
    {

        private string commentField;

        private string nameField;

        private string typeNameField;

        private string topologicalAddressField;

        /// <remarks/>
        public string comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string topologicalAddress
        {
            get
            {
                return this.topologicalAddressField;
            }
            set
            {
                this.topologicalAddressField = value;
            }
        }
    }


    #endregion

    #region ST

    // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class STExchangeFile
    {

        private STExchangeFileFileHeader fileHeaderField;

        private STExchangeFileContentHeader contentHeaderField;

        private STExchangeFileProgram programField;

        private List<VariablesExchangeFileVariables> dataBlockField;

        /// <remarks/>
        public STExchangeFileFileHeader fileHeader
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
        public STExchangeFileContentHeader contentHeader
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
        public STExchangeFileProgram program
        {
            get
            {
                return this.programField;
            }
            set
            {
                this.programField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("variables", IsNullable = false)]
        public List<VariablesExchangeFileVariables> dataBlock
        {
            get
            {
                return this.dataBlockField;
            }
            set
            {
                this.dataBlockField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STExchangeFileFileHeader
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
    public partial class STExchangeFileContentHeader
    {

        private string nameField;

        private string versionField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STExchangeFileProgram
    {

        private STExchangeFileProgramIdentProgram identProgramField;

        private string sTSourceField;

        /// <remarks/>
        public STExchangeFileProgramIdentProgram identProgram
        {
            get
            {
                return this.identProgramField;
            }
            set
            {
                this.identProgramField = value;
            }
        }

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STExchangeFileProgramIdentProgram
    {

        private string nameField;

        private string typeField;

        private string taskField;

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
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string task
        {
            get
            {
                return this.taskField;
            }
            set
            {
                this.taskField = value;
            }
        }
    }







    #endregion

    #region DDT


    // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DDTExchangeFile
    {

        private DDTExchangeFileFileHeader fileHeaderField;

        private DDTExchangeFileContentHeader contentHeaderField;

        private List<DDTExchangeFileDDTSource> dDTSourceField;
        public DDTExchangeFile()
        {
            DDTSource = new List<DDTExchangeFileDDTSource>();
        }

        /// <remarks/>
        public DDTExchangeFileFileHeader fileHeader
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
        public DDTExchangeFileContentHeader contentHeader
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
        [System.Xml.Serialization.XmlElementAttribute("DDTSource")]
        public List<DDTExchangeFileDDTSource> DDTSource
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
    public partial class DDTExchangeFileFileHeader
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
    public partial class DDTExchangeFileContentHeader
    {

        private string nameField;

        private string versionField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DDTExchangeFileDDTSource
    {

        private DDTExchangeFileDDTSourceAttribute attributeField;

        private List<DDTExchangeFileDDTSourceVariables> structureField;

        private string dDTNameField;

        private decimal versionField;

        private string dateTimeField;
        public DDTExchangeFileDDTSource() { }
        public DDTExchangeFileDDTSource(string name)
        {
            dDTNameField = name;
            version = 0.00M;
            dateTime = "dt#1990-01-01-00:00:00";
            attribute = new DDTExchangeFileDDTSourceAttribute("TypeSignatureCheckSumString", "F0B8");
            structure = new List<DDTExchangeFileDDTSourceVariables>();
        }
        /// <remarks/>
        public DDTExchangeFileDDTSourceAttribute attribute
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
        public List<DDTExchangeFileDDTSourceVariables> structure
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
    public partial class DDTExchangeFileDDTSourceAttribute
    {

        private string nameField;

        private string valueField;
        public DDTExchangeFileDDTSourceAttribute() { }
        public DDTExchangeFileDDTSourceAttribute(string n, string v)
        {
            name = n;
            value = v;
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
    public partial class DDTExchangeFileDDTSourceVariables
    {

        private string nameField;

        private string typeNameField;

        private DDTExchangeFileDDTSourceVariableAttribute attributeField;

        public DDTExchangeFileDDTSourceVariables() { }
        public DDTExchangeFileDDTSourceVariables(string n,string t)
        {
            name = n;
            typeName = t;
        }

        /// <remarks/>
        public DDTExchangeFileDDTSourceVariableAttribute attribute
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

    public partial class DDTExchangeFileDDTSourceVariableAttribute
    {

        private string nameField;

        private string valueField;
        public DDTExchangeFileDDTSourceVariableAttribute() { }
        public DDTExchangeFileDDTSourceVariableAttribute(string n, string v)
        {
            name = n;
            value = v;
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


    #endregion
}
