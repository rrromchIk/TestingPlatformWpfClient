using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;
using TestingPlatformWpfClient.Services;
using System.Windows.Input;
using System.Windows;
using TestingPlatformWpfClient.Exceptions;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace TestingPlatformWpfClient.ViewModels {
    public partial class TestCreationViewModel : ObservableObject {
        private readonly ITestService _playerService;

        private string? firstName;

        public string? FirstName {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string? lastName;

        public string? LastName {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private DateTime? birthDate;

        public DateTime? BirthDate {
            get => birthDate;
            set => SetProperty(ref birthDate, value);
        }

        private string? country;

        public string? Country {
            get => country;
            set => SetProperty(ref country, value);
        }

        private double height;

        public double Height {
            get => height;
            set => SetProperty(ref height, value);
        }

        private double weight;

        public double Weight {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        private string? position;

        public string? Position {
            get => position;
            set => SetProperty(ref position, value);
        }

        private int? teamId;

        public int? TeamId {
            get => teamId;
            set => SetProperty(ref teamId, value);
        }

        public ICommand AddTestCommand { get; }

        public TestCreationViewModel(ITestService playerService) {
            _playerService = playerService;
            AddTestCommand = new AsyncRelayCommand(AddTestAsync);
        }

        private async Task AddTestAsync() {
            Debug.WriteLine(
                $"FirstName: {FirstName}, LastName: {LastName}, BirthDate: {BirthDate}, Country: {Country}, height:{Height}, weight:{Weight}, position:{Position}, team Id:{TeamId}");
            
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                BirthDate is null ||
                Height == 0.0 ||
                Weight == 0.0 ||
                string.IsNullOrWhiteSpace(Position) ||
                TeamId is null) {
                MessageBox.Show("Please fill in all fields.", "Incomplete Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (Position != "Guard" && Position != "Forward" &&
                Position != "Center") {
                MessageBox.Show("Position must be \"Guard\", \"Forward\" or \"Center\"!", "Incomplete Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try {
                bool isTeamExist = await _teamService.IsTeamExistAsync(TeamId ?? 0);

                if (!isTeamExist) {
                    MessageBox.Show("Team with these team id does not exist!", "Team Not Found", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var player = new Player() {
                    Id = 0,
                    FirstName = (string)FirstName,
                    LastName = (string)LastName,
                    DateOfBirth = (DateTime)BirthDate,
                    Height = Height,
                    Weight = Weight,
                    Country = (string)Country,
                    Position = (string)Position,
                    TeamId = TeamId,
                };
                var newPlayer = await _playerService.CreatePlayerAsync(player);


                FirstName = string.Empty;
                LastName = string.Empty;
                BirthDate = DateTime.Now;
                Country = string.Empty;
                Height = 0.0;
                Weight = 0.0;
                Position = string.Empty;
                TeamId = 1;
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