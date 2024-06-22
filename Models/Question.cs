namespace Assignment_Campus_Placement.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
    }
}
