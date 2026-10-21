using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Hirondelle.SummaryTabs;

public partial class ConfigListTab : UserControl
{
    public ConfigListTab()
    {
        InitializeComponent();

        ConfigList.ItemsSource = HirondelleManager.CurrentProject!.ConfigurationNames;
    }
}