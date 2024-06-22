using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using Xunit;
public class QuestionServiceTests
{
    private readonly Mock<Container> _mockContainer;
    private readonly QuestionService _questionService;

    public QuestionServiceTests()
    {
        var mockCosmosClient = new Mock<CosmosClient>();
        _mockContainer = new Mock<Container>();

        mockCosmosClient
            .Setup(client => client.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(_mockContainer.Object);

        _questionService = new QuestionService(mockCosmosClient.Object, "testDb", "Questions");
    }

    [Fact]
    public async Task AddQuestionAsync_ShouldAddQuestion()
    {
        var questionDto = new QuestionDto
        {
            Type = "Paragraph",
            Text = "Describe yourself",
            Options = new List<string>()
        };

        await _questionService.AddQuestionAsync(questionDto);

        _mockContainer.Verify(container => container.CreateItemAsync(It.IsAny<Question>(), null, null, default), Times.Once);
    }

    [Fact]
    public async Task GetQuestionByIdAsync_ShouldReturnQuestion()
    {
        var question = new Question
        {
            Id = "1",
            Type = "Paragraph",
            Text = "Describe yourself",
            Options = new List<string>()
        };

        var responseMock = new Mock<ItemResponse<Question>>();
        responseMock.Setup(r => r.Resource).Returns(question);

        _mockContainer.Setup(container => container.ReadItemAsync<Question>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default))
            .ReturnsAsync(responseMock.Object);

        var result = await _questionService.GetQuestionByIdAsync("1");

        result.Should().BeEquivalentTo(question);
    }

    [Fact]
    public async Task UpdateQuestionAsync_ShouldUpdateQuestion()
    {
        var questionDto = new QuestionDto
        {
            Type = "Paragraph",
            Text = "Describe yourself",
            Options = new List<string>()
        };

        await _questionService.UpdateQuestionAsync("1", questionDto);

        _mockContainer.Verify(container => container.ReplaceItemAsync(It.IsAny<Question>(), "1", null, null, default), Times.Once);
    }

    [Fact]
    public async Task GetAllQuestionsAsync_ShouldReturnAllQuestions()
    {
        var questions = new List<Question>
        {
            new Question
            {
                Id = "1",
                Type = "Paragraph",
                Text = "Describe yourself",
                Options = new List<string>()
            },
            new Question
            {
                Id = "2",
                Type = "YesNo",
                Text = "Are you above 18?",
                Options = new List<string>()
            }
        };

        var feedResponseMock = new Mock<FeedResponse<Question>>();
        feedResponseMock.Setup(fr => fr.GetEnumerator()).Returns(questions.GetEnumerator());

        var feedIteratorMock = new Mock<FeedIterator<Question>>();
        feedIteratorMock.Setup(fi => fi.HasMoreResults).Returns(true);
        feedIteratorMock.Setup(fi => fi.ReadNextAsync(default)).ReturnsAsync(feedResponseMock.Object);

        _mockContainer.Setup(container => container.GetItemQueryIterator<Question>(
            It.IsAny<string>(), null, null))
            .Returns(feedIteratorMock.Object);

        var result = await _questionService.GetAllQuestionsAsync();

        result.Should().BeEquivalentTo(questions);
    }
}
