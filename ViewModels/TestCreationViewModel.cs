using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using TestingPlatformWpfClient.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TestingPlatformWpfClient.Exceptions;
using TestingPlatformWpfClient.Models;

namespace TestingPlatformWpfClient.ViewModels {
    public class TestCreationViewModel : ObservableObject {
        private readonly ITestService _testService;
        private readonly INavigationService _navigationService;

        public string Name { get; set; }
        public string Subject { get; set; }
        public string Duration { get; set; }
        public string Difficulty { get; set; }
        public ICommand AddTestCommand { get; }

        public TestCreationViewModel(ITestService playerService,
            INavigationService navigationService) {
            _testService = playerService;
            _navigationService = navigationService;
            AddTestCommand = new AsyncRelayCommand(AddTestAsync);
        }

        private async Task AddTestAsync() {
            if (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Subject) ||
                string.IsNullOrEmpty(Duration) ||
                string.IsNullOrEmpty(Difficulty)) {
                
                MessageBox.Show("Please fill in all fields.", "Incomplete Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return; // Don't proceed further if at least one field is empty
            }

            if (!int.TryParse(Duration, out int number) || number <= 0) {
                MessageBox.Show("Duration has to be number grater than 0",
                    "Incomplete Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 

            }
    
            if (!(Difficulty.Equals("easy", StringComparison.OrdinalIgnoreCase) ||
                Difficulty.Equals("medium", StringComparison.OrdinalIgnoreCase) || 
                Difficulty.Equals("hard", StringComparison.OrdinalIgnoreCase))) {
                
                MessageBox.Show("Difficult must be \"easy\", \"medium\" or \"hard\"! (case insensetive)",
                    "Incomplete Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try {
                var test = new Test {
                    Id = 0,
                    Name = Name,
                    Subject = Subject,
                    Duration = int.Parse(Duration),
                    Difficulty = Difficulty,
                };

                await _testService.CreateTestAsync(test);
            }
            catch (ApiException ex) {
                // Handle the ApiException (e.g., show an error message)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex) {
                // Handle other unexpected exceptions
                // MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                //     MessageBoxImage.Error);
                
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally {
                Name = string.Empty;
                Subject = string.Empty;
                Duration = string.Empty;
                Difficulty = string.Empty;
                
                _navigationService.NavigateToMainView();
            }
        }
    }
}