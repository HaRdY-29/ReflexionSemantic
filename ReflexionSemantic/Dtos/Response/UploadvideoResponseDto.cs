namespace ReflexionSemantic.Dtos.Response
{
    public class UploadvideoResponseDto
    {
        public string? _id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime estimated_time { get; set; }
        public string? index_id { get; set; }
        public Metadata? metadata { get; set; }
        public string? status { get; set; }
        public string? type { get; set; }
        public DateTime updated_at { get; set; }
        public string? video_id { get; set; }
    }

    public class Metadata
    {
        public double duration { get; set; }
        public string? filename { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}
