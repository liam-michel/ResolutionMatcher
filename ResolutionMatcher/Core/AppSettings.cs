
public record ResolutionConfig(int Width, int Height);
public class AppSettings
{
    public string GameProcessName { get; set; } = "cs2.exe";
    public int TargetWidth { get; set; } = 1024;
    public int TargetHeight { get; set; } = 768;

    public string TargetAspectRatio { get; set; } = "4:3";
  public Dictionary<string, List<ResolutionConfig>> Resolutions { get; set; } = new Dictionary<string, List<ResolutionConfig>>
  {
    {
      "4_3", new List<ResolutionConfig>
      {
        new ResolutionConfig(1024, 768),
        new ResolutionConfig(1280, 960),
        new ResolutionConfig(1600, 1200),
        new ResolutionConfig(1920, 1440),
        new ResolutionConfig(2560, 1920),
        new ResolutionConfig(3840, 2880)
      }
    },
    {"5_4", new List<ResolutionConfig>
      {
        new ResolutionConfig(1280, 1024),
        new ResolutionConfig(2560, 2048)
      }
    },
    {
      "16_9", new List<ResolutionConfig>
      {
        new ResolutionConfig(1280, 720),
        new ResolutionConfig(1920, 1080),
        new ResolutionConfig(2560, 1440),
        new ResolutionConfig(3840, 2160)
      }
    },
    {"16_10", new List<ResolutionConfig>
      {
        new ResolutionConfig(1280, 800),
        new ResolutionConfig(1440, 900),
        new ResolutionConfig(1680, 1050),
        new ResolutionConfig(1920, 1200),
        new ResolutionConfig(2560, 1600),
        new ResolutionConfig(3840, 2400)
      }
    }
  };
}



