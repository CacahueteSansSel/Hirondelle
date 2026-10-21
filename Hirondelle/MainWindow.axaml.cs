using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace Hirondelle;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        IReadOnlyList<IStorageFolder> list = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Select a WINDEV project folder"
        });
        
        if (list.Count != 1) return;

        string path = list.First().Path.AbsolutePath;

        Hide();
        HirondelleManager.LoadProject(path);
    }
}