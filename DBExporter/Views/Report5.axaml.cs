using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBExporter.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DBExporter.Views;

public partial class Report5 : UserControl
{
    public Report5()
    {
        InitializeComponent();
        DataContext = Program.ServiceProvider!.GetRequiredService<Report5ViewModel>();
    }
}