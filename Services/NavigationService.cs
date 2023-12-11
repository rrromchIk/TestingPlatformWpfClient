using System.Windows.Controls;
using TestingPlatformWpfClient.Views;

namespace TestingPlatformWpfClient.Services {
    public class NavigationService: INavigationService {
        public Frame? Frame { get; set; }
        
        public void NavigateToTestCreation() {
            var testCreationView = new TestCreationView();
            Frame?.Navigate(testCreationView);
        }

        public void NavigateToMainView() {
            var mainView = new MainView();
            Frame?.Navigate(mainView);
        }
    }
}
