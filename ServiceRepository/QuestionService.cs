using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;
using Assignment_Campus_Placement.Services;
using Microsoft.Azure.Cosmos;

public class QuestionService : IQuestionService
{
    private readonly Container _container;

    public QuestionService(CosmosClient dbClient, string databaseName, string containerName)
    {
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddQuestionAsync(QuestionDto questionDto)
    {
        var question = new Question
        {
            Id = Guid.NewGuid().ToString(),
            Type = questionDto.Type,
            Text = questionDto.Text,
            Options = questionDto.Options
        };
        await _container.CreateItemAsync(question, new PartitionKey(question.Id));
    }

    public async Task UpdateQuestionAsync(string id, QuestionDto questionDto)
    {
        var question = await GetQuestionByIdAsync(id);
        if (question == null) throw new Exception("Question not found");

        question.Type = questionDto.Type;
        question.Text = questionDto.Text;
        question.Options = questionDto.Options;

        await _container.UpsertItemAsync(question, new PartitionKey(id));
    }

    public async Task<Question> GetQuestionByIdAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Question>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        var query = _container.GetItemQueryIterator<Question>(new QueryDefinition("SELECT * FROM c"));
        var results = new List<Question>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task DeleteQuestionAsync(string id)
    {
        await _container.DeleteItemAsync<Question>(id, new PartitionKey(id));
    }
}