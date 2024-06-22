namespace Assignment_Campus_Placement.Dtos
{
    public class CandidateApplicationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
