using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Slingbox.Services;
using Slingbox.Services.Model;

namespace Slingbox.API.NetCore2
{
    [Route("/api/stream")]
    public class StreamController : ControllerBase
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
        public HttpResponseMessage Slingbox()
        {
            var response = new HttpResponseMessage();

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
    }
}
