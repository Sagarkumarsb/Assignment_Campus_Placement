using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;
using Assignment_Campus_Placement.Services;
using Microsoft.Azure.Cosmos;

public class CandidateApplicationService : ICandidateApplicationService
{
    private readonly Container _container;

    public CandidateApplicationService(CosmosClient dbClient, string databaseName, string containerName)
    {
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task SubmitApplicationAsync(CandidateApplicationDto applicationDto)
    {
        var application = new CandidateApplication
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = applicationDto.FirstName,
            LastName = applicationDto.LastName,
            Email = applicationDto.Email,
            Answers = applicationDto.Answers.Select(a => new Answer { QuestionId = a.QuestionId, Response = a.Response }).ToList()
        };
        await _container.CreateItemAsync(application, new PartitionKey(application.Id));
    }

    public async Task<CandidateApplication> GetApplicationByIdAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<CandidateApplication>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<List<CandidateApplication>> GetAllApplicationsAsync()
    {
        var query = _container.GetItemQueryIterator<CandidateApplication>(new QueryDefinition("SELECT * FROM c"));
        var results = new List<CandidateApplication>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }
}