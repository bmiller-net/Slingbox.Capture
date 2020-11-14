using System.Xml.Serialization;

namespace Slingbox.Services.Model
{
    public class Event
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("streamtime")]
        public int StreamTime { get; set; }

        [XmlElement("lock")]
        public string Lock { get; set; }

        [XmlElement("aspect_ratio")]
        public string AspectRatio { get; set; }

        [XmlElement("width")]
        public int Width { get; set; }

        [XmlElement("height")]
        public int Height { get; set; }
    }
}