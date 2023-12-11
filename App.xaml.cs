using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using TestingPlatformWpfClient.ViewModels;
using TestingPlatformWpfClient.Services;

namespace TestingPlatformWpfClient {
    public partial class App : Application {
        public static IServiceProvider Services { get; private set; } = null!;

        public App() {
            InitializeServices();
        }
        private static void InitializeServices() {
            var services = new ServiceCollection();

            services.AddSingleton<CommonHttpClientService>();
            services.AddSingleton<ITestService, TestService>();
            services.AddSingleton<INavigationService, NavigationService>();
            
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TestCreationViewModel>();

            Services = services.BuildServiceProvider();
        }
    }
}
