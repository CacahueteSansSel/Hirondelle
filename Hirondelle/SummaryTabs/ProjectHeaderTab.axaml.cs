using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Hirondelle.Windows;

namespace Hirondelle.SummaryTabs;

public partial class ProjectHeaderTab : UserControl
{
    public ProjectHeaderTab()
    {
        InitializeComponent();

        if (!Design.IsDesignMode) FillValues();
    }

    private void FillValues()
    {
        ProjectNameValue.Text = HirondelleManager.CurrentProject.Name;
        WindevNumVersionValue.Text = HirondelleManager.CurrentProject.Header.WindevVersion.ToString();
        WindevVersionValue.Text = HirondelleManager.CurrentProject.Header.ProductBuild;
        WindevRevisionValue.Text = HirondelleManager.CurrentProject.Header.ProductRevision.ToString();
    }

    private void OpenCodeButtonClicked(object? sender, RoutedEventArgs e)
    {
        new CodeViewerWindow(HirondelleManager.CurrentProject.CodeBlocks).ShowDialog(WindevProjectSummaryWindow.Instance);
    }
}