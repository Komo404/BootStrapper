namespace Template
{
    public class Template
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Version { get; set; }
        public required int minUnityVersion { get; set; }
        public required int maxUnityVersion { get; set; } 
        public required string Category { get; set; }
        public required string Author { get; set; }
        public required string[] Dependencies { get; set; }  
    }
}