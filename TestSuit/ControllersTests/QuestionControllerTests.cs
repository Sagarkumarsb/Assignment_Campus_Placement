
using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;
using Assignment_Campus_Placement.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class QuestionControllerTests
{
    private readonly Mock<IQuestionService> _mockQuestionService;
    private readonly QuestionController _controller;

    public QuestionControllerTests()
    {
        _mockQuestionService = new Mock<IQuestionService>();
        _controller = new QuestionController(_mockQuestionService.Object);
    }

    [Fact]
    public async Task AddQuestion_ShouldReturnOk()
    {
        var questionDto = new QuestionDto
        {
            Type = "Paragraph",
            Text = "Describe yourself",
            Options = new List<string>()
        };

        var result = await _controller.AddQuestion(questionDto);

        result.Should().BeOfType<OkResult>();
        _mockQuestionService.Verify(service => service.AddQuestionAsync(questionDto), Times.Once);
    }

    [Fact]
    public async Task GetQuestion_ShouldReturnQuestion()
    {
        var question = new Question
        {
            Id = "1",
            Type = "Paragraph",
            Text = "Describe yourself",
            Options = new List<string>()
        };

        _mockQuestionService.Setup(service => service.GetQuestionByIdAsync("1")).ReturnsAsync(question);

        var result = await _controller.GetQuestionById("1") as OkObjectResult;

        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(question);
    }

    [Fact]
    public async Task UpdateQuestion_ShouldReturnOk()
    {
        var questionDto = new QuestionDto
        {
            Type = "Paragraph",
            Text = "Describe yourself",
            Options = new List<string>()
        };

        var result = await _controller.UpdateQuestion("1", questionDto);

        result.Should().BeOfType<OkResult>();
        _mockQuestionService.Verify(service => service.UpdateQuestionAsync("1", questionDto), Times.Once);
    }

    [Fact]
    public async Task GetAllQuestions_ShouldReturnAllQuestions()
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

        _mockQuestionService.Setup(service => service.GetAllQuestionsAsync()).ReturnsAsync(questions);

        var result = await _controller.GetAllQuestions() as OkObjectResult;

        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(questions);
    }
}
