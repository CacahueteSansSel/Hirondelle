using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using WindevReader;

namespace Hirondelle.Windows;

public partial class CodeViewerWindow : Window
{
    private IEnumerable<WindevCodeBlock> _blocks;
    
    public CodeViewerWindow()
    {
        InitializeComponent();
    }

    public CodeViewerWindow(IEnumerable<WindevCodeBlock> blocks) : this()
    {
        _blocks = blocks;
        
        BuildSourceCode();
    }

    private void BuildSourceCode()
    {
        string code = "";
        
        foreach (WindevCodeBlock block in _blocks)
        {
            if (OptionBlockComments.IsChecked!.Value)
            {
                code += $"// Bloc de code WINDEV 0x{block.EventId:x4} ({block.EventId})" + Environment.NewLine;
            }
            
            foreach (WindevCodeLine line in block.Lines)
                code += line.Text.Replace("\t", "    ") + Environment.NewLine;

            code += Environment.NewLine;
        }

        TextEditorView.Text = code;
    }

    private void OptionBlockComments_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        BuildSourceCode();
    }

    private async void ExportButtonClicked(object? sender, RoutedEventArgs e)
    {
        IStorageFile? file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            DefaultExtension = "wlang",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Fichier source WLanguage en clair")
                {
                    Patterns = ["*.wlang"]
                }
            },
            Title = "Export du code WLanguage en clair vers..."
        });
        
        if (file == null) return;

        using StreamWriter wr = new(await file.OpenWriteAsync());
        await wr.WriteAsync(TextEditorView.Text);
    }
}