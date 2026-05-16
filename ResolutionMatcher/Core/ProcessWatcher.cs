namespace ResolutionMatcher.Core;

using System.Diagnostics;
using System.IO;
using System.Management;
public interface IProcessWatcher
{
    event EventHandler<Process> ProcessStarted;
    public void StartWatching(string processName);
    public void StopWatching();


}
public class ProcessWatcher : IProcessWatcher, IDisposable
{
    public event EventHandler<Process>? ProcessStarted;
    private ManagementEventWatcher? _watcher;

    public void StartWatching(string ProcessName)
    {
        var query = $"SELECT * FROM Win32_ProcessStartTrace WHERE ProcessName = '{ProcessName}.exe'";
        _watcher = new ManagementEventWatcher(new WqlEventQuery(query));
        _watcher.EventArrived += (sender, e) =>
        {
            var processId = (uint)e.NewEvent.Properties["ProcessID"].Value;
            var process = Process.GetProcessById((int)processId);
            ProcessStarted?.Invoke(this, process);
        };
        _watcher.Start();
    }

    public void StopWatching()
    {
        _watcher?.Stop();
    }
    public void Dispose()
    {
        _watcher?.Stop();
        _watcher?.Dispose();
        GC.SuppressFinalize(this);
    }

}