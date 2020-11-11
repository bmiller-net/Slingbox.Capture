namespace Slingbox.API.Model
{
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.slingbox.com")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://www.slingbox.com", ElementName = "session", IsNullable = false)]
    public class InitialForceOKStatus
    {

        [System.Xml.Serialization.XmlElement("events")]
        public SessionAddress Events { get; set; }


        [System.Xml.Serialization.XmlElement("events_v2")]
        public SessionAddress EventsV2 { get; set; }


        [System.Xml.Serialization.XmlElement("events_reg")]
        public SessionAddress EventsReg { get; set; }


        [System.Xml.Serialization.XmlElement("caps")]
        public SessionAddress Caps { get; set; }


        [System.Xml.Serialization.XmlElement("avinputs")]
        public SessionAddress AVInputs { get; set; }


        [System.Xml.Serialization.XmlElement("current_avinput")]
        public SessionAddress CurrentAVInput { get; set; }


        [System.Xml.Serialization.XmlElement("streams")]
        public SessionAddress Streams { get; set; }


        [System.Xml.Serialization.XmlElement("device")]
        public SessionAddress Device { get; set; }


        [System.Xml.Serialization.XmlElement("whatison")]
        public SessionAddress WhatIsOn { get; set; }


        [System.Xml.Serialization.XmlElement("setup")]
        public SessionAddress Setup { get; set; }


        [System.Xml.Serialization.XmlElement("host_app")]
        public SessionAddress HostApp { get; set; }


        [System.Xml.Serialization.XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
        public string SessionAddress { get; set; }
    }

    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.slingbox.com")]
    public class SessionAddress
    {
        [System.Xml.Serialization.XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
        public string Address { get; set; }
    }
}