namespace ReflexionSemantic.Dtos.Response
{
    public class TaskStatusResponseDto
    {
        public string _id { get; set; }
        public string index_id { get; set; }
        public string video_id { get; set; }
        public string status { get; set; }
        public Metadata2 metadata { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime estimated_time { get; set; }
        public string type { get; set; }
        public Hls hls { get; set; }
    }
    public class Hls
    {
        public string? video_url { get; set; }
        public List<string>? thumbnail_urls { get; set; }
        public string? status { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Metadata2
    {
        public double duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
