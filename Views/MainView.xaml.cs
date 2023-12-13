using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TestingPlatformWpfClient.ViewModels;

namespace TestingPlatformWpfClient.Views {
    public partial class MainView : Page {
        public MainViewModel ViewModel { get; }
        public MainView() {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<MainViewModel>();
            DataContext = ViewModel;
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            await ViewModel.InitializeAsync();
        }
    }
}