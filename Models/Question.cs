namespace TestingPlatformWpfClient.Models {
    public class Question {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int Score { get; set; }
        public int TestId { get; set; }
    }
}

