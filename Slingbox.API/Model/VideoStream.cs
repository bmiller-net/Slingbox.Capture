using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Slingbox.API.Model
{
    public class VideoStream : IDisposable, INotifyPropertyChanged
    {
        private bool _isStreaming;

        public Stream Stream { get; set; }

        public bool IsStreaming
        {
            get { return _isStreaming; }
            set
            {
                OnPropertyChanged();
                _isStreaming = value;
            }
        }

        public void Dispose()
        {
            IsStreaming = false;
            Stream.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
