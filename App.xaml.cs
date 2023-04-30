using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.Services.Implementations;
using WPFBankDepartmentMVVM.View;
using WPFBankDepartmentMVVM.ViewModels;
using static System.Formats.Asn1.AsnWriter;

namespace WPFBankDepartmentMVVM
{
    public partial class App : Application
    {
        private static IServiceProvider? _Services;

        public static IServiceProvider Services => _Services ??= InitializeServices().BuildServiceProvider();

        private static IServiceCollection InitializeServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<AuthWindowViewModel>();
            services.AddTransient<AddNewClientViewModel>();
            services.AddScoped<ChangingLogViewModel>();
            services.AddScoped<ClientWindowViewModel>();
            services.AddScoped<TransferWindowViewModel>();

            services.AddSingleton<IUserDialog, UserDialogService>();
            services.AddSingleton<IMessageBus, MessageBusService>();

            #region Регистрация окон
            services.AddTransient(
                s =>
            {
                var model = s.GetRequiredService<MainWindowViewModel>();
                var window = new MainWindow { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();
                return window;
            });

            services.AddTransient(
                s =>
                {
                    var model = s.GetRequiredService<AuthWindowViewModel>();
                    var window = new AuthWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var model = s.GetRequiredService<AddNewClientViewModel>();
                    var window = new AddNewClient { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = scope.ServiceProvider.GetRequiredService<ChangingLogViewModel>();
                    var window = new ChangingLog { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    window.Closed += (_, _) => scope.Dispose();
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = scope.ServiceProvider.GetRequiredService<ClientWindowViewModel>();
                    var window = new ClientWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    window.Closed += (_, _) => scope.Dispose();
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = scope.ServiceProvider.GetRequiredService<TransferWindowViewModel>();
                    var window = new TransferWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    window.Closed += (_, _) => scope.Dispose();
                    return window;
                });

            #endregion

            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Services.GetRequiredService<IUserDialog>().OpenMainWindow();
        }
    }


}
