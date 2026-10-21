using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Hirondelle.SummaryTabs;
using WindevReader;

namespace Hirondelle;

public partial class WindevProjectSummaryWindow : Window
{
    public static WindevProjectSummaryWindow Instance { get; private set; }
    
    public WindevProjectSummaryWindow()
    {
        Instance = this;
        
        InitializeComponent();

        SetupProjectInfos(HirondelleManager.CurrentProject!);
        SetContent(new ProjectHeaderTab());
    }

    private void SetupProjectInfos(WdpWindevProject currentProject)
    {
        ProjectNameText.Text = currentProject.Name;
        ProjectWDVersionText.Text = $"WINDEV {currentProject.Header.WindevVersion} / {currentProject.Header.ProductBuild}";
    }

    void SetContent(Control control)
    {
        CurrentTabContent.Content = control;
    }

    private void HeaderTabButtonClicked(object? sender, RoutedEventArgs e)
    {
        SetContent(new ProjectHeaderTab());
    }

    private void ConfigListButtonClicked(object? sender, RoutedEventArgs e)
    {
        SetContent(new ConfigListTab());
    }

    private void ElementListButtonClicked(object? sender, RoutedEventArgs e)
    {
        SetContent(new ElementListTab());
    }

    private void AnalysisButtonClicked(object? sender, RoutedEventArgs e)
    {
        HirondelleManager.OpenAnalysis();
    }
}