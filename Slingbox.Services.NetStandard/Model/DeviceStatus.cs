namespace Slingbox.Services.Model
{
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.slingbox.com")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://www.slingbox.com", ElementName = "device", IsNullable = false)]
    public class DeviceStatus
    {
        [System.Xml.Serialization.XmlElement("id")]
        public string Id { get; set; }

        [System.Xml.Serialization.XmlElement("product")]
        public string Product { get; set; }

        [System.Xml.Serialization.XmlElement("irblaster")]
        public string IRBlaster { get; set; }

        [System.Xml.Serialization.XmlElement("hardware_version")]
        public string HardwareVersion { get; set; }

        [System.Xml.Serialization.XmlElement("firmware_version")]
        public string FirmwareVersion { get; set; }

        [System.Xml.Serialization.XmlElement("firmware_date")]
        public string FirmwareDate { get; set; }

        [System.Xml.Serialization.XmlElement("mac")]
        public string MACAddress { get; set; }

        [System.Xml.Serialization.XmlElement("utf8_box_name")]
        public string UTF8BoxName { get; set; }

        [System.Xml.Serialization.XmlElement("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement("remote_config")]
        public string RemoteConfig { get; set; }

        [System.Xml.Serialization.XmlElement("remote_access")]
        public string RemoteAccess { get; set; }

        [System.Xml.Serialization.XmlArray("accounts")]
        public Account[] Accounts { get; set; }

        [System.Xml.Serialization.XmlElement("ip")]
        public IPInformation IPInformation { get; set; }
    }
    
    public class Account
    {
        [System.Xml.Serialization.XmlElement("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement("description")]
        public string Description { get; set; }
    }

    public class IPInformation
    {
        [System.Xml.Serialization.XmlElement("type")]
        public string Type { get; set; }

        [System.Xml.Serialization.XmlElement("address")]
        public string Address { get; set; }

        [System.Xml.Serialization.XmlElement("subnet_mask")]
        public string SubnetMask { get; set; }

        [System.Xml.Serialization.XmlElement("gateway")]
        public string Gateway { get; set; }

        [System.Xml.Serialization.XmlElement("port")]
        public string Port { get; set; }

        [System.Xml.Serialization.XmlElement("mtu")]
        public string MTU { get; set; }

        [System.Xml.Serialization.XmlElement("dns_preferred")]
        public string DNSPreferred { get; set; }

        [System.Xml.Serialization.XmlElement("dns_alernate")]
        public string DNSAlternate { get; set; }
    }
}