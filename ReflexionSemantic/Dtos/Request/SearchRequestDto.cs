namespace ReflexionSemantic.Dtos.Request
{
    public class SearchRequestDto
    {
        public string? query { get; set; }
        public string? index_id { get; set; }
        public List<string>? search_options { get; set; }
        public string? conversation_option { get; set; }
    }
}
