using System.IO;
using System.Linq;
using Hirondelle.Windows;
using WindevReader;

namespace Hirondelle;

public static class HirondelleManager
{
    private static WindevOpenOptions _openOptions;
    public static WdpWindevProject? CurrentProject { get; private set; }
    public static string Path { get; private set; }

    public static bool LoadProject(string path)
    {
        _openOptions = new WindevOpenOptions() { AllowUntestedVersions = true };
        
        Path = path;
        
        string wdpPath = FindFileInPath(path, "wdp");
        CurrentProject = WdpWindevProject.Open(wdpPath, _openOptions);
        
        new WindevProjectSummaryWindow().Show();

        return true;
    }

    private static string FindFileInPath(string path, string ext)
        => Directory.GetFiles(path, $"*.{ext}", SearchOption.AllDirectories).First();

    public static void OpenAnalysis()
    {
        string filename = FindFileInPath(Path, "wdd");
        new AnalysisWindow(WddWindevAnalysis.Open(filename, _openOptions)).ShowDialog(WindevProjectSummaryWindow.Instance);
    }

    public static void OpenFile(string filename)
    {
        switch (System.IO.Path.GetExtension(filename).TrimStart('.'))
        {
            case "wdg":
                new GlobalProceduresViewerWindow(WdgWindevGlobalProcedures.Open(filename, _openOptions))
                    .ShowDialog(WindevProjectSummaryWindow.Instance);
                
                break;
            case "wdw":
                new WindowViewerWindow(WdwWindevWindow.Open(filename, _openOptions))
                    .ShowDialog(WindevProjectSummaryWindow.Instance);
                
                break;
        }
    }
}