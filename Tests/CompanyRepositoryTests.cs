using Contracts;
using Xunit;
using Moq;
using Entities.Models;

namespace Tests;
public class CompanyRepositoryTests
{
    [Fact]
    public void GetAllCompaniesAsync_ReturnsListOfCompanies_WithASingleCompany()
    {
        // Arrange
        var mockRepo = new Mock<ICompanyRepository>();
        mockRepo.Setup(repo => repo.GetAllCompaniesAsync(false)).Returns(Task.FromResult(GetCompanies()));
        // Act
        var result = mockRepo.Object.GetAllCompaniesAsync(false)
        .GetAwaiter()
        .GetResult()
        .ToList();
        // Assert
        Assert.IsType<List<Company>>(result);
        Assert.Single(result);
    }
    public IEnumerable<Company> GetCompanies()
    {
        return new List<Company>()
        {
            new Company()
            {
                Id = Guid.NewGuid(),
                Name = "Test Company",
                Country = "United States",
                Address = "908 Woodrow Way"
            }
        };
    }
}