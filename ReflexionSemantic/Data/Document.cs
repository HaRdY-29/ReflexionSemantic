using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReflexionSemantic.Data
{
    public abstract class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime? CreatedTs { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedTs { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
