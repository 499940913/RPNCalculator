using System.Xml.Serialization;

namespace RPNDemoWPF
{   
   
    public class ButtonModel
    {
        [XmlAttribute]
        public string Symbol { get; set;}
        [XmlAttribute]
        public string Display { get; set; }
        [XmlAttribute]
        public bool IsOperator { get; set; }
    }
}
