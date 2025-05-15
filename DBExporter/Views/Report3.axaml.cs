using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBExporter.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DBExporter.Views;

public partial class Report3 : UserControl
{
    public Report3()
    {
        InitializeComponent();
        DataContext = Program.ServiceProvider!.GetRequiredService<Report3ViewModel>();
    }
}