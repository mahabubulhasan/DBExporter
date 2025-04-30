using Avalonia.Controls;
using DBExporter.ViewModels;

namespace DBExporter.Views
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();

            if (Program.ServiceProvider != null)
            {
                DataContext = Program.ServiceProvider.GetService(typeof(SettingsViewModel));
            }
        }
    }
}