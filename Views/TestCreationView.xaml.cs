using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TestingPlatformWpfClient.ViewModels;

namespace TestingPlatformWpfClient.Views {
    public partial class TestCreationView : Page {
        private TestCreationViewModel TestCreationViewModel { get; }

        public TestCreationView() {
            InitializeComponent();
            
            TestCreationViewModel = App.Services.GetRequiredService<TestCreationViewModel>();
            DataContext = TestCreationViewModel;
        }
    }
}
