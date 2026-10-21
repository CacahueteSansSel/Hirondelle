using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Hirondelle.Windows;
using WindevReader;

namespace Hirondelle.SummaryTabs;

public partial class ElementListTab : UserControl
{
    public ElementListTab()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            ElementList.ItemsSource = new[]
            {
                new WindevProjectReference("Element1", 0, "Tata"),
                new WindevProjectReference("Element2", 2, "Toto"),
                new WindevProjectReference("Element3", 4, "Titi"),
            };
        }
        else
        {
            ElementList.ItemsSource = HirondelleManager.CurrentProject.References
                .Select(t => new WindevProjectReference(Path.GetFileName(t.Name), t.Offset, t.ConfigurationName));
        }
    }

    private void ElementList_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (ElementList.SelectedItem is not WindevProjectReference reference)
            return;

        string? filename = Directory.GetFiles(HirondelleManager.Path, $"{reference.Name}.*").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(filename))
        {
            new InfoWindow($"Impossible de trouver le fichier de l'élément {reference.Name}").ShowDialog(
                WindevProjectSummaryWindow.Instance);
            
            return;
        }
        
        HirondelleManager.OpenFile(filename);
    }
}