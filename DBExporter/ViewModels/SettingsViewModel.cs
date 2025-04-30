using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBExporter.Models;

namespace DBExporter.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IDatabaseService _databaseService;

        [ObservableProperty]
        private string _host = string.Empty;

        [ObservableProperty]
        private string _port = "1433";

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _report1Sp = string.Empty;

        [ObservableProperty]
        private string _hostError = string.Empty;

        [ObservableProperty]
        private string _portError = string.Empty;

        [ObservableProperty]
        private string _usernameError = string.Empty;

        [ObservableProperty]
        private string _passwordError = string.Empty;
        
        [ObservableProperty]
        private string _report1SpError = string.Empty;

        [ObservableProperty]
        private bool _hasHostError;

        [ObservableProperty]
        private bool _hasPortError;

        [ObservableProperty]
        private bool _hasUsernameError;

        [ObservableProperty]
        private bool _hasPasswordError;
        
        [ObservableProperty]
        private bool _hasReport1SpError;

        [ObservableProperty]
        private string _testMessage = string.Empty;

        public SettingsViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadSettingsAsync().ConfigureAwait(false);
        }

        private async Task LoadSettingsAsync()
        {
            var config = await DatabaseConfig.LoadAsync();
            Host = config.Host;
            Port = config.Port;
            Username = config.Username;
            Password = config.Password;
            Report1Sp = config.Report1Sp;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (ValidateForm())
            {
                var config = new DatabaseConfig
                {
                    Host = Host,
                    Port = Port,
                    Username = Username,
                    Password = Password,
                    Report1Sp = Report1Sp
                };

                await config.SaveAsync();
            }
        }

        [RelayCommand]
        private async Task TestConnectionAsync()
        {
            var connected = await _databaseService.TestConnectionAsync();
            if(connected)
            {
                TestMessage = "Connection successful!";
            }
            else
            {
                TestMessage = "Connection failed! Please check your settings.";
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Validate Host
            if (string.IsNullOrWhiteSpace(Host))
            {
                HostError = "Host is required";
                HasHostError = true;
                isValid = false;
            }
            else
            {
                HostError = string.Empty;
                HasHostError = false;
            }

            // Validate Port
            if (string.IsNullOrWhiteSpace(Port))
            {
                PortError = "Port is required";
                HasPortError = true;
                isValid = false;
            }
            else if (!int.TryParse(Port, out var portNumber) || portNumber <= 0 || portNumber > 65535)
            {
                PortError = "Port must be a valid number between 1-65535";
                HasPortError = true;
                isValid = false;
            }
            else
            {
                PortError = string.Empty;
                HasPortError = false;
            }

            // Validate Username
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Username is required";
                HasUsernameError = true;
                isValid = false;
            }
            else
            {
                UsernameError = string.Empty;
                HasUsernameError = false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
                HasPasswordError = true;
                isValid = false;
            }
            else
            {
                PasswordError = string.Empty;
                HasPasswordError = false;
            }
            
            // Validate Report1Sp
            if (string.IsNullOrWhiteSpace(Report1Sp))
            {
                Report1SpError = "Report 1 Stored Procedure Name is required";
                HasReport1SpError = true;
                isValid = false;
            }
            else
            {
                Report1SpError = string.Empty;
                HasReport1SpError = false;
            }

            return isValid;
        }
    }
}
