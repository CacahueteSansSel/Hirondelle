using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Web;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using WindevReader;

namespace Hirondelle;

public partial class MainWindow : Window
{
    private List<KnownProjectEntry> _knownProjectEntries = [];
    
    public MainWindow()
    {
        InitializeComponent();

        if (File.Exists("known_projects.json"))
        {
            _knownProjectEntries = JsonSerializer.Deserialize<KnownProjectEntry[]>(
                File.ReadAllText("known_projects.json"))!.ToList();
        }

        KnownProjectsList.ItemsSource = _knownProjectEntries;

        VersionText.Text =
            $"Hirondelle {Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)} — windev-re {typeof(WindevFile).Assembly.GetName().Version!.ToString(3)}";
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        IReadOnlyList<IStorageFolder> list = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Select a WINDEV project folder"
        });
        
        if (list.Count != 1) return;

        string path = HttpUtility.UrlDecode(list.First().Path.AbsolutePath);

        LoadProject(path);
    }

    private void LoadProject(string path)
    {
        Hide();
        if (!HirondelleManager.LoadProject(path) || HirondelleManager.CurrentProject == null)
        {
            Environment.Exit(1);
            return;
        }
        
        if (_knownProjectEntries.All(entry => entry.Path != path))
            _knownProjectEntries.Add(new KnownProjectEntry(HirondelleManager.CurrentProject!.Name, path));
        
        SaveKnownEntries();
    }

    private void SaveKnownEntries()
    {
        File.WriteAllText("known_projects.json", JsonSerializer.Serialize(_knownProjectEntries.ToArray()));
    }

    private void KnownProjectsList_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (KnownProjectsList.SelectedItem is not KnownProjectEntry project) return;
        
        LoadProject(project.Path);
    }
}

public record KnownProjectEntry(string Name, string Path);