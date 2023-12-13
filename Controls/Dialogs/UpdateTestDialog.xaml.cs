using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestingPlatformWpfClient.Models;

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
    
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(SubjectTextBox.Text) ||
                string.IsNullOrEmpty(DurationTextBox.Text) ||
                string.IsNullOrEmpty(DifficultyTextBox.Text)) {
                
                MessageBox.Show("Please fill in all fields.", "Incomplete Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return; // Don't proceed further if at least one field is empty
            }

            if (!int.TryParse(DurationTextBox.Text, out int number) || number <= 0) {
                MessageBox.Show("Duration has to be number grater than 0",
                    "Incomplete Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Don't proceed further if at least one field is empty

            }
            
            if (!(DifficultyTextBox.Text.Equals("easy", StringComparison.OrdinalIgnoreCase) ||
                  DifficultyTextBox.Text.Equals("medium", StringComparison.OrdinalIgnoreCase) || 
                  DifficultyTextBox.Text.Equals("hard", StringComparison.OrdinalIgnoreCase))) {
                
                MessageBox.Show("Difficult must be \"easy\", \"medium\" or \"hard\"! (case insensetive)",
                    "Incomplete Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try {
                var updatedTest = new Test {
                    Id = TestToUpdate.Id,
                    Name = NameTextBox.Text,
                    Subject = SubjectTextBox.Text,
                    Duration = int.Parse(DurationTextBox.Text),
                    Difficulty = DifficultyTextBox.Text,
                };
            
                // If all fields are filled, close the dialog and set the result
                CloseDialog();
                dismissTaskSource.TrySetResult(UpdateConfirmationDialogResult.Confirmed(updatedTest));
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
    }
}