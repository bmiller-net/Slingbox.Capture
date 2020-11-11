using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Slingbox.API.Model;

namespace Slingbox.API
{
    public class SlingboxService : IDisposable
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private int _requestIndex = 1;
        private EventsForceOkStatus _heartbeatData;
        private Timer _timer;
        private bool _hasReceivedLastHeartbeat = true;

        private string _sessionURI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public SlingboxService(IPAddress ipAddress, int port, string username, string password)
        {
            _ipAddress = ipAddress;
            _port = port;
            _username = username;
            _password = password;
        }

        public bool IsConnected { get; private set; }

        public void Initialize()
        {
            var uri = "?forceOkStatus";

            var body = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" +
                       "<client xmlns=\"http://www.slingbox.com\">" +
                       "  <description>Austin Flash 10 client @ Windows 10</description>" +
                       "</client>";

            var response = SendRequestToSlingbox<InitialForceOKStatus>(uri, body);

            if (!string.IsNullOrWhiteSpace(response.SessionAddress))
            {
                IsConnected = true;
                _sessionURI = response.SessionAddress;
            }
        }

        private void BeginHeartbeat()
        {
            _timer = new Timer(state => SendHeartbeat(), _heartbeatData, 0, 5000);
        }

        private void CancelHeartbeat()
        {
            _timer.Dispose();
        }

        public void SendHeartbeat()
        {
            if (_hasReceivedLastHeartbeat)
            {
                _hasReceivedLastHeartbeat = false;
                _heartbeatData = GetEventStatus();
                _hasReceivedLastHeartbeat = true;
            }
        }

        public DeviceStatus GetDeviceStatus()
        {
            var uri = "/device?Method=GET&forceOkStatus";
            var body = "<body>dummy</body>";

            return SendRequestToSlingbox<DeviceStatus>(uri, body);
        }

        public EventsForceOkStatus GetEventStatus()
        {
            var uri = "/events?Method=GET&forceOkStatus&timeout=5";
            var body = "<body>dummy</body>";

            return SendRequestToSlingbox<EventsForceOkStatus>(uri, body);
        }

        public DisconnectStatus Disconnect()
        {
            var uri = "?Method=DELETE&forceOkStatus";
            var body = "<body>dummy</body>";

            CancelHeartbeat();

            return SendRequestToSlingbox<DisconnectStatus>(uri, body);
        }

        public StreamStatus InitializeStreams()
        {
            var uri = "/streams?forceOkStatus";
            var body = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" +
                       "<streaming xmlns=\"http://www.slingbox.com\">" +
                       "  <lebowski_profile>2</lebowski_profile>" +
                       "  <video>" +
                       "    <codec>h264</codec>" +
                       "    <profile>high</profile>" +
                       "    <level>4.0</level>" +
                       "    <resolution>640x480</resolution>" +
                       "    <bitrate>" +
                       "      <min>1200</min>" +
                       "      <max>10000</max>" +
                       "    </bitrate>" +
                       "    <framerate>" +
                       "      <min>5</min>" +
                       "      <max>30</max>" +
                       "    </framerate>" +
                       "  </video>" +
                       "  <audio>" +
                       "    <codec>AAC</codec>" +
                       "    <mode>stereo</mode>" +
                       "    <bitrate>" +
                       "      <min>32</min>" +
                       "      <max>96</max>" +
                       "    </bitrate>" +
                       "  </audio>" +
                       "</streaming>";

            BeginHeartbeat();

            return SendRequestToSlingbox<StreamStatus>(uri, body);
        }

        public HttpWebResponse GetStream(string uri)
        {
            return SendRequestToSlingbox(uri);
        }

        private T SendRequestToSlingbox<T>(string uri, string body = null) where T : class
        {
            var response = SendRequestToSlingbox(uri, body);
            var responseStream = response.GetResponseStream();

            if (responseStream == null)
                return null;

            var serializer = new XmlSerializer(typeof(T));

            return (T) serializer.Deserialize(responseStream);
        }

        public HttpWebResponse ConnectToStream(string uri)
        {
            if (!IsConnected && !string.IsNullOrWhiteSpace(_sessionURI))
                throw new Exception("Not connected to Slingbox. Call Initialize() method first.");

            // if URI was passed in with a leading slash, remove it
            if (uri.StartsWith("/"))
                uri = uri.Substring(1);

            var request = WebRequest.CreateHttp($"http://{_ipAddress}:{_port}/slingbox{_sessionURI}/{uri}");
            request.Method = WebRequestMethods.Http.Get;
            request.KeepAlive = true;
            request.Headers["Origin"] = "http://download.slingmedia.com";
            request.Headers["X-Requested-With"] = "ShockwaveFlash/23.0.0.207";
            request.Headers["Sling-Authorization"] = GenerateSlingAuthorizationHeader(_requestIndex, _username, _sessionURI, _password);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";
            request.Headers["Pragma"] = "Sling-Connection-Type=Stream, Session-Id=0";
            //request.ContentType = "text/xml";
            request.Accept = "*/*";
            request.Referer = "http://download.slingmedia.com/player/embedded/v28/austinapp.swf";
            //request.Headers["Accept-Encoding"] = "gzip, deflate";
            //request.Headers["Accept-Language"] = "en-US,en;q=0.8";
            
            _requestIndex++;

            var response = (HttpWebResponse)request.GetResponse();

            return response;
        }

        private HttpWebResponse SendRequestToSlingbox(string uri, string body = null)
        {
            if (!IsConnected && !string.IsNullOrWhiteSpace(_sessionURI))
                throw new Exception("Not connected to Slingbox. Call Initialize() method first.");

            // if URI was passed in with a leading slash, remove it
            if (uri.StartsWith("/"))
                uri = uri.Substring(1);

            var request = WebRequest.CreateHttp($"http://{_ipAddress}:{_port}/slingbox{_sessionURI}/{uri}");
            request.Method = WebRequestMethods.Http.Post;
            request.KeepAlive = true;
            request.Headers["Origin"] = "http://download.slingmedia.com";
            request.Headers["X-Requested-With"] = "ShockwaveFlash/23.0.0.207";
            request.Headers["Sling-Authorization"] = GenerateSlingAuthorizationHeader(_requestIndex, _username, _sessionURI, _password);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";
            request.ContentType = "text/xml";
            request.Accept = "*/*";
            request.Referer = "http://download.slingmedia.com/player/embedded/v28/austinapp.swf";
            request.Headers["Accept-Encoding"] = "gzip, deflate";
            request.Headers["Accept-Language"] = "en-US,en;q=0.8";

            if (!string.IsNullOrWhiteSpace(body))
            {
                using (var requestStream = request.GetRequestStream())
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    streamWriter.Write(body);
                }
            }

            _requestIndex++;

            var response = (HttpWebResponse) request.GetResponse();

            return response;
        }

        private static string GenerateSlingAuthorizationHeader(int requestIndex, string username, string numericURIComponent, string password)
        {
            var cnonce = Guid.NewGuid().ToString("N").ToLower();
            var digest = GenerateDigestHash(requestIndex, username, cnonce, numericURIComponent, password);

            var slingAuthorizationHeader = $"account={username}, counter={requestIndex}, cnonce={cnonce}, digest={digest}";

            return slingAuthorizationHeader;
        }

        private static string GenerateDigestHash(int requestIndex, string username, string cnonce, string numericURIComponent, string password)
        {
            var md5 = new MD5Cng();
            md5.Initialize();

            var digestComponents = $"{username}:{numericURIComponent}:{cnonce}:{requestIndex}:{password}";
            var digestComponentsBytes = Encoding.ASCII.GetBytes(digestComponents);
            var digestBytes = md5.ComputeHash(digestComponentsBytes);

            var sb = new StringBuilder();
            foreach (var digestByte in digestBytes)
            {
                sb.Append(digestByte.ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public void Dispose()
        {
            if (IsConnected)
            {
                Disconnect();
            }
        }
    }
}
