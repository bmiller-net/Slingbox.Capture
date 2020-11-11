using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Slingbox.API;
using Slingbox.API.Model;

namespace Slingbox.Capture
{
    public class StreamController : ApiController
    {
        private readonly VideoStream _videoStream;
        private readonly SlingboxService _slingboxService;

        public StreamController(VideoStream videoStream, SlingboxService slingboxService)
        {
            _videoStream = videoStream;
            _slingboxService = slingboxService;
        }
        
        [HttpGet]
        public HttpResponseMessage Slingbox()
        {
            var response = Request.CreateResponse();

            if (!_videoStream.IsStreaming)
            {
                Debug.Write("Issuing forceOkStatus request... ");

                _slingboxService.Initialize();

                Debug.WriteLine("COMPLETE");

                Debug.Write("Issuing device request... ");

                var deviceStatus = _slingboxService.GetDeviceStatus();

                Debug.WriteLine("COMPLETE");

                Debug.Write("Issuing stream request... ");

                var streamStatus = _slingboxService.InitializeStreams();

                Debug.WriteLine("COMPLETE");

                Debug.Write("Opening video stream... ");

                var streamResponse = _slingboxService.ConnectToStream("/streams/stream.asf");
                var videoStream = streamResponse.GetResponseStream();

                if (videoStream == null)
                {
                    Debug.WriteLine("ERROR OCCURRED");
                    response.StatusCode = HttpStatusCode.InternalServerError;
                }
                else
                {
                    Debug.WriteLine("COMPLETE\r\n");

                    Debug.WriteLine("Beginning stream playback...");
                    response.StatusCode = HttpStatusCode.OK;

                    _videoStream.IsStreaming = true;

                    _videoStream.Stream = new BufferedStream(videoStream);
                }
            }

            if (_videoStream.IsStreaming)
            {
                response.Content = new StreamContent(_videoStream.Stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("video/h264");
            }

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videoStream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
