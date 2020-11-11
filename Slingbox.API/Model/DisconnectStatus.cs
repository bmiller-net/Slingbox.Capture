namespace Slingbox.API.Model
{
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.slingbox.com")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://www.slingbox.com", ElementName = "session", IsNullable = false)]
    public class DisconnectStatus
    {

        [System.Xml.Serialization.XmlElement("close")]
        public string Close { get; set; }
    }
}