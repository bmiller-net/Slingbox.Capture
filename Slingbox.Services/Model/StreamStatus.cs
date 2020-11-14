using System.Xml;

namespace Slingbox.Services.Model
{
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.slingbox.com")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://www.slingbox.com", ElementName = "stream", IsNullable = false)]
    public class StreamStatus
    {
        [System.Xml.Serialization.XmlElement("lebowski_profile")]
        public string LebowskiProfile { get; set; }

        [System.Xml.Serialization.XmlElement("video")]
        public Video Video { get; set; }

        [System.Xml.Serialization.XmlElement("audio")]
        public Audio Audio { get; set; }

        [System.Xml.Serialization.XmlElement("encryption")]
        public string Encryption { get; set; }

        [System.Xml.Serialization.XmlArray("stream_links")]
        [System.Xml.Serialization.XmlArrayItem("stream_link")]
        public StreamLink[] StreamLinks { get; set; }

        [System.Xml.Serialization.XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
        public string StreamAddress { get; set; }
    }

    public class StreamLink
    {
        [System.Xml.Serialization.XmlAttribute("id", Namespace = "")]
        public string Id { get; set; }

        [System.Xml.Serialization.XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
        public string Address { get; set; }
    }

    public class Audio
    {
        [System.Xml.Serialization.XmlElement("codec")]
        public string Codec { get; set; }

        [System.Xml.Serialization.XmlElement("mode")]
        public string Profile { get; set; }

        [System.Xml.Serialization.XmlElement("sampling")]
        public string Level { get; set; }

        [System.Xml.Serialization.XmlElement("bitrate")]
        public Rate Bitrate { get; set; }
    }

    public class Video
    {
        [System.Xml.Serialization.XmlElement("codec")]
        public string Codec { get; set; }

        [System.Xml.Serialization.XmlElement("profile")]
        public string Profile { get; set; }

        [System.Xml.Serialization.XmlElement("level")]
        public string Level { get; set; }

        [System.Xml.Serialization.XmlElement("resolution")]
        public string Resolution { get; set; }

        [System.Xml.Serialization.XmlElement("iframeinterval")]
        public string IframeInterval { get; set; }

        [System.Xml.Serialization.XmlElement("videomode")]
        public string VideoMode { get; set; }

        [System.Xml.Serialization.XmlElement("forceiframe")]
        public string ForceIframe { get; set; }

        [System.Xml.Serialization.XmlElement("prefiltering")]
        public string Prefiltering { get; set; }

        [System.Xml.Serialization.XmlElement("smoothness")]
        public string Smoothness { get; set; }

        [System.Xml.Serialization.XmlElement("bitrate")]
        public Rate Bitrate { get; set; }

        [System.Xml.Serialization.XmlElement("framerate")]
        public Rate Framerate { get; set; }
    }

    public class Rate
    {
        [System.Xml.Serialization.XmlElement("min")]
        public string Min { get; set; }

        [System.Xml.Serialization.XmlElement("max")]
        public string Max { get; set; }
    }
    

}