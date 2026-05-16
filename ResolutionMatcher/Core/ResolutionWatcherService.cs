using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ResolutionMatcher.Display;
using System.Diagnostics;
namespace ResolutionMatcher.Core;


public class ResolutionWatcherService : IHostedService
{
    private readonly IProcessWatcher _processWatcher;
    private readonly IDisplayService _displayService;
    private readonly AppSettings _settings;

    private ResolutionInfo? _originalResolution;
    public ResolutionWatcherService(IProcessWatcher processWatcher, IDisplayService displayService, IOptions<AppSettings> settings)
    {
        _processWatcher = processWatcher;
        _displayService = displayService;
        _settings = settings.Value;

    }
    private void OnProcessStarted(object? sender, Process process)
    {
        // Handle the process start event here
        Console.WriteLine($"Process started: {process.ProcessName} (ID: {process.Id})");
        // Save the original resolution
        _originalResolution = _displayService.GetCurrentResolution();
        //set the new resolution to target resolution
        _displayService.SetResolution(_settings.TargetWidth, _settings.TargetHeight);
        //start a task to monitor the process and restore resolution when it exits
        Task.Run(() =>
        {
            process.WaitForExit();
            // Restore the original resolution when the process exits
            if (_originalResolution != null)
            {
                _displayService.SetResolution(_originalResolution.Width, _originalResolution.Height);
            }
        });

    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _processWatcher.ProcessStarted += OnProcessStarted;
        _processWatcher.StartWatching(_settings.GameProcessName);
        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _processWatcher.StopWatching();
        return Task.CompletedTask;
    }
}