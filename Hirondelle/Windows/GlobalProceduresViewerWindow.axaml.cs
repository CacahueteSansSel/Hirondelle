using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using WindevReader;

namespace Hirondelle.Windows;

public partial class GlobalProceduresViewerWindow : Window
{
    private WdgWindevGlobalProcedures _file;
    
    public GlobalProceduresViewerWindow()
    {
        InitializeComponent();
        if (Design.IsDesignMode) FillValues();
    }

    public GlobalProceduresViewerWindow(WdgWindevGlobalProcedures file) : this()
    {
        _file = file;

        FillValues();
    }

    private void FillValues()
    {
        if (Design.IsDesignMode)
        {
            CollectionName.Text = "Procédures Globales de Truc";
            ProcList.ItemsSource = new WindevProcedure[]
            {
                new WindevProcedure("Procedure 1", []),
                new WindevProcedure("Procedure 2", []),
                new WindevProcedure("Procedure 3", [])
            };
            
            return;
        }

        CollectionName.Text = _file.Name;
        ProcList.ItemsSource = _file.Procedures;
    }

    private void ProcList_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (ProcList.SelectedItem is not WindevProcedure proc)
            return;

        new CodeViewerWindow(proc.CodeBlocks).ShowDialog(this);
    }
}