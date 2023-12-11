using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPlatformWpfClient.Exceptions {
    public class ApiErrorDetails {
        public string Title { get; set; } = null!;
        public Dictionary<string, string[]> Errors { get; set; } = null!;
    }
}
