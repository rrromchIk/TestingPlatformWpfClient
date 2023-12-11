using System.Windows.Controls;

namespace TestingPlatformWpfClient.Services {
    public interface INavigationService {
        Frame? Frame { get; set; }
        void NavigateToTestCreation();
        void NavigateToMainView();
    }
}