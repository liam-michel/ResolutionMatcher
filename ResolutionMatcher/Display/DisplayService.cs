using System.Runtime.InteropServices;
using System.Windows.Forms;





namespace ResolutionMatcher.Display;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
struct DEVMODE
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmDeviceName;
    public short dmSpecVersion;
    public short dmDriverVersion;
    public short dmSize;
    public short dmDriverExtra;
    public int dmFields;
    public int dmPositionX;
    public int dmPositionY;
    public int dmDisplayOrientation;
    public int dmDisplayFixedOutput;
    public short dmColor;
    public short dmDuplex;
    public short dmYResolution;
    public short dmTTOption;
    public short dmCollate;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmFormName;
    public short dmLogPixels;
    public int dmBitsPerPel;
    public int dmPelsWidth;
    public int dmPelsHeight;
    public int dmDisplayFlags;
    public int dmDisplayFrequency;
}


public record ResolutionInfo(int Width, int Height);
public interface IDisplayService
{
    ResolutionInfo GetCurrentResolution();
    bool SetResolution(int Width, int Height);

}


public class DisplayService : IDisplayService
{
    private const int DM_PELSWIDTH = 0x80000;
    private const int DM_PELSHEIGHT = 0x100000;
    //PInvoke declarations for changing display settings
    [DllImport("user32.dll")]
    private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

    public ResolutionInfo GetCurrentResolution()
    {
        var screen = Screen.PrimaryScreen ?? throw new InvalidOperationException("Primary screen not found");
        return new ResolutionInfo(screen.Bounds.Width, screen.Bounds.Height);
    }
    public bool SetResolution(int Width, int Height)
    {
        DEVMODE dm = new();
        {
            dm.dmSize = (short)Marshal.SizeOf<DEVMODE>();
            dm.dmPelsWidth = Width;
            dm.dmPelsHeight = Height;
            dm.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT;
        }
        int result = ChangeDisplaySettings(ref dm, 0);
        Console.WriteLine($"ChangeDisplaySettings result: {result}");
        return result == 0; // DISP_CHANGE_SUCCESSFUL
    }



}