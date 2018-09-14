using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tag_Explorer
{


    // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Tags
    {

        private List<TagsTag> tagField;
        public Tags()
        {
            tagField = new List<TagsTag>();
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Tag")]
        public List<TagsTag> Tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TagsTag
    {

        private List<TagsTagProperty> propertyField;

        private TagsTagAlarms alarmsField;
        private List<TagsTag> tagField;
        private string nameField;
        private TagsTagParameters parametersField;
        private string pathField;

        private string typeField;
        public TagsTag() { propertyField = new List<TagsTagProperty>(); tagField = new List<TagsTag>();  }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public List<TagsTagProperty> Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }

        /// <remarks/>
        public TagsTagAlarms Alarms
        {
            get
            {
                return this.alarmsField;
            }
            set
            {
                this.alarmsField = value;
            }
        }
        public bool ShouldSerializealarmsField()
        {
            return alarmsField != null;
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
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("Tag")]
        public List<TagsTag> Tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }

        public TagsTagParameters Parameters
        {
            get { return parametersField; }
            set { parametersField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TagsTagProperty
    {

        private string nameField;

        private string valueField;
        public TagsTagProperty() { }
        public TagsTagProperty(string _n,string _v)
        {
            name = _n;
            Value = _v;
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
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
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
    public partial class TagsTagParameters
    {

        private List<TagsTagParametersProperty> propertyField;

        public TagsTagParameters() { propertyField = new List<TagsTagParametersProperty>(); }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public List<TagsTagParametersProperty> Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }
    }



    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TagsTagAlarms
    {

        private TagsTagAlarmsAlarm alarmField;
        public TagsTagAlarms() { alarmField = new TagsTagAlarmsAlarm(); }
        /// <remarks/>
        public TagsTagAlarmsAlarm Alarm
        {
            get
            {
                return this.alarmField;
            }
            set
            {
                this.alarmField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TagsTagAlarmsAlarm
    {

        private List<TagsTagAlarmsAlarmProperty> propertyField;

        private string nameField;
        public TagsTagAlarmsAlarm() { propertyField = new List<TagsTagAlarmsAlarmProperty>(); }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public List<TagsTagAlarmsAlarmProperty> Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
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
    public partial class TagsTagAlarmsAlarmProperty
    {

        private string nameField;

        private string valueField;
        public TagsTagAlarmsAlarmProperty()
        {

        }

        public TagsTagAlarmsAlarmProperty(string _n,string _v )
        {
            name = _n;
            Value = _v;
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
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
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

    public static class IgnitionHelper
    {
        public static Dictionary<int,string> Types { get; set; }

        public static void Load()
        {
            Types = new Dictionary<int, string>();
            Types.Add(6, "BOOL");
        }
    }


    

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TagsTagParametersProperty
    {

        private string nameField;

        private string typeField;

        private string valueField;


        public TagsTagParametersProperty() { }

        public TagsTagParametersProperty(string _n , string _t , string val)
        {
            name = _n;
            type = _t;
            Value = val;
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
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
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


}
