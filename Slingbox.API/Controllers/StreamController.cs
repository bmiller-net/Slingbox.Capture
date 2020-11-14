using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Slingbox.Services;
using Slingbox.Services.Model;

namespace Slingbox.API.Controllers
{
    [Route("/api/stream")]
    public class StreamController : Controller
    {
        private readonly VideoStream _videoStream;
        private readonly SlingboxService _slingboxService;
        private bool _streamingStatus { get; set; }
        
        public StreamController(VideoStream videoStream, SlingboxService slingboxService)
        {
            _videoStream = videoStream;
            _slingboxService = slingboxService;

            _videoStream.PropertyChanged += _videoStream_PropertyChanged;
        }

        private void _videoStream_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Property changed: " + e.PropertyName);

            if (e.PropertyName == "IsStreaming")
            {
                Debug.WriteLine($"   Old value was: {_streamingStatus}");
                _streamingStatus = _videoStream.IsStreaming;
                Debug.WriteLine($"   New value is: {_streamingStatus}");
            }
        }

        [Route("slingbox")]
        [HttpGet]
        public ActionResult Slingbox()
        {
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
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
                else
                {
                    Debug.WriteLine("COMPLETE\r\n");

                    Debug.WriteLine("Beginning stream playback...");
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                    _videoStream.IsStreaming = true;

                    _videoStream.Stream = new BufferedStream(videoStream);
                }
            }

            if (_videoStream.IsStreaming)
            {
                return new FileStreamResult(_videoStream.Stream, "video/h264");
            }

            return new NoContentResult();
        }
    }
}
