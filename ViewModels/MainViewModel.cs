using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestingPlatformWpfClient.Controls.Dialogs;
using TestingPlatformWpfClient.Exceptions;
using TestingPlatformWpfClient.Models;
using TestingPlatformWpfClient.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TestingPlatformWpfClient.ViewModels {
    public class MainViewModel : ObservableObject {
        public ICommand DeleteTestCommand { get; private set; }
        public ICommand OpenUpdateTestCommand { get; private set; }
        
        private readonly ITestService _testService;
        private ObservableCollection<Test> _tests = new ObservableCollection<Test>();

        public ObservableCollection<Test> Tests {
            get => _tests;
            set => SetProperty(ref _tests, value);
        }

        public MainViewModel(ITestService testService) {
            _testService = testService;
            InitializeCommands();
        }
        
        private void InitializeCommands() {
            OpenUpdateTestCommand = new AsyncRelayCommand<Test>(ShowUpdateTestDialogAsync);
            DeleteTestCommand = new AsyncRelayCommand<Test>(DeleteTestAsync);
        }
        
        private async Task DeleteTestAsync(Test test) {
            var confirmationDialog = new ConfirmationDialog();
            var result = await confirmationDialog.ShowAsync(
                $"Are you sure you want to delete  {test.Name}?");

            if (result == ConfirmationDialogResult.Confirmed) {
                Tests.Remove(test);

                try {
                    await _testService.DeleteTestAsync(test.Id);
                } 
                catch (ApiException ex) {
                    // Handle the ApiException (e.g., show an error message)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                } 
                catch (Exception ex) {
                    // Handle other unexpected exceptions
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private async Task ShowUpdateTestDialogAsync(Test test) {
            Debug.WriteLine("Delete Test Async");

            var updateTestDialog = new UpdateTestDialog();
            var result = await updateTestDialog.ShowAsync(test);
            
            
            if (result.IsConfirmed) {
                try {
                    Test updatedTest = result.UpdatedTest;
                    updatedTest.Questions = test.Questions;
                    
                    await _testService.UpdateTestAsync(updatedTest.Id, updatedTest);
                    int index = Tests.ToList().FindIndex(t => t.Id == updatedTest.Id);
                    
                    Tests.RemoveAt(index);
                    Tests.Insert(index, updatedTest);
                }
                catch (ApiException ex) {
                    // Handle the ApiException (e.g., show an error message)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                catch (Exception ex) {
                    // Handle other unexpected exceptions
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        
        public async Task InitializeAsync() {
            await LoadTestsAsync();
        }

        private async Task LoadTestsAsync() {
            try {
                var fetchedTests = await _testService.GetAllTestsAsync();
                Tests = new ObservableCollection<Test>(fetchedTests);
            }
            catch (ApiException ex) {
                // Handle the ApiException (e.g., show an error message)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex) {
                // Handle other unexpected exceptions
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}