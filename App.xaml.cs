using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.Services.Implementations;
using WPFBankDepartmentMVVM.View;
using WPFBankDepartmentMVVM.ViewModels;

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
            services.AddTransient<ChangingLogViewModel>();
            services.AddTransient<ClientWindowViewModel>();
            services.AddTransient<TransferWindowViewModel>();

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
                    var model = s.GetRequiredService<ChangingLogViewModel>();
                    var window = new ChangingLog { DataContext = model };
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var model = s.GetRequiredService<ClientWindowViewModel>();
                    var window = new ClientWindow { DataContext = model };
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var model = s.GetRequiredService<TransferWindowViewModel>();
                    var window = new TransferWindow { DataContext = model };
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
