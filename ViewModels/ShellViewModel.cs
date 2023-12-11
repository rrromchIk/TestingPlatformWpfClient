using TestingPlatformWpfClient.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace TestingPlatformWpfClient.ViewModels {
    public class ShellViewModel : ObservableObject {
        public ICommand OpenTestViewCommand { get; private set; } = null!;
        public ICommand OpenTestCreationViewCommand { get; private set; } = null!;
        private readonly INavigationService _navigationService;

        public ShellViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            InitializeCommands();
        }
        
        private void InitializeCommands() {
            OpenTestViewCommand = new RelayCommand(_navigationService.NavigateToMainView);
            OpenTestCreationViewCommand = new RelayCommand(_navigationService.NavigateToTestCreation);
        }
    }
}