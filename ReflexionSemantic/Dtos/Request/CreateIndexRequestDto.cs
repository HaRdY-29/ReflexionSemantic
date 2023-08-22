namespace ReflexionSemantic.Dtos.Request
{
    public class CreateIndexRequestDto
    {
        public string? IndexName { get; set; }
        public string? engine_id { get; set; } = "marengo2.5";
        public bool visual { get; set; }
        public bool conversation { get; set; }
        public bool text_in_video { get; set; }
        public bool logo { get; set; }
    }
}
