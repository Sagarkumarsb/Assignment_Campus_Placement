using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;

namespace Assignment_Campus_Placement.Services
{
    public interface IQuestionService
    {
        Task AddQuestionAsync(QuestionDto question);
        Task UpdateQuestionAsync(string id, QuestionDto question);
        Task<Question> GetQuestionByIdAsync(string id);
        Task<List<Question>> GetAllQuestionsAsync();
        Task DeleteQuestionAsync(string id);
    }
}
