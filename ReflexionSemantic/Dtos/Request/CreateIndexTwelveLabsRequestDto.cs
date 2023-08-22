namespace ReflexionSemantic.Dtos.Request
{
    public class CreateIndexTwelveLabsRequestDto
    {
        public string? engine_id { get; set; }
        public List<string>? index_options { get; set; }
        public string? index_name { get; set; }
    }
}
