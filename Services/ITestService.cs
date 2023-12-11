using System.Collections.Generic;
using System.Threading.Tasks;
using TestingPlatformWpfClient.Models;

namespace TestingPlatformWpfClient.Services {
    public interface ITestService {
        Task<IEnumerable<Test>> GetAllTestsAsync();
        Task<Test> GetTestByIdAsync(int id);
        Task<Test> CreateTestAsync(Test test);
        Task UpdateTestAsync(int id, Test test);
        Task DeleteTestAsync(int id);
    }
}