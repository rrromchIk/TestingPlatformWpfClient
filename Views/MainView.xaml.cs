using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TestingPlatformWpfClient.ViewModels;

namespace TestingPlatformWpfClient.Views {
    public partial class MainView : Page {
        private MainViewModel MainViewModel { get; }
        public MainView() {
            InitializeComponent();
            MainViewModel = App.Services.GetRequiredService<MainViewModel>();
            DataContext = MainViewModel;
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            await MainViewModel.InitializeAsync();
        }
    }
}