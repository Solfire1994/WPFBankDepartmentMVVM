using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
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
            services.AddTransient<AuthWindowViewModel>();
            services.AddTransient<AddNewClientViewModel>();
            services.AddTransient<ChangingLogViewModel>();
            services.AddTransient<ClientWindowViewModel>();
            services.AddTransient<TransferWindowViewModel>();

            return services;
        }
    }


}
