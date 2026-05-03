namespace Project
{
    public class Project
    {
        public required string Name { get; set; }
        public required DateTime CreationDate { get; set; }
        public required string Path { get; set; }

        public required string[] Templates { get; set; }
        public required int UnityVersion { get; set; }
        public string[]? ChangeHistory { get; set; }
    }
}