using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBExporter.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DBExporter.Views;

public partial class Report1 : UserControl
{
    public Report1()
    {
        InitializeComponent();
        DataContext = Program.ServiceProvider!.GetRequiredService<Report1ViewModel>();
    }
}