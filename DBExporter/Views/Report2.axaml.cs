using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBExporter.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DBExporter.Views;

public partial class Report2 : UserControl
{
    public Report2()
    {
        InitializeComponent();
        DataContext = Program.ServiceProvider!.GetRequiredService<Report2ViewModel>();
    }
}