using System.Windows;
using System.Windows.Controls;
using TestingPlatformWpfClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using TestingPlatformWpfClient.Services;

namespace TestingPlatformWpfClient {
    public partial class MainWindow : Window {
        public static MainWindow Current { get; private set; } = null!;
        public ShellViewModel ShellViewModel { get; private set; } = null!;
        public ContentControl ContentControl => DialogsContainer;
        
        private readonly INavigationService _navigationService;

        public MainWindow() {
            InitializeComponent();

            Current = this;
            _navigationService = App.Services.GetRequiredService<INavigationService>();
            DataContext = App.Services.GetRequiredService<ShellViewModel>();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _navigationService.Frame = RootFrame;
            _navigationService.NavigateToMainView();
        }
    }
}