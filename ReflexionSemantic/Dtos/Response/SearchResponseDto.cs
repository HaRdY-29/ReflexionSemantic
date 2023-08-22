using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReflexionSemantic.Dtos.Response
{
    public class SearchResponseDto
    {
        public SearchPool? search_pool { get; set; }
        public List<Datum>? data { get; set; }
        public PageInfo? page_info { get; set; }
    }
    public class SearchPool
    {
        public int? total_count { get; set; }
        public int? total_duration { get; set; }
        public string? index_id { get; set; }
    }
    public class Datum
    {
        public double? score { get; set; }
        public double? start { get; set; }
        public string? video_blob_url { get; set; }
        public double? end { get; set; }
        public List<Metadata1>? metadata { get; set; }
        public string? video_id { get; set; }
        public string? confidence { get; set; }
    }

    public class Metadata1
    {
        public string? type { get; set; }
    }

    public class PageInfo
    {
        public int? limit_per_page { get; set; }
        public int? total_results { get; set; }
        public DateTime? page_expired_at { get; set; }
    }
}
