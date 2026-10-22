using System;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using WindevReader;

namespace Hirondelle.Windows;

public partial class WindowViewerWindow : Window
{
    private WdwWindevWindow _window;
    
    public WindowViewerWindow()
    {
        InitializeComponent();
    }

    public WindowViewerWindow(WdwWindevWindow window) : this()
    {
        _window = window;

        FillValues();
    }

    private void FillValues()
    {
        // Preview image
        try
        {
            Bitmap bmp = new Bitmap(new MemoryStream(_window.Preview.BmpData.ToArray()));
            WindowPreviewImage.Source = bmp;
        }
        catch (Exception e)
        {
            
        }

        WindowNameText.Text = _window.Name;

        WindowTitleValue.Text = _window.Caption;
        if (_window.Bounds != null)
        {
            WindowXValue.Text = _window.Bounds.X.ToString();
            WindowYValue.Text = _window.Bounds.Y.ToString();
            WindowWidthValue.Text = _window.Bounds.Width.ToString();
            WindowHeightValue.Text = _window.Bounds.Height.ToString();
        }

        ElementsList.ItemsSource = _window.Elements;

        BuildPreview();
    }

    private void BuildPreview()
    {
        if (_window.Bounds == null) return;
        
        double winWidth = 0;
        
        double CalculateSizeX(float size)
        {
            return size / _window.Bounds.Width * winWidth;
        }
        double CalculateSizeY(float size)
        {
            return size / _window.Bounds.Height * PreviewPanel.Height;
        }

        PreviewWindow.Width = CalculateSizeY(_window.Bounds.Width);
        PreviewWindow.Height = CalculateSizeY(_window.Bounds.Height);
        winWidth = PreviewWindow.Width;

        PreviewWindow.Background = new ImageBrush((Bitmap)WindowPreviewImage.Source);

        foreach (WindevUiElement elmt in _window.Elements)
        {
            if (elmt.Bounds == null) continue;
            
            Panel panel = new();
            panel.Children.Add(new Border() {BorderThickness = new Thickness(2), BorderBrush = Brushes.Yellow});
            
            panel.Width = CalculateSizeY(elmt.Bounds.Width);
            panel.Height = CalculateSizeY(elmt.Bounds.Height);
            
            Canvas.SetLeft(panel, CalculateSizeX(elmt.Bounds.X));
            Canvas.SetTop(panel, CalculateSizeY(elmt.Bounds.Y));
            
            PreviewWindowCanvas.Children.Add(panel);
        }
    }

    private void ViewSourceCodeButtonClicked(object? sender, RoutedEventArgs e)
    {
        new CodeViewerWindow(_window.CodeBlocks).ShowDialog(this);
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
            Title = "Export de la fenêtre JSON"
        });
        
        if (file == null) return;

        JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };
        
        using StreamWriter wr = new(await file.OpenWriteAsync());
        await wr.WriteAsync(JsonSerializer.Serialize(_window, options));
    }
}