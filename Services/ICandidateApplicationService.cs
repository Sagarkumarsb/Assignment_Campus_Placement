using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Models;

namespace Assignment_Campus_Placement.Services
{
    public interface ICandidateApplicationService
    {
        Task SubmitApplicationAsync(CandidateApplicationDto application);
        Task<CandidateApplication> GetApplicationByIdAsync(string id);
        Task<List<CandidateApplication>> GetAllApplicationsAsync();
    }
}
