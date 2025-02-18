using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class FFmpegService : IDisposable
{
    private Process _ffmpegProcess;
    private CancellationTokenSource _cancellationTokenSource;

    public async Task StartContinuousScreenshotsAsync(string rtspUrl, string outputPath, Action onUpdate)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            // Overwrite the same file
            var processStartInfo = new ProcessStartInfo
            {
                FileName = @"\ffmpeg\bin\ffmpeg.exe",
                Arguments = $"-y -rtsp_transport tcp  -i \"{rtspUrl}\" -vf \"fps=1\" -q:v 1  -r 10 -frames:v 1 \"{outputPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"FFmpeg Error: {e.Data}");
                process.OutputDataReceived += (sender, e) => Console.WriteLine($"FFmpeg Output: {e.Data}");

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Screenshot saved to: {outputPath}");
                    onUpdate?.Invoke();
                }
                else
                {
                    Console.WriteLine($"FFmpeg failed with exit code: {process.ExitCode}");
                }
            }

            await Task.Delay(10, cancellationToken);
        }
    }

    public void StopContinuousScreenshots()
    {
        _cancellationTokenSource?.Cancel();
    }

    public void Dispose()
    {
        StopContinuousScreenshots();
        _cancellationTokenSource?.Dispose();
    }
}