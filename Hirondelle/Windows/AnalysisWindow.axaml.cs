using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Hirondelle.Exporters;
using WindevReader;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hirondelle.Windows;

public partial class AnalysisWindow : Window
{
    private WddWindevAnalysis _analysis;
    
    public AnalysisWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            FieldsList.ItemsSource = new WindevDataField[]
            {
                new WindevDataField(1, "IDTEST", "Id Test", "ABCD", "", 4, 4, 2, 5, 10),
            };
        }
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

    private async void JsonExportButtonClicked(object? sender, RoutedEventArgs e)
    {
        IStorageFile? file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            DefaultExtension = "json",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Fichier JSON")
                {
                    Patterns = ["*.json"]
                }
            },
            Title = "Export de l'analyse en JSON"
        });
        
        if (file == null) return;

        JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };
        
        using StreamWriter wr = new(await file.OpenWriteAsync());
        await wr.WriteAsync(JsonSerializer.Serialize(_analysis, options));
    }

    private async void EFExportButtonClicked(object? sender, RoutedEventArgs e)
    {
        IReadOnlyList<IStorageFolder> folder = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Sélection du dossier final"
        });
        
        if (folder == null || folder.Count != 1) return;
        
        EFAnalysisExportManager.Export(_analysis, HttpUtility.UrlDecode(folder.First().Path.AbsolutePath));
    }
}