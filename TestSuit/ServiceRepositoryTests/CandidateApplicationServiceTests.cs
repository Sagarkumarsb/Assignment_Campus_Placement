using System.Collections.Generic;
using System.Threading.Tasks;
using Assignment_Campus_Placement.Models;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using Xunit;

public class CandidateApplicationServiceTests
{
    private readonly Mock<Container> _mockContainer;
    private readonly CandidateApplicationService _applicationService;

    public CandidateApplicationServiceTests()
    {
        var mockCosmosClient = new Mock<CosmosClient>();
        _mockContainer = new Mock<Container>();

        mockCosmosClient
            .Setup(client => client.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(_mockContainer.Object);

        _applicationService = new CandidateApplicationService(mockCosmosClient.Object, "testDb", "CandidateApplications");
    }

    [Fact]
    public async Task GetAllApplicationsAsync_ShouldReturnAllApplications()
    {
        var applications = new List<CandidateApplication>
        {
            new CandidateApplication
            {
                Id = "1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Answers = new List<Answer>
                {
                    new Answer { QuestionId = "1", Response = "Test response" }
                }
            },
            new CandidateApplication
            {
                Id = "2",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Answers = new List<Answer>
                {
                    new Answer { QuestionId = "2", Response = "Another test response" }
                }
            }
        };

        var feedResponseMock = new Mock<FeedResponse<CandidateApplication>>();
        feedResponseMock.Setup(fr => fr.GetEnumerator()).Returns(applications.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<CandidateApplication>>();
        feedIteratorMock.Setup(fi => fi.HasMoreResults).Returns(true);
        feedIteratorMock.Setup(fi => fi.ReadNextAsync(default)).ReturnsAsync(feedResponseMock.Object);

        _mockContainer.Setup(container => container.GetItemQueryIterator<CandidateApplication>(
            It.IsAny<string>(), null, null))
            .Returns(feedIteratorMock.Object);

        var result = await _applicationService.GetAllApplicationsAsync();

        result.Should().BeEquivalentTo(applications);
    }
}
