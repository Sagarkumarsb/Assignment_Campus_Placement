using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddQuestion([FromBody] QuestionDto questionDto)
    {
        await _questionService.AddQuestionAsync(questionDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion(string id, [FromBody] QuestionDto questionDto)
    {
        await _questionService.UpdateQuestionAsync(id, questionDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestionById(string id)
    {
        var question = await _questionService.GetQuestionByIdAsync(id);
        if (question == null)
            return NotFound();
        return Ok(question);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuestions()
    {
        var questions = await _questionService.GetAllQuestionsAsync();
        return Ok(questions);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(string id)
    {
        await _questionService.DeleteQuestionAsync(id);
        return Ok();
    }
}
