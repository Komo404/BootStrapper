namespace BootStrapper.Core.Models
{
    public class Project
    {
        public required string Name { get; set; }
        public required DateTime CreationDate { get; set; } = DateTime.Now;
        public required string Path { get; set; }
        public required string[] Templates { get; set; }
        public required int UnityVersion { get; set; }
        public string[]? ChangeHistory { get; set; }
    }
}