using System.Xml.Serialization;

namespace Slingbox.API.Model
{
    [XmlType(AnonymousType = true, Namespace = "http://www.slingbox.com")]
    [XmlRoot(Namespace = "http://www.slingbox.com", ElementName = "events", IsNullable = false)]
    public class EventsForceOkStatus
    {
        [XmlAttribute("streamtime")]
        public int StreamTime { get; set; }

        [XmlArrayItem("event")]
        [XmlArray]
        public Event[] Events { get; set; }
    }
}