using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TestingPlatformWpfClient.Models;
using TestingPlatformWpfClient.Services;

namespace TestingPlatformWpfClient.Controls.Dialogs {
    public partial class UpdateTestDialog : UserControl {
        public UpdateTestDialog() {
            InitializeComponent();
        }


        private TaskCompletionSource<UpdateConfirmationDialogResult> dismissTaskSource = new();
        public Test TestToUpdate { get; set; }

        public async Task<UpdateConfirmationDialogResult> ShowAsync(Test test) {
            DataContext = test;
            TestToUpdate = test;
            var dialogsContainer = MainWindow.Current.DialogsContainer;
            dialogsContainer.Content = this;
            Visibility = Visibility.Visible;
            dismissTaskSource = new TaskCompletionSource<UpdateConfirmationDialogResult>();
            var result = await dismissTaskSource.Task;
            return result;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            CloseDialog();
            dismissTaskSource.TrySetResult(UpdateConfirmationDialogResult.Cancelled());
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            // Check if at least one TextBox or DatePicker is empty
            if (string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                string.IsNullOrEmpty(LastNameTextBox.Text) ||
                BirthDatePicker.SelectedDate == null ||
                string.IsNullOrEmpty(CountryTextBox.Text) ||
                string.IsNullOrEmpty(PositionTextBox.Text)) {
                
                MessageBox.Show("Please fill in all fields.", "Incomplete Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return; // Don't proceed further if at least one field is empty
            }

            if (PositionTextBox.Text != "Guard" && PositionTextBox.Text != "Forward" &&
                PositionTextBox.Text != "Center") {
                MessageBox.Show("Position must be \"Guard\", \"Forward\" or \"Center\"!", "Incomplete Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try {
                var testService = App.Services.GetRequiredService<ITestService>();
                bool isTeamExist = await teamService.IsTeamExistAsync(int.Parse(TeamIdTextBox.Text));

                if (!isTeamExist) {
                    MessageBox.Show("Team with these team id does not exist!", "Team Not Found", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }


                var updatedPlayer = new Player {
                    Id = PlayerToUpdate.Id,
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    DateOfBirth = BirthDatePicker.SelectedDate ?? DateTime.MinValue,
                    Country = CountryTextBox.Text,
                    Height = HeightNumericTextBox.Value,
                    Weight = WeightNumericTextBox.Value,
                    Position = PositionTextBox.Text,
                    TeamId = int.Parse(TeamIdTextBox.Text)
                };

                // If all fields are filled, close the dialog and set the result
                CloseDialog();
                dismissTaskSource.TrySetResult(UpdateConfirmationDialogResult.Confirmed(updatedPlayer));
            }
            catch (Exception exception) {
                MessageBox.Show($"An unexpected error occurred: {exception.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        
        private void CloseDialog() {
            Visibility = Visibility.Collapsed;
            if (MainWindow.Current.DialogsContainer.Content == this) {
                MainWindow.Current.DialogsContainer.Content = null;
            }
        }
        
        private static bool IsPositiveDigit(string text) {
            return int.TryParse(text, out int result) && result > 0;
        }
    }
}