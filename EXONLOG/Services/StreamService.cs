namespace EXONLOG.Services
{
    using System.Diagnostics;

    public class StreamService
    {
        private Process? _ffmpegProcess;
        private const string RtspUrl = "rtsp://alex:admin123!@192.168.60.110:554/Streaming/Channels/101";
        private const string OutputStream = "http://localhost:8080/live";

        public bool IsStreaming { get; private set; }

        public void StartStreaming()
        {
            if (IsStreaming)
                return;

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-rtsp_transport tcp -i \"{RtspUrl}\" -f mpegts -codec:v mpeg1video -s 640x360 {OutputStream}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                _ffmpegProcess = new Process { StartInfo = processInfo };
                _ffmpegProcess.ErrorDataReceived += (sender, args) => Debug.WriteLine(args.Data);
                _ffmpegProcess.OutputDataReceived += (sender, args) => Debug.WriteLine(args.Data);

                _ffmpegProcess.Start();
                _ffmpegProcess.BeginErrorReadLine();
                _ffmpegProcess.BeginOutputReadLine();
                IsStreaming = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FFmpeg failed: {ex.Message}");
            }
        }

        public void StopStreaming()
        {
            if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
            {
                _ffmpegProcess.Kill();
                _ffmpegProcess.Dispose();
                _ffmpegProcess = null;
                IsStreaming = false;
            }
        }
    }

}
