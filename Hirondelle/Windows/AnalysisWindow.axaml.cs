using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WindevReader;

namespace Hirondelle.Windows;

public partial class AnalysisWindow : Window
{
    private WddWindevAnalysis _analysis;
    
    public AnalysisWindow()
    {
        InitializeComponent();
    }

    public AnalysisWindow(WddWindevAnalysis analysis) : this()
    {
        _analysis = analysis;

        FillValues();
    }

    private void FillValues()
    {
        AnalysisVersionText.Text = $"Analyse v{_analysis.Generation}";
        AnalysisGuidText.Text = _analysis.Guid;

        TableList.ItemsSource = _analysis.Tables;
    }

    private void TableList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TableList.SelectedItem is not WindevDataTable table) return;

        FillTableValues(table);
    }

    private void FillTableValues(WindevDataTable table)
    {
        TableNameText.Text = table.Name;
        TableCaptionText.Text = table.Caption;
        TablePhysicalNameText.Text = table.PhysicalName;

        FieldsList.ItemsSource = table.Fields;
    }
}