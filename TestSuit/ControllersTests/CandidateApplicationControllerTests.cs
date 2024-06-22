using System.Collections.Generic;
using System.Threading.Tasks;
using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;
using Assignment_Campus_Placement.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class CandidateApplicationControllerTests
{
    private readonly Mock<ICandidateApplicationService> _mockApplicationService;
    private readonly CandidateApplicationController _controller;

    public CandidateApplicationControllerTests()
    {
        _mockApplicationService = new Mock<ICandidateApplicationService>();
        _controller = new CandidateApplicationController(_mockApplicationService.Object);
    }

    [Fact]
    public async Task SubmitApplication_ShouldReturnOk()
    {
        var applicationDto = new CandidateApplicationDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Answers = new List<AnswerDto>
            {
                new AnswerDto { QuestionId = "1", Response = "Test response" }
            }
        };

        var result = await _controller.SubmitApplication(applicationDto);

        result.Should().BeOfType<OkResult>();
        _mockApplicationService.Verify(service => service.SubmitApplicationAsync(applicationDto), Times.Once);
    }

    [Fact]
    public async Task GetApplication_ShouldReturnApplication()
    {
        var application = new CandidateApplication
        {
            Id = "1",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Answers = new List<Answer>
            {
                new Answer { QuestionId = "1", Response = "Test response" }
            }
        };

        _mockApplicationService.Setup(service => service.GetApplicationByIdAsync("1")).ReturnsAsync(application);

        var result = await _controller.GetApplicationById("1") as OkObjectResult;

        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(application);
    }

    [Fact]
    public async Task GetAllApplications_ShouldReturnAllApplications()
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

        _mockApplicationService.Setup(service => service.GetAllApplicationsAsync()).ReturnsAsync(applications);

        var result = await _controller.GetAllApplications() as OkObjectResult;

        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(applications);
    }
}
