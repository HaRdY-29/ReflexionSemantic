using ReflexionSemantic.Data;
using System.ComponentModel.DataAnnotations;

namespace ReflexionSemantic.Models
{
    public class Indexes : Document
    {        
        public string? IndexName { get; set; }
        public string? EngineId { get; set; }
        public string? Indexoptions { get; set; }
        public string? TwelvelabIndexId { get; set; }
    }


    public class Videos : Document
    {
        public string? TwelvelabIndexId { get; set; }
        public string? RawData { get; set; }
        public string? VideoBlobUrl { get; set; }
        public string? FileName { get; set; }
        public string? TwelvelabVideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? TwelvelabVideoId { get; set; }
        public string? TwelvelabTaskId { get; set; }
        public string? status { get; set; }        
        public bool is_ready { get; set; }
        public string? Metadata { get; set; }
        public DateTime? EstimatedTime { get; set; }
    }
    
}
