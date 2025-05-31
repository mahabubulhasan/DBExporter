using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBExporter.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DBExporter.Views;

public partial class Report4 : UserControl
{
    public Report4()
    {
        InitializeComponent();
        DataContext = Program.ServiceProvider!.GetRequiredService<Report4ViewModel>();
    }
}