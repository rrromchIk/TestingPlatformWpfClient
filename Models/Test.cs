using System.Collections.Generic;

namespace TestingPlatformWpfClient.Models {
    public class Test {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public int Duration { get; set; }
        public string Difficulty { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = null!;
    }
}