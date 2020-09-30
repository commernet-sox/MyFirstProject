using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    public class HttpDownload
    {
        private const int DownBlockSize = 409600;
        private CancellationTokenSource _cts;

        public event EventHandler<DownloadProcessChangedEventArgs> ProcessChanged;

        public event EventHandler<DownloadStatusChangedEventArgs> StatusChanged;

        public Action<byte[], int> SaveBlock;

        public DownloadStatus Status { get; private set; } = DownloadStatus.None;

        public readonly string FileUrl;

        public HttpDownload(string url)
        {
            FileUrl = url;
            StatusChanged += (sender, e) =>
            {
                Status = e.Status;
            };
        }

        public void Cancel()
        {
            _cts.Cancel();
            StatusChanged?.Invoke(this, new DownloadStatusChangedEventArgs(DownloadStatus.Cancel));
        }

        public async Task DownloadAsync(long startPosition = 0, Action<HttpWebRequest> action = null)
        {
            if (Status == DownloadStatus.Downloading)
            {
                throw new InvalidOperationException("the file is being downloaded");
            }

            try
            {
                _cts = new CancellationTokenSource();

                var request = WebRequest.CreateHttp(FileUrl);
                if (startPosition > 0)
                {
                    request.AddRange(startPosition);
                }

                action?.Invoke(request);

                var response = (await request.GetResponseAsync()) as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    StatusChanged?.Invoke(this, new DownloadStatusChangedEventArgs(DownloadStatus.Error));
                    return;
                }

                var fileSize = response.ContentLength;
                var read = 0;
                var num = 0L;
                var stream = response.GetResponseStream();
                var buffer = new byte[DownBlockSize];
                StatusChanged?.Invoke(this, new DownloadStatusChangedEventArgs(DownloadStatus.Downloading));
                while ((read = await stream.ReadAsync(buffer, 0, DownBlockSize, _cts.Token)) > 0)
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    num += read;
                    SaveBlock?.Invoke(buffer, read);
                    _cts.Token.ThrowIfCancellationRequested();
                    var per = Math.Round(num / (double)fileSize * 100d, 2);
                    ProcessChanged?.Invoke(this, new DownloadProcessChangedEventArgs(per));
                    _cts.Token.ThrowIfCancellationRequested();
                }

                response.Close();
                StatusChanged?.Invoke(this, new DownloadStatusChangedEventArgs(DownloadStatus.Finish));
            }
            catch
            {
                StatusChanged?.Invoke(this, new DownloadStatusChangedEventArgs(DownloadStatus.Error));
            }
        }
    }

    public enum DownloadStatus
    {
        None,
        Downloading,
        Error,
        Cancel,
        Finish
    }

    public class DownloadStatusChangedEventArgs : EventArgs
    {
        public DownloadStatusChangedEventArgs(DownloadStatus status) => Status = status;

        public DownloadStatus Status { get; private set; } = DownloadStatus.None;
    }

    public class DownloadProcessChangedEventArgs : EventArgs
    {
        public DownloadProcessChangedEventArgs(double per) => Percentage = per;

        public double Percentage { get; private set; }
    }
}
